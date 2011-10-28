﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vux.Neuro.App.DataTransferObjects.Exception
{
    /// <summary>
    /// Thrown when a neural network error occurs.
    /// </summary>
    public class NeuralNetworkError : System.Exception
    {
        /// <summary>
        /// Construct a message exception.
        /// </summary>
        /// <param name="str">The message.</param>
        public NeuralNetworkError(String str)
            : base(str)
        {
        }
    }
}
