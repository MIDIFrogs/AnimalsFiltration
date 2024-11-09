// Copyright 2024 (c) IOExcept10n (contact https://github.com/IOExcept10n)
// Distributed under APGL v3.0 license. See LICENSE.md file in the project root for more information
using Newtonsoft.Json;
using System.IO;

namespace AnimaFiltering.Services
{
    public record class AppPreferences(int MinWidth, int MinHeight, bool PreferGPU)
    {
        private readonly string filePath = "Options.json";

        private AppPreferences(int minWidth, int minHeight, bool preferGPU, string path) : this(minWidth, minHeight, preferGPU)
        {
            filePath = path;
        }

        private AppPreferences(AppPreferences prototype, string path) : this(prototype)
        {
            filePath = path;
        }

        public void Save()
        {
            File.WriteAllText(filePath, JsonConvert.SerializeObject(this));
        }

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