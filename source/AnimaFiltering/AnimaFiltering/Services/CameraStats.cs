using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimaFiltering.Services
{
    public class CameraStats
    {
        public string Name { get; set; }

        public int ProcessedImages { get; set; }

        public int GoodImages { get; set; }
    }
}
