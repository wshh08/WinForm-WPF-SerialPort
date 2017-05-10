using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.IO.Ports;
using System.Windows.Threading;

namespace Skeleton
{
    //方法类
    public partial class MainWindow : Window
    {
        //扫描串口
        public string[] SPCount = null;           //用来存储计算机串口名称数组
        public int comcount = 0;                  //用来存储计算机可用串口数目，初始化为0
        public string motor_com = null;           //存储电机所用串口
        public string press_com = null;           //存储压力及倾角传感器所用串口
        public string angle_com = null;           //存储角度传感器所用串口
        public bool flag = false;

        //4个电机
        public byte[] enable = new byte[4];       //使能
        public byte[] direction = new byte[4];    //方向
        public double[] speed = new double[4];    //转速
        public double[] current = new double[4];   //电流

        //8个压力传感器
        //4个倾角传感器（分别有x轴和y轴）
        public Int16[] tempPress = new Int16[8];   //存储压力AD转换后的值（0-4096）
        private Int16[] tempAngle = new Int16[8];  //存储倾角AD转换后的值（0-4096）
        public double[] dirangle = new double[8];  //存储角度值（-90°到90°）

        //6个角度传感器
        public double[] _angle = new double[6];
        private int _number = 0;
        private double[] _angleInitialization = new double[6];

        //窗口初始化
        private DispatcherTimer ShowTimer;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //打开窗口后进行的初始化操作

            ShowTimer = new System.Windows.Threading.DispatcherTimer();
            ShowTimer.Tick += new EventHandler(ShowCurTimer); //Tick是超过计时器间隔时发生事件，此处为Tick增加了一个叫ShowCurTimer的取当前时间并扫描串口的委托
            ShowTimer.Interval = new TimeSpan(0, 0, 0, 1, 0); //设置刻度之间的时间值，设定为1秒（即文本框会1秒改变一次输出文本？）
            ShowTimer.Start();

            DispatcherTimer showTimer = new DispatcherTimer();
            showTimer.Tick += new EventHandler(ShowSenderTimer); //增加了一个叫ShowSenderTimer的在电机和传感器的只读文本框中输出信息的委托
            showTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);  //文本变化间隔是100毫秒
            showTimer.Start();

        }

        public void ShowCurTimer(object sender, EventArgs e)
        {
            //取当前时间的委托
            string nowTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            Time_textBox.Text = nowTime;
            ScanPorts();
        }

        public void ShowSenderTimer(object sender, EventArgs e)
        {
            //8个压力传感器的文本框输出
            Pressure1_Textbox.Text = tempPress[0].ToString();
            Pressure2_Textbox.Text = tempPress[1].ToString();
            Pressure3_Textbox.Text = tempPress[2].ToString();
            Pressure4_Textbox.Text = tempPress[3].ToString();
            Pressure5_Textbox.Text = tempPress[4].ToString();
            Pressure6_Textbox.Text = tempPress[5].ToString();
            Pressure7_Textbox.Text = tempPress[6].ToString();
            Pressure8_Textbox.Text = tempPress[7].ToString();

            //4个倾角传感器各自的x轴和y轴的文本框输出
            Dip1_x_TextBox.Text = dirangle[0].ToString("F");
            Dip1_y_TextBox.Text = dirangle[1].ToString("F");
            Dip2_x_TextBox.Text = dirangle[2].ToString("F");
            Dip2_y_TextBox.Text = dirangle[3].ToString("F");
            Dip3_x_TextBox.Text = dirangle[4].ToString("F");
            Dip3_y_TextBox.Text = dirangle[5].ToString("F");
            Dip4_x_TextBox.Text = dirangle[6].ToString("F");
            Dip4_y_TextBox.Text = dirangle[7].ToString("F");

            TextBoxAngle1.Text = _angle[0].ToString("F");
            TextBoxAngle2.Text = _angle[1].ToString("F");
            TextBoxAngle3.Text = _angle[2].ToString("F");
            TextBoxAngle4.Text = _angle[3].ToString("F");
            TextBoxAngle5.Text = _angle[4].ToString("F");
            TextBoxAngle6.Text = _angle[5].ToString("F");

            TextBoxa1.Text = enable[0].ToString();
            TextBoxa2.Text = direction[0].ToString("F");
            TextBoxa3.Text = speed[0].ToString("F");
            TextBoxa4.Text = current[0].ToString("F");

            TextBoxb1.Text = enable[1].ToString();
            TextBoxb2.Text = direction[1].ToString("F");
            TextBoxb3.Text = speed[1].ToString("F");
            TextBoxb4.Text = current[1].ToString("F");

            TextBoxc1.Text = enable[2].ToString();
            TextBoxc2.Text = direction[2].ToString("F");
            TextBoxc3.Text = speed[2].ToString("F");
            TextBoxc4.Text = current[2].ToString("F");

            TextBoxd1.Text = enable[3].ToString();
            TextBoxd2.Text = direction[3].ToString("F");
            TextBoxd3.Text = speed[3].ToString("F");
            TextBoxd4.Text = current[3].ToString("F");
        }

        #region 扫描串口
        public void ScanPorts()
        {
            //扫描可用串口
            SPCount = SerialPort.GetPortNames();      //获得计算机可用串口名称数组
            ComboBoxItem tempComboBoxItem = new ComboBoxItem();   
            if(comcount != SPCount.Length)            //SPCount.length其实就是可用串口的个数
            {
                //当可用串口计数器与实际可用串口个数不相符时
                //初始化三个下拉窗口并将flag初始化为false

                Motor_comboBox.Items.Clear();
                Press_comboBox.Items.Clear();
                Angle_comboBox.Items.Clear();

                tempComboBoxItem = new ComboBoxItem();
                tempComboBoxItem.Content = "请选择串口";
                Motor_comboBox.Items.Add(tempComboBoxItem);
                Motor_comboBox.SelectedIndex = 0;

                tempComboBoxItem = new ComboBoxItem();
                tempComboBoxItem.Content = "请选择串口";
                Press_comboBox.Items.Add(tempComboBoxItem);
                Press_comboBox.SelectedIndex = 0;

                tempComboBoxItem = new ComboBoxItem();
                tempComboBoxItem.Content = "请选择串口";
                Angle_comboBox.Items.Add(tempComboBoxItem);
                Angle_comboBox.SelectedIndex = 0;

                motor_com = null;
                press_com = null;
                angle_com = null;

                flag = false;

                if(comcount != 0)
                {
                    //在操作过程中增加或减少串口时发生
                    MessageBox.Show("串口数目已改变，请重新选择串口");
                }

                comcount = SPCount.Length;     //将可用串口计数器与现在可用串口个数匹配
            }

            if(!flag)
            {
                if(SPCount.Length > 0)
                {
                    //有可用串口时执行
                    comcount = SPCount.Length;

                    Out_textBox.Text = "检测到" + SPCount.Length + "个串口!";

                    for (int i = 0; i < SPCount.Length; i++)
                    {
                        //分别将可用串口添加到三个下拉窗口中
                        string tempstr = "串口" + SPCount[i];

                        tempComboBoxItem = new ComboBoxItem();
                        tempComboBoxItem.Content = tempstr;
                        Motor_comboBox.Items.Add(tempComboBoxItem);

                        tempComboBoxItem = new ComboBoxItem();
                        tempComboBoxItem.Content = tempstr;
                        Press_comboBox.Items.Add(tempComboBoxItem);

                        tempComboBoxItem = new ComboBoxItem();
                        tempComboBoxItem.Content = tempstr;
                        Angle_comboBox.Items.Add(tempComboBoxItem);
                    }

                    flag = true;

                }
                else
                {
                    comcount = 0;
                    Out_textBox.Text = "未检测到串口!";
                }
            }
        }
        #endregion

        #region 串口初始化
        public void InitPort(SerialPort myPort, string portName)
        {
            //串口初始化
            myPort = new SerialPort();
            myPort.PortName = portName;
            myPort.BaudRate = 115200;
            myPort.Parity = Parity.None;
            myPort.StopBits = StopBits.One;
            myPort.Open();
        }
        #endregion

        #region 电机算法
        private void motor_DataReceived(object sender, SerialDataReceivedEventArgs e)//压力倾角串口接收代码
        {
            try
            {
                int bufferlen = motor_SerialPort.BytesToRead;    //先记录下来，避免某种原因，人为的原因，操作几次之间时间长，缓存不一致
                if (bufferlen >= 27)
                {
                    byte[] bytes = new byte[bufferlen];          //声明一个临时数组存储当前来的串口数据
                    motor_SerialPort.Read(bytes, 0, bufferlen);  //读取串口内部缓冲区数据到buf数组
                    motor_SerialPort.DiscardInBuffer();          //清空串口内部缓存
                    //处理和存储数据
                    Int16 endFlag = BitConverter.ToInt16(bytes, 25);
                    if (endFlag == 2573)
                    {
                        if (bytes[0] == 0x23)
                            for (int f = 0; f < 4; f++)
                            {
                                enable[f] = bytes[f * 6 + 1];
                                direction[f] = bytes[f * 6 + 2];
                                speed[f] = bytes[f * 6 + 3] * 256 + bytes[f * 6 + 4];
                                if (speed[f] >= 2048) speed[f] = (speed[f] - 2048) / 4096 * 5180;
                                else speed[f] = (2048 - speed[f]) / 4096 * -5180;
                                current[f] = bytes[f * 6 + 5] * 256 + bytes[f * 6 + 6];
                                if (current[f] >= 2048) current[f] = (current[f] - 2048) / 4096 * 30;
                                else current[f] = (2048 - current[f]) / 4096 * -30;
                            }
                    }
                }
            }
            catch { }
        }
        #endregion

        #region 压力和倾角传感器算法
        public void press_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //接受压力相关数据的委托事件
            int bufferlen = press_SerialPort.BytesToRead;//先记录下来，避免某种原因，人为的原因，操作几次之间时间长，缓存不一致
            if (bufferlen >= 34)
            {
                byte[] bytes = new byte[bufferlen];//声明一个临时数组存储当前来的串口数据
                press_SerialPort.Read(bytes, 0, bufferlen);//读取串口内部缓冲区数据到buf数组
                press_SerialPort.DiscardInBuffer();//清空串口内部缓存
                                              //处理和存储数据
                Int16 endFlag = BitConverter.ToInt16(bytes, 32);
                if (endFlag == 2573)
                {
                    for (int f = 0; f < 8; f++)
                    {
                        tempPress[f] = BitConverter.ToInt16(bytes, f * 2);
                        tempAngle[f] = BitConverter.ToInt16(bytes, f * 2 + 16);
                        dirangle[f] = (Convert.ToDouble(tempAngle[f]) * (3.3 / 4096) - 0.7444) / 1.5 * 180;
                    }
                }
            }
        }
        #endregion

        #region 角度传感器算法
        private void angle_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //对角度传感器串口增加的委托事件
            _number = angle_SerialPort.BytesToRead;

            if (_number < 25)
                return;

            try
            {
                byte[] data = new byte[_number];
                angle_SerialPort.Read(data, 0, _number);

                if (0xFF -
                    (data[1] + data[2] + data[3] + data[4] + data[5] + data[6] + data[7] + data[8] + data[9] +
                     data[10] +
                     data[11] + data[12] + data[13] + data[14] + data[15] + data[16] + data[17] + data[18] +
                     data[19] +
                     data[20] + data[21] + data[22]) % 256 == data[23])
                {
                    // 左髋、左膝、右髋、右膝
                    _angle[0] = 256 * data[7] + data[6];
                    _angle[1] = 256 * data[9] + data[8];
                    _angle[2] = 256 * data[11] + data[10];
                    _angle[3] = 256 * data[13] + data[12];
                    _angle[4] = 256 * data[15] + data[14];
                    _angle[5] = 256 * data[17] + data[16];
                    for (int i = 0; i < 6; i++)
                    {
                        if (_angle[i] >= 0x1000)
                        {
                            _angle[i] = 0xFFFF - _angle[i] + 1;
                            _angle[i] *= -1;
                        }
                        _angle[i] = _angle[i] / 10.0 - _angleInitialization[i];
                    }

                }
            }
            catch { }
    
            SendCommands();
        }
        private void SendCommands()
        {
            //传送一些十六进制转二进制的数据作命令，但这些命令是干什么的呢？？？
            byte[] command = new byte[9]; //byte类型用于存放二进制数据
            command[0] = 0x77;   //0x开头的是十六进制数据
            command[1] = 0x00;
            command[2] = 0x01;
            command[3] = 0x01;
            command[4] = 0x00;
            command[5] = 0x0E;
            command[6] = 0x00;
            command[7] = Convert.ToByte(0xFF - (command[1] + command[2] + command[3] + command[4] + command[5] + command[6]));
            command[8] = 0xAA;

            angle_SerialPort.Write(command, 0, 9);
        }
        #endregion

    }
}
