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

    public partial class MainWindow : Window
    {
        //事件类
        private SerialPort motor_SerialPort;
        private SerialPort press_SerialPort;
        private SerialPort angle_SerialPort;

        private void Motor_comboBox_DropDownClosed(object sender, EventArgs e)
        {
            //电机及控制串口下拉窗口关闭后执行
            ComboBoxItem item = Motor_comboBox.SelectedItem as ComboBoxItem; //下拉窗口当前选中的项赋给item
            string tempstr = item.Content.ToString();                        //将选中的项目转为字串存储在tempstr中

            for (int i = 0; i < SPCount.Length; i++)
            {
                if(tempstr == "串口" + SPCount[i])
                {
                    //当选中串口为串口SPCount[i]时
                    if(press_com == SPCount[i] || angle_com == SPCount[i])
                    {
                        //压力与倾角传感器或倾角传感器已占用串口SPCount[i]时
                        MessageBox.Show("串口" + SPCount[i] + "已被占用!");
                    }
                    else
                    {
                        motor_com = SPCount[i];

                        if (motor_SerialPort.IsOpen)   //如果电机正在使用串口，则关闭串口以备初始化
                            motor_SerialPort.Close();

                        motor_SerialPort = new SerialPort();
                        motor_SerialPort.PortName = SPCount[i];
                        motor_SerialPort.BaudRate = 115200;
                        motor_SerialPort.Parity = Parity.None;
                        motor_SerialPort.StopBits = StopBits.One;
                        motor_SerialPort.Open();
                        motor_SerialPort.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(controlPort_DataReceived);
                    }
                }
            }
        }

        private void controlPort_DataReceived(object sender, SerialDataReceivedEventArgs e)//压力倾角串口接收代码
        {
            try
            {
                int bufferlen = motor_SerialPort.BytesToRead;//先记录下来，避免某种原因，人为的原因，操作几次之间时间长，缓存不一致
                if (bufferlen >= 27)
                {
                    byte[] bytes = new byte[bufferlen];//声明一个临时数组存储当前来的串口数据
                    motor_SerialPort.Read(bytes, 0, bufferlen);//读取串口内部缓冲区数据到buf数组
                    motor_SerialPort.DiscardInBuffer();//清空串口内部缓存
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
            catch
            {


            }
        }
    }
}
