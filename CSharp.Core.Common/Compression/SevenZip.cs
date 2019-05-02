using System;
using System.IO;
using SevenZip;

namespace CSharp.Core.Common
{
    /// <summary>
    /// 壓縮/解壓縮工具
    /// </summary>
    public static class SevenZip
    {
        static SevenZip()
        {
            string refLib = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "7z.dll");
            SevenZipBase.SetLibraryPath(refLib);
        }

        static SevenZipCompressor szCompressor = null;
        static SevenZipCompressor SzCompressor
        {
            get
            {
                if (szCompressor == null)
                    szCompressor = new SevenZipCompressor
                    {
                        //CompressionMethod = CompressionMethod.Lzma2,
                        CompressionMethod = CompressionMethod.Default,
                        CompressionLevel = CompressionLevel.Ultra,
                        CompressionMode = CompressionMode.Create,
                        //ArchiveFormat = OutArchiveFormat.SevenZip
                        ArchiveFormat = OutArchiveFormat.Zip
                    };
                return szCompressor;
            }
        }

        /// <summary>
        /// 壓縮檔案
        /// </summary>
        /// <param name="zip">壓縮檔</param>
        /// <param name="password">密碼</param>
        /// <param name="files">欲壓縮檔案</param>
        /// <returns></returns>
        public static bool CompressFiles(string zip, string password = "", params string[] files)
        {
            SevenZipExtractor extractor = null;
            try
            {
                zip = Path.Combine(Path.GetDirectoryName(zip), string.Format("{0}.zip", Path.GetFileNameWithoutExtension(zip))); //force .zip extension
                using (var fs = new FileStream(zip, FileMode.Create))
                {
                    if (!string.IsNullOrWhiteSpace(password))
                    {
                        SzCompressor.ZipEncryptionMethod = ZipEncryptionMethod.Aes256;
                        SzCompressor.CompressFilesEncrypted(fs, password, files);
                    }
                    else
                        SzCompressor.CompressFiles(fs, files);
                }

                //7z Check() ok, but zip Check() always return false
                //using (var fs = new FileStream(zip, FileMode.Open))
                //{
                //    if (!string.IsNullOrWhiteSpace(password))
                //        extractor = new SevenZipExtractor(fs, password);
                //    else
                //        extractor = new SevenZipExtractor(fs);
                //    return extractor.Check();
                //}
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.AppLogException(ex, "Unhandled exception", "CompressFiles", "SevenZip.txt");
                return false;
            }
            finally
            {
                if (extractor != null)
                    extractor.Dispose();
            }
        }

        /// <summary>
        /// 目錄壓縮
        /// 注意: 來源和目標目錄必須不同
        /// </summary>
        /// <param name="srcFolder">欲壓縮目錄</param>
        /// <param name="zip">目標壓縮檔案輸出目錄</param>
        /// <param name="password">密碼</param>
        public static bool CompressFolder(string srcFolder, string zip, string password = "")
        {
            SevenZipExtractor extractor = null;
            try
            {
                zip = Path.Combine(Path.GetDirectoryName(zip), string.Format("{0}.zip", Path.GetFileNameWithoutExtension(zip))); //force .zip extension
                if (!string.IsNullOrEmpty(password))
                {
                    SzCompressor.ZipEncryptionMethod = ZipEncryptionMethod.Aes256;
                    SzCompressor.CompressDirectory(srcFolder, zip, true, password);
                }
                else
                    SzCompressor.CompressDirectory(srcFolder, zip, true);

                //7z Check() ok, but zip Check() always return false
                //using (var fs = new FileStream(zip, FileMode.Open))
                //{
                //    if (!string.IsNullOrEmpty(password))
                //        extractor = new SevenZipExtractor(fs, password);
                //    else
                //        extractor = new SevenZipExtractor(fs);
                //    return extractor.Check();
                //}
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.AppLogException(ex, "Unhandled exception", "CompressFolder", "SevenZip.txt");
                return false;
            }
            finally
            {
                if (extractor != null)
                    extractor.Dispose();
            }
        }

        /// <summary>
        /// 解壓縮
        /// </summary>
        /// <param name="zip">壓縮檔</param>
        /// <param name="exPath">解壓縮目錄</param>
        /// <param name="password">密碼</param>
        /// <returns></returns>
        public static bool Decompress(string zip, string exPath = "", string password = "")
        {
            if (!File.Exists(zip)) return false;
            SevenZipExtractor extractor = null;

            try
            {
                if (string.IsNullOrEmpty(exPath))
                    exPath = Path.GetDirectoryName(zip);

                if (!Directory.Exists(exPath))
                    Directory.CreateDirectory(exPath);

                using (var fs = new FileStream(zip, FileMode.Open))
                {
                    if (!string.IsNullOrEmpty(password))
                        extractor = new SevenZipExtractor(fs, password);
                    else
                        extractor = new SevenZipExtractor(fs);
                    extractor.ExtractArchive(exPath);
                    return true;
                }

                //7z Check() ok, but zip Check() always return false
                //if (extractor.Check())
                //{
                //}
                //return false;
            }
            catch (Exception ex)
            {
                LogHelper.AppLogException(ex, "Unhandled exception", "Decompress", "SevenZip.txt");
                return false;
            }
            finally
            {
                if (extractor != null)
                    extractor.Dispose();
            }
        }
    }
}
