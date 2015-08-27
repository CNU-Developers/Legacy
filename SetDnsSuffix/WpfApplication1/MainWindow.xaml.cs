using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace WpfApplication1
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Initialized_1(object sender, EventArgs e)
        {
            // 관리자 권한 필요!
            const string DNS_SUFFIX = "cnu.sogang.ac.kr";
            const string DNS_WANT_TO_APPLY = "192.168.200.99";
            string[] interfaceKeys;

            Microsoft.Win32.RegistryKey key, key2;

            key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\services\\Tcpip\\Parameters\\Interfaces", true);

            interfaceKeys = key.GetSubKeyNames();

            for (int i = 0; i < key.SubKeyCount; i++)
            {
                try
                {
                    key2 = key.OpenSubKey(interfaceKeys[i], true);
                    if (key2.GetValue("NameServer", "").ToString().Contains(DNS_WANT_TO_APPLY))
                        key2.SetValue("Domain", DNS_SUFFIX);
                    else
                        key2.SetValue("Domain", "");
                    key2.Close();
                }
                catch
                {}
            }

            key.Close();
            
            this.Close();
        }
    }
}
