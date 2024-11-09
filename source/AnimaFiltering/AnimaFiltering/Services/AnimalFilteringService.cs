using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoloDotNet;

namespace AnimaFiltering.Services
{
    public class AnimalFilteringService(YoloProvider yolo, UserFilters filters)
    {
        public const string GoodClassName = "good";
        public const string BadClassName = "bad";

        public async IAsyncEnumerable<AnimalDetectInfo> FilterAnimalsAsync(string datasetPath)
        {
            var files = Directory.EnumerateFiles(datasetPath, "*.jpg");
#if DEBUG
            int count = files.Count();
            int i = 0;
#endif
            foreach (var file in files)
            {
                using var bitmap = SKBitmap.Decode(file);
                using var image = SKImage.FromBitmap(bitmap);
                var result = await Task.Run(() => yolo.Yolo.RunObjectDetection(image, iou: 0.5));
#if DEBUG
                if (++i % 20 == 0)
                   Debug.WriteLine($"Processing image {i}/{count}...");
#endif
                foreach (var animal in result)
                {
                    yield return new(Path.GetFileName(file), animal.BoundingBox, image.Info.Size, animal.Label.Name == BadClassName || filters.Any(x => !x.CheckDetection(animal, image)));
                }
            }
#if DEBUG
            Debug.WriteLine("Processing completed.");
#endif
        }
    }
}
