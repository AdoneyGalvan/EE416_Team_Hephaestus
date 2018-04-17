using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataPointViewModelServerWebsite
{
    public class DataPointFFTModel
    {
        public int DataPointGroupID { get; set; }
        public int DataPointUniqueID { get; set; }
        public DateTime DataPointDateTime { get; set; }
        public double DataPointX { get; set; }
        public double DataPointY { get; set; }
        public double DataPointZ { get; set; }
        public double DataPointFreq { get; set; }
    }
}
