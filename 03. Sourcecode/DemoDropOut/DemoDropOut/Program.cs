using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DemoDropOut
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //var test_index = new double[1][];
            //test_index[0] = new double[] { 1, 1, 0 };
            //var v_processing = new DemoDropOut.Apps.BussinessLogicLayer.DataPreprocessingBlo();
            //var v_test_column = new DemoDropOut.Apps.Objects.ColumnDetails();
            //v_processing.DecodeCategoricalColumnByBinary(test_index, v_test_column);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new F001_MainProgram());
        }
    }
}
