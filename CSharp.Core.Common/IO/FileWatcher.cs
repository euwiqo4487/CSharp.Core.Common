using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharp.Core.Common
{
    /// <summary>
    /// FileSystemWatcher
    /// </summary>
    public class FileWatcher
    {
        object objectLock = new Object();
        private FileSystemWatcher watcher;
        /// <summary>
        /// FileSystemWatcher
        /// </summary>
        /// <param name="path">檔案位置</param>
        /// <param name="filter">過濾條件</param>
        public FileWatcher(string path = "", string filter = "*.config")
        {
            string _path;
            if (path == "")
            {
                _path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            }
            else
            {
                _path = path;
            }
#if (DEBUG)
            Console.WriteLine("File Path:" + _path);
#endif
            watcher = new FileSystemWatcher()
            {
                Path = _path,
                NotifyFilter = NotifyFilters.LastWrite,
                Filter = filter
            };
        }
        /// <summary>
        /// true: Begin watching. false:stop watching.
        /// </summary>
        public bool EnableRaisingEvents
        {
            set
            {
                watcher.EnableRaisingEvents = value;
            }
        }
        /// <summary>
        /// Changed
        /// </summary>
        public event FileSystemEventHandler Changed
        {
            add
            {
                lock (objectLock)
                {
                    watcher.Changed += value;
                }
            }
            remove
            {
                lock (objectLock)
                {
                    watcher.Changed -= value;
                }
            }
        }
        /// <summary>
        /// Created
        /// </summary>
        public event FileSystemEventHandler Created
        {
            add
            {
                lock (objectLock)
                {
                    watcher.Created += value;
                }
            }
            remove
            {
                lock (objectLock)
                {
                    watcher.Created -= value;
                }
            }
        }
        /// <summary>
        ///  Deleted
        /// </summary>
        public event FileSystemEventHandler Deleted
        {
            add
            {
                lock (objectLock)
                {
                    watcher.Deleted += value;
                }
            }
            remove
            {
                lock (objectLock)
                {
                    watcher.Deleted -= value;
                }
            }
        }
    }
}
