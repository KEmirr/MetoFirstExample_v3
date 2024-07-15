using System;
using System.Threading.Tasks;
using System.Configuration;
using System.Windows.Forms;
using MetoLibrary.BLL.Services;
using MetoLibrary.DAL.Repositorys;
using MetoLibrary.Utilities.FileWatcher;
using MetoLibrary.Utilities.Logger;
using System.IO;

namespace MetoFirstExample_v3
{
    public partial class Form1 : DevExpress.XtraEditors.XtraForm
    {
        private readonly ImageService _imageService;
        private readonly FileWatcher _fileWatcher;

        public Form1()
        {
            InitializeComponent();

            // Connection string'i yapılandırma dosyasından okuyun
            LogToUI("Reading connection string from configuration.");
            string connectionString = ConfigurationManager.ConnectionStrings["MyDatabase"].ConnectionString;
            LogToUI($"Connection string: {connectionString}");
            ImageRepository imageRepository = new ImageRepository(connectionString);
            _imageService = new ImageService(imageRepository);

            // FileWatcher'ı başlat
            LogToUI("Starting FileWatcher.");
            string pathToWatch = @"C:\Users\USER55\Desktop\Captured_Images2"; 
            _fileWatcher = new FileWatcher(pathToWatch, OnFileDetectedAsync);

            LogToUI("Application started.");
            _fileWatcher.Start();
        }

        private void LogToUI(string message)
        {
            Logger.Log(message); // Dosyaya yazma

            if (richTextBoxLogs.InvokeRequired)
            {
                richTextBoxLogs.Invoke(new Action(() =>
                {
                    richTextBoxLogs.AppendText($"{DateTime.Now}: {message}{Environment.NewLine}");
                }));
            }
            else
            {
                richTextBoxLogs.AppendText($"{DateTime.Now}: {message}{Environment.NewLine}");
            }
        }

        private async Task OnFileDetectedAsync(string filePath)
        {
            try
            {
                LogToUI($"File detected: {filePath}");
                bool isProcessed = await _imageService.ProcessImageAsync(filePath);
                if (isProcessed)
                {
                    LogToUI("Image processed and saved.");
                    // Dosyayı silme
                    File.Delete(filePath);
                    LogToUI($"File deleted: {filePath}");
                }
                else
                {
                    LogToUI("Failed to process image.");
                }
            }
            catch (Exception ex)
            {
                LogToUI($"Error during file processing: {ex.Message}");
                richTextBoxLogs.AppendText($"{DateTime.Now}: {ex.Message}\n{ex.StackTrace}{Environment.NewLine}");
            }
        }

    }
}
