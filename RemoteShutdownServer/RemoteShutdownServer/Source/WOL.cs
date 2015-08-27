using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Globalization;

namespace RemoteShutdownServer.Source
{
    public class WOLClass : UdpClient
    {
        public WOLClass()
            : base()
        { }
        //this is needed to send broadcast packet
        public void SetClientToBrodcastMode()
        {
            if (this.Active)
                this.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 0);
        }
    }

    public class MACTable
    {
        private String[] w ={"00245493A43A","00245493A3B3","00245493A3CC","002454933282","002454934385","00245493A3B5",
            "00245493A40C","00245493A3BB","00245493A3BC","00245493A3C2","00245493A483","00245493E6B6",
            "00245490322A","00245493A3C7","00245493A2CA","00245493E5A2","00245493A397","00245493E88C",
            "00245493E735","00245493A44D","00245493E6AF","00245493A32B","00245493EA7F","00245493A492",
            "00245493A42A","00245493A3AF","00245493A3FB","00245493E6EF","00245493A3D4","00245493A3A8",
            "00245493E9B0","00245493E567","00245493A448","00245493E429","00245493E888","00245493E6B7",
            "00245493A436","00245493E5AF","00245493A482","00245493E946","00245493A3D2","00245493E43D","00245493E91F",
            "00245493E96D","00245493EA8F","002454930122","00245493A27C","00245493A47F","00245493E949",
            "00245493A426","00245493E631","00245493A408","00245493E7B3","00245493E8A9","00245493E621"};
        public string GetStringFromIndex(int index)
        {
            return w[index];
        }
    }

    class WOL
    {
        public void WakeFunction(string MAC_ADDRESS)
        {
            WOLClass client = new WOLClass();
            client.Connect(new IPAddress(0xffffffff), 0x2fff);
            client.SetClientToBrodcastMode();

            int counter = 0;

            byte[] bytes = new byte[1024];   // more than enough :-)

            for (int y = 0; y < 6; y++)
                bytes[counter++] = 0xFF;

            for (int y = 0; y < 16; y++)
            {
                int i = 0;
                for (int z = 0; z < 6; z++)
                {
                    bytes[counter++] =
                        byte.Parse(MAC_ADDRESS.Substring(i, 2),
                        NumberStyles.HexNumber);
                    i += 2;
                }
            }

            //now send wake up packet
            int reterned_value = client.Send(bytes, 1024);
        }
    }
}
