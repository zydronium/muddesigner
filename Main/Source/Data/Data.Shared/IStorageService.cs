//-----------------------------------------------------------------------
// <copyright file="IStorageRepository.cs" company="Sully Studios">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Mud.Data.Shared
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Mud.Engine.Runtime.Services;


    /// <summary>
    /// Represents a storage service that can be used to save, load and delete data from files.
    /// </summary>
    public interface IFileStorageService : IService
    {
        /// <summary>
        /// Saves a collection of strings to a file, one line per item in the collection.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="content">The content.</param>
        /// <param name="appendToFile">if set to <c>true</c> the content will be appended to the file if it exists.</param>
        /// <returns>Returns true if the save was completed without any issues.</returns>
        Task SaveToFileAsync(string filename, IEnumerable<string> content);

        /// <summary>
        /// Saves a collection of strings to a file, one line per item in the collection.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="content">The content.</param>
        /// <param name="appendToFile">if set to <c>true</c> the content will be appended to the file if it exists.</param>
        /// <returns>Returns true if the save was completed without any issues.</returns>
        Task AppendToFileAsync(string filename, IEnumerable<string> content);

        /// <summary>
        /// Saves the value and its associated key to file.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="dataToSave">The data to save.</param>
        /// <returns>Returns true if the save was completed without any issues.</returns>
        Task SaveValueByKeyAsync(string filename, string key, string value);

        /// <summary>
        /// Saves multiple values sharing the same key to file.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="dataToSave">The data to save.</param>
        /// <returns>Returns true if the save was completed without any issues.</returns>
        Task SaveMultipleValuesByKeyAsync(string filename, string key, string[] values);

        /// <summary>
        /// Loads a value associated with the key from the file.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="key">The key.</param>
        /// <returns>Returns the value associated with the key</returns>
        Task<string> LoadValueFromKeyAsync(string filename, string key);

        /// <summary>
        /// Loads all values that share the same key from the file.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="key">The key.</param>
        /// <returns>Returns the value associated with the key</returns>
        Task<IEnumerable<string>> LoadMultipleValuesFromKeyAsync(string filename, string key);

        /// <summary>
        /// Deletes the file.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns></returns>
        Task DeleteFileAsync(string filename);

        /// <summary>
        /// Deletes a key and its value from the file.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        Task DeleteKeyAsync(string filename, string key);

        Task<IEnumerable<string>> GetAllFilesByExtension(string extension, string path = "");
    }
}
