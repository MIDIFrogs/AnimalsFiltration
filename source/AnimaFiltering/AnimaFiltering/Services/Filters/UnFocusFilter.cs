using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using YoloDotNet.Models;
using OpenCvSharp;

namespace AnimaFiltering.Services.Filters
{
    internal class SizeFilter(AppPreferences preferences) : IPostProcessingFilter
    {
        public bool CheckDetection(ObjectDetection detection)
        {
            return detection.BoundingBox.Width >= preferences.MinWidth && detection.BoundingBox.Height >= preferences.MinHeight;
        }
        11                              11` 11  1   1                                                                                                                                                                               
        public class UnFocus()
        {
            Mat grad = new Mat();
            int scale = 1;
            int delta = 0;
            int ddepth = MatType.CV_8U;
            Mat grad_x = new Mat();
            Mat grad_y = new Mat();
            Mat abs_grad_x = new Mat();
            Mat abs_grad_y = new Mat();

            // Gradient X
            Cv2.Sobel(matFromSensor, grad_x, ddepth, 1, 0, 3, scale, delta, BorderTypes.Default);

            // Gradient Y
            Cv2.Sobel(matFromSensor, grad_y, ddepth, 0, 1, 3, scale, delta, BorderTypes.Default);

            // Convert to absolute scale
            Cv2.ConvertScaleAbs(grad_x, abs_grad_x);
            Cv2.ConvertScaleAbs(grad_y, abs_grad_y);

            // Combine the two gradients
            Cv2.AddWeighted(abs_grad_x, 0.5, abs_grad_y, 0.5, 0, grad);

            // Calculate mean and standard deviation
            Cv2.MeanStdDev(grad, out Scalar mu, out Scalar sigma);

            // Calculate focus measure
            double focusMeasure = mu[0] * mu[0];
        }
    }
}
