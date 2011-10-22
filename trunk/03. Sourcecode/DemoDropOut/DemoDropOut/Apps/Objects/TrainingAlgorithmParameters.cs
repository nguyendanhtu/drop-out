using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemoDropOut.Apps.Objects
{
    public struct TrainingAlgorithmParameters
    {
        /// <summary>
        /// Thuật toán luyện mạng
        /// </summary>
        public TrainingAlgorithmEnum TrainingAlgorithm;
        /// <summary>
        /// Hệ số thuật toán lan truyền nhanh
        /// </summary>
        public double QuickPropagationCoefficient;
        /// <summary>
        /// Hệ số học
        /// </summary>
        public double LearningRate;
        /// <summary>
        /// Mô men học
        /// </summary>
        public double Momentum;
        /// <summary>
        /// Giới hạn lỗi
        /// </summary>
        public double ErrorLimit;
        /// <summary>
        /// Giới hạn lặp
        /// </summary>
        public int IterationsLimit;
        /// <summary>
        /// Sử dụng điều kiện dừng giới hạn vòng lặp
        /// </summary>
        public bool UseIterationsLimit
        {
            get;
            set;
        }

        public bool UseErrorLimit
        {
            get;
            set;
        }   
        /// <summary>
        /// Khoảng sinh số ngẫu nhiên
        /// </summary>
        public double RandomizationRange;
    }
}
