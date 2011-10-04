using System;
using System.Collections.Generic;
using System.Text;

namespace DemoDropOut.Apps.Objects
{
    public class ColumnDetails
    {
        public double Mean = 0;
        public double StandardDeviation;
        private ColumnType m_column_type;
        private ColumnFormat m_column_format;
        private int encColumnInto;
        private IList<string> m_list_category = new List<string>();
        private ScalingRange m_scaling_range;
        private double m_db_minValue = double.MaxValue;
        private double m_db_maxValue = double.MinValue;
        // private int m_int_numeric_count = 0;
        private string m_str_colname;

        public string ColumnName
        {
            get { return m_str_colname; }
            set { m_str_colname = value; }
        }

        public int NumericCount
        {
            get
            {
                //return m_int_numeric_count;
                return m_list_category.Count;
            }
        }
        /// <summary>
        /// Cập nhật giá trị max - min
        /// </summary>
        public double MaxMinUpdateValue
        {
            set
            {
                if (value > m_db_maxValue)
                {
                    m_db_maxValue = value;
                }
                else if (value < m_db_minValue)
                {
                    m_db_minValue = value;
                }
                // Đếm số lượt cập nhật giá trị max - min
                // m_int_numeric_count++;
                var v_str_value = value.ToString();
                if (m_list_category.Contains(v_str_value) == false)
                {
                    m_list_category.Add(v_str_value);
                }
            }
        }
        /// <summary>
        /// Lấy thông tin giá trị Max nếu cột này mang dữ liệu kiểu numeric
        /// </summary>
        public double MaxValue
        {
            get
            {
                if (m_column_format != ColumnFormat.Numerical)
                    throw new Exception("Dữ liệu không thuộc kiểu số");
                return m_db_maxValue;
            }
        }

        public double MinValue
        {
            get
            {
                if (m_column_format != ColumnFormat.Numerical)
                    throw new Exception("Dữ liệu không thuộc kiểu số");
                return m_db_minValue;
            }
        }

        public IList<string> Categories
        {
            get { return m_list_category; }
            set { m_list_category = value; }
        }

        public ScalingRange ScalingRange
        {
            get
            {
                if (m_column_format != ColumnFormat.Numerical)
                {
                    throw new ArgumentException("Dữ liệu không thuộc kiểu số, không thể định khoảng tỉ lệ", "ScalingRange");
                }
                return m_scaling_range;
            }
            set { m_scaling_range = value; }
        }

        public double ScalingFactor
        {
            get
            {
                return (m_scaling_range.Upper - m_scaling_range.Lower) / (m_db_maxValue - m_db_minValue);
            }
        }

        public ColumnFormat Format
        {
            get { return m_column_format; }
            set
            {
                if (value == ColumnFormat.Numerical)
                {
                    m_scaling_range = new ScalingRange();
                }
                m_column_format = value;
            }
        }

        public ColumnType Type
        {
            get { return m_column_type; }
            set
            {
                if (value == ColumnType.Input)
                {
                    m_scaling_range = new ScalingRange(-1, 1);
                }
                else
                {
                    m_scaling_range = new ScalingRange(0, 1);
                }
                m_column_type = value;
            }
        }

        public int EncodedColumns
        {
            get
            {
                if (m_column_format == ColumnFormat.Numerical)
                {
                    return 1;
                }
                else if (m_column_format == ColumnFormat.Date || m_column_format == ColumnFormat.Time)
                {
                    return 2;
                }
                else if (m_column_format == ColumnFormat.Categorical)
                {
                    return encColumnInto;
                }
                return m_list_category.Count;
            }
            set
            {
                encColumnInto = value;
            }
        }

        public ColumnDetails()
        {
            m_column_type = ColumnType.Input;
            m_column_format = ColumnFormat.Numerical;
            encColumnInto = 1;
        }

        public ColumnDetails(ColumnType type, ColumnFormat format)
        {
            Type = type;
            Format = format;
        }

        public ColumnDetails(ColumnType type, ColumnFormat format, int encodeColNumber)
            : this(type, format)
        {
            encColumnInto = encodeColNumber;
        }

        public override string ToString()
        {
            return string.IsNullOrEmpty(m_str_colname) ? m_column_format.ToString() : m_str_colname;
        }
    }

    public enum ColumnFormat
    {
        Unknow,
        Numerical,
        Categorical,
        Date,
        Time
    }

    public enum ColumnType
    {
        Input,
        Ouput
    }

    public class ScalingRange
    {
        public double Lower;
        public double Upper;

        public ScalingRange()
        {
            Lower = -1;
            Upper = 1;
        }

        public ScalingRange(double lower, double upper)
        {
            Lower = lower;
            Upper = upper;
        }

        public override string ToString()
        {
            return string.Format("[{0}..{1}]", Lower, Upper);
        }
    }
}
