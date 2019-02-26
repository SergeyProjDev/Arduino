using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO.Ports;
using System.Collections.Generic;
using System.Diagnostics;

namespace SocketServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Program program = new Program();
            string com = program.GetSerialPorts();
            Console.WriteLine("Using port: "+com);

            IPHostEntry ipHost = Dns.GetHostEntry("localhost");
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, 1234);


            Socket sListener = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            program.StartCmd();

            try
            {
                sListener.Bind(ipEndPoint);
                sListener.Listen(10);

                Console.WriteLine("Arduino is listening...\n");
                
                program.loop(sListener, com);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                Console.WriteLine("Program ended.");
                Console.ReadLine();
            }
        }



        void StartCmd()
        {
            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();

            cmd.StandardInput.WriteLine("ipconfig");
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            cmd.WaitForExit();
            Console.WriteLine(cmd.StandardOutput.ReadToEnd());
        }


        private void loop(Socket sListener, string com)
        {
            while (true)
            {
                Socket handler = sListener.Accept();
                string data = null;

                byte[] bytes = new byte[1024];
                int bytesRec = handler.Receive(bytes);

                data += Encoding.UTF8.GetString(bytes, 0, bytesRec);

                SerialPort ComPort = new SerialPort
                {
                    PortName = com,
                    BaudRate = 9600,
                    Parity = Parity.None,
                    StopBits = StopBits.One,
                    DataBits = 8,
                    Handshake = Handshake.None,
                    ReadTimeout = 500
                };
                ComPort.Open();

                // Показываем данные на консоли и даем приказ Ардуино
                Console.Write("Query: ");
                if (data == "1")
                {
                    ComPort.DiscardOutBuffer();
                    ComPort.Write("1");
                    Console.Write("Light ON\n");
                }
                if (data == "2")
                {
                    ComPort.DiscardOutBuffer();
                    ComPort.Write("2");
                    Console.Write("Light OFF\n");
                }

                handler.Shutdown(SocketShutdown.Both);
                handler.Close();

                ComPort.Close();
                Console.WriteLine();
            }
        }


        private string GetSerialPorts()
        {
            string[] ArrayComPortsNames = null;
            int index = -1;
            string ComPortName = null;

            ArrayComPortsNames = SerialPort.GetPortNames();
            Dictionary<int, string> ports = new Dictionary<int, string>();
            Console.WriteLine("Available ports.:\n");
            do
            {
                index += 1;
                string temp = ArrayComPortsNames[index].ToString();
                string portName = String.Empty;
                foreach (char port in temp)
                {
                    if ((port >= 'A' && port <= 'Z') || (port >= '0' && port <= '9')) portName += port;
                }
                ports.Add(index, portName);
                Console.WriteLine("   "+index + ". "+ portName);
            }
            while (!((ArrayComPortsNames[index] == ComPortName) ||
                                (index == ArrayComPortsNames.GetUpperBound(0))));
            Console.WriteLine("\nSelect number of Arduino port");
                int choice = -1;
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key != ConsoleKey.Enter)
                    choice = int.Parse(key.KeyChar.ToString());
            return ports[choice]; 
        }
    }
}