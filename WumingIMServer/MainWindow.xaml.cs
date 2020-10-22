using fastJSON;
using MySql.Data;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace WumingIMServer
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        Settings settings = new Settings();
        bool ServerRunning = false;
        static Dictionary<string, Socket> Clients = new Dictionary<string, Socket> { };
        static Socket socket = null;
        bool SocketReady = false;
        RSAInfo Info;
        RSACryption rSACryption;

        public MainWindow()
        {
            //MessageBox.Show(Environment.CurrentDirectory + "\\logs\\" + DateTime.Now.ToString("yyyy-MM-dd") + ".log");
            InitializeComponent();
            Directory.CreateDirectory(Environment.CurrentDirectory + "\\logs");
            SetLog("====================WumingIMServer====================", "INFO");
            SetLog("正在为启动服务端做准备", "INFO");
            if (File.Exists("Config.xml"))
            {
                ReadConfigFile();
            }
            else
            {
                SetLog("配置文件不存在", "WARN");
                CreateConfigFile();
              //  SetLog("已创建配置文件", "INFO");
                ReadConfigFile();
            }
          

            SetLog("服务器将在端口"+settings.serverPort+"启动", "INFO");
            SetLog("====================WumingIMServer====================", "INFO");
        }

        private void CreateConfigFile()
        {
            SetLog("正在创建配置文件", "XML");
            XmlDocument xml = new XmlDocument();
            xml.AppendChild(xml.CreateXmlDeclaration("1.0", "UTF-8", null));
            XmlNode rootnode = xml.CreateElement("Settings");

            XmlNode ServerPort = xml.CreateElement("ServerPort"); //服务器端口
            ServerPort.InnerText = "11451";
            SetLog("已添加元素:ServerPort", "XML");
            XmlNode SqlServerIP = xml.CreateElement("SqlServerIP");
            SqlServerIP.InnerText = "localhost";
            SetLog("已添加元素:SqlServerIP", "XML");
            XmlNode SqlAccessAccount = xml.CreateElement("SqlAccessAccount");
            SqlAccessAccount.InnerText = "root";
            SetLog("已添加元素:SqlAccessAccount", "XML");
            XmlNode SqlAccessPassword = xml.CreateElement("SqlAccessPassword");
            SqlAccessPassword.InnerText = "root";
            SetLog("已添加元素:SqlAccessPassword", "XML");
            XmlNode SqlDBName = xml.CreateElement("SqlDBName");
            SqlDBName.InnerText = "WumingIM";
            SetLog("已添加元素:SqlAccessPassword", "XML");
         
            rootnode.AppendChild(ServerPort);
            rootnode.AppendChild(SqlServerIP);
            rootnode.AppendChild(SqlAccessAccount);
            rootnode.AppendChild(SqlAccessPassword);

          //  rootnode.AppendChild(ServerPort);

            xml.AppendChild(rootnode);
            xml.Save("Config.xml");
            SetLog("配置文件已创建", "XML");
        }

        private void SetLog(string log, string logType)
        {
            //throw new NotImplementedException();
            TEXTOUTPUT.AppendText("[" + DateTime.Now + "]" + "[" + logType + "]" + " " + log + "\n");
            try
            {
                 if (!File.Exists(Environment.CurrentDirectory + "\\logs\\" + DateTime.Now.ToString("yyyy-MM-dd") + ".log"))
                 {
                    File.Create(Environment.CurrentDirectory + "\\logs\\" + DateTime.Now.ToString("yyyy-MM-dd") + ".log");
                    File.AppendAllText(Environment.CurrentDirectory + "\\logs\\" + DateTime.Today + ".log", "[" + DateTime.Now + "]" + "[" + logType + "]" + " " + log + "\n");
                }
                else
                 {
                    File.AppendAllText(Environment.CurrentDirectory + "\\logs\\" + DateTime.Today + ".log", "[" + DateTime.Now + "]" + "[" + logType + "]" + " " + log + "\n");
                 }
            }
            catch (Exception e)
            {
                TEXTOUTPUT.AppendText(e.ToString());


            }
           
            //TEXTOUTPUT.ScrollToVerticalOffset(TEXTOUTPUT.sc)
        }

        private void ReadConfigFile()
        {
            SetLog("正在读取配置文件", "XML");
            try
            {
                XmlDocument xml = new XmlDocument();
                XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
                XmlReader reader = XmlReader.Create(@"Config.xml", xmlReaderSettings);
                xml.Load(reader);
                XmlNode root = xml.SelectSingleNode("Settings");
                XmlElement Serverport = (XmlElement)root.SelectSingleNode("ServerPort");
                settings.serverPort = Serverport.InnerText;
                XmlElement SqlServerIP = (XmlElement)root.SelectSingleNode("SqlServerIP");
                settings.sqlServerIP = SqlServerIP.InnerText;
                XmlElement SqlAccessAccount = (XmlElement)root.SelectSingleNode("SqlAccessAccount");
                settings.sqlAccessAccount = SqlAccessAccount.InnerText;
                XmlElement sqlAccessPassword = (XmlElement)root.SelectSingleNode("SqlAccessPassword");
                settings.sqlAccessPassword = sqlAccessPassword.InnerText;

                SetLog("配置文件读取完毕", "INFO");
                SetLog("设置已应用", "INFO");

            }
            catch (Exception e)
            {
                SetLog(e.ToString(), "ERROR");
                //SetLog(e.StackTrace, "EXCEPTION");
                //MessageBox.Show(e.Message);
               // throw;
            }
           
        }

        private void SaveConfigFile()
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Window1 window1 = new Window1();
            window1.Show();
            
        }

        private string command;

        private void TEXTCOMMAND_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
            {
                return;
            }
            else
            {
                command = TEXTCOMMAND.Text;
                TEXTCOMMAND.Text = "";
                ExecuteCommand(command);
                //TEXTOUTPUT.AppendText(command + "\n");
            }
        }

   

        private void ExecuteCommand(string cmd)
        {
            // throw new NotImplementedException();
            SetLog(">>"+" "+cmd, "CMD");
            if (cmd.Contains(" "))
            {
                string[] cmds = cmd.Split(' ');

            }
            else
            {
                if (cmd == "start")
                {
                    if (!ServerRunning)
                    {
                        StartServer();
                    }
                    else
                    {
                        SetLog("服务已经启动！", "WARN");
                    }
                }
                else
                {
                    if (cmd == "stop")
                    {
                        if (!ServerRunning)
                        {
                            
                        }
                        else
                        {
                            StopServer();
                        }
                        
                    }
                    else
                    {
                        SetLog("命令解析失败", "ERROR");
                    }
                }
            }
        }

        private void StopServer()
        {
            // throw new NotImplementedException();
            ServerRunning = false;
        }

        private void StartServer()
        {
            //throw new NotImplementedException();
            SetLog("正在生成新的RSA密钥", "INFO");
            Info = new RSAInfo(512);
            settings.RSAkey = Info.Export();
            SetLog(settings.RSAkey, "DEBUG");
            SetLog("正在准备RSA加解密", "INFO");
            rSACryption = new RSACryption(settings.RSAkey);
            SetLog("正在测试RSA加解密", "INFO");
            string s = "竹子";
            try
            { 
                string en = rSACryption.Encrypt(s);
                SetLog("ENCRYPTED: " + en, "INFO");
                string de = rSACryption.Decrypt(en);
                SetLog("DECRYPTED: " + de, "INFO");
            }
            catch (Exception e)
            {
                SetLog("在测试RSA加解密时发生异常", "ERROR");
                SetLog(e.ToString(), "ERROR");
               
            }
            
            
            try
            {
                SetLog("正在准备Socket", "SOCKET");

                if (!SocketReady)
                {
                    int port = int.Parse(settings.serverPort);
                    IPAddress address = IPAddress.Any;
                    IPEndPoint endPoint = new IPEndPoint(address, port);
                    socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    socket.Bind(endPoint);
                    socket.Listen(255);
                    SocketReady = true;
                }
               
                Thread thread = new Thread(MListenThread);
                thread.IsBackground = true;
                thread.Start();
                ServerRunning = true;
            }
            catch (Exception e)
            {

                SetLog("在尝试启动服务时发生异常", "ERROR"); 
                SetLog(e.ToString(), "ERROR");

            }
            
        }

        private void MListenThread()
        {
            void Log(string logtext)
            {
                Action action = ()=>{
                  SetLog(logtext, "ListenThread");
                };

               MainWind.Dispatcher.BeginInvoke(action);
            }
            Log("ListenThread已经启动");
            while (true)
            {
                try
                {
                    socket.Accept();
                }
                catch (Exception e)
                {

                    Log(e.ToString());
                }
                

            }
           
        }
    }
}
