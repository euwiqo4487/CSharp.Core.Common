using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace CSharp.Core.Common
{
    /// <summary>
    /// 壓縮/解壓縮工具類別
    /// </summary>
    public static class Compressor
    {
        /// <summary>
        /// 單檔壓縮
        /// </summary>
        /// <param name="fileToCompress">檔案Path</param>
        /// <param name="level">壓縮等級: NoCompression(無壓縮) / Fastest(最快) / Optimal(最佳)[預設]</param>
        public static void CompressFile(string fileToCompress, CompressionLevel level = CompressionLevel.Optimal)
        {
            FileInfo info = new FileInfo(fileToCompress);
            if (info.Exists)
                CompressFile(info, level);
        }

        /// <summary>
        /// 單檔壓縮
        /// </summary>
        /// <param name="fileToCompress">檔案資訊</param>
        /// <param name="level">壓縮等級: NoCompression(無壓縮) / Fastest(最快) / Optimal(最佳)[預設]</param>
        public static void CompressFile(FileInfo fileToCompress, CompressionLevel level = CompressionLevel.Optimal)
        {
            using (FileStream originalFileStream = fileToCompress.OpenRead())
            {
                if ((File.GetAttributes(fileToCompress.FullName) & FileAttributes.Hidden) != FileAttributes.Hidden & fileToCompress.Extension != ".zip")
                {
                    using (FileStream compressedFileStream = File.Create(fileToCompress.FullName + ".zip"))
                    {
                        using (GZipStream compressionStream = new GZipStream(compressedFileStream, level))
                        {
                            originalFileStream.CopyTo(compressionStream);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 目錄壓縮
        /// 注意: 來源和目標目錄必須不同
        /// </summary>
        /// <param name="folderToCompress">欲壓縮目錄</param>
        /// <param name="zipPath">目標壓縮檔案Path</param>
        /// <param name="level">壓縮等級</param>
        public static void CompressFolder(string folderToCompress, string zipPath, CompressionLevel level = CompressionLevel.Optimal)
        {
            string targetPath = Path.GetDirectoryName(zipPath);
            if (Directory.Exists(folderToCompress) && Directory.Exists(targetPath) && folderToCompress != targetPath)
                ZipFile.CreateFromDirectory(folderToCompress, zipPath, level, true, Encoding.Default);
        }

        /// <summary>
        /// 單檔解壓縮
        /// </summary>
        /// <param name="fileToDecompress">檔案Path</param>
        public static void DecompressFile(string fileToDecompress)
        {
            FileInfo info = new FileInfo(fileToDecompress);
            if (info.Exists)
                DecompressFile(info);
        }

        /// <summary>
        /// 單檔解壓縮
        /// </summary>
        /// <param name="fileToDecompress">檔案資訊</param>
        public static void DecompressFile(FileInfo fileToDecompress)
        {
            using (FileStream originalFileStream = fileToDecompress.OpenRead())
            {
                string currentFileName = fileToDecompress.FullName;
                string newFileName = currentFileName.Remove(currentFileName.Length - fileToDecompress.Extension.Length);

                using (FileStream decompressedFileStream = File.Create(newFileName))
                {
                    using (GZipStream decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress))
                    {
                        decompressionStream.CopyTo(decompressedFileStream);
                    }
                }
            }
        }

        /// <summary>
        /// 目錄解壓縮
        /// </summary>
        /// <param name="zipPath">壓縮檔Path</param>
        /// <param name="folderToDecompress">欲解壓縮至目標目錄</param>
        public static void DecompressFolder(string zipPath, string folderToDecompress)
        {
            if (Directory.Exists(folderToDecompress) && Directory.Exists(Path.GetDirectoryName(zipPath)))
                ZipFile.ExtractToDirectory(zipPath, folderToDecompress, Encoding.Default);
        }
    }
}
