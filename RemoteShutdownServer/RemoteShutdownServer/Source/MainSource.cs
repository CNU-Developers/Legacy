using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Windows;

namespace RemoteShutdownServer.Source
{
    class MainSource
    {
        public string[] sendLists;
        public int listCounter=0;
        public void SendMessageShutdown(string[] sendList)
        {
            this.sendLists = sendList;
            Thread[] threads = new Thread[sendList.Length];
            for (int i = 0; i < sendList.Length; i++)
            {
                threads[i] = new Thread(Shutdown);
                threads[i].Start(i);
                //Shutdown(i);
                Thread.Sleep(100);
            }
        }

        public void Shutdown(object i)
        {
            int portNumber = 8100;
            int mylistCounter = (int)i;
            try
            {
                TcpClient tcpClient = new TcpClient(sendLists[mylistCounter], portNumber);
                NetworkStream netStream = tcpClient.GetStream();
                StreamWriter streamSender = new StreamWriter(netStream);
                streamSender.WriteLine("(((SHUTDOWN)))");
                streamSender.Flush();
                streamSender.Close();
                netStream.Close();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }

    }
}
