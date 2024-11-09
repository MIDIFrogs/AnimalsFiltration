// Copyright 2024 (c) MIDIFrogs (contact https://github.com/MIDIFrogs)
// Distributed under AGPL v3.0 license. See LICENSE.md file in the project root for more information
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace AnimaFiltering.Services
{
    /// <summary>
    /// Represents a collection of camera stats.
    /// </summary>
    public class CameraManager : ObservableCollection<CameraStats>
    {
        private readonly string filePath;

        private CameraManager(IEnumerable<CameraStats> items, string path)
        {
            filePath = path;
            foreach (var item in items)
            {
                Add(item);
            }
        }

        public CameraManager()
        {
        }

        public CameraManager(string path)
        {
            filePath = path;
        }

        /// <summary>
        /// Loads camera stats of initializes a new ones.
        /// </summary>
        /// <param name="filePath">Path to a file with camera stats.</param>
        /// <returns>An instance of the <see cref="CameraManager"/>.</returns>
        public static CameraManager LoadOrCreate(string filePath)
        {
            if (File.Exists(filePath))
            {
                return new(JsonConvert.DeserializeObject<CameraManager>(File.ReadAllText(filePath))!, filePath);
            }
            // HACK
            return new(filePath)
            {
                new(){ Name = "Камера 1"},
                new(){ Name = "Камера 2"},
                new(){ Name = "Камера 3"},
            };
        }

        public void Save()
        {
            File.WriteAllText(filePath, JsonConvert.SerializeObject(this));
        }
    }
}