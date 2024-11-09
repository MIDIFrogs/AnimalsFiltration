// Copyright 2024 (c) MIDIFrogs (contact https://github.com/MIDIFrogs)
// Distributed under AGPL v3.0 license. See LICENSE.md file in the project root for more information
using SkiaSharp;
using YoloDotNet.Models;

namespace AnimaFiltering.Services.Filters
{
    /// <summary>
    /// Filter that checks if the bounding box size is correct.
    /// </summary>
    /// <param name="preferences">App preferences for filtering.</param>
    internal class SizeFilter(AppPreferences preferences) : IPostProcessingFilter
    {
        public bool CheckDetection(ObjectDetection detection, SKImage image)
        {
            return detection.BoundingBox.Width >= preferences.MinWidth && detection.BoundingBox.Height >= preferences.MinHeight;
        }
    }
}