using Microsoft.Win32;
using Ookii.Dialogs.Wpf;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Gat.Controls;
using System.Windows.Media.Imaging;
using static System.Net.WebRequestMethods;

namespace PackageAnalyzerDesktop
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void FileOrFolder_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, true))
            {
                string[] droppedFiles = e.Data.GetData(DataFormats.FileDrop, true) as string[];

                if (droppedFiles != null)
                {
                    List<string> fileNames = new List<string>();

                    foreach (string file in droppedFiles)
                    {
                        if (System.IO.File.Exists(file))
                        {
                            fileNames.Add(Path.GetFileName(file));
                        }
                        else if (Directory.Exists(file))
                        {
                            fileNames.Add(Path.GetFileName(file) + " (Folder)");
                        }
                    }
                    //AddFilesToListBox(fileNames);
                }
            }
        }

        private void UploadFileOrFolder_Click(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                var box = ShowCustomMessageBox("Select Files or Folder", "Do you want to select files or a folder?", "File (.zip)", "Folder");
                var result = box.Show();
                if (result == Gat.Controls.MessageBoxResult.Yes) // Select Files
                {
                    OpenFileDialog openFileDialog = new OpenFileDialog
                    {
                        Multiselect = true,
                        Title = "Select Files",
                        Filter = "Zip Files|*.zip|All Files|*.*",
                    };

                    if (openFileDialog.ShowDialog() == true)
                    {
                        //AddFilesToListBox(openFileDialog.FileNames);
                    }
                }
                else if (result == Gat.Controls.MessageBoxResult.No) // Select Folder
                {
                    VistaFolderBrowserDialog folderBrowserDialog = new VistaFolderBrowserDialog
                    {
                        Description = "Select Folder",
                    };

                    if (folderBrowserDialog.ShowDialog() == true)
                    {
                        string selectedFolder = folderBrowserDialog.SelectedPath;
                        //AddFilesToListBox(Directory.GetFiles(selectedFolder));
                    }
                }     
            }
        }

        //private void AddFilesToListBox(IEnumerable<string> files)
        //{
        //    List<string> currentFileNames = fileListBox.Items.Cast<string>().ToList();

        //    // Extract new file names from the provided files
        //    List<string> newFileNames = files.Select(file => Path.GetFileName(file)).ToList();

        //    // Merge existing and new file names
        //    List<string> mergedFileNames = currentFileNames.Union(newFileNames).ToList();

        //    // Bind the merged file names to the ListBox
        //    Binding binding = new Binding();
        //    binding.Source = mergedFileNames;
        //    fileListBox.SetBinding(ItemsControl.ItemsSourceProperty, binding);
        //}



        private MessageBoxViewModel ShowCustomMessageBox(string title, string message, string buttonContent1, string buttonContent2)
        {
            Gat.Controls.MessageBoxView messageBox = new Gat.Controls.MessageBoxView();
            Gat.Controls.MessageBoxViewModel vm = (Gat.Controls.MessageBoxViewModel)messageBox.FindResource("ViewModel");

            vm.Message = message;
            vm.Yes = buttonContent1;
            vm.No = buttonContent2;
            vm.OkVisibility = false;
            vm.CancelVisibility = false;
            vm.YesVisibility = true;
            vm.NoVisibility = true;
            vm.Caption = title;
            // Center functionality
            vm.Position = MessageBoxPosition.CenterOwner;
            vm.Owner = this;
            return vm;
        }

    }
}
