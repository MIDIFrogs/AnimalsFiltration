using Avalonia.Logging;
using SkiaSharp;
using System;
using YoloDotNet;
using YoloDotNet.Models;

namespace AnimaFiltering.Services.Filters
{
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
