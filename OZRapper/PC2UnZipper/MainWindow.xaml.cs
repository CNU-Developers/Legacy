using System;
using System.Collections.Generic;
using System.Windows;
using System.Net;
using System.IO;

using Ionic.Zip;

namespace PC2UnZipper
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

        private void Window_Initialized(object sender, EventArgs e)
        {
            String location = Path.Combine(Path.GetTempPath(), "UnZipIt.zip");
            using(WebClient my = new WebClient()){
                my.DownloadFile("http://cnu.sogang.ac.kr/update/pc2.zip", location);
            }
            
            using(ZipFile my = new ZipFile(location, System.Text.Encoding.GetEncoding(949))){  // cp949 == EUC-KR == 한글.
                my.ExtractAll(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), ExtractExistingFileAction.OverwriteSilently);
            }
            this.Close();
        }
    }
}
