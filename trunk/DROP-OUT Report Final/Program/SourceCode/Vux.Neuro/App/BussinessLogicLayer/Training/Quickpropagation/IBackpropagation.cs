using System;
using System.Collections.Generic;

using System.Text;

namespace Vux.Neuro.App.BussinessLogicLayer.Training.Quickpropagation
{
    public interface IBackpropagation
    {
        /// <summary>
        /// Số neuro ẩn
        /// </summary>
        ushort HiddenCount
        {
            get;
        }
        /// <summary>
        /// Số biến nhập
        /// </summary>
        ushort InputCount
        {
            get;
        }
        /// <summary>
        /// Số biến xuất
        /// </summary>
        ushort OutputCount
        {
            get;
        }
        /// <summary>
        /// Số mẫu luyện
        /// </summary>
        ushort ExampleCount
        {
            get;
        }

        double[][] Input
        {
            get;
        }

        double[][] Ideal
        {
            get;
        }

        double RMSError
        {
            get;
        }

        double[] ComputeOutput(double[] input);
        double CalculateRMSError(double[][] input, double[][] ideal);

        void Learn();
    }
}
