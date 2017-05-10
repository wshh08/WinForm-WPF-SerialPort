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
        private SerialPort motor_SerialPort; //电机串口
        private SerialPort press_SerialPort; //压力与倾角传感器串口
        private SerialPort angle_SerialPort; //角度传感器串口

        #region comboBox控件
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

        private void Press_comboBox_DropDownClosed(object sender, EventArgs e)
        {
            //压力及倾角串口下拉窗口关闭后执行
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

                        if (press_SerialPort.IsOpen)   //如果电机正在使用串口，则关闭串口以备初始化
                            press_SerialPort.Close();

                        //电机串口初始化
                        InitPort(press_SerialPort, SPCount[i]);
                        press_SerialPort.DataReceived += new SerialDataReceivedEventHandler(press_DataReceived);  //接收事件处理方法press_DataReceived即电机的算法
                    }
                }
            }
        }

        private void Angle_comboBox_DropDownClosed(object sender, EventArgs e)
        {
            //角度传感器串口下拉窗口关闭后执行
            ComboBoxItem item = Angle_comboBox.SelectedItem as ComboBoxItem; //下拉窗口当前选中的项赋给item
            string tempstr = item.Content.ToString();                        //将选中的项目转为字串存储在tempstr中

            for (int i = 0; i < SPCount.Length; i++)
            {
                if (tempstr == "串口" + SPCount[i])
                {
                    //当选中串口为串口SPCount[i]时
                    if (motor_com == SPCount[i] || press_com == SPCount[i])
                    {
                        //电机或角度传感器已占用串口SPCount[i]时
                        MessageBox.Show("串口" + SPCount[i] + "已被占用!");
                    }
                    else
                    {
                        angle_com = SPCount[i];

                        if (angle_SerialPort.IsOpen)   //如果电机正在使用串口，则关闭串口以备初始化
                            angle_SerialPort.Close();

                        //电机串口初始化
                        InitPort(angle_SerialPort, SPCount[i]);
                        angle_SerialPort.DataReceived += new SerialDataReceivedEventHandler(angle_DataReceived);  //接收事件处理方法angle_DataReceived即电机的算法

                        SendCommands();
                    }
                }
            }
        }
        #endregion


    }
}
