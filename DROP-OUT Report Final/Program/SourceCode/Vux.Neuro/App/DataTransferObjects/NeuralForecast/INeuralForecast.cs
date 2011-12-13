using System;
using System.Collections.Generic;

using System.Text;

namespace Vux.Neuro.App.DataTransferObjects.NeuralForecast
{
    public interface INeuralForecast
    {
        double[] ComputeOutput(double[] ip_db_input);
        double[][] ComputeOutput(double[][] ip_db_input);
        double Error
        {
            get;
        }
    }
}
