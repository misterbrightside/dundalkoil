using Microsoft.Win32;
using System;
using System.Windows;
using System.IO;

namespace DundalkOil
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SelectDirectoryDataFolder(object sender, RoutedEventArgs e)
        {
            /* FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
             DialogResult folder = folderBrowserDialog.ShowDialog();
             if (!String.IsNullOrWhiteSpace(folderBrowserDialog.SelectedPath))
             {
                 dataFolderName.Text = folderBrowserDialog.SelectedPath;

             }*/
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "Excel |*.xlsx";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (openFileDialog.ShowDialog() == true)
            {
                string[] files = openFileDialog.FileNames;
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
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
