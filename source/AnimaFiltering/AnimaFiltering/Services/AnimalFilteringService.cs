using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoloDotNet;

namespace AnimaFiltering.Services
{
    public class AnimalFilteringService(Yolo yolo, UserFilters filters)
    {
        public const string GoodClassName = "good";
        public const string BadClassName = "bad";

        public async IAsyncEnumerable<AnimalDetectInfo> FilterAnimalsAsync(string datasetPath)
        {
            foreach (var file in Directory.EnumerateFiles(datasetPath))
            {
                var image = SKImage.FromBitmap(SKBitmap.Decode(file));
                var result = await Task.Run(() => yolo.RunObjectDetection(image));
                foreach (var animal in result)
                {
                    yield return new(Path.GetFileName(file), animal.BoundingBox, animal.Label.Name == BadClassName || filters.Any(x => !x.CheckDetection(animal)));
                }
            }
        }
    }
}
