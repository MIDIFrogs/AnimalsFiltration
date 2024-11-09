// Copyright 2024 (c) MIDIFrogs (contact https://github.com/MIDIFrogs)
// Distributed under AGPL v3.0 license. See LICENSE.md file in the project root for more information
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AnimaFiltering.Services
{
    /// <summary>
    /// Represents a service that filters animal images.
    /// </summary>
    /// <param name="yolo">An instance of the YOLO model provider.</param>
    /// <param name="filters">Set of custom filters to test an image.</param>
    public class AnimalFilteringService(YoloProvider yolo, UserFilters filters)
    {
        public const string GoodClassName = "good";
        public const string BadClassName = "bad";

        /// <summary>
        /// Filters all images in given directory.
        /// </summary>
        /// <param name="datasetPath">Path to a directory to filter.</param>
        /// <returns>An enumerable of detection results with their classes.</returns>
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