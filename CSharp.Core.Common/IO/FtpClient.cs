using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;

namespace CSharp.Core.Common
{
    /// <summary>
    /// Ftp目錄資訊, 搭配FtpClient使用
    /// </summary>
    public class FtpDirectoryEntry
    {
        /// <summary>
        /// Name
        /// </summary>
        public string Name;
        /// <summary>
        /// Create Time
        /// </summary>
        public DateTime CreateTime;
        /// <summary>
        /// Is Directory?
        /// </summary>
        public bool IsDirectory;
        /// <summary>
        /// Size
        /// </summary>
        public Int64 Size;
        /// <summary>
        /// Group (UNIX only)
        /// </summary>
        public string Group;    // UNIX only
        /// <summary>
        /// Owner
        /// </summary>
        public string Owner;
        /// <summary>
        /// Flags
        /// </summary>
        public string Flags;
    }

    /// <summary>
    /// 通用FTP客戶端類別
    /// </summary>
    public class FtpClient : IDisposable
    {
        /// <summary>
        /// 目前FTP位置的工具類別
        /// </summary>
        protected FtpDirectory _host;
        bool disposed;
        /// <summary>
        /// 存取目前FTP domain(根目錄)或其下指定的目錄
        /// </summary>
        public string Host
        {
            set
            {
                _host.IsVirtualMask = IsVirtualMask;
                _host.SetUrl(value);
            }
            get { return _host.GetUrl(); }
        }

        /// <summary>
        /// 存取虛擬FTP表示
        /// </summary>
        public bool IsVirtualMask { get; set; }

        /// <summary>
        /// 存取使用者名稱
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// 存取使用者密碼
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 是否為根目錄
        /// </summary>
        public bool IsRootDirectory
        {
            get { return _host.IsRootDirectory; }
        }
        /// <summary>
        /// 建構子
        /// </summary>
        public FtpClient()
        {
            _host = new FtpDirectory();
        }

        /// <summary>
        /// 目前指定FTP位置的目錄清單
        /// </summary>
        /// <returns>目錄清單</returns>
        public List<FtpDirectoryEntry> ListDirectory()
        {
            FtpWebRequest request = GetRequest();
            request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;

            string listing;
            using (var response = request.GetResponse() as FtpWebResponse)
            {
                using (var sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    listing = sr.ReadToEnd();
            }
            return ParseDirectoryListing(listing);
        }

        /// <summary>
        /// 變更目前指定的FTP位置. 如果是 "/" 則是相對於根目錄. 如果是 ".." 則代表父目錄
        /// </summary>
        /// <param name="directory">FTP位置</param>
        public void ChangeDirectory(string directory)
        {
            _host.CurrentDirectory = directory;
        }

        /// <summary>
        /// 遠端檔案是否存在
        /// </summary>
        /// <param name="file">檔案</param>
        /// <returns>true : 存在</returns>
        public bool FileExists(string file)
        {
            var dt = GetFileLastModified(file);
            return dt != null;
        }

        /// <summary>
        /// 遠端目錄是否存在
        /// </summary>
        /// <param name="directory">測試的目錄(可用絕對或相對的路徑表示)</param>
        /// <returns>true : 存在</returns>
        public bool DirectoryExists(string directory)
        {
            try
            {
                //System.Diagnostics.TextWriterTraceListener myListener = new System.Diagnostics.TextWriterTraceListener("TextWriterOutput.log", "myListener");
                FtpWebRequest request = GetRequest(directory);
                request.Method = WebRequestMethods.Ftp.ListDirectory;

                using (FtpWebResponse response = request.GetResponse() as FtpWebResponse)
                {
                    StreamReader sr = new StreamReader(response.GetResponseStream(),
                        System.Text.Encoding.UTF8);
                    sr.ReadToEnd();
                    sr.Close();
                    //response.Close();
                }
                //myListener.Flush();
                return true;
            }
            catch { }
            return false;
        }
        /// <summary>
        /// 取得遠端檔案上次修改的時間
        /// </summary>
        /// <param name="path">檔案位置</param>
        /// <returns>上次修改時間</returns>
        public DateTime? GetFileLastModified(string path)
        {
            DateTime? datetime = null;
            try
            {
                FtpWebRequest request = GetRequest(path);
                request.Method = WebRequestMethods.Ftp.GetDateTimestamp;

                using (FtpWebResponse response = request.GetResponse() as FtpWebResponse)
                {
                    datetime = response.LastModified;
                    response.Close();
                }
            }
            catch (Exception ex)
            {
                LogHelper.AppLogException(ex, "Warn", string.Format("GetFileLastModified('{0}')", path), "FtpClient.txt");
            }
            return datetime;
        }

        /// <summary>
        /// 建立遠端目錄, 一個路徑中的所有階層的目錄如果不存在都會被建立
        /// </summary>
        /// <param name="directory">目錄位置表示</param>
        public void CreateDirectory(string directory)
        {
            // Get absolute directory
            directory = _host.ApplyDirectory(directory);

            // Split into path components
            string[] steps = directory.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            // Build list of full paths to each path component
            List<string> paths = new List<string>();
            for (int i = 1; i <= steps.Length; i++)
                paths.Add(FtpDirectory.ForwardSlash + String.Join(FtpDirectory.ForwardSlash, steps, 0, i));

            // Find first path component that needs creating
            int createIndex;
            for (createIndex = paths.Count; createIndex > 0; createIndex--)
            {
                if (DirectoryExists(paths[createIndex - 1]))
                    break;
            }

            // Created needed paths
            for (; createIndex < paths.Count; createIndex++)
            {
                FtpWebRequest request = GetRequest(paths[createIndex]);
                request.Method = WebRequestMethods.Ftp.MakeDirectory;
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                response.Close();
            }
        }

        /// <summary>
        /// 搬移檔案
        /// </summary>
        /// <param name="source">來源檔案</param>
        /// <param name="dest">目標檔案</param>
        public void MoveFile(string source, string dest)
        {
            // create dest directory if not exist
            string destFile = PathHelper.GetFileName(dest), destDir = dest.Replace("/" + destFile, "");
            if (!DirectoryExists(destDir))
                CreateDirectory(destDir);
            // get request base on source
            FtpWebRequest request = GetRequest(source);
            request.Method = WebRequestMethods.Ftp.Rename;
            // calculate relative dest position, the Rename method requires it
            request.RenameTo = PathHelper.RelativeDestination(source, dest);
            request.GetResponse().Close();
        }

        /// <summary>
        /// 上傳多個檔案至遠端目前指定的位置
        /// </summary>
        /// <param name="paths">多個檔案位置</param>
        public void UploadFiles(params string[] paths)
        {
            foreach (string path in paths)
            {
                FtpWebRequest request = GetRequest(Path.GetFileName(path));
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.UseBinary = true;

                var info = new FileInfo(path);
                request.ContentLength = info.Length;

                using (FileStream source = info.OpenRead())
                {
                    using (Stream target = request.GetRequestStream())
                        source.CopyTo(target);
                }

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                response.Close();
            }
        }

        /// <summary>
        /// 上傳多個檔案至遠端目前指定的位置
        /// </summary>
        /// <param name="sources">多個檔案串流</param>
        public void UploadStreams(Dictionary<string, byte[]> sources)
        {
            foreach (var item in sources)
            {
                FtpWebRequest request = GetRequest(item.Key);
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.UseBinary = true;
                request.ContentLength = item.Value.Length;
                using (MemoryStream source = new MemoryStream(item.Value))
                {
                    using (Stream target = request.GetRequestStream())
                        source.CopyTo(target);
                }
                using (var response = (FtpWebResponse)request.GetResponse()) { }
            }
        }

        /// <summary>
        /// 上傳檔案至遠端目前指定的位置
        /// </summary>
        /// <param name="fileName">檔名</param>
        /// <param name="file">檔案內容(byte陣列)</param>
        public void UploadStream(string fileName, byte[] file)
        {
            var tmp = new Dictionary<string, byte[]>();
            tmp.Add(fileName, file);
            UploadStreams(tmp);
        }

        /// <summary>
        /// 下載遠端(多個)檔案
        /// </summary>
        /// <param name="path">遠端目錄</param>
        /// <param name="files">該目錄下的指定檔案, 若未指定則目錄下所有檔案皆會被下載</param>
        public void DownloadFiles(string path, params string[] files)
        {
            foreach (string file in files)
            {
                FtpWebRequest request = GetRequest(file);
                request.Method = WebRequestMethods.Ftp.DownloadFile;
                request.UseBinary = true;
                using (var response = (FtpWebResponse)request.GetResponse())
                {
                    using (Stream source = response.GetResponseStream())
                    {
                        using (var target = new FileStream(Path.Combine(path, file), FileMode.Create, FileAccess.Write, FileShare.None))
                            source.CopyTo(target);
                    }
                }
            }
        }

        /// <summary>
        /// 下載遠端(多個)檔案
        /// </summary>
        /// <param name="files">該目錄下的指定檔案, 若未指定則目錄下所有檔案皆會被下載</param>
        /// <returns></returns>
        public Dictionary<string, byte[]> DownloadStreams(params string[] files)
        {
            var tmp = new Dictionary<string, byte[]>();
            foreach (string file in files)
            {
                FtpWebRequest request = GetRequest(file);
                request.Method = WebRequestMethods.Ftp.DownloadFile;
                request.UseBinary = true;

                using (var target = new MemoryStream())
                {
                    var response = (FtpWebResponse)request.GetResponse();
                    using (Stream source = response.GetResponseStream())
                        source.CopyTo(target);
                    response.Close();
                    tmp.Add(PathHelper.GetFileName(file), target.ToArray());
                }
            }
            return tmp;
        }

        /// <summary>
        /// 下載遠端檔案
        /// </summary>
        /// <param name="file">該目錄下的指定檔案</param>
        /// <returns></returns>
        public byte[] DownloadStream(string file)
        {
            string fileName = PathHelper.GetFileName(file);
            Dictionary<string, byte[]> tmp = DownloadStreams(file);
            if (tmp != null && tmp.ContainsKey(fileName))
                return tmp[fileName];
            return null;
        }

        /// <summary>
        /// 刪除(多個)遠端檔案
        /// </summary>
        /// <param name="files">(多個)檔案位置</param>
        public void DeleteFiles(params string[] files)
        {
            foreach (string file in files)
            {
                FtpWebRequest request = GetRequest(file);
                request.Method = WebRequestMethods.Ftp.DeleteFile;
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                response.Close();
            }
        }

        /// <summary>
        /// 刪除遠端目錄 (注意: 該目錄下所有檔案及子目錄會全被刪除)
        /// </summary>
        /// <param name="directory">目錄位置</param>
        public void DeleteDirectory(string directory)
        {
            FtpWebRequest request = GetRequest(directory);
            request.Method = WebRequestMethods.Ftp.RemoveDirectory;
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            response.Close();
        }

        #region Protected helper methods

        /// <summary>
        /// Constructs an FTP web request
        /// </summary>
        /// <returns>FTP web request</returns>
        protected FtpWebRequest GetRequest()
        {
            return GetRequest("");
        }

        /// <summary>
        /// Constructs an FTP web request with the given filename
        /// </summary>
        /// <param name="filename">檔名</param>
        /// <returns>FTP web request</returns>
        protected FtpWebRequest GetRequest(string filename)
        {
            try
            {
                string url = _host.GetUrl(filename);
                var request = WebRequest.Create(url) as FtpWebRequest;
                //SetMethodRequiresCWD();
                var auth = CSharpCustomer.RequestFileTransferAuth(Username, Password, url);
                if (auth.Item1.Trim() == "<<ErrMsg>>")
                    throw new InvalidOperationException(auth.Item2);
                var nc = new NetworkCredential(auth.Item2, auth.Item3);
                if (!string.IsNullOrWhiteSpace(auth.Item1))
                    nc.Domain = auth.Item1.Trim();
                request.Credentials = nc;
                request.KeepAlive = false;
                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                if (config != null && config.AppSettings.Settings != null)
                {
                    var ss = config.AppSettings.Settings;
                    bool tmp;
                    if (ss["FtpUsePassive"] != null &&
                        !string.IsNullOrEmpty(ss["FtpUsePassive"].Value) &&
                        bool.TryParse(ss["FtpUsePassive"].Value, out tmp) &&
                        request.UsePassive != tmp)
                        request.UsePassive = tmp; //UsePassive預設是true
                    if (ss["FtpKeepAlive"] != null &&
                        !string.IsNullOrEmpty(ss["FtpKeepAlive"].Value) &&
                        bool.TryParse(ss["FtpKeepAlive"].Value, out tmp) &&
                        request.KeepAlive != tmp)
                        request.KeepAlive = tmp;
                }
                return request;
            }
            catch (Exception ex)
            {
                LogHelper.AppLogException(ex, "Panic", string.Format("GetRequest('{0}')", filename), "FtpClient.txt");
                throw ex;
            }
        }

        private static void SetMethodRequiresCWD()
        {
            Type requestType = typeof(FtpWebRequest);
            FieldInfo methodInfoField = requestType.GetField("m_MethodInfo", BindingFlags.NonPublic | BindingFlags.Instance);
            Type methodInfoType = methodInfoField.FieldType;


            FieldInfo knownMethodsField = methodInfoType.GetField("KnownMethodInfo", BindingFlags.Static | BindingFlags.NonPublic);
            Array knownMethodsArray = (Array)knownMethodsField.GetValue(null);

            FieldInfo flagsField = methodInfoType.GetField("Flags", BindingFlags.NonPublic | BindingFlags.Instance);

            int MustChangeWorkingDirectoryToPath = 0x100;
            foreach (object knownMethod in knownMethodsArray)
            {
                int flags = (int)flagsField.GetValue(knownMethod);
                flags |= MustChangeWorkingDirectoryToPath;
                flagsField.SetValue(knownMethod, flags);
            }
        }

        delegate FtpDirectoryEntry ParseLine(string lines);

        /// <summary>
        /// Converts a directory listing to a list of FtpDirectoryEntrys
        /// </summary>
        /// <param name="listing">FtpDirectoryEntrys</param>
        /// <returns>list of FtpDirectoryEntrys</returns>
        protected List<FtpDirectoryEntry> ParseDirectoryListing(string listing)
        {
            ParseLine parseFunction = null;
            List<FtpDirectoryEntry> entries = new List<FtpDirectoryEntry>();
            string[] lines = listing.Split('\n');
            foreach (string line in lines)
            {
                if (line.Length > 0)
                {
                    if (parseFunction == null)
                    {
                        if (IsWinListing(line))
                            parseFunction = ParseWindowsDirectoryListing;
                        else
                            parseFunction = ParseUnixDirectoryListing;
                    }
                    FtpDirectoryEntry entry = parseFunction(line);
                    if (entry.Name != "." && entry.Name != "..")
                        entries.Add(entry);
                }
            }
            return entries;
        }
        protected bool IsWinListing(string text)
        {
            string dateStr = text.Substring(0, 8);
            string[] date = dateStr.Split('-');
            DateTime dt;
            return DateTime.TryParse(string.Format("{0}-{1}-{2}", date[2], date[0], date[1]), out dt);
        }
        /// <summary>
        /// Parses a line from a Windows-format listing
        /// </summary>
        /// <param name="text">Assumes listing style as:09-13-18  07:35PM                   67 CoolMergeFirst.txt</param>
        /// <returns>FtpDirectoryEntry</returns>
        protected FtpDirectoryEntry ParseWindowsDirectoryListing(string text)
        {
            /*
             * 09-13-18  07:35PM                   67 CoolMergeFirst.txt
             * 09-13-18  07:35PM                   42 CoolMergeTPSec.txt
             * 09-14-18  06:05PM       <DIR>          dir1
             * 09-14-18  06:05PM       <DIR>          dir2
             * 09-13-18  07:35PM                   26 TestMerge2xAdd.txt
             * 09-13-18  07:35PM                   12 TestMerge3xSpec.csv
             */
            FtpDirectoryEntry entry = new FtpDirectoryEntry();

            text = text.Trim();
            string dateStr = text.Substring(0, 8);
            string[] date = dateStr.Split('-');
            text = text.Substring(8).Trim();
            string timeStr = text.Substring(0, 7);
            text = text.Substring(7).Trim();
            entry.CreateTime = DateTime.Parse(String.Format("{0}-{1}-{2} {3}", date[2], date[0], date[1], timeStr));
            if (text.Substring(0, 5) == "<DIR>")
            {
                entry.IsDirectory = true;
                text = text.Substring(5).Trim();
            }
            else
            {
                entry.IsDirectory = false;
                int pos = text.IndexOf(' ');
                entry.Size = Int64.Parse(text.Substring(0, pos));
                text = text.Substring(pos).Trim();
            }
            entry.Name = text;  // Rest is name

            return entry;
        }
        /// <summary>
        /// Parses a line from a Unix-format listing
        /// </summary>
        /// <param name="text">Assumes listing style as:-rw-r--r-- 1 ftp ftp             67 Sep 13 19:35 CoolMergeFirst.txt</param>
        /// <returns>FtpDirectoryEntry</returns>
        protected FtpDirectoryEntry ParseUnixDirectoryListing(string text)
        {
            /*
             * -rw-r--r-- 1 ftp ftp           1252 Aug 11  2008 btn_home_h.gif
             * -rw-r--r-- 1 ftp ftp             67 Sep 13 19:35 CoolMergeFirst.txt
             * -rw-r--r-- 1 ftp ftp             42 Sep 13 19:35 CoolMergeTPSec.txt
             * drwxr-xr-x 1 ftp ftp              0 Sep 14 18:05 dir1
             * drwxr-xr-x 1 ftp ftp              0 Sep 14 18:05 dir2
             * -rw-r--r-- 1 ftp ftp             26 Sep 13 19:35 TestMerge2xAdd.txt
             * -rw-r--r-- 1 ftp ftp             12 Sep 13 19:35 TestMerge3xSpec.csv
             * -rw-r--r-- 1 ftp ftp             26 Sep 13 19:35 TestMergeFirst.txt
            */
            Console.WriteLine(text);
            var entry = new FtpDirectoryEntry();

            int pos;
            text = text.Substring(20).Trim();
            pos = text.IndexOf(" ");
            entry.Size = Int64.Parse(text.Substring(0, pos));
            text = text.Substring(pos + 1);
            pos = text.LastIndexOf(" ");
            string datetimeStr = text.Substring(0, pos);
            text = text.Substring(pos + 1);
            entry.Name = text;
            if (Path.GetExtension(entry.Name).Trim() == "" && entry.Size == 0)
                entry.IsDirectory = true;
            if (datetimeStr.Length > 0 && datetimeStr.Length <= 12)
            {
                int month, day;
                pos = datetimeStr.LastIndexOf(" ");
                string timeStr = datetimeStr.Substring(pos).Trim();
                datetimeStr = datetimeStr.Substring(0, pos).Trim();
                string[] monthNames = CultureInfo.GetCultureInfo("en").DateTimeFormat.AbbreviatedMonthGenitiveNames;
                month = Array.IndexOf(monthNames, datetimeStr.Substring(0, 3)) + 1;
                day = int.Parse(datetimeStr.Substring(3).Trim());
                entry.CreateTime = DateTime.ParseExact(string.Format("{0}-{1}-{2} {3}", timeStr.IndexOf(":") == -1 ? timeStr : DateTime.Now.Year.ToString(), month.ToString().PadLeft(2, '0'), day, timeStr.IndexOf(":") == -1 ? "00:00" : timeStr), "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
            }
            return entry;
        }

        /// <summary>
        /// Removes the token ending in the specified character
        /// </summary>
        /// <param name="s">目標字串</param>
        /// <param name="c">比對字元</param>
        /// <param name="startIndex">起始位置</param>
        /// <returns>token ending</returns>         
        protected string CutSubstringWithTrim(ref string s, char c, int startIndex)
        {
            int pos = s.IndexOf(c, startIndex);
            if (pos < 0) pos = s.Length;
            string retString = s.Substring(0, pos);
            s = (s.Substring(pos)).Trim();
            return retString;
        }

        #endregion

        #region IDisposable
        /// <summary>
        /// 關閉 host
        /// </summary>
        /// <param name="disposing"> true:要關閉host false:不關閉host</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _host = null;
                }
            }
            //dispose unmanaged ressources
            disposed = true;
        }
        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }

    /// <summary>
    /// 目前FTP位置的工具類別, 搭配FtpClient使用
    /// (目前不開放獨立使用)
    /// </summary>
    public class FtpDirectory
    {
        // Static members
        /// <summary>
        /// 尾隨斜線
        /// </summary>
        protected static char[] _slashes = { '/', '\\' };
        /// <summary>
        /// Back Slash
        /// </summary>
        public static string BackSlash = "\\";
        /// <summary>
        /// Forward Slash
        /// </summary>
        public static string ForwardSlash = "/";

        // Member variables
        /// <summary>
        /// No trailing slash
        /// </summary>
        protected string _domain;    // No trailing slash
        /// <summary>
        /// Leading, no trailing slash
        /// </summary>
        protected string _cwd;        // Leading, no trailing slash
        /// <summary>
        /// Domain (沒有尾隨斜線)
        /// </summary>
        public string Domain { get { return _domain; } }

        /// <summary>
        /// 存取虛擬FTP表示
        /// </summary>
        public bool IsVirtualMask { get; set; }

        /// <summary>
        ///  Construction
        /// </summary>
        public FtpDirectory()
        {
            _domain = String.Empty;
            _cwd = ForwardSlash;    // Root directory
        }

        /// <summary>
        /// Determines if the current directory is the root directory.
        /// </summary>
        public bool IsRootDirectory
        {
            get { return _cwd == ForwardSlash; }
        }

        /// <summary>
        /// Gets or sets the current FTP directory.
        /// </summary>
        public string CurrentDirectory
        {
            get { return _cwd; }
            set { _cwd = ApplyDirectory(value); }
        }

        /// <summary>
        /// Sets the domain and current directory from a URL.
        /// </summary>
        /// <param name="url">URL to set to</param>
        public void SetUrl(string url)
        {
            // Separate domain from directory
            int pos = url.IndexOf("://");
            pos = url.IndexOfAny(_slashes, (pos < 0) ? 0 : pos + 3);

            _mask = GetMask(url);
            if (pos < 0)
            {
                _domain = url.ToLower();
                _cwd = ForwardSlash;
            }
            else
            {
                _domain = url.Substring(0, pos).ToLower();

                // Normalize directory string
                if (_mask.ToLower() == url.ToLower())
                    _cwd = ApplyDirectory("");
                else
                    _cwd = ApplyDirectory(url.Substring(pos));
            }
        }

        private string _mask;
        private string GetMask(string url)
        {
            string tmp = string.Empty;
            try
            {
                var uri = new Uri(url);
                if (IsVirtualMask)
                    tmp = string.Format("ftp://{0}/{1}", uri.Host.ToLower(), uri.Segments[1]);
                else
                    tmp = string.Format("ftp://{0}", uri.Host.ToLower());

            }
            catch { }
            return tmp.TrimEnd(_slashes); ;
        }

        /// <summary>
        /// Returns the domain and current directory as a URL.
        /// </summary>
        /// <returns>Url</returns>
        public string GetUrl()
        {
            return GetUrl(String.Empty);
        }
        /// <summary>
        ///  Returns the domain and current directory as a URL.
        /// </summary>
        /// <param name="directory">Partial directory or filename applied to the current working directory</param>
        /// <returns>Url</returns>        
        public string GetUrl(string directory)
        {
            if (directory.Length == 0)
                return _domain + _cwd;
            return _domain + ApplyDirectory(directory);
        }

        /// <summary>
        /// Applies the given directory to the current directory and returns the
        /// result.
        /// 
        /// If directory starts with "/", it replaces all of the current directory.
        /// If directory is "..", the top-most subdirectory is removed from
        /// the current directory.
        /// </summary>
        /// <param name="directory">The directory to apply</param>
        /// <returns>Ftp位置</returns>       
        public string ApplyDirectory(string directory)
        {
            // Normalize directory
            directory = directory.Trim();
            directory = directory.Replace(BackSlash, ForwardSlash);
            directory = directory.TrimEnd(_slashes);
            if (directory.ToLower().IndexOf(_mask.ToLower()) > -1)
                directory = directory.ToLower().Replace(_mask.ToLower(), "");

            if (directory == "..")
            {
                int pos = _cwd.LastIndexOf(ForwardSlash);
                return (pos <= 0) ? ForwardSlash : _cwd.Substring(0, pos);
            }
            else if (directory.StartsWith(ForwardSlash))
            {
                // Specifies complete directory path
                return directory;
            }
            else
            {
                // Relative to current directory
                if (_cwd == ForwardSlash)
                    return _cwd + directory;
                else
                    return _cwd + ForwardSlash + directory;
            }
        }

        /// <summary>
        /// Returns the domain and current directory as a URL
        /// </summary>
        /// <returns>URL</returns>
        public override string ToString()
        {
            return GetUrl();
        }
    }
}
