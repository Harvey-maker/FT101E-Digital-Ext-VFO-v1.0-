using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Threading;
using System.Windows.Forms;


namespace ComputerToArduino
{
    public partial class Form1 : Form
    {
        private BackgroundWorker ScanmemBackgroundWorker = new BackgroundWorker();
        private long FullScaleDelta = 7500000;
        private long VFO_Constant = 5000000;
        private long CrystalFrequency = 125001271;
        private long Cal_Constant = 203799;
        private const string EnabledValue = "1";
        private const string DisabledValue = "0";
        private int Trimscrolloset = -500;
        private int Trim = 0;
        private int scanspeed = 3;
        private int soundenable = 0;



        private bool isConnected = false;
        private String[] ports;
        private SerialPort port;

        private string SaveFile = @"z:\\vfo_state.txt";
        private string SaveFile1 = @"z:\\freq1.txt";
        private string SaveFile2 = @"z:\\freq2.txt";
        private string SaveFile3 = @"z:\\freq3.txt";
        private string SaveFile4 = @"z:\\freq4.txt";
        private string SaveFile5 = @"z:\\freq5.txt";
        private string Freqstring = "";
        private string Band = "0";
        private bool DoWork1 = true;
        private string Dialfreq = "";


        public object Exit { get; private set; }
        public string exit { get; private set; }

        public Form1()
        {
            InitializeComponent();
            disableControls();
            getAvailableComPorts();

            foreach (string port in ports)
            {
                comboBox1.Items.Add(port);
                Console.WriteLine(port);
                if (ports[0] != null)
                {
                    comboBox1.SelectedItem = ports[0];
                }
            }
            // INITIALISATION CODE TOP

            textBox5.Text = "OFF";
            textBox5.ForeColor = Color.Red;
            textBox1.Text += "SELECT OPERATING BAND" + Environment.NewLine;
            button16.ForeColor = Color.Red; // initalize scan rate button
            button14.ForeColor = Color.Red; // initalize scan stop button
            

            // INITIALISATION CODE BOTTOM
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!isConnected)
            {
                connectToArduino();
                Console.Beep(1500, 200);
            }
            else
            {
                disconnectFromArduino();
                Console.Beep(1500, 200);
            }
        }

        void getAvailableComPorts()
        {
            ports = SerialPort.GetPortNames();
        }

        private void connectToArduino()
        {
            isConnected = true;
            string selectedPort = comboBox1.GetItemText(comboBox1.SelectedItem);
            port = new SerialPort(selectedPort, 14400, Parity.None, 8, StopBits.One);
            port.DataReceived += Port_DataReceived;
            port.Open();

            button1.Text = "Disconnect";
            enableControls();
        }

        private void Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            switch (port.ReadExisting())
            {
                case EnabledValue:
                    UpdateState("ON", Color.Red);
                    return;
                case DisabledValue:
                    UpdateState("OFF", Color.Red);
                    return;
            }
        }

        private void UpdateState(string state, Color stateColor)
        {
            textBox5.Invoke((MethodInvoker)delegate
            {
                textBox5.Text = state;
                textBox5.ForeColor = stateColor;
            });
        }

        private void disconnectFromArduino()
        {
            isConnected = false;
            port.Close();
            button1.Text = "Connect";
            disableControls();
            resetDefaults();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (isConnected)
            {

            }
        }

        private void enableControls()
        {

        }

        private void disableControls()
        {

        }

        private void resetDefaults()
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            CalculateBase10FrequencyValue_Click();
        }



        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click_2(object sender, EventArgs e)
        {

        }

        private void buttonUpdateFreq_Click(object sender, EventArgs e)
        {
            CalculateBase10FrequencyValue_Click();

        }

        private void CalculateBase10FrequencyValue_Click()
        {
            var dialFrequency = long.Parse(UpDownVales());
            var invertedDialFrequency = FullScaleDelta - dialFrequency;
            var frequencyOfOperation = invertedDialFrequency + VFO_Constant + Cal_Constant + Trim;

            var n = (Math.Pow(2, 32) * frequencyOfOperation) / CrystalFrequency;

            if (isConnected)
            {
                port.Write($"{Convert.ToInt64(n)}\n");
            }

        }

        private string UpDownVales() => $"{numericUpDown1.Value}{numericUpDown2.Value}{numericUpDown3.Value}{numericUpDown4.Value}" +
                $"{numericUpDown5.Value}{numericUpDown6.Value}{numericUpDown7.Value}{numericUpDown8.Value}";

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            CalculateBase10FrequencyValue_Click();
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            CalculateBase10FrequencyValue_Click();
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            CalculateBase10FrequencyValue_Click();
        }

        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {
            CalculateBase10FrequencyValue_Click();
        }

        private void numericUpDown6_ValueChanged(object sender, EventArgs e)
        {
            CalculateBase10FrequencyValue_Click();
        }

        private void numericUpDown7_ValueChanged(object sender, EventArgs e)
        {
            CalculateBase10FrequencyValue_Click();
        }

        private void numericUpDown8_ValueChanged(object sender, EventArgs e)
        {
            CalculateBase10FrequencyValue_Click();
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            radioButton1.Checked = true;
            button20.PerformClick();
            button12.PerformClick();
            button10.PerformClick();
            button8.PerformClick();
            button6.PerformClick();
            //label3.BackColor = Color.White; // required if  all lamps off after label boot update.
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        

        private void radioButton10_CheckedChanged(object sender, EventArgs e)
        {
            // 160M Band orignal copied from 80M radiobutton 1
            FullScaleDelta = 3500000;
            VFO_Constant = 6700000;
            CrystalFrequency = 125001271;
            Cal_Constant = 501243;
            Band = "10";
            hScrollBar2.Minimum = 1500000;
            hScrollBar2.Maximum = 2000000;
            numericUpDown1.Value = 0;
            numericUpDown2.Value = 1;
            numericUpDown3.Value = 5;
            numericUpDown4.Value = 0;
            numericUpDown5.Value = 0;
            numericUpDown6.Value = 0;
            numericUpDown7.Value = 0;
            numericUpDown8.Value = 0;
            hScrollBar2.Value = 1500000;


            textBox1.Clear();
            textBox1.Text += "1.5 MHz - 2.0 MHz" + Environment.NewLine;
            textBox1.Text += "" + Environment.NewLine;
            textBox1.Text += "Set FT101E Band control to 160M" + Environment.NewLine;
            textBox1.Text += "Set FT101E VFO control to Ext." + Environment.NewLine;
            textBox1.Text += "Set Ext. VFO to Connect." + Environment.NewLine;
            textBox1.Text += "Select * LSB * on Transciever." + Environment.NewLine;
            textBox1.Text += "Select Frequency." + Environment.NewLine;
            textBox1.Text += "Peak Pre-Select for 160M Rx/Tx.";
        }

        private void radioButton1_CheckedChanged_1(object sender, EventArgs e)
        {
            // 80M Band
            FullScaleDelta = 7500000;
            VFO_Constant = 5000000;
            CrystalFrequency = 125001271;
            Cal_Constant = 201027;
            Band = "1";
            hScrollBar2.Minimum = 3500000;
            hScrollBar2.Maximum = 4000000;
            numericUpDown1.Value = 0;
            numericUpDown2.Value = 3;
            numericUpDown3.Value = 5;
            numericUpDown4.Value = 0;
            numericUpDown5.Value = 0;
            numericUpDown6.Value = 0;
            numericUpDown7.Value = 0;
            numericUpDown8.Value = 0;
            hScrollBar2.Value = 3500000;


            textBox1.Clear();
            textBox1.Text += "3.5 MHz - 4.0 MHz" + Environment.NewLine;
            textBox1.Text += "" + Environment.NewLine;
            textBox1.Text += "Set FT101E Band control to 80M" + Environment.NewLine;
            textBox1.Text += "Set FT101E VFO control to Ext." + Environment.NewLine;
            textBox1.Text += "Set Ext. VFO to Connect." + Environment.NewLine;
            textBox1.Text += "Select * LSB * on Transciever." + Environment.NewLine;
            textBox1.Text += "Select Frequency." + Environment.NewLine;
            textBox1.Text += "Peak Pre-Select for 80M Rx/Tx.";

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            // 40M Band
            FullScaleDelta = 14500000;
            VFO_Constant = 1500000;
            CrystalFrequency = 125001271;
            Cal_Constant = 201333;
            Band = "2";
            hScrollBar2.Minimum = 7000000;
            hScrollBar2.Maximum = 7300000;
            numericUpDown1.Value = 0;
            numericUpDown2.Value = 7;
            numericUpDown3.Value = 0;
            numericUpDown4.Value = 0;
            numericUpDown5.Value = 0;
            numericUpDown6.Value = 0;
            numericUpDown7.Value = 0;
            numericUpDown8.Value = 0;
            hScrollBar2.Value = 7000000;


            textBox1.Clear();
            textBox1.Text += "7.0 MHz - 7.3 MHz" + Environment.NewLine;
            textBox1.Text += "" + Environment.NewLine;
            textBox1.Text += "Set FT101E Band control to 40M" + Environment.NewLine;
            textBox1.Text += "Set FT101E VFO control to Ext." + Environment.NewLine;
            textBox1.Text += "Set Ext. VFO to Connect." + Environment.NewLine;
            textBox1.Text += "Select * LSB * on Transciever." + Environment.NewLine;
            textBox1.Text += "Select Frequency." + Environment.NewLine;
            textBox1.Text += "Peak Pre-Select for 40M Rx/Tx.";
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            // 20M Band
            FullScaleDelta = 28500000;
            VFO_Constant = -5500000;
            CrystalFrequency = 125001271;
            Cal_Constant = 198336;
            Band = "3";
            hScrollBar2.Minimum = 14000000;
            hScrollBar2.Maximum = 14350000;
            numericUpDown1.Value = 1;
            numericUpDown2.Value = 4;
            numericUpDown3.Value = 0;
            numericUpDown4.Value = 0;
            numericUpDown5.Value = 0;
            numericUpDown6.Value = 0;
            numericUpDown7.Value = 0;
            numericUpDown8.Value = 0;
            hScrollBar2.Value = 14000000;


            textBox1.Clear();
            textBox1.Text += "14.0 MHz - 14.35 MHz" + Environment.NewLine;
            textBox1.Text += "" + Environment.NewLine;
            textBox1.Text += "Set FT101E Band control to 20M" + Environment.NewLine;
            textBox1.Text += "Set FT101E VFO control to Ext." + Environment.NewLine;
            textBox1.Text += "Set Ext. VFO to Connect." + Environment.NewLine;
            textBox1.Text += "Select * USB * on Transciever." + Environment.NewLine;
            textBox1.Text += "Select Frequency." + Environment.NewLine;
            textBox1.Text += "Peak Pre-Select for 20M Rx/Tx.";
        }


        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {
            
        }


        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            // 15M Band
            FullScaleDelta = 42500000;
            VFO_Constant = -12500000;
            CrystalFrequency = 125001271;
            Cal_Constant = 198224;
            Band = "4";
            hScrollBar2.Minimum = 21000000;
            hScrollBar2.Maximum = 21450000;
            numericUpDown1.Value = 2;
            numericUpDown2.Value = 1;
            numericUpDown3.Value = 0;
            numericUpDown4.Value = 0;
            numericUpDown5.Value = 0;
            numericUpDown6.Value = 0;
            numericUpDown7.Value = 0;
            numericUpDown8.Value = 0;
            hScrollBar2.Value = 21000000;



            textBox1.Clear();
            textBox1.Text += "21.0 MHz - 21.45 MHz" + Environment.NewLine;
            textBox1.Text += "" + Environment.NewLine;
            textBox1.Text += "Set FT101E Band control to 15M" + Environment.NewLine;
            textBox1.Text += "Set FT101E VFO control to Ext." + Environment.NewLine;
            textBox1.Text += "Set Ext. VFO to Connect." + Environment.NewLine;
            textBox1.Text += "Select * USB * on Transciever." + Environment.NewLine;
            textBox1.Text += "Select Frequency." + Environment.NewLine;
            textBox1.Text += "Peak Pre-Select for 15M Rx/Tx.";

        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            // 11M Band
            FullScaleDelta = 54500000;
            VFO_Constant = -18500000;
            CrystalFrequency = 125001271;
            Cal_Constant = 197345;
            Band = "5";
            hScrollBar2.Minimum = 27000000;
            hScrollBar2.Maximum = 27500000;
            numericUpDown1.Value = 2;
            numericUpDown2.Value = 7;
            numericUpDown3.Value = 0;
            numericUpDown4.Value = 0;
            numericUpDown5.Value = 0;
            numericUpDown6.Value = 0;
            numericUpDown7.Value = 0;
            numericUpDown8.Value = 0;
            hScrollBar2.Value = 27000000;


            textBox1.Clear();
            textBox1.Text += "27.0 MHz - 27.5 MHz" + Environment.NewLine;
            textBox1.Text += "" + Environment.NewLine;
            textBox1.Text += "Set FT101E Band control to 11M" + Environment.NewLine;
            textBox1.Text += "Set FT101E VFO control to Ext." + Environment.NewLine;
            textBox1.Text += "Set Ext. VFO to Connect." + Environment.NewLine;
            textBox1.Text += "Select * USB * on Transciever." + Environment.NewLine;
            textBox1.Text += "Select Frequency." + Environment.NewLine;
            textBox1.Text += "Peak Pre-Select for 11M Rx/Tx.";
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            // 10A Band
            FullScaleDelta = 56500000;
            VFO_Constant = -19500000;
            CrystalFrequency = 125001271;
            Cal_Constant = 198508;
            Band = "6";
            hScrollBar2.Minimum = 28000000;
            hScrollBar2.Maximum = 28500000;
            numericUpDown1.Value = 2;
            numericUpDown2.Value = 8;
            numericUpDown3.Value = 0;
            numericUpDown4.Value = 0;
            numericUpDown5.Value = 0;
            numericUpDown6.Value = 0;
            numericUpDown7.Value = 0;
            numericUpDown8.Value = 0;
            hScrollBar2.Value = 28000000;


            textBox1.Clear();
            textBox1.Text += "28.0 MHz - 28.5 MHz" + Environment.NewLine;
            textBox1.Text += "" + Environment.NewLine;
            textBox1.Text += "Set FT101E Band control to 10A" + Environment.NewLine;
            textBox1.Text += "Set FT101E VFO control to Ext." + Environment.NewLine;
            textBox1.Text += "Set Ext. VFO to Connect." + Environment.NewLine;
            textBox1.Text += "Select * USB * on Transciever." + Environment.NewLine;
            textBox1.Text += "Select Frequency." + Environment.NewLine;
            textBox1.Text += "Peak Pre-Select for 10M A Rx/Tx.";
        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            // 10B Band
            FullScaleDelta = 57500000;
            VFO_Constant = -20500000;
            CrystalFrequency = 125001271;
            Cal_Constant = 698040;
            Band = "7";
            hScrollBar2.Minimum = 28500000;
            hScrollBar2.Maximum = 29000000;
            numericUpDown1.Value = 2;
            numericUpDown2.Value = 8;
            numericUpDown3.Value = 5;
            numericUpDown4.Value = 0;
            numericUpDown5.Value = 0;
            numericUpDown6.Value = 0;
            numericUpDown7.Value = 0;
            numericUpDown8.Value = 0;
            hScrollBar2.Value = 28500000;

            textBox1.Clear();
            textBox1.Text += "28.5 MHz - 29.0 MHz" + Environment.NewLine;
            textBox1.Text += "" + Environment.NewLine;
            textBox1.Text += "Set FT101E Band control to 10B" + Environment.NewLine;
            textBox1.Text += "Set FT101E VFO control to Ext." + Environment.NewLine;
            textBox1.Text += "Set Ext. VFO to Connect." + Environment.NewLine;
            textBox1.Text += "Select * USB * on Transciever." + Environment.NewLine;
            textBox1.Text += "Select Frequency." + Environment.NewLine;
            textBox1.Text += "Peak Pre-Select for 10M B Rx/Tx.";
        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            // 10C Band
            FullScaleDelta = 58500000;
            VFO_Constant = -21500000;
            CrystalFrequency = 125001271;
            Cal_Constant = 1198441;
            Band = "8";
            hScrollBar2.Minimum = 29000000;
            hScrollBar2.Maximum = 29500000;
            numericUpDown1.Value = 2;
            numericUpDown2.Value = 9;
            numericUpDown3.Value = 0;
            numericUpDown4.Value = 0;
            numericUpDown5.Value = 0;
            numericUpDown6.Value = 0;
            numericUpDown7.Value = 0;
            numericUpDown8.Value = 0;
            hScrollBar2.Value = 29000000;


            textBox1.Clear();
            textBox1.Text += "29.0 MHz - 29.5 MHz" + Environment.NewLine;
            textBox1.Text += "" + Environment.NewLine;
            textBox1.Text += "Set FT101E Band control to 10C" + Environment.NewLine;
            textBox1.Text += "Set FT101E VFO control to Ext." + Environment.NewLine;
            textBox1.Text += "Set Ext. VFO to Connect." + Environment.NewLine;
            textBox1.Text += "Select * USB * on Transciever." + Environment.NewLine;
            textBox1.Text += "Select Frequency." + Environment.NewLine;
            textBox1.Text += "Peak Pre-Select for 10M C Rx/Tx.";
        }

        private void radioButton9_CheckedChanged(object sender, EventArgs e)
        {
            // 10D Band
            FullScaleDelta = 59500000;
            VFO_Constant = -21000000;
            CrystalFrequency = 125001271;
            Cal_Constant = 197615;
            Band = "9";
            hScrollBar2.Minimum = 29500000;
            hScrollBar2.Maximum = 30000000;
            numericUpDown1.Value = 2;
            numericUpDown2.Value = 9;
            numericUpDown3.Value = 5;
            numericUpDown4.Value = 0;
            numericUpDown5.Value = 0;
            numericUpDown6.Value = 0;
            numericUpDown7.Value = 0;
            numericUpDown8.Value = 0;
            hScrollBar2.Value = 29500000;


            textBox1.Clear();
            textBox1.Text += "29.5 MHz - 30.0 MHz" + Environment.NewLine;
            textBox1.Text += "" + Environment.NewLine;
            textBox1.Text += "Set FT101E Band control to 10D" + Environment.NewLine;
            textBox1.Text += "Set FT101E VFO control to Ext." + Environment.NewLine;
            textBox1.Text += "Set Ext. VFO to Connect." + Environment.NewLine;
            textBox1.Text += "Select * USB * on Transciever." + Environment.NewLine;
            textBox1.Text += "Select Frequency." + Environment.NewLine;
            textBox1.Text += "Peak Pre-Select for 10M D Rx/Tx.";

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            hScrollBar1.Value = 500;
            label1.Text = "0";
            Trim = 0;
            CalculateBase10FrequencyValue_Click();
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            label1.Text = Convert.ToString(hScrollBar1.Value + Trimscrolloset);
            Trim = (-1 * (hScrollBar1.Value + Trimscrolloset));

            var dialFrequency = long.Parse(UpDownVales());
            var invertedDialFrequency = FullScaleDelta - dialFrequency;
            var frequencyOfOperation = invertedDialFrequency + VFO_Constant + Cal_Constant + Trim;

            var n = (Math.Pow(2, 32) * frequencyOfOperation) / CrystalFrequency;

            if (isConnected)
            {
                port.Write($"{Convert.ToInt64(n)}\n");
            }
        }
        //*****************************************Save button
        private void button3_Click(object sender, EventArgs e)
        {

            File.WriteAllText(SaveFile, label1.Text);

        }
        // ***************************************Recall button
        private void button4_Click(object sender, EventArgs e)
        {
            var value = File.ReadAllText(SaveFile);
            Trim = Convert.ToInt32(value);
            label1.Text = (value);
            hScrollBar1.Value = Trim + 500;


            label1.Text = Convert.ToString(hScrollBar1.Value + Trimscrolloset);
            Trim = (-1 * (hScrollBar1.Value + Trimscrolloset));

            var dialFrequency = long.Parse(UpDownVales());
            var invertedDialFrequency = FullScaleDelta - dialFrequency;
            var frequencyOfOperation = invertedDialFrequency + VFO_Constant + Cal_Constant + Trim;

            var n = (Math.Pow(2, 32) * frequencyOfOperation) / CrystalFrequency;

            if (isConnected)
            {
                port.Write($"{Convert.ToInt64(n)}\n");
            }

        }

        // ***************************************************************   MEM. / Recall Frequency


        // private void hScrollBar2_Scroll(object sender, ScrollEventArgs e)
        // {
        //    textBox2.Text = Convert.ToString(hScrollBar2.Value);
        /// }

        private void Form1_Load_1(object sender, EventArgs e)
        {

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void button5_Click_1(object sender, EventArgs e) // Mem 1
        {

            Console.Beep(2000, 200);
            //Console.Beep(1500, 200);

            // Digit and Band Selection * Frequency digit positions 0,1,2,3,4,5,6,7, + Band ID 8.  END

            Freqstring = $"{numericUpDown1.Value + ","}{numericUpDown2.Value + ","}{numericUpDown3.Value + ","}{numericUpDown4.Value + ","}" +
                            $"{numericUpDown5.Value + ","}{numericUpDown6.Value + ","}{numericUpDown7.Value + ","}{numericUpDown8.Value + ","}{Band}";

            File.WriteAllText(SaveFile1, Freqstring);

            button6.PerformClick();

            // MessageBox.Show(Freqstring);
        }

        private void button6_Click_1(object sender, EventArgs e) // Recall 1
        {
            
            if (soundenable == 1)
            {
                Console.Beep(1500, 200);
            }
            else
            {
                soundenable = 1;
            }

            label3.BackColor = Color.LightGreen;
            label4.BackColor = Color.White;
            label5.BackColor = Color.White;
            label6.BackColor = Color.White;
            label7.BackColor = Color.White;

            var value = File.ReadAllText(SaveFile1);
            
            var values = value.Split(',');

            var Digit1 = values[0];
            var Digit2 = values[1];
            var Digit3 = values[2];
            var Digit4 = values[3];
            var Digit5 = values[4];
            var Digit6 = values[5];
            var Digit7 = values[6];
            var Digit8 = values[7];
            var Digit9 = values[8]; // Band ID bit

            textBox2.Text = Digit1 + Digit2 + Digit3 + Digit4 + Digit5 + Digit6 + Digit7 + Digit8;

            // ******************************************** Digits Update

            numericUpDown1.Value = Convert.ToInt32(Digit1);
            numericUpDown2.Value = Convert.ToInt32(Digit2);
            numericUpDown3.Value = Convert.ToInt32(Digit3);
            numericUpDown4.Value = Convert.ToInt32(Digit4);
            numericUpDown5.Value = Convert.ToInt32(Digit5);
            numericUpDown6.Value = Convert.ToInt32(Digit6);
            numericUpDown7.Value = Convert.ToInt32(Digit7);
            numericUpDown8.Value = Convert.ToInt32(Digit8);
            Band = Digit9;

            //MessageBox.Show(Band);

            // ******************************************** Select Band

            if (Band == "1")
            {
                radioButton1.Checked = true;
            }
            else
            {
            }

            if (Band == "2")
            {
                radioButton2.Checked = true;
            }
            else
            {
            }

            if (Band == "3")
            {
                radioButton3.Checked = true;
            }
            else
            {
            }

            if (Band == "4")
            {
                radioButton4.Checked = true;
            }
            else
            {
            }

            if (Band == "5")
            {
                radioButton5.Checked = true;
            }
            else
            {
            }

            if (Band == "6")
            {
                radioButton6.Checked = true;
            }
            else
            {
            }

            if (Band == "7")
            {
                radioButton7.Checked = true;
            }
            else
            {
            }

            if (Band == "8")
            {
                radioButton8.Checked = true;
            }
            else
            {
            }

            if (Band == "9")
            {
                radioButton9.Checked = true;
            }
            else
            {
            }

            if (Band == "10")
            {
                radioButton10.Checked = true;
            }
            else
            {
            }

            System.Threading.Thread.Sleep(50);
            CalculateBase10FrequencyValue_Click();

        }
        private void button7_Click_1(object sender, EventArgs e) // Mem 2
        {
            Console.Beep(2000, 200);
            //Console.Beep(1500, 200);

            // Digit and Band Selection * Frequency digit positions 0,1,2,3,4,5,6,7, + Band ID 8.  END

            Freqstring = $"{numericUpDown1.Value + ","}{numericUpDown2.Value + ","}{numericUpDown3.Value + ","}{numericUpDown4.Value + ","}" +
                            $"{numericUpDown5.Value + ","}{numericUpDown6.Value + ","}{numericUpDown7.Value + ","}{numericUpDown8.Value + ","}{Band}";

            File.WriteAllText(SaveFile2, Freqstring);

            button8.PerformClick();

            // MessageBox.Show(Freqstring);
            var value = File.ReadAllText(SaveFile2);

            var values = value.Split(',');

            var Digit1 = values[0];
            var Digit2 = values[1];
            var Digit3 = values[2];
            var Digit4 = values[3];
            var Digit5 = values[4];
            var Digit6 = values[5];
            var Digit7 = values[6];
            var Digit8 = values[7];
            var Digit9 = values[8]; // Band ID bit

            // ******************************************** Digits Update

            numericUpDown1.Value = Convert.ToInt32(Digit1);
            numericUpDown2.Value = Convert.ToInt32(Digit2);
            numericUpDown3.Value = Convert.ToInt32(Digit3);
            numericUpDown4.Value = Convert.ToInt32(Digit4);
            numericUpDown5.Value = Convert.ToInt32(Digit5);
            numericUpDown6.Value = Convert.ToInt32(Digit6);
            numericUpDown7.Value = Convert.ToInt32(Digit7);
            numericUpDown8.Value = Convert.ToInt32(Digit8);
            Band = Digit9;

            //MessageBox.Show(Band);

            // ******************************************** Select Band

            if (Band == "1")
            {
                radioButton1.Checked = true;
            }
            else
            {
            }

            if (Band == "2")
            {
                radioButton2.Checked = true;
            }
            else
            {
            }

            if (Band == "3")
            {
                radioButton3.Checked = true;
            }
            else
            {
            }

            if (Band == "4")
            {
                radioButton4.Checked = true;
            }
            else
            {
            }

            if (Band == "5")
            {
                radioButton5.Checked = true;
            }
            else
            {
            }

            if (Band == "6")
            {
                radioButton6.Checked = true;
            }
            else
            {
            }

            if (Band == "7")
            {
                radioButton7.Checked = true;
            }
            else
            {
            }

            if (Band == "8")
            {
                radioButton8.Checked = true;
            }
            else
            {
            }

            if (Band == "9")
            {
                radioButton9.Checked = true;
            }
            else
            {
            }

            if (Band == "10")
            {
                radioButton10.Checked = true;
            }
            else
            {
            }

            System.Threading.Thread.Sleep(50);
            CalculateBase10FrequencyValue_Click();

        }

        private void button8_Click_1(object sender, EventArgs e) // Recall 2
        {
           
            if (soundenable == 1)
            {
                Console.Beep(1500, 200);
            }

            label3.BackColor = Color.White;
            label4.BackColor = Color.LightGreen;
            label5.BackColor = Color.White;
            label6.BackColor = Color.White;
            label7.BackColor = Color.White;

            var value = File.ReadAllText(SaveFile2);
            
            var values = value.Split(',');

            var Digit1 = values[0];
            var Digit2 = values[1];
            var Digit3 = values[2];
            var Digit4 = values[3];
            var Digit5 = values[4];
            var Digit6 = values[5];
            var Digit7 = values[6];
            var Digit8 = values[7];
            var Digit9 = values[8]; // Band ID bit

            textBox6.Text = Digit1 + Digit2 + Digit3 + Digit4 + Digit5 + Digit6 + Digit7 + Digit8;

            // ******************************************** Digits Update

            numericUpDown1.Value = Convert.ToInt32(Digit1);
            numericUpDown2.Value = Convert.ToInt32(Digit2);
            numericUpDown3.Value = Convert.ToInt32(Digit3);
            numericUpDown4.Value = Convert.ToInt32(Digit4);
            numericUpDown5.Value = Convert.ToInt32(Digit5);
            numericUpDown6.Value = Convert.ToInt32(Digit6);
            numericUpDown7.Value = Convert.ToInt32(Digit7);
            numericUpDown8.Value = Convert.ToInt32(Digit8);
            Band = Digit9;

            //MessageBox.Show(Band);

            // ******************************************** Select Band

            if (Band == "1")
            {
                radioButton1.Checked = true;
            }
            else
            {
            }

            if (Band == "2")
            {
                radioButton2.Checked = true;
            }
            else
            {
            }

            if (Band == "3")
            {
                radioButton3.Checked = true;
            }
            else
            {
            }

            if (Band == "4")
            {
                radioButton4.Checked = true;
            }
            else
            {
            }

            if (Band == "5")
            {
                radioButton5.Checked = true;
            }
            else
            {
            }

            if (Band == "6")
            {
                radioButton6.Checked = true;
            }
            else
            {
            }

            if (Band == "7")
            {
                radioButton7.Checked = true;
            }
            else
            {
            }

            if (Band == "8")
            {
                radioButton8.Checked = true;
            }
            else
            {
            }

            if (Band == "9")
            {
                radioButton9.Checked = true;
            }
            else
            {
            }

            if (Band == "10")
            {
                radioButton10.Checked = true;
            }
            else
            {
            }

            System.Threading.Thread.Sleep(100);
            CalculateBase10FrequencyValue_Click();

        }

        private void button9_Click_1(object sender, EventArgs e) // Mem 3
        {
            Console.Beep(2000, 200);
            //Console.Beep(1500, 200);
            // Digit and Band Selection * Frequency digit positions 0,1,2,3,4,5,6,7, + Band ID 8.  END

            Freqstring = $"{numericUpDown1.Value + ","}{numericUpDown2.Value + ","}{numericUpDown3.Value + ","}{numericUpDown4.Value + ","}" +
                            $"{numericUpDown5.Value + ","}{numericUpDown6.Value + ","}{numericUpDown7.Value + ","}{numericUpDown8.Value + ","}{Band}";

            File.WriteAllText(SaveFile3, Freqstring);

            button10.PerformClick();

            // MessageBox.Show(Freqstring);

        }

        private void button10_Click_1(object sender, EventArgs e) //Recall 3
        {
           
            if (soundenable == 1)
            {
                Console.Beep(1500, 200);
            }

            label3.BackColor = Color.White;
            label4.BackColor = Color.White;
            label5.BackColor = Color.LightGreen;
            label6.BackColor = Color.White;
            label7.BackColor = Color.White;

            var value = File.ReadAllText(SaveFile3);
            
            var values = value.Split(',');

            var Digit1 = values[0];
            var Digit2 = values[1];
            var Digit3 = values[2];
            var Digit4 = values[3];
            var Digit5 = values[4];
            var Digit6 = values[5];
            var Digit7 = values[6];
            var Digit8 = values[7];
            var Digit9 = values[8]; // Band ID bit

            textBox7.Text = Digit1 + Digit2 + Digit3 + Digit4 + Digit5 + Digit6 + Digit7 + Digit8;

            // ******************************************** Digits Update

            numericUpDown1.Value = Convert.ToInt32(Digit1);
            numericUpDown2.Value = Convert.ToInt32(Digit2);
            numericUpDown3.Value = Convert.ToInt32(Digit3);
            numericUpDown4.Value = Convert.ToInt32(Digit4);
            numericUpDown5.Value = Convert.ToInt32(Digit5);
            numericUpDown6.Value = Convert.ToInt32(Digit6);
            numericUpDown7.Value = Convert.ToInt32(Digit7);
            numericUpDown8.Value = Convert.ToInt32(Digit8);
            Band = Digit9;

            //MessageBox.Show(Band);

            // ******************************************** Select Band

            if (Band == "1")
            {
                radioButton1.Checked = true;
            }
            else
            {
            }

            if (Band == "2")
            {
                radioButton2.Checked = true;
            }
            else
            {
            }

            if (Band == "3")
            {
                radioButton3.Checked = true;
            }
            else
            {
            }

            if (Band == "4")
            {
                radioButton4.Checked = true;
            }
            else
            {
            }

            if (Band == "5")
            {
                radioButton5.Checked = true;
            }
            else
            {
            }

            if (Band == "6")
            {
                radioButton6.Checked = true;
            }
            else
            {
            }

            if (Band == "7")
            {
                radioButton7.Checked = true;
            }
            else
            {
            }

            if (Band == "8")
            {
                radioButton8.Checked = true;
            }
            else
            {
            }

            if (Band == "9")
            {
                radioButton9.Checked = true;
            }
            else
            {
            }

            if (Band == "9")
            {
                radioButton9.Checked = true;
            }
            else
            {
            }

            if (Band == "10")
            {
                radioButton10.Checked = true;
            }
            else
            {
            }

            System.Threading.Thread.Sleep(50);
            CalculateBase10FrequencyValue_Click();

        }

        private void button11_Click_1(object sender, EventArgs e) // Mem 4
        {
            Console.Beep(2000, 200);
            //Console.Beep(1500, 200);
            // Digit and Band Selection * Frequency digit positions 0,1,2,3,4,5,6,7, + Band ID 8.  END

            Freqstring = $"{numericUpDown1.Value + ","}{numericUpDown2.Value + ","}{numericUpDown3.Value + ","}{numericUpDown4.Value + ","}" +
                            $"{numericUpDown5.Value + ","}{numericUpDown6.Value + ","}{numericUpDown7.Value + ","}{numericUpDown8.Value + ","}{Band}";

            File.WriteAllText(SaveFile4, Freqstring);

            button12.PerformClick();

            // MessageBox.Show(Freqstring);

        }

        private void button12_Click_1(object sender, EventArgs e) // Recall 4
        {

            if (soundenable == 1)
            {
                Console.Beep(1500, 200);
            }

            label3.BackColor = Color.White;
            label4.BackColor = Color.White;
            label5.BackColor = Color.White;
            label6.BackColor = Color.LightGreen;
            label7.BackColor = Color.White;

            var value = File.ReadAllText(SaveFile4);
            
            var values = value.Split(',');

            var Digit1 = values[0];
            var Digit2 = values[1];
            var Digit3 = values[2];
            var Digit4 = values[3];
            var Digit5 = values[4];
            var Digit6 = values[5];
            var Digit7 = values[6];
            var Digit8 = values[7];
            var Digit9 = values[8]; // Band ID bit

            textBox8.Text = Digit1 + Digit2 + Digit3 + Digit4 + Digit5 + Digit6 + Digit7 + Digit8;

            // ******************************************** Digits Update

            numericUpDown1.Value = Convert.ToInt32(Digit1);
            numericUpDown2.Value = Convert.ToInt32(Digit2);
            numericUpDown3.Value = Convert.ToInt32(Digit3);
            numericUpDown4.Value = Convert.ToInt32(Digit4);
            numericUpDown5.Value = Convert.ToInt32(Digit5);
            numericUpDown6.Value = Convert.ToInt32(Digit6);
            numericUpDown7.Value = Convert.ToInt32(Digit7);
            numericUpDown8.Value = Convert.ToInt32(Digit8);
            Band = Digit9;

            //MessageBox.Show(Band);

            // ******************************************** Select Band

            if (Band == "1")
            {
                radioButton1.Checked = true;
            }
            else
            {
            }

            if (Band == "2")
            {
                radioButton2.Checked = true;
            }
            else
            {
            }

            if (Band == "3")
            {
                radioButton3.Checked = true;
            }
            else
            {
            }

            if (Band == "4")
            {
                radioButton4.Checked = true;
            }
            else
            {
            }

            if (Band == "5")
            {
                radioButton5.Checked = true;
            }
            else
            {
            }

            if (Band == "6")
            {
                radioButton6.Checked = true;
            }
            else
            {
            }

            if (Band == "7")
            {
                radioButton7.Checked = true;
            }
            else
            {
            }

            if (Band == "8")
            {
                radioButton8.Checked = true;
            }
            else
            {
            }

            if (Band == "9")
            {
                radioButton9.Checked = true;
            }
            else
            {
            }

            if (Band == "10")
            {
                radioButton10.Checked = true;
            }
            else
            {
            }

            System.Threading.Thread.Sleep(50);
            CalculateBase10FrequencyValue_Click();

        }

        private void hScrollBar2_Scroll(object sender, ScrollEventArgs e)
        {

            // ****************************************** Scroll Bar 2 Coding Start


            // ****************************************** Split out comma seperated string "value"
            var values = (hScrollBar2.Value).ToString().ToCharArray();

            //var Dig1 = "0";
            //var Dig2 = "0";
            var Dig3 = "0";
            var Dig4 = "0";
            var Dig5 = "0";
            var Dig6 = "0";
            var Dig7 = "0";
            var Dig8 = "0";

            if (values.Length > 7)
            {

                //Dig1 = values[0].ToString();
                //Dig2 = values[1].ToString();
                Dig3 = values[2].ToString();
                Dig4 = values[3].ToString();
                Dig5 = values[4].ToString();
                Dig6 = values[5].ToString();
                //Dig7 = values[6].ToString();
                //Dig8 = values[7].ToString();

            }
            else
            {
                //Dig2 = values[0].ToString();
                Dig3 = values[1].ToString();
                Dig4 = values[2].ToString();
                Dig5 = values[3].ToString();
                Dig6 = values[4].ToString();
                //Dig7 = values[5].ToString();
                //Dig8 = values[6].ToString();

            }



            // ******************************************** NumericUpDown Digits Value Update

            //numericUpDown1.Value = Convert.ToInt32(Dig1);
            //numericUpDown2.Value = Convert.ToInt32(Dig2);
            numericUpDown3.Value = Convert.ToInt32(Dig3);
            numericUpDown4.Value = Convert.ToInt32(Dig4);
            numericUpDown5.Value = Convert.ToInt32(Dig5);
            numericUpDown6.Value = Convert.ToInt32(Dig6);
            //numericUpDown7.Value = Convert.ToInt32(Dig7);
            //numericUpDown8.Value = Convert.ToInt32(Dig8);


            // ********************************************* Scroll Bar 2 Coding Stop

        }

        private void button13_Click(object sender, EventArgs e)
        {
            button13.ForeColor = Color.Red;
            button14.ForeColor = Color.Black;

            DoWork1 = true;

            // Allocates a method to run in the background worker
            ScanmemBackgroundWorker.DoWork += ScanmembackgroundWorker_DoWork;
            ScanmemBackgroundWorker.RunWorkerAsync();
        }

        private void ScanmembackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (DoWork1)
            {
                Thread.Sleep(TimeSpan.FromSeconds(scanspeed));

                if (DoWork1)
                {

                    button6.Invoke((MethodInvoker)delegate
                    {
                        button6_Click_1(null, null);
                    });
                }

                Thread.Sleep(TimeSpan.FromSeconds(scanspeed));

                if (DoWork1)
                {

                    button8.Invoke((MethodInvoker)delegate
                    {
                        button8_Click_1(null, null);
                    });
                }

                Thread.Sleep(TimeSpan.FromSeconds(scanspeed));

                if (DoWork1)
                {

                    button10.Invoke((MethodInvoker)delegate
                    {
                        button10_Click_1(null, null);
                    });
                }

                Thread.Sleep(TimeSpan.FromSeconds(scanspeed));

                if (DoWork1)
                {

                    button12.Invoke((MethodInvoker)delegate
                    {
                        button12_Click_1(null, null);
                    });
                }
                Thread.Sleep(TimeSpan.FromSeconds(scanspeed));

                if (DoWork1)
                {

                    button20.Invoke((MethodInvoker)delegate
                    {
                        button20_Click(null, null);
                    });
                }
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {

            button14.ForeColor = Color.Red;
            button13.ForeColor = Color.Black;

            DoWork1 = false;
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button16_Click(object sender, EventArgs e)
        {
            button16.ForeColor = Color.Red;
            button15.ForeColor = Color.Black;
            button17.ForeColor = Color.Black;

            scanspeed = 3;
        }

        private void button17_Click(object sender, EventArgs e)
        {
            button17.ForeColor = Color.Red;
            button15.ForeColor = Color.Black;
            button16.ForeColor = Color.Black;

            scanspeed = 9;

        }

        private void button15_Click(object sender, EventArgs e)
        {

            button15.ForeColor = Color.Red;
            button16.ForeColor = Color.Black;
            button17.ForeColor = Color.Black;

            scanspeed = 1;
        }



        private void button21_Click(object sender, EventArgs e)
        {
            Console.Beep(2000, 200);
            //Console.Beep(1500, 200);

            
            // Digit and Band Selection * Frequency digit positions 0,1,2,3,4,5,6,7, + Band ID 8.  END

            Freqstring = $"{numericUpDown1.Value + ","}{numericUpDown2.Value + ","}{numericUpDown3.Value + ","}{numericUpDown4.Value + ","}" +
                            $"{numericUpDown5.Value + ","}{numericUpDown6.Value + ","}{numericUpDown7.Value + ","}{numericUpDown8.Value + ","}{Band}";

            File.WriteAllText(SaveFile5, Freqstring);

            button20.PerformClick();

            // MessageBox.Show(Freqstring);
        }

        private void button20_Click(object sender, EventArgs e)
        {
            
            if (soundenable == 1)
            {
                Console.Beep(1500, 200);
            }

                     
            label3.BackColor = Color.White;
            label4.BackColor = Color.White;
            label5.BackColor = Color.White;
            label6.BackColor = Color.White;
            label7.BackColor = Color.LightGreen;

            var value = File.ReadAllText(SaveFile5);
            
            var values = value.Split(',');

            var Digit1 = values[0];
            var Digit2 = values[1];
            var Digit3 = values[2];
            var Digit4 = values[3];
            var Digit5 = values[4];
            var Digit6 = values[5];
            var Digit7 = values[6];
            var Digit8 = values[7];
            var Digit9 = values[8]; // Band ID bit

            textBox10.Text = Digit1 + Digit2 + Digit3 + Digit4 + Digit5 + Digit6 + Digit7 + Digit8;

            // ******************************************** Digits Update

            numericUpDown1.Value = Convert.ToInt32(Digit1);
            numericUpDown2.Value = Convert.ToInt32(Digit2);
            numericUpDown3.Value = Convert.ToInt32(Digit3);
            numericUpDown4.Value = Convert.ToInt32(Digit4);
            numericUpDown5.Value = Convert.ToInt32(Digit5);
            numericUpDown6.Value = Convert.ToInt32(Digit6);
            numericUpDown7.Value = Convert.ToInt32(Digit7);
            numericUpDown8.Value = Convert.ToInt32(Digit8);
            Band = Digit9;

            //MessageBox.Show(Band);

            // ******************************************** Select Band

            if (Band == "1")
            {
                radioButton1.Checked = true;
            }
            else
            {
            }

            if (Band == "2")
            {
                radioButton2.Checked = true;
            }
            else
            {
            }

            if (Band == "3")
            {
                radioButton3.Checked = true;
            }
            else
            {
            }

            if (Band == "4")
            {
                radioButton4.Checked = true;
            }
            else
            {
            }

            if (Band == "5")
            {
                radioButton5.Checked = true;
            }
            else
            {
            }

            if (Band == "6")
            {
                radioButton6.Checked = true;
            }
            else
            {
            }

            if (Band == "7")
            {
                radioButton7.Checked = true;
            }
            else
            {
            }

            if (Band == "8")
            {
                radioButton8.Checked = true;
            }
            else
            {
            }

            if (Band == "9")
            {
                radioButton9.Checked = true;
            }
            else
            {
            }

            if (Band == "10")
            {
                radioButton10.Checked = true;
            }
            else
            {
            }

            System.Threading.Thread.Sleep(50);
            CalculateBase10FrequencyValue_Click();
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void button18_Click(object sender, EventArgs e)
        {

            Dialfreq = (Convert.ToString(numericUpDown1.Value) + Convert.ToString(numericUpDown2.Value) + Convert.ToString(numericUpDown3.Value) + Convert.ToString(numericUpDown4.Value) +
            Convert.ToString(numericUpDown5.Value) + Convert.ToString(numericUpDown6.Value) + Convert.ToString(numericUpDown7.Value) + Convert.ToString(numericUpDown8.Value));


             if (hScrollBar2.Value > hScrollBar2.Minimum)
            {
               hScrollBar2.Value = Convert.ToInt32(Dialfreq);
             }

            if (hScrollBar2.Value < hScrollBar2.Maximum)
            {
              hScrollBar2.Value = Convert.ToInt32(Dialfreq);
            }

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void textBox6_TextChanged_1(object sender, EventArgs e)
        {

        }

        
    }
}



