using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.IO.Ports;


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

        //开始采集
        private string dataString = null;

        public void ScanPorts()//扫描可用串口
        {
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

        public void InitPort(SerialPort myPort, string portName)//串口初始化
        {
            myPort = new SerialPort();
            myPort.PortName = portName;
            myPort.BaudRate = 115200;
            myPort.Parity = Parity.None;
            myPort.StopBits = StopBits.One;
            myPort.Open();
        }

        private void SendCommands()//角度串口写入字节命令
        {
            byte[] command = new byte[9]; //byte类型用于存放二进制数据
            command[0] = 0x77;            //0x开头的是十六进制数据
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

        private void PickCode(object sender, EventArgs e)//开始采集
        {
            if (isPick)
            {
                dataString += DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString() + ":" + DateTime.Now.Millisecond.ToString(); //记录此时的时间

                for (int f = 0; f < 8; f++)//记录此时8个压力传感器的值，保留2位小数
                {
                    dataString += " " + tempPress[f].ToString("F2");
                }
                for (int f = 0; f < 6; f++)//记录此时6个角度传感器的值
                {
                    dataString += " " + _angle[f].ToString("F2");
                }
                for (int f = 0; f < 2; f++)//记录此时第1个倾角传感器x轴和y轴的值
                {
                    dataString += " " + dirangle[f].ToString("F2");
                }
                for (int f = 6; f < 8; f++)//记录此时第4个倾角传感器x轴和y轴的值
                {
                    dataString += " " + dirangle[f].ToString("F2");
                }
                dataString += System.Environment.NewLine; //换行

                wr.Write(dataString);  //将以上记录的数据写入文件
                wr.Flush();         /*？？？确保数据都已写入文件？？？*/
                dataString = null;  //清空数据
            }

        }

        public void Angle_SerialPortInit()//角度初始化
        {
            for (int i = 0; i < 6; i++)
                _angleInitialization[i] = 0;

            int numberOfGather = 5;
            for (int i = 0; i < numberOfGather; i++)
            {
                for (int j = 0; j < 6; j++)
                    _angleInitialization[j] += _angle[j];
            }
            for (int i = 0; i < 6; i++)
                _angleInitialization[i] /= numberOfGather;
        }

        public void angleIntReset()//重置
        {
            for (int i = 0; i < 6; i++)
                _angleInitialization[i] = 0;
        }

    }
}
