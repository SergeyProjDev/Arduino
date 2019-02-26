using System;
using System.Drawing;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI_Temp_and_Wet
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
            void GetSerialPorts()
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
        }


        SerialPort ComPort;
        private void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                ComPort = new SerialPort
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

                pictureBox1.Hide();
                Task GetValue = new Task(new Action(asyncDataGetter));
                GetValue.Start();
            }
            catch (UnauthorizedAccessException ex){
                MessageBox.Show(ex.Message);}
            catch (NullReferenceException){
                MessageBox.Show("Выберете COM порт!");}
        }



        void asyncDataGetter()
        {
            while (true)
            {
                ComPort.DiscardOutBuffer();
                ComPort.Write("1");

                string data = ComPort.ReadExisting();
                Thread.Sleep(500);

                if (data.Length == 0) continue;

                string wet = data.Substring(0, 5) + " %";
                string temp = data.Substring(8, 5) + " °C";

                label2.Invoke((MethodInvoker)(() => label2.Text = "Temperature:\n   "+temp));
                label3.Invoke((MethodInvoker)(() => label3.Text = "Wetness:\n   "+wet));
            }
        }



        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try{
                ComPort.Close();}
            catch (Exception) {
                }
            finally{
                Application.Exit();}
        }
    }
}
