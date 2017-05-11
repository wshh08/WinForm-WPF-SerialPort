using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.IO.Ports;
using System.Windows.Threading;
using System.IO;
using System.Windows.Navigation;
using System.Windows.Media;

namespace Skeleton
{

    public partial class MainWindow : Window
    {

        #region 定义参数
        //打开窗口
        private DispatcherTimer ShowTimer;

        //关闭窗口
        //private DispatcherTimer ShowTimer1;  //角度用Timer

        //串口
        private SerialPort motor_SerialPort = new SerialPort(); //电机串口
        private SerialPort press_SerialPort = new SerialPort(); //压力与倾角传感器串口
        private SerialPort angle_SerialPort = new SerialPort(); //角度传感器串口

        //4个电机所需参数
        public byte[] enable = new byte[4];       //使能
        public byte[] direction = new byte[4];    //方向
        public double[] speed = new double[4];    //转速
        public double[] current = new double[4];   //电流

        //8个压力传感器所需参数
        //4个倾角传感器（分别有x轴和y轴）所需参数
        public Int16[] tempPress = new Int16[8];   //存储压力AD转换后的值（0-4096）
        private Int16[] tempAngle = new Int16[8];  //存储倾角AD转换后的值（0-4096）
        public double[] dirangle = new double[8];  //存储角度值（-90°到90°）

        //6个角度传感器所需参数
        public double[] _angle = new double[6];
        private int _number = 0;
        private double[] _angleInitialization = new double[6];

        //发送按钮所需参数
        private byte[] cmdSendBytes = new byte[19]; //储存从电机控制窗口输入的字节命令
        public int choosecount = 0;                 //记录已添加命令的电机的个数

        //开始采集按钮所需参数
        private StreamWriter wr;
        private FileStream fs;
        private bool isPick = false;
        private DispatcherTimer showTimer1 = new DispatcherTimer(); //作为实行PickCode存TXT的Timer
        #endregion

        #region 打开窗口及文本输出
        private void Window_Loaded(object sender, RoutedEventArgs e)//打开窗口后进行的初始化操作
        {
            ShowTimer = new System.Windows.Threading.DispatcherTimer();
            ShowTimer.Tick += new EventHandler(ShowCurTimer); //Tick是超过计时器间隔时发生事件，此处为Tick增加了一个叫ShowCurTimer的取当前时间并扫描串口的委托
            ShowTimer.Interval = new TimeSpan(0, 0, 0, 1, 0); //设置刻度之间的时间值，设定为1秒（即文本框会1秒改变一次输出文本？）
            ShowTimer.Start();

            DispatcherTimer showTimer = new DispatcherTimer();
            showTimer.Tick += new EventHandler(ShowSenderTimer); //增加了一个叫ShowSenderTimer的在电机和传感器的只读文本框中输出信息的委托
            showTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);  //文本变化间隔是100毫秒
            showTimer.Start();

        }

        public void ShowCurTimer(object sender, EventArgs e)//取当前时间的委托
        {
            string nowTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            Time_textBox.Text = nowTime;
            ScanPorts();
        }

        public void ShowSenderTimer(object sender, EventArgs e)//电机状态，压力，倾角，角度传感器状态的文本输出
        {
            //电机1的文本框输出
            Motor1_enable_textBox.Text = enable[0].ToString();             //使能
            Motor1_direction_textBox.Text = direction[0].ToString("F");    //方向；"F"格式，默认保留两位小数
            Motor1_speed_textBox.Text = speed[0].ToString("F");            //转速
            Motor1_current_textBox.Text = current[0].ToString("F");        //电流
            //电机2的文本框输出
            Motor2_enable_textBox.Text = enable[1].ToString();
            Motor2_direction_textBox.Text = direction[1].ToString("F");
            Motor2_speed_textBox.Text = speed[1].ToString("F");
            Motor2_current_textBox.Text = current[1].ToString("F");
            //电机3的文本框输出
            Motor3_enable_textBox.Text = enable[2].ToString();
            Motor3_direction_textBox.Text = direction[2].ToString("F");
            Motor3_speed_textBox.Text = speed[2].ToString("F");
            Motor3_current_textBox.Text = current[2].ToString("F");
            //电机4的文本框输出
            Motor4_enable_textBox.Text = enable[3].ToString();
            Motor4_direction_textBox.Text = direction[3].ToString("F");
            Motor4_speed_textBox.Text = speed[3].ToString("F");
            Motor4_current_textBox.Text = current[3].ToString("F");

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

            //6个角度传感器的文本框输出
            Angle1_Textbox.Text = _angle[0].ToString("F");
            Angle2_Textbox.Text = _angle[1].ToString("F");
            Angle3_Textbox.Text = _angle[2].ToString("F");
            Angle4_Textbox.Text = _angle[3].ToString("F");
            Angle5_Textbox.Text = _angle[4].ToString("F");
            Angle6_Textbox.Text = _angle[5].ToString("F");
        }
        #endregion

        #region 关闭窗口
        private void Window_Closed(object sender, EventArgs e)//串口去掉相应接受数据的委托，及关闭串口
        {
            if (motor_SerialPort != null)
            {
                motor_SerialPort.DataReceived -= new System.IO.Ports.SerialDataReceivedEventHandler(motor_DataReceived);
                motor_SerialPort.Close();
            }
            if (press_SerialPort != null)
            {
                press_SerialPort.DataReceived -= new System.IO.Ports.SerialDataReceivedEventHandler(press_DataReceived);
                press_SerialPort.Close();
            }
            if (angle_SerialPort != null)
            {
                angle_SerialPort.DataReceived -= new System.IO.Ports.SerialDataReceivedEventHandler(angle_DataReceived);
                //ShowTimer1.Stop();
                angle_SerialPort.Close();
            }
        }
        #endregion

        #region comboBox控件

        #region 电机算法
        private void Motor_comboBox_DropDownClosed(object sender, EventArgs e)//电机及控制串口下拉窗口关闭后执行
        {
            ComboBoxItem item = Motor_comboBox.SelectedItem as ComboBoxItem; //下拉窗口当前选中的项赋给item
            string tempstr = item.Content.ToString();                        //将选中的项目转为字串存储在tempstr中

            for (int i = 0; i < SPCount.Length; i++)
            {
                if(tempstr == "串口" + SPCount[i])
                {
                    //当选中串口为串口SPCount[i]时
                    if(press_com == SPCount[i] || angle_com == SPCount[i])
                    {
                        //压力与倾角传感器或角度传感器已占用串口SPCount[i]时
                        MessageBox.Show("串口" + SPCount[i] + "已被占用!");
                    }
                    else
                    {
                        motor_com = SPCount[i];

                        if (motor_SerialPort.IsOpen)   //如果电机正在使用串口，则关闭串口以备初始化
                            motor_SerialPort.Close();

                        //电机串口初始化
                        InitPort(motor_SerialPort, SPCount[i]);
                        motor_SerialPort.DataReceived += new SerialDataReceivedEventHandler(motor_DataReceived);  //接收事件处理方法motor_DataReceived即电机的算法
                    }
                }
            }
        }
        private void motor_DataReceived(object sender, SerialDataReceivedEventArgs e)//电机及控制串口接收数据（算法）
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
        private void Press_comboBox_DropDownClosed(object sender, EventArgs e)//压力及倾角串口下拉窗口关闭后执行
        {
            ComboBoxItem item = Press_comboBox.SelectedItem as ComboBoxItem; //下拉窗口当前选中的项赋给item
            string tempstr = item.Content.ToString();                        //将选中的项目转为字串存储在tempstr中

            for (int i = 0; i < SPCount.Length; i++)
            {
                if (tempstr == "串口" + SPCount[i])
                {
                    //当选中串口为串口SPCount[i]时
                    if (motor_com == SPCount[i] || angle_com == SPCount[i])
                    {
                        //电机或角度传感器已占用串口SPCount[i]时
                        MessageBox.Show("串口" + SPCount[i] + "已被占用!");
                    }
                    else
                    {
                        press_com = SPCount[i];

                        if (press_SerialPort.IsOpen)   //如果压力与倾角传感器正在使用串口，则关闭串口以备初始化
                            press_SerialPort.Close();

                        //压力与倾角传感器串口初始化
                        InitPort(press_SerialPort, SPCount[i]);
                        press_SerialPort.DataReceived += new SerialDataReceivedEventHandler(press_DataReceived);  //接收事件处理方法press_DataReceived即电机的算法
                    }
                }
            }
        }
        public void press_DataReceived(object sender, SerialDataReceivedEventArgs e)//接受压力相关数据的委托事件（算法）
        {
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
        private void Angle_comboBox_DropDownClosed(object sender, EventArgs e)//角度传感器串口下拉窗口关闭后执行
        {
            ComboBoxItem item = Angle_comboBox.SelectedItem as ComboBoxItem; //下拉窗口当前选中的项赋给item
            string tempstr = item.Content.ToString();                        //将选中的项目转为字串存储在tempstr中

            for (int i = 0; i < SPCount.Length; i++)
            {
                if (tempstr == "串口" + SPCount[i])
                {
                    //当选中串口为串口SPCount[i]时
                    if (motor_com == SPCount[i] || press_com == SPCount[i])
                    {
                        //电机或压力与倾角传感器已占用串口SPCount[i]时
                        MessageBox.Show("串口" + SPCount[i] + "已被占用!");
                    }
                    else
                    {
                        angle_com = SPCount[i];

                        if (angle_SerialPort.IsOpen)   //如果角度传感器正在使用串口，则关闭串口以备初始化
                            angle_SerialPort.Close();

                        //电机串口初始化
                        InitPort(angle_SerialPort, SPCount[i]);
                        angle_SerialPort.DataReceived += new SerialDataReceivedEventHandler(angle_DataReceived);  //接收事件处理方法angle_DataReceived即电机的算法

                        SendCommands();
                    }
                }
            }
        }
        private void angle_DataReceived(object sender, SerialDataReceivedEventArgs e)//对角度传感器串口增加的委托事件（算法）
        {
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
        #endregion

        #endregion

        #region Button控件
        private void Add_button_Click(object sender, RoutedEventArgs e)//点击【添加命令】按钮时执行
        {
            int add_enable = Convert.ToInt16(MotorControl_enable_textBox.Text);
            int add_direction = Convert.ToInt16(MotorControl_direction_textBox.Text);
            int add_speed = Convert.ToInt16(MotorControl_speed_textBox.Text);
            int add_motorNum = Convert.ToInt16(MotorControl_motorNum_textBox.Text);
                     
            if (add_motorNum != 1 && add_motorNum != 2 && add_motorNum != 3 && add_motorNum != 4)
                MessageBox.Show("选择电机号请输入1或2或3或4");
            else
            {
                if (add_enable != 0 && add_enable != 1)
                    MessageBox.Show("选择是否使能请输入0或1");
                else
                {
                    if (add_speed > 16200 || add_speed < 0)
                        MessageBox.Show("输入转速无效，请在范围0~16200内选择");
                    else
                    {
                        /*？？？似乎还缺少方向输入的判断？？？*/
                        choosecount++;

                        if (choosecount < 5) /*？？？重新输入重号的电机怎么办，此处注定一次输入只能最多输入4个？？？*/
                        MotorControl_chosenCount_textBox.Text = choosecount.ToString(); //显示已添加电机数

                        byte add_enablebyte = Convert.ToByte(add_enable);           //使能数值命令转字节命令
                        byte add_directionbyte = Convert.ToByte(add_direction);     //方向数值命令转字节命令
                        byte[] add_speedbytes = BitConverter.GetBytes(add_speed);   //转速数值命令转字节命令
                        byte add_motorNumbyte = Convert.ToByte(add_motorNum);       //电机号数值命令转字节命令

                        cmdSendBytes[0] = 0x23;//开始字符
                        cmdSendBytes[17] = 0x0D;//结束字符
                        cmdSendBytes[18] = 0x0A;

                        switch (add_motorNum)
                        {
                            case 1: //电机1添加字节命令
                                cmdSendBytes[1] = add_enablebyte;
                                cmdSendBytes[2] = add_directionbyte;
                                cmdSendBytes[3] = add_speedbytes[1];
                                cmdSendBytes[4] = add_speedbytes[0];
                                In_textBox.Text = "电机" + add_motorNum + "使能" + add_enable + "转速" + add_speed;
                                break;
                            case 2: //电机2添加字节命令
                                cmdSendBytes[5] = add_enablebyte;
                                cmdSendBytes[6] = add_directionbyte;
                                cmdSendBytes[7] = add_speedbytes[1];
                                cmdSendBytes[8] = add_speedbytes[0];
                                In_textBox.Text = "电机" + add_motorNum + "使能" + add_enable + "转速" + add_speed;
                                break;
                            case 3: //电机3添加字节命令
                                cmdSendBytes[9] = add_enablebyte;
                                cmdSendBytes[10] = add_directionbyte;
                                cmdSendBytes[11] = add_speedbytes[1];
                                cmdSendBytes[12] = add_speedbytes[0];
                                In_textBox.Text = "电机" + add_motorNum + "使能" + add_enable + "转速" + add_speed;
                                break;
                            case 4: //电机4添加字节命令
                                cmdSendBytes[13] = add_enablebyte;
                                cmdSendBytes[14] = add_directionbyte;
                                cmdSendBytes[15] = add_speedbytes[1];
                                cmdSendBytes[16] = add_speedbytes[0];
                                In_textBox.Text = "电机" + add_motorNum + "使能" + add_enable + "转速" + add_speed;
                                break;
                        }
                    }
                }
            }
        }

        private void Send_button_Click(object sender, RoutedEventArgs e)//点击【发送命令】按钮时执行
        {
            if (cmdSendBytes != null)
            {
                //当cmdSendBytes中包含字节命令时
                Send_button.Content = "正在发送...";           //改变发送命令按钮的文本为“正在发送...”，表示正在给电机串口写入字节命令  
                try
                {
                    motor_SerialPort.Write(cmdSendBytes, 0, 19);   //给电机串口写入字节命令
                }
                catch(Exception err)
                {
                    MessageBox.Show(err.ToString());
                }
                Send_button.Content = "发送命令";              //命令发送完毕后按钮文本变回来
                Out_textBox.Text = "已发送至电机";             //输出信息窗口显示“已发送至电机”
                cmdSendBytes = new byte[19] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; //重置字节命令存储器cmdSendBytes
                In_textBox.Text = "";                          //清空输入信息窗口
                choosecount = 0;                               //重置已添加命令电机个数
            }
            else
            {
                MessageBox.Show("未给电机添加参数");
            }

        }

        private void MotorStop_button_Click(object sender, RoutedEventArgs e)//点击【电机停止】按钮时执行
        {
            byte[] clearBytes = new byte[19] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            clearBytes[0] = 0x23;//开始字符
            clearBytes[17] = 0x0D;//结束字符
            clearBytes[18] = 0x0A;
            try
            {
                motor_SerialPort.Write(clearBytes, 0, 19); //将清除了包含电机使能，方向，转速的命令写入电机串口
            }
            catch(Exception err)
            {
                MessageBox.Show(err.ToString());
            }
        }

        private void StartPick_button_Click(object sender, RoutedEventArgs e)//点击【开始采集】按钮时执行
        {
            int setTime = Convert.ToInt16(SetTime_textBox.Text);      //将采集率文本框中的字符串转为整型数存到setTime里
            if (setTime > 0)
            {
                string nowTime = System.DateTime.Now.ToString("yyyyMMddHHmmssfff");
                string filestream = nowTime + ".txt";                       
                fs = new FileStream(filestream, FileMode.Append); //可写入数据的文件是命名为当前时间的txt即yyyyMMddHHmmssfff.txt
                wr = new StreamWriter(fs);             //wr作为写入数据的对象在PickCode方法中被调用并被写入数据
                StartPick_button.IsEnabled = false;    //开始采集按钮无法再按

                showTimer1.Tick += new EventHandler(PickCode);
                showTimer1.Interval = new TimeSpan(0, 0, 0, 0, setTime);   //根据采样率setTime设置时间间隔，过一段setTime所表示的毫秒则执行一次PickCode方法
                showTimer1.Start();

                isPick = true;
            }
            else
            {
                MessageBox.Show("采样时间设置应大于0！");
            }
        }

        private void EndPick_button_Click(object sender, RoutedEventArgs e)//点击【结束采集】按钮时执行
        {
            StartPick_button.IsEnabled = true; //开始采集按钮此时变为可按
            wr.Close();
            fs.Close();
            dataString = null;
            isPick = false;
            showTimer1.Stop();
        }

        private void AngleInit_button_Click(object sender, RoutedEventArgs e)//点击【角度初始化】按钮时执行
        {
            AngleInit_button.Background = new SolidColorBrush(Color.FromArgb(255, 173, 255, 47));
            Angle_SerialPortInit();
            AngleInit_button.Content = "初始化已完成";
            AngleInit_button.IsEnabled = false;
        }

        private void Reset_button_Click(object sender, RoutedEventArgs e)//点击【角度初始化】按钮时执行
        {
            AngleInit_button.Background = new SolidColorBrush(Color.FromArgb(255, 240, 245, 255));
            AngleInit_button.IsEnabled = true;
            AngleInit_button.Content = "角度初始化";
            angleIntReset();
        }
        #endregion

    }
}
