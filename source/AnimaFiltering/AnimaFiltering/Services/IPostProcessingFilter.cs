// Copyright 2024 (c) MIDIFrogs (contact https://github.com/MIDIFrogs)
// Distributed under AGPL v3.0 license. See LICENSE.md file in the project root for more information
using SkiaSharp;
using YoloDotNet.Models;

namespace AnimaFiltering.Services
{
    /// <summary>
    /// Represents an interface for the postprocessing filter.
    /// </summary>
    public interface IPostProcessingFilter
    {
        /// <summary>
        /// Checks if the detection result is valid.
        /// </summary>
        /// <param name="detection">Detection instance to check.</param>
        /// <param name="image">Source image to validate.</param>
        /// <returns><see langword="true"/> if the detection is valid; otherwise <see langword="false"/>.</returns>
        bool CheckDetection(ObjectDetection detection, SKImage image);
    }
}