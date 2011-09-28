using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DemoDropOut.Apps.Objects
{
    public class DataSamples
    {
        private DataTable m_dt_train;
        private DataTable m_dt_validation;
        private DataTable m_dt_test;

        /// <summary>
        /// Tập dữ liệu học (ideal)
        /// </summary>
        public DataTable TrainSample
        {
            get { return m_dt_train; }
            //set { m_dt_train = value; }
        }

        /// <summary>
        /// Tập dữ liệu chuẩn (ideal)
        /// </summary>
        public DataTable ValidationSample
        {
            get { return m_dt_validation; }
            //set { m_dt_validation = value; }
        }

        /// <summary>
        /// Tập dữ liệu kiểm tra (test)
        /// </summary>
        public DataTable TestSample
        {
            get { return m_dt_test; }
            //set { m_dt_test = value; }
        }

        public void RandomPartition(DataTable ip_table_samples)
        {
            throw new NotImplementedException("Chưa cài đặt chương trình");
        }

        public void SpecificOrderPartition(DataTable ip_table_samples)
        {
            throw new NotImplementedException("Chưa cài đặt chương trình");
        }
    }
}
