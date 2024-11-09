using OpenCvSharp;
using SkiaSharp;
using YoloDotNet.Extensions;
using YoloDotNet.Models;

namespace AnimaFiltering.Services.Filters
{
    internal class UnFocus(AppPreferences preferences) : IPostProcessingFilter
    {
        public Mat ConvertSkiaSharpImageToMat(SKImage skImage, SKRectI clipRect)
        {
            // Step 1: Create a new SKImage from the clipped area
            using (var croppedImage = skImage.Subset(SKRectI.Intersect(skImage.Info.Rect, clipRect)))
            {
                // Step 2: Encode the cropped SKImage to byte array
                using (var data = croppedImage.Encode())
                {
                    // Step 3: Create a Mat from the byte array
                    using (var mat = Mat.FromImageData(data.Span))
                    {
                        return mat.Clone(); // Return a clone to avoid disposing issues
                    }
                }
            }
        }

        public bool CheckDetection(ObjectDetection detection, SKImage image)
        {
            using Mat t = ConvertSkiaSharpImageToMat(image, detection.BoundingBox);
            int scale = 1;
            using Mat grad = new Mat();
            int delta = 0;
            int ddepth = MatType.CV_8U;
            using Mat grad_x = new Mat();
            using Mat grad_y = new Mat();
            using Mat abs_grad_x = new Mat();
            using Mat abs_grad_y = new Mat();

            // Gradient X
            Cv2.Sobel(t, grad_x, ddepth, 1, 0, 3, scale, delta, BorderTypes.Default);

            // Gradient Y
            Cv2.Sobel(t, grad_y, ddepth, 0, 1, 3, scale, delta, BorderTypes.Default);

            // Convert to absolute scale
            Cv2.ConvertScaleAbs(grad_x, abs_grad_x);
            Cv2.ConvertScaleAbs(grad_y, abs_grad_y);

            // Combine the two gradients
            Cv2.AddWeighted(abs_grad_x, 0.5, abs_grad_y, 0.5, 0, grad);

            // Calculate mean and standard deviation
            Cv2.MeanStdDev(grad, out Scalar mu, out Scalar sigma);

            // Calculate focus measure
            double focusMeasure = mu[0] * mu[0];
            return focusMeasure > 70;
        }
    }
}
