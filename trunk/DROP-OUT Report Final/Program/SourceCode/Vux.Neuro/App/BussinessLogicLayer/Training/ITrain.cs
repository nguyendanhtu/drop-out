using System;
using System.Collections.Generic;

using System.Text;
using Vux.Neuro.App.DataTransferObjects.Feedforward;

namespace Vux.Neuro.App.BussinessLogicLayer.Training
{
    public interface ITrain
    {
        /// <summary>
        /// Get the current best network from the training.
        /// </summary>
        FeedforwardNetwork Network
        {
            get;
        }

        /// <summary>
        /// Get the current error percent from the training.
        /// </summary>
        double Error
        {
            get;
        }

        /// <summary>
        /// Perform one iteration of training.
        /// </summary>
        void Iteration();
    }
}
