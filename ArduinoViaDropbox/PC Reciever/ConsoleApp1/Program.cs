using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Net;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            Program Listener = new Program();
            string str;

            string COM = GetSerialPorts();

            if (COM.Equals("d")) //debug mode
            {
                while (true)
                {
                    str = Listener.getFileContent();
                    Console.WriteLine(str);
                }
            }
            else //sending command on Arduino
            {
                Console.WriteLine("   1 - Light ON\n   2 - Light OFF");

                SerialPort ComPort = new SerialPort
                {
                    PortName = COM,
                    BaudRate = 9600,
                    Parity = Parity.None,
                    StopBits = StopBits.One,
                    DataBits = 8,
                    Handshake = Handshake.None,
                    ReadTimeout = 500
                };
                ComPort.Open();

                string prev = "";
                while (true)
                {
                    str = Listener.getFileContent();
                    if (!str.Equals(prev))
                    {
                        prev = str;
                        Console.WriteLine(str);
                        Listener.sendOnCOM(str, ComPort);
                    } 
                }
            }  
        }



        public string getFileContent()
        {
            string link = "https://www.dl.dropboxusercontent.com/s/vk5rdm126ga31oc/site.txt?dl=1";
            string file = "test.txt";
            string str = "";
            
            var client = new WebClient();
            
            StreamReader streamReader;
            client.DownloadFile(link, file);

            streamReader = new StreamReader(file);
            while (!streamReader.EndOfStream) str = streamReader.ReadLine();
            streamReader.Close();

            System.IO.File.Delete(file);

            return str;
        }


        public void sendOnCOM(string data, SerialPort COM) //2 - off, 1 - on
        {
            COM.DiscardOutBuffer();
            COM.Write(data);
        }


        private static string GetSerialPorts()
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
                Console.WriteLine("   " + index + ". " + portName);
            }
            while (!((ArrayComPortsNames[index] == ComPortName) ||
                                (index == ArrayComPortsNames.GetUpperBound(0))));
            Console.WriteLine("\nSelect number of Arduino port (or `d` to debugging)");
            int choice = -1;
            ConsoleKeyInfo key = Console.ReadKey(true);
            if (key.Key != ConsoleKey.Enter)
                try
                {
                    choice = int.Parse(key.KeyChar.ToString());
                }
                catch
                {
                    return "d";
                }

            return ports[choice];
        }
    }
}