using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using System.Windows;

namespace ChangeIP.Source
{
    //수정완료
    class SetIpAddress
    {
        public void setHostName(string hostName)
        {
            ManagementClass changePc = new ManagementClass("Win32_ComputerSystem");
            ManagementObjectCollection changePcC = changePc.GetInstances();
            foreach (ManagementObject change in changePcC)
            {
                ManagementBaseObject inParams = change.GetMethodParameters("Rename");
                inParams["Name"] = hostName;
                change.InvokeMethod("Rename", inParams, null);
            }
        }

        public void setAddress(string IPAddress, string SubnetMask, string Gateway)
        {
            ManagementClass objMC = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection objMOC = objMC.GetInstances();

            foreach (ManagementObject objMO in objMOC)
            {
                if (!(bool)objMO["IPEnabled"])
                    continue;
                try
                {
                    ManagementBaseObject objSetIP = null;
                    ManagementBaseObject objNewIP = null;
                    ManagementBaseObject objNewGate = null;

                    objNewIP = objMO.GetMethodParameters("EnableStatic");
                    objNewGate = objMO.GetMethodParameters("SetGateways");

                    objNewIP["IPAddress"] = new string[] { IPAddress };
                    objNewIP["SubnetMask"] = new string[] { SubnetMask };

                    objNewGate["DefaultIPGateway"] = new string[] { Gateway };
                    objNewGate["GatewayCostMetric"] = new int[] { 1 };

                    objSetIP = objMO.InvokeMethod("EnableStatic", objNewIP, null);
                    objSetIP = objMO.InvokeMethod("SetGateways", objNewGate, null);

                    Console.WriteLine("Updated IPAddress, SubnetMask and Default Gateway!");
                    //MessageBox.Show("IPChanged");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Unable to Set IP : " + ex.Message);
                    //MessageBox.Show("IPnotchanged");
                }
            }
        }

        public void setDns(string mainDns, string SubDns)
        {
            ManagementClass objMC = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection objMOC = objMC.GetInstances();
            string[] sIPs = { mainDns, SubDns };

            foreach (ManagementObject objMO in objMOC)
            {
                if (!(bool)objMO["IPEnabled"])
                    continue;
                try
                {   
                    ManagementBaseObject objDNS = null;

                    objDNS = objMO.GetMethodParameters("SetDNSServerSearchOrder");
                    objDNS["DNSServerSearchOrder"] = sIPs;
                    objMO.InvokeMethod("SetDNSServerSearchOrder", objDNS, null);

                    Console.WriteLine("Updated IPAddress, SubnetMask and Default Gateway!");
                    //MessageBox.Show("DNSChanged");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Unable to Set DNS : " + ex.Message);
                    //MessageBox.Show("DNSnotchanged");
                }
            }
        }
    }
}