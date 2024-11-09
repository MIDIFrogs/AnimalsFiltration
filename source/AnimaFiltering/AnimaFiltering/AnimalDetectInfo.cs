using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoloDotNet.Models;

namespace AnimaFiltering
{
    public readonly record struct AnimalDetectInfo(string FileName, SKRectI Detection, SKSizeI ImageSize, bool IsGood);
}
