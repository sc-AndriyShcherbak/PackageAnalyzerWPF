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


        private void ProcessCheckboxes(string filePath)
        {
            string fileOrFolderPath = ArchiveHandler.Unarchive(filePath);
            if (fileOrFolderPath != filePath)
            {
                tempFolder = fileOrFolderPath;
            }
            tempFolder = fileOrFolderPath;
            foreach (var child in CheckBoxPanel.Children)
            {
                if (child is CheckBox checkbox)
                {
                    switch (checkbox.Content.ToString())
                    {
                        case "Sitecore (pre-release) version":
                            // Process logic for "Sitecore (pre-release) version" checkbox
                            if (checkbox.IsChecked == true)
                            {
                                SitecoreData versions = new SitecoreData();
                                versions.Identifier = checkbox.Content.ToString();
                                versions.Value = PackageAnalyzerAdapter.GetSitecoreVersions(fileOrFolderPath);
                                dataToShow.Add(versions);
                            }
                            break;

                        case "Sitecore roles from web.config":
                            // Process logic for "Sitecore roles from web.config" checkbox
                            if (checkbox.IsChecked == true)
                            {
                                SitecoreData roles = new SitecoreData();
                                roles.Identifier = checkbox.Content.ToString();
                                roles.Value = PackageAnalyzerAdapter.GetSitecoreRoles(fileOrFolderPath);
                                dataToShow.Add(roles);
                            }
                            break;

                        case "Installed modules":
                            // Process logic for "Installed modules" checkbox
                            if (checkbox.IsChecked == true)
                            {
                                // Checkbox is checked, perform corresponding action
                                // Add your logic here
                            }
                            break;

                        case "Hotfixes installed":
                            // Process logic for "Hotfixes installed" checkbox
                            if (checkbox.IsChecked == true)
                            {
                                // Checkbox is checked, perform corresponding action
                                // Add your logic here
                            }
                            break;

                        case "Assembly versions":
                            // Process logic for "Assembly versions" checkbox
                            if (checkbox.IsChecked == true)
                            {
                                // Checkbox is checked, perform corresponding action
                                // Add your logic here
                            }
                            break;

                        case "Key settings values":
                            // Process logic for "Key settings values" checkbox
                            if (checkbox.IsChecked == true)
                            {
                                // Checkbox is checked, perform corresponding action
                                // Add your logic here
                            }
                            break;

                        case "Topology (XM/XP)":
                            // Process logic for "Topology (XM/XP)" checkbox
                            if (checkbox.IsChecked == true)
                            {
                                // Checkbox is checked, perform corresponding action
                                // Add your logic here
                            }
                            break;

                        // Add more cases for additional checkboxes if needed

                        default:
                            // Handle any other checkboxes not covered in cases
                            break;
                    }
                }
            }
            //return dataToShow;
        }



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
