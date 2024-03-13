using Microsoft.Win32;
using Ookii.Dialogs.Wpf;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using static System.Net.WebRequestMethods;
using PackageAnalyzer.Core;
using PackageAnalyzer.Core.Readers;
using System.Collections.ObjectModel;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Text.RegularExpressions;
using PackageAnalyzerDesktop.Core;

namespace PackageAnalyzerDesktop
{
    public partial class MainWindow : Window
    {
        ObservableCollection<SitecoreData> dataToShow;
        string tempFolder = string.Empty;
        public MainWindow()
        {
            dataToShow = new ObservableCollection<SitecoreData>();
            InitializeComponent();
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Clean up and remove the temporary folder
            CleanUpTemporaryFolder();
        }

        private void CleanUpTemporaryFolder()
        {
            string tempFolderPath = tempFolder;

            // Check if the temporary folder exists before attempting to delete it
            if (Directory.Exists(tempFolderPath))
            {
                try
                {
                    // Delete the temporary folder and its contents
                    Directory.Delete(tempFolderPath, true);
                }
                catch (Exception ex)
                {
                    // Handle any exceptions that may occur during the cleanup process
                    MessageBox.Show($"Error cleaning up temporary folder: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
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
                            fileNames.Add(file);
                        }
                        else if (Directory.Exists(file))
                        {
                            fileNames.Add(file + " (Folder)");
                        }
                    }
                    AddFilesToListBox(fileNames);
                    ProcessCheckboxes(droppedFiles[0]);
                    AddRowToDataGrid();
                }

            }
        }

        #region ProcessCheckboxes
        private void ProcessCheckboxes(string filePath)
        {
            string fileOrFolderPath = ArchiveHandler.Unarchive(filePath);

            if (fileOrFolderPath != filePath)
            {
                tempFolder = fileOrFolderPath;
            }

            foreach (var child in CheckBoxPanel.Children.OfType<CheckBox>().Where(c => c.IsChecked == true))
            {
                switch (child.Content.ToString())
                {
                    case "Sitecore (pre-release) version":
                        ProcessCheckbox("Sitecore (pre-release) version", fileOrFolderPath, PackageAnalyzerAdapter.GetSitecoreVersions);
                        break;

                    case "Sitecore roles from web.config":
                        ProcessCheckbox("Sitecore roles from web.config", fileOrFolderPath, PackageAnalyzerAdapter.GetSitecoreRoles);
                        break;

                    case "Installed modules":
                        // Process logic for "Installed modules" checkbox
                        // Add your logic here
                        break;

                    case "Hotfixes installed":
                        // Process logic for "Hotfixes installed" checkbox
                        // Add your logic here
                        break;

                    case "Assembly versions":
                        // Process logic for "Assembly versions" checkbox
                        // Add your logic here
                        break;

                    case "Key settings values":
                        // Process logic for "Key settings values" checkbox
                        // Add your logic here
                        break;

                    case "Topology (XM/XP)":
                        ProcessCheckboxWithExceptionHandling("Topology (XM/XP)", fileOrFolderPath, CheckTopology);
                        break;

                    // Add more cases for additional checkboxes if needed

                    default:
                        // Handle any other checkboxes not covered in cases
                        break;
                }
            }
            //return dataToShow;
        }

        private void ProcessCheckbox(string identifier, string filePath, Func<string, string> valueProvider)
        {
            SitecoreData data = new SitecoreData
            {
                Identifier = identifier,
                Value = valueProvider(filePath)
            };
            dataToShow.Add(data);
        }

        private void ProcessCheckboxWithExceptionHandling(string identifier, string filePath, Func<string, string> valueProvider)
        {
            try
            {
                ProcessCheckbox(identifier, filePath, valueProvider);
            }
            catch (Exception ex)
            {
                // Handle exception appropriately (log, display message, etc.)
                // You can customize this part based on your requirements
                throw;
            }
        }

        private string CheckTopology(string filePath)
        {
            TopologyChecker topologyChecker = new TopologyChecker();
            return topologyChecker.CheckTopology(filePath);
        }

        #endregion

        private void AddRowToDataGrid()
        {
            //SitecoreDataGrid.DataContext = dataToShow;
            SitecoreDataGrid.ItemsSource = dataToShow;
        }

        private void FileListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Your double-click event handling logic here
            // For example, open the selected file or perform an action on it
            if (fileListBox.SelectedItem != null)
            {
                string selectedFile = fileListBox.SelectedItem.ToString();
                // Add your logic to handle the double-click on the selected file
                MessageBox.Show($"Double-clicked on: {selectedFile}");
            }
        }

        private void AddFilesToListBox(IEnumerable<string> files)
        {
            List<string> currentFileNames = fileListBox.Items.Cast<string>().ToList();

            // Extract new file names from the provided files
            List<string> newFileNames = files.Select(file => Path.GetFileName(file)).ToList();

            // Merge existing and new file names
            List<string> mergedFileNames = currentFileNames.Union(newFileNames).ToList();

            // Bind the merged file names to the ListBox
            Binding binding = new Binding();
            binding.Source = mergedFileNames;
            fileListBox.SetBinding(ItemsControl.ItemsSourceProperty, binding);
        }

    }
}
