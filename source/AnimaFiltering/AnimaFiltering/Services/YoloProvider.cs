using Avalonia.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoloDotNet;

namespace AnimaFiltering.Services
{
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
