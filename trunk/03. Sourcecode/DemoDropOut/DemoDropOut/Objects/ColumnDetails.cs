using System;
using System.Collections.Generic;
using System.Text;

namespace DemoDropOut.Objects
{
    public class ColumnDetails
    {
        public ColumnType Type;
        public ColumnFormat Format;
        public int EncodedColumnInto;
        public double MinValue = double.MaxValue;
        public double MaxValue = double.MinValue;

        public ColumnDetails()
        {
            Type = ColumnType.Input;
            Format = ColumnFormat.Numerical;
            EncodedColumnInto = 1;
        }

        public ColumnDetails(ColumnType type, ColumnFormat format)
        {
            Type = type;
            Format = format;
        }

        public ColumnDetails(ColumnType type, ColumnFormat format, int encodeColNumber)
            : this(type, format)
        {
            EncodedColumnInto = encodeColNumber;
        }
    }

    public enum ColumnFormat
    {
        Numerical,
        Categorical,
        Date
    }

    public enum ColumnType
    {
        Input,
        Ouput
    }
}
