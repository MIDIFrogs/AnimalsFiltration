using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimaFiltering.Services
{
    internal class ReportBuilder
    {
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
