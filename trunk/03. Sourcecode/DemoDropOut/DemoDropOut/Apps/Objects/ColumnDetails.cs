using System;
using System.Collections.Generic;
using System.Text;

namespace DemoDropOut.Apps.Objects
{
    public class ColumnDetails
    {
        public ColumnType Type;
        public ColumnFormat Format;
        public double MinValue = double.MaxValue;
        public double MaxValue = double.MinValue;
        private int encColumnInto;

        public int EncodedCoulumnInto
        {
            get
            {
                if (Format == ColumnFormat.Numerical)
                {
                    return 1;
                }
                else if (Format == ColumnFormat.Date)
                {
                    return 2;
                }
                else// if (Format == ColumnFormat.Categorical)
                {
                    return encColumnInto;
                }
            }
            set
            {
                encColumnInto = value;
            }
        }

        public ColumnDetails()
        {
            Type = ColumnType.Input;
            Format = ColumnFormat.Numerical;
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
    }

    public enum ColumnFormat
    {
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
}
