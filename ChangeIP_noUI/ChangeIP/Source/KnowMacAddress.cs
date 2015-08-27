using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.NetworkInformation;

namespace ChangeIP.Source
{
    class KnowMacAddress // mac Address를 추출
    {
        private string macAddress = "";

        public void FindMacAddress()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            macAddress = nics[0].GetPhysicalAddress().ToString();
        }

        public string GetMacAddress()
        {
            return macAddress;
        }
    }
}
