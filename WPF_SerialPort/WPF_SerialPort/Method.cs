using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.IO.Ports;

namespace WPF_SerialPort
{
    //方法类
    public partial class MainWindow : Window
    {
        SerialPort serialPort1 = new SerialPort();

        private void ScanSerialPort(SerialPort MyPort, ComboBox MyBox)
        {
            //扫描可用按钮
            string Buffer;               //缓存
            MyBox.Items.Clear();         //清空端口下拉菜单
            for (int i = 1; i < 20; i++)
            {
                try
                {
                    Buffer = "COM" + i.ToString();
                    MyPort.PortName = Buffer;
                    MyPort.Open();
                    MyBox.Items.Add(Buffer);   //如果串口打开成功，说明可用，将其名称添加到下拉菜单中
                    MyPort.Close();
                }
                catch
                {
                    //ignore
                }
            }
        }

        private void InitializeWPF()
        {
            //打开窗口进行的初始化方法，代替WinForm中的Form1_Load方法
            for (int i = 1; i <= 20; i++)
            {
                SerialPortComboBox.Items.Add("COM" + i.ToString());        //添加端口下拉菜单选择
            }
            SerialPortComboBox.Text = "COM1";                              //设定端口下拉菜单默认值         
            BaudComboBox.Text = "4800";                                    //设定波特率下拉菜单默认值

            serialPort1.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);   //必须手动添加串口数据接收事件的处理方法

            ScanSerialPort(serialPort1, SerialPortComboBox);
        }

        private void port_DataReceived(object sender, SerialDataReceivedEventArgs e) 
        {
            //串口数据接收事件
            if (ReceivedValueRadioButton.IsChecked == false) //若接收模式为字符模式
            {
                string str = serialPort1.ReadExisting();    //字符串方式读
                ReceivedRichTextBox.AppendText(str); //接收文本框添加接收数据内容
            }
            else                                  //若接收模式为数值模式
            {
                byte data = (byte)serialPort1.ReadByte(); //将从串口接收的int型数值转换为byte型数值（相当于C语言中的unchar）
                string str = Convert.ToString(data, 16).ToUpper(); //将byte型转为十六进制字符串
                ReceivedRichTextBox.AppendText("0x" + (str.Length % 2 == 1 ? str + "0" : str) + " ");  //若为一位则在前面补0
            }
        }
    }
}
