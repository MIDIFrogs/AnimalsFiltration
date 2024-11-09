// Copyright 2024 (c) MIDIFrogs (contact https://github.com/MIDIFrogs)
// Distributed under AGPL v3.0 license. See LICENSE.md file in the project root for more information
using Newtonsoft.Json;
using System.IO;

namespace AnimaFiltering.Services
{
    /// <summary>
    /// Represents preferences for the application.
    /// </summary>
    public record class AppPreferences
    {
        private readonly string filePath = "Options.json";

        /// <summary>
        /// Minimal detection width to check.
        /// </summary>
        public int MinWidth { get; set; }

        /// <summary>
        /// Minimal detection height to check.
        /// </summary>
        public int MinHeight { get; set; }

        /// <summary>
        /// Determines whether the GPU with CUDA is preferred (CudNN is required).
        /// </summary>
        public bool PreferGPU { get; set; }

        public AppPreferences()
        {
        }

        private AppPreferences(int minWidth, int minHeight, bool preferGPU, string path)
        {
            MinWidth = minWidth;
            MinHeight = minHeight;
            PreferGPU = preferGPU;
            filePath = path;
        }

        private AppPreferences(AppPreferences prototype, string path) : this(prototype)
        {
            filePath = path;
        }

        /// <summary>
        /// Saves app settings.
        /// </summary>
        public void Save()
        {
            File.WriteAllText(filePath, JsonConvert.SerializeObject(this));
        }

        /// <summary>
        /// Loads app settings or initializes new ones.
        /// </summary>
        /// <param name="filePath">File path to save.</param>
        /// <returns>An instance of the <see cref="AppPreferences"/>.</returns>
        public static AppPreferences LoadOrCreate(string filePath)
        {
            if (File.Exists(filePath))
            {
                return new(JsonConvert.DeserializeObject<AppPreferences>(File.ReadAllText(filePath))!, filePath);
            }
            return new(128, 128, false, filePath);
        }
    }
}