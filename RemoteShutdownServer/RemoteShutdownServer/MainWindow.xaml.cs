using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using RemoteShutdownServer.Source;

namespace RemoteShutdownServer
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        List<string> lst = new List<string>();
        List<Ellipse> Elist = new List<Ellipse>();
        List<string> MAClst = new List<string>();

        public MainWindow()
        {
            InitializeComponent();
            AddElist();

            Thread[] pingThread = new Thread[Elist.Count];
            for (int i = 0; i < pingThread.Length-2; i++)
            {
                pingThread[i] = new Thread(PingPing);
                pingThread[i].Start("163.239.200."+(i+45));
            }
            pingThread[54] = new Thread(PingPing);
            pingThread[54].Start("163.239.200.100");
            pingThread[55] = new Thread(PingPing);
            pingThread[55].Start("163.239.200.120");
        }


        private void Button_Shutdown_Click(object sender, RoutedEventArgs e)
        {
            MainSource aa = new MainSource();
            aa.SendMessageShutdown(lst.ToArray());
        }


        public void PingPing(object ipAddress)
        {
            while (true)
            {
                Ping sendPing = new Ping();
                PingOptions opPing = new PingOptions();
                opPing.DontFragment = true;
                string data = "aaaaaaaaaaa";
                byte[] buffer = Encoding.ASCII.GetBytes(data);
                int timeoutLimit = 120;
                PingReply reply = sendPing.Send(ipAddress.ToString(), timeoutLimit, buffer, opPing);
                if (reply.Status == IPStatus.Success)
                {
                    Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                    {
                        EclipseBrush(ipAddress.ToString(), true);
                    }));
                }
                else
                {
                    Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                    {
                        EclipseBrush(ipAddress.ToString(), false);
                    }));
                }
                Thread.Sleep(1000);
                sendPing.Dispose();
            }
        }


        private void EclipseBrush(string ipAddress, bool pingTest)
        {
            for (int counter = 0; counter < Elist.Count-1; counter++)
            {
                if (string.CompareOrdinal(ipAddress, "163.239.200."+(counter+45)) == 0)
                {
                    if (pingTest == true)
                        Elist[counter].Fill = new SolidColorBrush(Colors.Green);
                    else if (pingTest == false)
                        Elist[counter].Fill = new SolidColorBrush(Colors.Red);
                }
            }
            if (string.CompareOrdinal(ipAddress, "163.239.200.100") == 0)
            {
                if (pingTest == true)
                    Elist[54].Fill = new SolidColorBrush(Colors.Green);
                else if (pingTest == false)
                    Elist[54].Fill = new SolidColorBrush(Colors.Red);
            }
        }


        #region checkbox select part
        private void Select1_Checked(object sender, RoutedEventArgs e)
        {
            Int32 computernumber = Int32.Parse(((CheckBox)sender).Content.ToString());
            lst.Add("163.239.200." + (computernumber +44).ToString());
            MAClst.Add(new MACTable().GetStringFromIndex(computernumber - 1));
        }

        private void Select55_Checked(object sender, RoutedEventArgs e)
        {
            lst.Add("163.239.200.100");
            MAClst.Add(new MACTable().GetStringFromIndex(54));
        }

        private void Select1_Unchecked(object sender, RoutedEventArgs e)
        {
            Int32 computernumber = Int32.Parse(((CheckBox)sender).Content.ToString());
            lst.Remove("163.239.200." + (computernumber + 44).ToString());
            MAClst.Remove(new MACTable().GetStringFromIndex(computernumber - 1));
        }

        private void Select55_Unchecked(object sender, RoutedEventArgs e)
        {
            lst.Remove("163.239.200.100");
            MAClst.Remove(new MACTable().GetStringFromIndex(54));
        }
        #endregion

        private void Button_AllSelect_Click(object sender, RoutedEventArgs e)
        {
            Select1.IsChecked = true;
            Select2.IsChecked = true; 
            Select3.IsChecked = true;
            Select4.IsChecked = true;
            Select5.IsChecked = true;
            Select6.IsChecked = true;
            Select7.IsChecked = true;
            Select8.IsChecked = true;
            Select9.IsChecked = true;
            Select10.IsChecked = true;
            Select11.IsChecked = true;
            Select12.IsChecked = true;
            Select13.IsChecked = true;
            Select14.IsChecked = true;
            Select15.IsChecked = true;
            Select16.IsChecked = true;
            Select17.IsChecked = true;
            Select18.IsChecked = true;
            Select19.IsChecked = true;
            Select20.IsChecked = true;
            Select21.IsChecked = true;
            Select22.IsChecked = true;
            Select23.IsChecked = true;
            Select24.IsChecked = true;
            Select25.IsChecked = true;
            Select26.IsChecked = true;
            Select27.IsChecked = true;
            Select28.IsChecked = true;
            Select29.IsChecked = true;
            Select30.IsChecked = true;
            Select31.IsChecked = true;
            Select32.IsChecked = true;
            Select33.IsChecked = true;
            Select34.IsChecked = true;
            Select35.IsChecked = true;
            Select36.IsChecked = true;
            Select37.IsChecked = true;
            Select38.IsChecked = true;
            Select39.IsChecked = true;
            Select40.IsChecked = true;
            Select41.IsChecked = true;
            Select42.IsChecked = true;
            Select43.IsChecked = true;
            Select44.IsChecked = true;
            Select45.IsChecked = true;
            Select46.IsChecked = true;
            Select47.IsChecked = true;
            Select48.IsChecked = true;
            Select49.IsChecked = true;
            Select50.IsChecked = true;
            Select51.IsChecked = true;
            Select52.IsChecked = true;
            Select53.IsChecked = true;
            Select54.IsChecked = true;
            Select55.IsChecked = true;
        }

        private void Button_Deselect_Click(object sender, RoutedEventArgs e)
        {
            Select1.IsChecked = false;
            Select2.IsChecked = false;
            Select3.IsChecked = false;
            Select4.IsChecked = false;
            Select5.IsChecked = false;
            Select6.IsChecked = false;
            Select7.IsChecked = false;
            Select8.IsChecked = false;
            Select9.IsChecked = false;
            Select10.IsChecked = false;
            Select11.IsChecked = false;
            Select12.IsChecked = false;
            Select13.IsChecked = false;
            Select14.IsChecked = false;
            Select15.IsChecked = false;
            Select16.IsChecked = false;
            Select17.IsChecked = false;
            Select18.IsChecked = false;
            Select19.IsChecked = false;
            Select20.IsChecked = false;
            Select21.IsChecked = false;
            Select22.IsChecked = false;
            Select23.IsChecked = false;
            Select24.IsChecked = false;
            Select25.IsChecked = false;
            Select26.IsChecked = false;
            Select27.IsChecked = false;
            Select28.IsChecked = false;
            Select29.IsChecked = false;
            Select30.IsChecked = false;
            Select31.IsChecked = false;
            Select32.IsChecked = false;
            Select33.IsChecked = false;
            Select34.IsChecked = false;
            Select35.IsChecked = false;
            Select36.IsChecked = false;
            Select37.IsChecked = false;
            Select38.IsChecked = false;
            Select39.IsChecked = false;
            Select40.IsChecked = false;
            Select41.IsChecked = false;
            Select42.IsChecked = false;
            Select43.IsChecked = false;
            Select44.IsChecked = false;
            Select45.IsChecked = false;
            Select46.IsChecked = false;
            Select47.IsChecked = false;
            Select48.IsChecked = false;
            Select49.IsChecked = false;
            Select50.IsChecked = false;
            Select51.IsChecked = false;
            Select52.IsChecked = false;
            Select53.IsChecked = false;
            Select54.IsChecked = false;
            Select55.IsChecked = false;
        }

        private void AddElist()
        {
            Elist.Add(alive1);
            Elist.Add(alive2);
            Elist.Add(alive3);
            Elist.Add(alive4);
            Elist.Add(alive5);
            Elist.Add(alive6);
            Elist.Add(alive7);
            Elist.Add(alive8);
            Elist.Add(alive9);
            Elist.Add(alive10);
            Elist.Add(alive11);
            Elist.Add(alive12);
            Elist.Add(alive13);
            Elist.Add(alive14);
            Elist.Add(alive15);
            Elist.Add(alive16);
            Elist.Add(alive17);
            Elist.Add(alive18);
            Elist.Add(alive19);
            Elist.Add(alive20);
            Elist.Add(alive21);
            Elist.Add(alive22);
            Elist.Add(alive23);
            Elist.Add(alive24);
            Elist.Add(alive25);
            Elist.Add(alive26);
            Elist.Add(alive27);
            Elist.Add(alive28);
            Elist.Add(alive29);
            Elist.Add(alive30);
            Elist.Add(alive31);
            Elist.Add(alive32);
            Elist.Add(alive33);
            Elist.Add(alive34);
            Elist.Add(alive35);
            Elist.Add(alive36);
            Elist.Add(alive37);
            Elist.Add(alive38);
            Elist.Add(alive39);
            Elist.Add(alive40);
            Elist.Add(alive41);
            Elist.Add(alive42);
            Elist.Add(alive43);
            Elist.Add(alive44);
            Elist.Add(alive45);
            Elist.Add(alive46);
            Elist.Add(alive47);
            Elist.Add(alive48);
            Elist.Add(alive49);
            Elist.Add(alive50);
            Elist.Add(alive51);
            Elist.Add(alive52);
            Elist.Add(alive53);
            Elist.Add(alive54);
            Elist.Add(alive55);
            Elist.Add(AliveDisplay);
        }
        

        private void Button_Start_Click(object sender, RoutedEventArgs e)
        {
            string[] MACArr = MAClst.ToArray();
            for (int i = 0; i < MACArr.Length; i++)
            {
                new WOL().WakeFunction(MACArr[i]);
            }
        }
        
        private void Window_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.DragMove();
            e.Handled = true;
        }

        private void Button_ApplicationShutdown_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

    }
}
