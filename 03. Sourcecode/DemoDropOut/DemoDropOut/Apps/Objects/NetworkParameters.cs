using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemoDropOut.Apps.Objects
{
    public struct NetworkParameters
    {
        private ActivationFunctionEnum m_active_func;
        private ActivationFunctionEnum m_output_func;
        private int m_input_neurons;
        private int m_hidden_neurons;
        private int m_output_neurons;

        public int OutputNeurons
        {
            get { return m_output_neurons; }
        }

        public int HiddenNeurons
        {
            get { return m_hidden_neurons; }
            set { m_hidden_neurons = value; }
        }

        public int InputNeurons
        {
            get { return m_input_neurons; }
        }

        public ActivationFunctionEnum OutputFunction
        {
            get { return m_output_func; }
            set { m_output_func = value; }
        }

        public ActivationFunctionEnum ActivationFunction
        {
            get { return m_active_func; }
            set { m_active_func = value; }
        }

        public NetworkParameters(int inputcount, int hiddencount, int outputcount)
        {
            m_input_neurons = inputcount;
            m_hidden_neurons = hiddencount;
            m_output_neurons = outputcount;
            m_active_func = ActivationFunctionEnum.Logistic;
            m_output_func = ActivationFunctionEnum.Logistic;
        }

        public int GenerateHiddenNeurons()
        {
            return (m_input_neurons >> 1) + 1;
        }
    }
}
