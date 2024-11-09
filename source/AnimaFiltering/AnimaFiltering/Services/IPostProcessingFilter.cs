using YoloDotNet.Models;

namespace AnimaFiltering.Services
{
    public interface IPostProcessingFilter
    {
        bool CheckDetection(ObjectDetection detection);
    }
}