// Copyright 2024 (c) MIDIFrogs (contact https://github.com/MIDIFrogs)
// Distributed under AGPL v3.0 license. See LICENSE.md file in the project root for more information
using Avalonia.Logging;
using System;
using YoloDotNet;

namespace AnimaFiltering.Services
{
    /// <summary>
    /// Represents a service that provides YOLO model for animals detection.
    /// </summary>
    /// <param name="preferences">App preferences to use.</param>
    public class YoloProvider(AppPreferences preferences)
    {
        private const string ModelPath = "model.onnx";

        private readonly Lazy<Yolo> lazy = new(() =>
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
        });

        public Yolo Yolo => lazy.Value;
    }
}