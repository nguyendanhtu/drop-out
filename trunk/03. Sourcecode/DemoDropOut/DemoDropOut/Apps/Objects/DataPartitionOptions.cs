using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemoDropOut.Apps.Objects
{
    public struct DataPartitionOptions
    {
        public double TrainPcent;
        public double ValidPcent;
        public double TestPcent;

        public int Total;

        public bool IsOne()
        {
            return (TrainPcent + ValidPcent + TestPcent) == 1;
        }

        public int GetTrainCount()
        {
            return (int)Math.Floor(TrainPcent * Total);
        }

        public int GetValidCount()
        {
            return (int)Math.Floor(ValidPcent * Total);
        }

        public int GetTestCount()
        {
            return Total - GetTrainCount() - GetValidCount();
        }
    }
}
