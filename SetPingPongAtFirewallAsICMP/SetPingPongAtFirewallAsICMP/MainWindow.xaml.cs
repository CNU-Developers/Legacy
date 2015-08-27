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

namespace SetPingPongAtFirewallAsICMP
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            SetPing();
            this.Close();
        }

        void SetPing()
        {
            /*
    [HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\services\SharedAccess\Parameters\FirewallPolicy\FirewallRules]
    "FPS-ICMP4-ERQ-In-NoScope"="v2.10|Action=Allow|Active=TRUE|Dir=In|Protocol=1|Profile=Domain|ICMP4=8:*|Name=@FirewallAPI.dll,-28543|Desc=@FirewallAPI.dll,-28547|EmbedCtxt=@FirewallAPI.dll,-28502|"
    "FPS-ICMP4-ERQ-In"="v2.10|Action=Allow|Active=TRUE|Dir=In|Protocol=1|Profile=Private|Profile=Public|ICMP4=8:*|RA4=LocalSubnet|Name=@FirewallAPI.dll,-28543|Desc=@FirewallAPI.dll,-28547|EmbedCtxt=@FirewallAPI.dll,-28502|"
             * 
             * 관리자권한필요
             */
            var PING_ON = "FPS-ICMP4-ERQ-In";
            var PING_ON_GLOBAL = "FPS-ICMP4-ERQ-In-NoScope";

            Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\services\\SharedAccess\\Parameters\\FirewallPolicy\\FirewallRules", true);
            if (key.GetValue(PING_ON).ToString().Contains("FALSE"))
            {
                key.SetValue(PING_ON, "v2.10|Action=Allow|Active=TRUE|Dir=In|Protocol=1|Profile=Private|Profile=Public|ICMP4=8:*|RA4=LocalSubnet|Name=@FirewallAPI.dll,-28543|Desc=@FirewallAPI.dll,-28547|EmbedCtxt=@FirewallAPI.dll,-28502|");
                key.SetValue(PING_ON_GLOBAL, "v2.10|Action=Allow|Active=TRUE|Dir=In|Protocol=1|Profile=Domain|ICMP4=8:*|Name=@FirewallAPI.dll,-28543|Desc=@FirewallAPI.dll,-28547|EmbedCtxt=@FirewallAPI.dll,-28502|");
                ShutDownexe();
            }
        }


        // 최종우의 ChangeIP에서 낼름.
        void ShutDownexe()
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
                }

            }
        }
    }
}
