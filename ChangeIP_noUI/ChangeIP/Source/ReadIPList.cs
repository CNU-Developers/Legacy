using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Windows;

//수정완료
namespace ChangeIP.Source
{
    public class IpList
    {
        public string macAddress;
        public string ipAddress;
        public string subnetMask;
        public string gatewayAddress;
        public string mainDns;
        public string subDns;
        public string hostName;

        public IpList(string macAddress, string ipAddress, string subnetMask,
            string gatewayAddress, string mainDns, string subDns, string hostName)
        {
            this.macAddress = macAddress;
            this.ipAddress = ipAddress;
            this.subnetMask = subnetMask;
            this.gatewayAddress = gatewayAddress;
            this.mainDns = mainDns;
            this.subDns = subDns;
            this.hostName = hostName;
        }
    }

    class ReadIPList
    {
        public List<IpList> myIpList = new List<IpList>();
        private enum  Addr { macAddress = 0, ipAddress, subnetMask, gatewayAddress, mainDns, subDns, hostName };
        private int lineCounter = 0;
        private string tempString = "";
        private string[] tempParseString = new string[7];

        private string _filePath = System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + "\\IPList.txt";
        public ReadIPList()
        {
            this.ReadTxtFile();
        }

        public void ReadTxtFile()
        {
            FileInfo existFile = new FileInfo(_filePath);
            if (existFile.Exists == true)
            {
                FileStream fs = new FileStream(_filePath, FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(fs, System.Text.Encoding.Default);
                sr.BaseStream.Seek(0, SeekOrigin.Begin);
                while (!sr.EndOfStream)
                {
                    this.tempString = sr.ReadLine();
                    if (string.CompareOrdinal(this.tempString, "") != 0)
                    {
                        this.tempParseString = this.tempString.Split(' ');
                        myIpList.Add(new IpList(this.tempParseString[(int)Addr.macAddress], this.tempParseString[(int)Addr.ipAddress],
                            this.tempParseString[(int)Addr.subnetMask], this.tempParseString[(int)Addr.gatewayAddress],
                            this.tempParseString[(int)Addr.mainDns], this.tempParseString[(int)Addr.subDns], this.tempParseString[(int)Addr.hostName]));
                        this.lineCounter++;
                    }
                }
                sr.Close();
                fs.Close();
            }
            else
            {
                MessageBox.Show("List파일이 존재하지 않습니다.");
                return;
            }
        }

        public int GetLineCounter()
        {
            return this.lineCounter;
        }

    }
}
