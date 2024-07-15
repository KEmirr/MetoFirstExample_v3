using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace MetoLibrary.Utilities.FileWatcher
{
    public class FileWatcher
    {
        private readonly FileSystemWatcher _watcher;
        private readonly Func<string, Task> _onFileDetected;

        public FileWatcher(string path, Func<string, Task> onFileDetected)
        {
            _watcher = new FileSystemWatcher(path)
            {
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite,
                Filter = "*.*"
            };
            _onFileDetected = onFileDetected;
            _watcher.Created += OnCreated;
        }

        public void Start()
        {
            _watcher.EnableRaisingEvents = true;
        }

        public void Stop()
        {
            _watcher.EnableRaisingEvents = false;
        }

        private async void OnCreated(object sender, FileSystemEventArgs e)
        {
            if (Path.GetExtension(e.FullPath).ToLower() == ".png" || Path.GetExtension(e.FullPath).ToLower() == ".jpg" || Path.GetExtension(e.FullPath).ToLower() == ".bmp")
            {
                await _onFileDetected(e.FullPath);
            }
        }

    }
}
