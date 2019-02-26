using System;
using System.Drawing;
using System.IO.Ports;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace PCPart
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        

        private void Form1_Load(object sender, EventArgs e)
        {
            GetSerialPorts();
        }



        int power = 0;
        private void SendData(object sender, EventArgs e)
        {
            try
            {
                SerialPort ComPort = new SerialPort{
                    PortName = comboBox1.SelectedItem.ToString(),
                    BaudRate = 9600,
                    Parity = Parity.None,
                    StopBits = StopBits.One,
                    DataBits = 8,
                    Handshake = Handshake.None,
                    ReadTimeout = 500
                };

                ComPort.Open();

                if (power == 0){
                    ComPort.DiscardOutBuffer();
                    ComPort.Write("1");
                    power = 1;
                    Image myimage = new Bitmap(@"on.jpg");
                    button1.BackgroundImage = myimage;
                }
                    else{
                        ComPort.DiscardOutBuffer();
                        ComPort.Write("2");
                        power = 0;
                        Image myimage = new Bitmap(@"off.jpg");
                        button1.BackgroundImage = myimage;
                    }

                ComPort.Close();
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show(ex.Message);
            } 
            catch (NullReferenceException)
            {
                MessageBox.Show("Выберете COM порт!");
            }
        }



        private void GetSerialPorts()
        {
            string[] ArrayComPortsNames = null;
            int index = -1;
            string ComPortName = null;

            ArrayComPortsNames = SerialPort.GetPortNames();
            do
            {
                index += 1;
                string portN = ArrayComPortsNames[index].ToString();
                string portName = String.Empty;
                foreach (char port in portN)
                {
                    if ((port >= 'A' && port <= 'Z') || (port >= '0' && port <= '9')) portName += port;
                }
                comboBox1.Items.Add(portName);
                comboBox1.SelectedItem = comboBox1.Items[index];
                this.ActiveControl = label1; //remove Focus
            }
            while (!((ArrayComPortsNames[index] == ComPortName) ||
                                (index == ArrayComPortsNames.GetUpperBound(0))));
        }



        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                SerialPort ComPort = new SerialPort
                {
                    PortName = comboBox1.SelectedItem.ToString(),
                    BaudRate = 9600,
                    Parity = Parity.None,
                    StopBits = StopBits.One,
                    DataBits = 8,
                    Handshake = Handshake.None,
                    ReadTimeout = 500
                };
                ComPort.Open();
                ComPort.Write("2");
                ComPort.Close();
            }
            catch (Exception) { }
        }



       
    }

   
}
