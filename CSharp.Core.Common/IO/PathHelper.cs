using System;
using System.IO;
using System.Linq;

namespace CSharp.Core.Common
{
    /// <summary>
    /// 檔案或網路格式通用工具
    /// </summary>
    public static class PathHelper
    {
        static readonly char _backSlash = '/';
        static readonly char _forwardSlash = '\\';
        static readonly string _webExpr = "://";
        static readonly char[] _slashes = new char[] { _backSlash, _forwardSlash };

        /// <summary>
        /// 連結不同的路徑成絕對
        /// </summary>
        /// <param name="paths">路徑部份表示</param>
        /// <returns></returns>
        public static string Combine(params string[] paths)
        {
            string tmp = string.Empty;
            if (paths.Length > 0)
            {
                tmp = string.Join(_backSlash.ToString(), paths);
                tmp = tmp.Trim().TrimEnd(_slashes);
                if (tmp.IndexOf(_webExpr) > -1) //web
                {
                    for (int i = 0; i < paths.Length; i++)
                    {
                        if (i == 0 && paths[i].IndexOf(_webExpr) > -1)
                        {
                            if (paths[i].EndsWith(_webExpr))
                                paths[i] = paths[i].Replace(_webExpr, ":/");
                            else
                                paths[i] = paths[i].Trim().TrimEnd(_slashes).Replace(_forwardSlash, _backSlash);
                        }
                        else
                            paths[i] = paths[i].Trim().TrimStart(_slashes).TrimEnd(_slashes).Replace(_forwardSlash, _backSlash);
                    }
                    var uri = new Uri(string.Join(_backSlash.ToString(), paths));
                    //tmp = uri.AbsoluteUri;
                    tmp = uri.OriginalString;
                }
                else // file system
                {
                    for (int i = 0; i < paths.Length; i++)
                    {
                        if (i == 0)
                            paths[i] = paths[i].Trim().Replace(_backSlash, _forwardSlash);
                        else
                            paths[i] = paths[i].Trim().TrimEnd(_slashes).TrimStart(_slashes).Replace(_backSlash, _forwardSlash);
                    }
                    tmp = Path.Combine(paths);
                }
            }
            return tmp;
        }

        /// <summary>
        /// 取得FTP site表頭
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetFtpSite(string path)
        {
            string tmp = "";
            if (path.IndexOf(_webExpr) > -1)
            {
                var uri = new Uri(path);
                tmp = PathHelper.Combine("ftp://" + uri.Host, uri.Segments[1].TrimStart(_slashes).TrimEnd(_slashes));
            }
            return tmp;
        }

        /// <summary>
        /// 取得路徑面向
        /// </summary>
        /// <param name="path">路徑</param>
        /// <returns></returns>
        public static PathScope GetScope(string path)
        {
            var tmp = PathScope.None;
            if (path.IndexOf(_webExpr) > -1)
                tmp = PathScope.Ftp;
            else if (path.IndexOf(@"\\") == 0)
                tmp = PathScope.Unc;
            else if (path.IndexOf(@":\") > -1)
                tmp = PathScope.Local;
            return tmp;
        }

        /// <summary>
        /// 取得檔案名稱
        /// </summary>
        /// <param name="path">路徑</param>
        /// <returns></returns>
        public static string GetFileName(string path)
        {
            var tmp = path.Trim();
            if (tmp.IndexOf(_webExpr) > -1) //web
            {
                if (tmp.IndexOf(_forwardSlash) > -1)
                    tmp = tmp.Replace(_forwardSlash, _backSlash);
                if (tmp.Length - 1 == tmp.LastIndexOf(_backSlash))
                    return "";
                var uri = new Uri(tmp);
                var fileName = uri.Segments[uri.Segments.Length - 1];
                if (fileName == _backSlash.ToString() && uri.Segments.Length == 1)
                    fileName = uri.Host;
                else
                    fileName = Path.GetFileName(uri.LocalPath); //避掉中文或全型字被base64編碼
                return fileName;
            }
            else
                return Path.GetFileName(tmp);
        }

        /// <summary>
        /// 取得檔案路徑
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetPath(string path)
        {
            var tmp = path.Trim();
            if (tmp.IndexOf(_webExpr) > -1) //web
            {
                if (tmp.IndexOf(_forwardSlash) > -1)
                    tmp = tmp.Replace(_forwardSlash, _backSlash);
                var file = GetFileName(tmp);
                if (string.IsNullOrEmpty(file))
                    return tmp.TrimEnd(_backSlash);
                else
                    return tmp.Replace(GetFileName(tmp), "").TrimEnd(_backSlash);
            }
            else
                return Path.GetDirectoryName(path);
        }

        /// <summary>
        /// 以來源為基準轉換目標對應相對位置
        /// </summary>
        /// <param name="source">來源</param>
        /// <param name="dest">目標</param>
        /// <returns></returns>
        public static string RelativeDestination(string source, string dest)
        {
            string tmpSource = "", tmpDest = "";
            if (!string.IsNullOrEmpty(source) && !string.IsNullOrEmpty(dest))
            {
                string destFile = GetFileName(dest); // dest file name
                // source path
                tmpSource = source.Trim().Replace(GetFileName(source), "").TrimStart(_slashes).TrimEnd(_slashes);
                // dest path
                tmpDest = dest.Trim().Replace(GetFileName(dest), "").TrimStart(_slashes).TrimEnd(_slashes);

                int prev;

                if (tmpSource.IndexOf(_backSlash) > 1)
                    prev = tmpSource.Split(_backSlash).Count();
                else
                    prev = 1;

                for (int i = 0; i < prev; i++)
                {
                    tmpDest = string.Format("../{0}", tmpDest);
                }
                tmpDest = Path.Combine(tmpDest, destFile);
            }
            return tmpDest;
        }
    }

    /// <summary>
    /// 路徑面向
    /// </summary>
    public enum PathScope
    {
        /// <summary>
        /// 無法解析
        /// </summary>
        None = 0,
        /// <summary>
        /// 本地端
        /// </summary>
        Local,
        /// <summary>
        /// Universal Naming Convention
        /// </summary>
        Unc,
        /// <summary>
        /// File Transfer Protocol
        /// </summary>
        Ftp
    }
}
