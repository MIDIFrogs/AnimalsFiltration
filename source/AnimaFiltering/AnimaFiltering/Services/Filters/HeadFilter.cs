// Copyright 2024 (c) MIDIFrogs (contact https://github.com/MIDIFrogs)
// Distributed under AGPL v3.0 license. See LICENSE.md file in the project root for more information
using Avalonia.Logging;
using SkiaSharp;
using System;
using YoloDotNet;
using YoloDotNet.Models;

namespace AnimaFiltering.Services.Filters
{
    /// <summary>
    /// Represents a filter that checks if animal's head is inside the bounding box.
    /// </summary>
    /// <param name="preferences"></param>
    internal class HeadFilter(AppPreferences preferences) : IPostProcessingFilter
    {
        public const string ModelPath = "head.onnx";

        private readonly Yolo yolo = InitYolo(preferences);
        private readonly AppPreferences preferences = preferences;

        public bool CheckDetection(ObjectDetection detection, SKImage image)
        {
            var results = yolo.RunObjectDetection(image.Subset(SKRectI.Intersect(image.Info.Rect, detection.BoundingBox)), iou: 0.5);
            // If there's one head and its size is more than 1/8 of preferences, image is good.
            return results.Count == 1 && results[0].BoundingBox.Width > preferences.MinWidth / 8 && results[0].BoundingBox.Height > preferences.MinHeight / 8;
        }

        /// <summary>
        /// Initializes YOLO model for head detection.
        /// </summary>
        /// <param name="preferences">App preferences to initialize.</param>
        /// <returns>YOLO model that finds heads in image BBoxes.</returns>
        private static Yolo InitYolo(AppPreferences preferences)
        {
            if (preferences.PreferGPU)
            {
                try
                {
                    return new Yolo(new()
                    {
                        Cuda = true,
                        ModelType = YoloDotNet.Enums.ModelType.ObjectDetection,
                        OnnxModel = ModelPath,
                        PrimeGpu = true,
                    });
                }
                catch (Exception ex)
                {
                    Logger.TryGet(LogEventLevel.Error, "CV")?.Log(null, "Couldn't load model with prime GPU. Exception details: {ex}", ex.Message);
                }
            }
            return new Yolo(new()
            {
                Cuda = false,
                ModelType = YoloDotNet.Enums.ModelType.ObjectDetection,
                OnnxModel = ModelPath,
                PrimeGpu = false,
            });
        }
    }
}