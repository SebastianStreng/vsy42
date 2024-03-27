using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace vsy42
{
    public partial class MainWindow : Window
    {
        //https://github.com/EldarMuradov/LocalChat/tree/master
        public MainWindow()
        {
            InitializeComponent();
        }
        public static int LocalPort;
        public static int RemotePort;
        public static IPAddress Ip;
        public static string Name = "";

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Ip = IPAddress.Parse("127.0.0.1");

            Name = tbUsername.Text; 

            LocalPort = Convert.ToInt32(tbReceivePort.Text);

            RemotePort = Convert.ToInt32(tbSendPort.Text);

            Thread thread = new(Reciever.ReceiveMessage);
            thread.Start();

            Sender.SendMessage();

            Console.ReadLine();
        }
    }
}

