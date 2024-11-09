using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            File.WriteAllText(filePath, JsonConvert.SerializeObject(filePath));
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
