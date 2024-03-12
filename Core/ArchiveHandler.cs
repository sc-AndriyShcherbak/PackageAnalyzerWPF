using System;
using System.IO;
using System.IO.Compression;
using SevenZip;

namespace PackageAnalyzerDesktop.Core
{
    class ArchiveHandler
    {
        public static string Unarchive(string filePath)
        {
            // Check if the path is a file or folder
            bool isFile = File.Exists(filePath);
            bool isFolder = Directory.Exists(filePath);

            if (!isFile && !isFolder)
            {
                throw new ArgumentException("Invalid file or folder path.");
            }
            if (isFolder) 
            {
                return filePath;
            }

            string destinationFolder = Path.Combine(Path.GetDirectoryName(filePath), GenerateUniqueFolderName());

            // Create the destination folder
            Directory.CreateDirectory(destinationFolder);

            if (isFile)
            {
                // Check if it's a supported archive format
                string extension = Path.GetExtension(filePath).ToLower();

                if (extension == ".zip" || extension == ".rar")
                {
                    // Unarchive the file using the System.IO.Compression namespace
                    ZipFile.ExtractToDirectory(filePath, destinationFolder);
                }
                else if (extension == ".7z")
                {
                    // Unarchive the file using the SevenZipSharp library
                    using (var extractor = new SevenZipExtractor(filePath))
                    {
                        extractor.ExtractArchive(destinationFolder);
                    }
                }
                else
                {
                    throw new ArgumentException("Unsupported archive format.");
                }
            }

            return destinationFolder;
        }

        private static string GenerateUniqueFolderName()
        {
            // Generate a unique and short folder name (you can customize the logic as needed)
            return Guid.NewGuid().ToString("N").Substring(0, 8);
        }
    }
}
