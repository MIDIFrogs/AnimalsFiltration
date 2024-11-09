using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoloDotNet.Models;

namespace AnimaFiltering.Services.Filters
{
    internal class SizeFilter(AppPreferences preferences) : IPostProcessingFilter
    {
        public bool CheckDetection(ObjectDetection detection, SKImage image)
        {
            return detection.BoundingBox.Width >= preferences.MinWidth && detection.BoundingBox.Height >= preferences.MinHeight;
        }
    }
}
