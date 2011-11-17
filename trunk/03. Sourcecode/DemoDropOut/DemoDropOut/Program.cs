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
            //var test_index = new double[1][];
            //test_index[0] = new double[] { 1, 1, 0 };
            //var v_processing = new DemoDropOut.Apps.BussinessLogicLayer.DataPreprocessingBlo();
            //var v_test_column = new DemoDropOut.Apps.Objects.ColumnDetails();
            //v_processing.DecodeCategoricalColumnByBinary(test_index, v_test_column);
            //TestQuick();
            //return;

            //var fstream = new FileStream("login.sql", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            //var reader = new StreamReader(fstream);
            //var writer = new StreamWriter("login.csv");
            //var line = string.Empty;
            //var startIndex = "INSERT INTO `` VALUES (".Length;
            //var i = 0;
            //while ((line = reader.ReadLine()) != null)
            //{
            //    var newLine = line.Substring(startIndex);
            //    var ok = newLine.Split(new char[] { '\'',' ', ',', ')', ';' }, StringSplitOptions.RemoveEmptyEntries);
            //    newLine = string.Format("{0},{1},{2} {3}", ok[0], ok[1], ok[2], ok[3]);
            //    writer.WriteLine(newLine);
            //    i++;
            //}
            //reader.Close();
            //writer.Close();

            //var v_sql_conn = new SqlConnection(@"server=localhost\SQLEXPRESS;user id=sa;Password=sa;database=DM_HV_DROPOUT");
            //var v_sql_command = new SqlCommand();
            //v_sql_command.CommandType = System.Data.CommandType.Text;
            //v_sql_command.Connection = v_sql_conn;
            //v_sql_command.Connection.Open();

            //var fstream = new FileStream("login.csv", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            //var reader = new StreamReader(fstream);
            //var line = string.Empty;
            //var i = 0;
            //while ((line = reader.ReadLine()) != null)
            //{
            //    var ok = line.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            //    var query = string.Format("insert into DM_USER_LOGIN VALUES('{0}','{1}','{2}')", ok[0], ok[1], ok[2]);
            //    v_sql_command.CommandText = query;
            //    var abc = v_sql_command.ExecuteNonQuery();
            //    i++;
            //}
            //reader.Close();
            //v_sql_command.Connection.Close();

            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new F001_MainProgram());
        }

        private static void TestQuick()
        {
            var v_xor_input = new double[][]
            {
                  new double[2]{0.0, 0.0},
                  new double[2]{1.0, 0.0},
                  new double[2]{0.0, 1.0},
                  new double[2]{1.0, 1.0}
             };
            var v_xor_ideal = new double[][]
            {
                new double[1]{0.0},
                new double[1]{1.0},
                new double[1]{1.0},
                new double[1]{0.0}
            };

            var command = string.Empty;
            do
            {
                Console.Write("So lan lap luyen mang: ");
                var echpo = int.Parse(Console.ReadLine());

                var v_network = new Quickpropagation(2, v_xor_input, v_xor_ideal);
                v_network.Initialize(); // khởi tạo thế hệ 1
                for (int i = 0; i < echpo; i++)
                {
                    v_network.Learn();
                }
                Console.WriteLine("RMS Error: " + v_network.RMSError);
                Console.WriteLine("Test Value");
                for (int i = 0; i < v_xor_input.Length; i++)
                {
                    var test = v_network.ComputeOutput(v_xor_input[i]);
                    Console.WriteLine(test[0]);
                }
                Console.Write("Nhan 'c' de tiep tuc!");
                command = Console.ReadLine();
            } while (command.Equals("c") == true);
        }

        private static void TestBP()
        {
            var v_xor_input = new double[][]
            {
                  new double[2]{0.0, 0.0},
                  new double[2]{1.0, 0.0},
                  new double[2]{0.0, 1.0},
                  new double[2]{1.0, 1.0}
             };
            var v_xor_ideal = new double[][]
            {
                new double[1]{0.0},
                new double[1]{1.0},
                new double[1]{1.0},
                new double[1]{0.0}
            };

            var command = string.Empty;
            do
            {
                Console.Write("So lan lap luyen mang: ");
                var echpo = int.Parse(Console.ReadLine());

                var v_network = new Backpropagation(2, v_xor_input, v_xor_ideal);
                for (int i = 0; i < echpo; i++)
                {
                    v_network.Learn();
                }
                Console.WriteLine("RMS Error: " + v_network.RMSError);
                Console.WriteLine("Test Value");
                for (int i = 0; i < v_xor_input.Length; i++)
                {
                    var test = v_network.ComputeOutput(v_xor_input[i]);
                    Console.WriteLine(test[0]);
                }
                Console.Write("Nhan 'c' de tiep tuc!");
                command = Console.ReadLine();
            } while (command.Equals("c") == true);
        }
    }
}
