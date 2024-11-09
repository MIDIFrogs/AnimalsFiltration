// Copyright 2024 (c) MIDIFrogs (contact https://github.com/MIDIFrogs)
// Distributed under AGPL v3.0 license. See LICENSE.md file in the project root for more information
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AnimaFiltering.Services
{
    /// <summary>
    /// Represents a builder for the CSV reports.
    /// </summary>
    internal class ReportBuilder
    {
        /// <summary>
        /// Writes filtering results into CSV file.
        /// </summary>
        /// <param name="detections">Set of detections to log.</param>
        /// <param name="selectedCamera">A camera to save stats for.</param>
        /// <param name="path">Path to save CSV to.</param>
        /// <param name="progress">Progress handler to notify about image processing.</param>
        public async Task WriteToCsvAsync(IAsyncEnumerable<AnimalDetectInfo> detections, CameraStats selectedCamera, string path, IProgress<int> progress)
        {
            using var writer = new StreamWriter(path);
            writer.WriteLine("Name,BBox,Class");
            int i = 0;
            await foreach (var detect in detections)
            {
                selectedCamera.ProcessedImages++;
                if (detect.IsGood)
                    selectedCamera.GoodImages++;
                double normalizedBBoxXC = (detect.Detection.Left + detect.Detection.Right) / (2.0 * detect.ImageSize.Width),
                       normalizedBBoxYC = (detect.Detection.Top + detect.Detection.Bottom) / (2.0 * detect.ImageSize.Height),
                       normalizedBBoxWidth = detect.Detection.Width / (double)detect.ImageSize.Width,
                       normalizedBBoxHeight = detect.Detection.Height / (double)detect.ImageSize.Height;
                writer.WriteLine($"{detect.FileName},\"{normalizedBBoxXC},{normalizedBBoxYC},{normalizedBBoxWidth},{normalizedBBoxHeight}\",{Convert.ToInt32(detect.IsGood)}");
                progress.Report(i++);
            }
        }
    }
}