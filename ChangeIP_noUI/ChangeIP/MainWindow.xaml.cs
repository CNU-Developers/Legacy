using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ChangeIP.Source;
using System.IO;
using System.Net;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading;

namespace ChangeIP
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public string firstHostName = Environment.UserDomainName;

        private string ipPattern = @"^([1-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])(\.([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])){3}$";
        private string hostPattern = @"^[a-zA-Z0-9가-힣]*$";

        enum rexType  {ipAddr=0, hoName};
        public string macAddress = "";
        public string hostName = "";
        public string ipAddress = "";
        public string subnetMask = "";
        public string gatewayAddress = "";
        public string mainDns = "";
        public string subDns = "";
        
        public MainWindow()
        {
            InitializeComponent();
            ReadMac();
            Application.Current.Shutdown();
        }

        private void ReadMac()
        {
            KnowMacAddress findMAC = new KnowMacAddress();
            findMAC.FindMacAddress();
            this.macAddress = findMAC.GetMacAddress();
            ChangeSettings();
        }

        private void ChangeSettings()
        {
            ReadIPList count = new ReadIPList();
            int lineCounter = count.GetLineCounter();
            bool isChangeSetting = false;
            if (lineCounter == 0)
                return;
            for (int i = 0; i < lineCounter; i++)
            {
                if (this.macAddress == count.myIpList[i].macAddress)
                {
                    this.hostName = count.myIpList[i].hostName;
                    this.ipAddress = count.myIpList[i].ipAddress;
                    this.subnetMask= count.myIpList[i].subnetMask;
                    this.gatewayAddress = count.myIpList[i].gatewayAddress;
                    this.mainDns = count.myIpList[i].mainDns;
                    this.subDns = count.myIpList[i].subDns;
                    isChangeSetting = true;
                }
            }
            if (isChangeSetting == false)
            {
                //MessageBox.Show("맞는 MAC 리스트가 없습니다");
                return;
            }
            
            ChangeAddress();
            ChangeDns();
            ChangeHostName();
        }

        //check 아이피타당성
        private bool isInputIpOk(string ipAddress, rexType type)
        {
            System.Text.RegularExpressions.Regex reg;
            if(type == rexType.ipAddr)
                reg = new System.Text.RegularExpressions.Regex(ipPattern);
            else
                reg = new System.Text.RegularExpressions.Regex(hostPattern);
            
            if(!reg.IsMatch(ipAddress))
                return false;
            else
                return true;
        }

        //hostname변경부, 첫 호스트네임과 다를경우에만 변경
        private bool ChangeHostName()
        {
            SetIpAddress changeHostname = new SetIpAddress();
            if (String.CompareOrdinal(this.firstHostName, this.hostName) != 0)
            {
                changeHostname.setHostName(this.hostName);
                ShutDownexe();
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool ChangeAddress()
        {
            SetIpAddress setIp = new SetIpAddress();
            if ((isInputIpOk(this.ipAddress, rexType.ipAddr)) && (isInputIpOk(this.subnetMask, rexType.ipAddr)) 
                && (isInputIpOk(this.gatewayAddress, rexType.ipAddr)))
            {
                setIp.setAddress(this.ipAddress, this.subnetMask, this.gatewayAddress);
                return true;
            }
            else
                return false;
        }

        private bool ChangeDns()
        {
            SetIpAddress setIp = new SetIpAddress();
            if ((isInputIpOk(this.mainDns, rexType.ipAddr)) && (isInputIpOk(this.subDns, rexType.ipAddr)))
            {
                setIp.setDns(this.mainDns, this.subDns);
                return true;
            }
            else
                return false;
        }

        private void ShutDownexe()
        {
            int portNumber = 8100;
            IPAddress ipAddress = IPAddress.Any;
            //IPAddress changAddress;

            while (true)
            {
                Thread.Sleep(3000);
                try//수정해씀
                {
                    string strHostName = Dns.GetHostName();
                    IPHostEntry ipEntry = Dns.GetHostEntry(strHostName);
                    IPAddress[] addr = ipEntry.AddressList;

                    for (int i = 0; i < addr.Length; i++)
                    {
                        if (string.CompareOrdinal(addr[i].AddressFamily.ToString(), "InterNetwork") == 0)
                            ipAddress = addr[i];
                    }
                    TcpClient tcpClient = new TcpClient(ipAddress.ToString(), portNumber);
                    NetworkStream netStream = tcpClient.GetStream();
                    NetworkStream networkReadStream = tcpClient.GetStream();
                    StreamWriter streamSender = new StreamWriter(netStream);
                    StreamReader streamReader = new StreamReader(networkReadStream);
                    
                    streamSender.WriteLine("(((RESTART)))");
                    streamSender.Flush();
                    if (streamReader.ReadLine() == "(((OK)))")
                        break;
                    
                    streamSender.Close();
                    netStream.Close();
                    networkReadStream.Close();

                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message);
                }
                
            }
        }
    }
}
