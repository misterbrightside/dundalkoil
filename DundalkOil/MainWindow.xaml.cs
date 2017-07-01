using Microsoft.Win32;
using System;
using System.Windows;
using System.IO;

namespace DundalkOil
{
    public partial class MainWindow : Window
    {
        private string url;
        private string[] files;
        private string skipFilePath;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void SelectDirectoryDataFolder(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "Excel |*.xlsx";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (openFileDialog.ShowDialog() == true)
            {
                this.files = openFileDialog.FileNames;
                dataFolderName.Text = Path.GetDirectoryName(files[0]);
            }
        }

        private void SelectSkipFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (openFileDialog.ShowDialog() == true)
            {
                skipListFileName.Text = openFileDialog.FileName;
                this.skipFilePath = openFileDialog.FileName;
            }
        }

        private void UploadFiles(object sender, RoutedEventArgs e)
        {
            Uploader uploader = new Uploader(this.url, this.skipFilePath, this.files);
            uploader.CleanUp();
        }

        private void SetURL(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            this.url = urlUploadLink.Text;
        }
    }
}
