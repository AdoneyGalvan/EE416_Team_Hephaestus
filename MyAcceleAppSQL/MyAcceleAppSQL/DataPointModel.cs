using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyAcceleAppSQL
{
    public class DataPointModel
    {
        public int DataPointID { get; set; }
        public DateTime DataPointDateTime { get; set; }
        public double DataPointX { get; set; }
        public double DataPointY { get; set; }
        public double DataPointZ { get; set; }
    }
}
