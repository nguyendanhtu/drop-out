using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Vux.Neuro.App.BussinessLogicLayer.Training.Quickpropagation;
using System.IO;
using System.Data.SqlClient;

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
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new F001_MainProgram());
        }
        #region Test Neunet

        //private static void TestQuick()
        //{
        //    var v_xor_input = new double[][]
        //    {
        //          new double[2]{0.0, 0.0},
        //          new double[2]{1.0, 0.0},
        //          new double[2]{0.0, 1.0},
        //          new double[2]{1.0, 1.0}
        //     };
        //    var v_xor_ideal = new double[][]
        //    {
        //        new double[1]{0.0},
        //        new double[1]{1.0},
        //        new double[1]{1.0},
        //        new double[1]{0.0}
        //    };

        //    var command = string.Empty;
        //    do
        //    {
        //        Console.Write("So lan lap luyen mang: ");
        //        var echpo = int.Parse(Console.ReadLine());

        //        var v_network = new Quickpropagation(2, v_xor_input, v_xor_ideal);
        //        v_network.Initialize(); // khởi tạo thế hệ 1
        //        for (int i = 0; i < echpo; i++)
        //        {
        //            v_network.Learn();
        //        }
        //        Console.WriteLine("RMS Error: " + v_network.RMSError);
        //        Console.WriteLine("Test Value");
        //        for (int i = 0; i < v_xor_input.Length; i++)
        //        {
        //            var test = v_network.ComputeOutput(v_xor_input[i]);
        //            Console.WriteLine(test[0]);
        //        }
        //        Console.Write("Nhan 'c' de tiep tuc!");
        //        command = Console.ReadLine();
        //    } while (command.Equals("c") == true);
        //}

        //private static void TestBP()
        //{
        //    var v_xor_input = new double[][]
        //    {
        //          new double[2]{0.0, 0.0},
        //          new double[2]{1.0, 0.0},
        //          new double[2]{0.0, 1.0},
        //          new double[2]{1.0, 1.0}
        //     };
        //    var v_xor_ideal = new double[][]
        //    {
        //        new double[1]{0.0},
        //        new double[1]{1.0},
        //        new double[1]{1.0},
        //        new double[1]{0.0}
        //    };

        //    var command = string.Empty;
        //    do
        //    {
        //        Console.Write("So lan lap luyen mang: ");
        //        var echpo = int.Parse(Console.ReadLine());

        //        var v_network = new Backpropagation(2, v_xor_input, v_xor_ideal);
        //        for (int i = 0; i < echpo; i++)
        //        {
        //            v_network.Learn();
        //        }
        //        Console.WriteLine("RMS Error: " + v_network.RMSError);
        //        Console.WriteLine("Test Value");
        //        for (int i = 0; i < v_xor_input.Length; i++)
        //        {
        //            var test = v_network.ComputeOutput(v_xor_input[i]);
        //            Console.WriteLine(test[0]);
        //        }
        //        Console.Write("Nhan 'c' de tiep tuc!");
        //        command = Console.ReadLine();
        //    } while (command.Equals("c") == true);
        //}
        #endregion

    }
}
