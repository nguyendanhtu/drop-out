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
        private int m_custom_hidden_neurons;
        private int m_output_neurons;
        private bool m_bl_useCustomHiddenNeuro;
        /// <summary>
        /// Sử dụng tùy chỉnh số neuron lớp ẩn
        /// </summary>
        public bool UseCustomHiddenNeuro
        {
            get { return m_bl_useCustomHiddenNeuro; }
            set { m_bl_useCustomHiddenNeuro = value; }
        }
        /// <summary>
        /// Số biến xuất
        /// </summary>
        public int OutputNeurons
        {
            get { return m_output_neurons; }
        }
        /// <summary>
        /// Số neuron lớp ẩn
        /// </summary>
        public int HiddenNeurons
        {
            get { return m_bl_useCustomHiddenNeuro == true ? m_custom_hidden_neurons : GenerateHiddenNeurons(); }
            set { m_custom_hidden_neurons = value; }
        }
        /// <summary>
        /// Số biến nhập
        /// </summary>
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
            m_custom_hidden_neurons = hiddencount;
            m_output_neurons = outputcount;
            m_bl_useCustomHiddenNeuro = false;
            m_active_func = ActivationFunctionEnum.Logistic;
            m_output_func = ActivationFunctionEnum.Logistic;
        }

        public NetworkParameters(int inputCount, int hiddenCount, int outputCount, bool useCustomHidden)
            : this(inputCount, hiddenCount, outputCount)
        {
            m_bl_useCustomHiddenNeuro = useCustomHidden;
        }

        public int GenerateHiddenNeurons()
        {
            return (m_input_neurons >> 1) + 1;
        }
    }
}
