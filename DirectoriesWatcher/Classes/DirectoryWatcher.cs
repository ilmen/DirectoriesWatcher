using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DirectoriesWatcher.Classes
{
    public class DirectoryWatcher
    {
        #region Fields and Properties
        FileSystemWatcher fsw;

        public string SourcePath
        { get; set; }

        public string TargetPath
        { get; set; }

        public INotifyIconManager NotifyIconManager
        { get; set; } 
        #endregion

        public DirectoryWatcher()
        {
            this.NotifyIconManager = new NotifyIconManager();
        }

        public void Start(string sourcePath, string targetPath)
        {
            if (!Directory.Exists(sourcePath)) throw new DirectoryNotFoundException();
            if (!Directory.Exists(targetPath)) Directory.CreateDirectory(targetPath);

            SourcePath = sourcePath.ToLower();
            TargetPath = targetPath.ToLower();

            fsw = CreateFileSystemWatcher(sourcePath);
            fsw.EnableRaisingEvents = true;
        }

        private FileSystemWatcher CreateFileSystemWatcher(string sourcePath)
        {
            var fsw = new FileSystemWatcher(sourcePath);
            fsw.IncludeSubdirectories = true;
            fsw.NotifyFilter =
                NotifyFilters.Attributes |
                NotifyFilters.DirectoryName |
                NotifyFilters.FileName |
                NotifyFilters.LastWrite |
                NotifyFilters.Security |
                NotifyFilters.Size;
            fsw.Filter = "*.*";

            fsw.Created += (s, e) => RunCoping(e);
            fsw.Renamed += (s, e) => RunCoping(e);
            fsw.Changed += (s, e) => RunCoping(e);
            fsw.Deleted += (s, e) => RunCoping(e);
            fsw.Error += (s, e) => NotifyIconManager.ShowBallonToolTip("Ошибка: " + e.GetException().ToString());

            return fsw;
        }

        private void RunCoping(FileSystemEventArgs e)
        {
            NotifyIconManager.ShowBallonToolTip("Обрабатывается файл " + e.FullPath);

            /*
             * отличать изменения папки и файла
             * вызывать по интерфейсу или обновление папки или файла
             */

            RunFileCoping(e);
        }

        private void RunFileCoping(FileSystemEventArgs e)
        {
            var copyPath = e.FullPath.ToLower().Replace(SourcePath, TargetPath);

            if (e is RenamedEventArgs)
            {
                RunRenamedFileCoping(e as RenamedEventArgs);
                return;
            }

            switch (e.ChangeType)
            {
                case WatcherChangeTypes.Changed:
                case WatcherChangeTypes.Created:
                    File.Copy(e.FullPath, copyPath, true);
                    break;
                case WatcherChangeTypes.Deleted:
                    File.Delete(copyPath);
                    break;
                default: throw new NotImplementedException("RunCoping - FileSystemEventArgs");
            }
        }

        private void RunRenamedFileCoping(RenamedEventArgs e)
        {
            if (e == null) throw new ArgumentNullException();

            var newCopyPath = e.FullPath.ToLower().Replace(SourcePath, TargetPath);
            var oldCopyPath = e.OldFullPath.ToLower().Replace(SourcePath, TargetPath);

            File.Copy(e.FullPath, newCopyPath, true);
            File.Delete(oldCopyPath);
        }
    }
}
