using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataPointViewModelHardware
{
    public class DataPointRMSModel
    {
        public int DataPointUniqueID { get; set; }
        public int DataPointGroupID { get; set; }
        public DateTime DataPointDateTime { get; set; }
        public double DataPointXRMS { get; set; }
        public double DataPointYRMS { get; set; }
        public double DataPointZRMS { get; set; }
    }
}
