//-----------------------------------------------------------------------
// <copyright file="FileStorageService.cs" company="Sully Studios">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Mud.Apps.Desktop.Windows.ServerApp
{
    using Mud.Data.Shared;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Windows 8 App Store specific file storage support.
    /// </summary>
    public class FileStorageService : IFileStorageService
    {
        /// <summary>
        /// Saves a collection of strings to a file, one line per item in the collection.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="content">The content.</param>
        /// <param name="appendToFile">if set to <c>true</c> the content will be appended to the file if it exists.</param>
        /// <returns>
        /// Returns true if the save was completed without any issues.
        /// </returns>
        public async Task SaveToFileAsync(string filename, IEnumerable<string> content)
        {
            string currentDirectory = "\{Environment.CurrentDirectory}\\\{filename}";
            using (var streamWriter = new StreamWriter(currentDirectory, false))
            {
                foreach (string line in content)
                {
                    await streamWriter.WriteLineAsync(line);
                }
            }
        }

        /// <summary>
        /// Saves a collection of strings to a file, one line per item in the collection.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="content">The content.</param>
        /// <param name="appendToFile">if set to <c>true</c> the content will be appended to the file if it exists.</param>
        /// <returns>
        /// Returns true if the save was completed without any issues.
        /// </returns>
        public async Task AppendToFileAsync(string filename, IEnumerable<string> content)
        {
            string currentDirectory = "\{Environment.CurrentDirectory}\\\{filename}";
            using (var streamWriter = new StreamWriter(currentDirectory, true))
            {
                foreach (string line in content)
                {
                    await streamWriter.WriteLineAsync(line);
                }
            }
        }

        /// <summary>
        /// Saves the value and its associated key to file.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="dataToSave">The data to save.</param>
        /// <returns>
        /// Returns true if the save was completed without any issues.
        /// </returns>
        public async Task SaveValueByKeyAsync(string filename, string key, string value)
        {
            string currentDirectory = "\{Environment.CurrentDirectory}\\\{filename}";
            if (File.Exists(currentDirectory))
            {
                // Delete the key if it already exists, then replace.
                IEnumerable<string> content = await this.LoadFileContentsAsync(filename);
                if (content.Any(l => l.StartsWith("\{key}=")))
                {
                    await this.DeleteKeyAsync(filename, key);
                }
            }
            using (var streamWriter = new StreamWriter(currentDirectory, true))
            {
                await streamWriter.WriteLineAsync("\{key}=\{value}");
            }
        }

        public async Task SaveMultipleValuesByKeyAsync(string filename, string key, string[] values)
        {
            string currentDirectory = "\{Environment.CurrentDirectory}\\\{filename}";
            using (var streamWriter = new StreamWriter(currentDirectory, true))
            {
                foreach (string value in values)
                {
                    await streamWriter.WriteLineAsync("\{key}=\{value}");
                }
            }
        }

        /// <summary>
        /// Loads a value associated with the key from the file.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="key">The key.</param>
        /// <returns>
        /// Returns the value associated with the key
        /// </returns>
        public async Task<string> LoadValueFromKeyAsync(string filename, string key)
        {
            string currentDirectory = "\{Environment.CurrentDirectory}\\\{filename}";
            if (File.Exists(currentDirectory))
            {
                // Delete the key if it already exists, then replace.
                IEnumerable<string> content = await this.LoadFileContentsAsync(filename);
                string value = content.FirstOrDefault(line => line.StartsWith("\{key}"));
                return value.Substring("\{key}=".Length);
            }
            else
            {
                return string.Empty;
            }
        }

        public async Task<IEnumerable<string>> LoadMultipleValuesFromKeyAsync(string filename, string key)
        {
            string currentDirectory = "\{Environment.CurrentDirectory}\\\{filename}";
            if (File.Exists(currentDirectory))
            {
                // Delete the key if it already exists, then replace.
                IEnumerable<string> content = await this.LoadFileContentsAsync(filename);
                IEnumerable<string> values = content
                    .Where(line => line.StartsWith("\{key}"))
                    .Select(line => line.Substring("\{key}=".Length));
                return values;
            }
            else
            {
                return new List<string>();
            }
        }

        /// <summary>
        /// Deletes the file.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns></returns>
        public async Task DeleteFileAsync(string filename)
        {
            string currentDirectory = "\{Environment.CurrentDirectory}\\\{filename}";
            if (File.Exists(currentDirectory))
            {
                // We run in a wrapped async operation in the event the file exists on a network share.
                await Task.Run(() => File.Delete(currentDirectory));
            }
        }

        /// <summary>
        /// Deletes a key and its value from the file.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public async Task DeleteKeyAsync(string filename, string key)
        {
            string currentDirectory = "\{Environment.CurrentDirectory}\\\{filename}";
            key = key.ToLower();

            List<string> content = new List<string>(await this.LoadFileContentsAsync(filename));
            content.RemoveAll(item => item.ToLower().StartsWith("\{key.ToLower()}="));

            // Once the line has been removed, save the remaining line.
            await this.SaveToFileAsync(filename, content);
        }

        /// <summary>
        /// Loads the file contents.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns></returns>
        private async Task<IEnumerable<string>> LoadFileContentsAsync(string filename)
        {
            string currentDirectory = "\{Environment.CurrentDirectory}\\\{filename}";
            var contents = new List<string>();
            string currentLine = string.Empty;

            using (var streamReader = new StreamReader(currentDirectory))
            {

                do
                {
                    currentLine = await streamReader.ReadLineAsync();

                    if (currentLine != null)
                    {
                        contents.Add(currentLine);
                    }
                } while (currentLine == null);
            }

            return contents;
        }

        public async Task<IEnumerable<string>> GetAllFilesByExtension(string extension, string path = "")
        {
            string currentDirectory = string.IsNullOrEmpty(path) ?
                "\{Environment.CurrentDirectory}" :
                "\{Environment.CurrentDirectory}\\\{path}";

            return await Task.Run(() => Directory.GetFiles(currentDirectory, extension, SearchOption.AllDirectories));
        }
    }
}
