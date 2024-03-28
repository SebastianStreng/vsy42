using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;

namespace vsy42
{
    public partial class MainWindow : Window
    {
        private UdpClient sender;
        private UdpClient receiver;
        private Thread receiveThread;
        private string username;
        private int localPort;
        private int remotePort;
        private IPAddress ip;

        public MainWindow()
        {
            InitializeComponent();
            sender = new UdpClient();
            receiveThread = new Thread(ReceiveMessage);
            receiveThread.IsBackground = true;
        }

        private void autoFillTextBoxes()
        {

        }

        private void StartChat()
        {
            try
            {
                username = tbUsername.Text;
                localPort = int.Parse(tbReceivePort.Text);
                remotePort = int.Parse(tbSendPort.Text);
                ip = IPAddress.Parse(tbIpAddress.Text);
                //ip = IPAddress.Parse("127.0.0.1");

                // Starte den Empfangsthread
                receiveThread.Start();

                // Starte den Sendevorgang
                SendMessage($"I joined the chat, Receiver: {localPort}, Sender: {remotePort}, IP: {ip}.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Starten des Chats: " + ex.Message);
            }
        }

        private void SendMessage(string message)
        {
            try
            {
                byte[] data = Encoding.UTF8.GetBytes(username + ": " + message);
                sender.Send(data, data.Length, new IPEndPoint(ip, remotePort));
                tbChatWindow.AppendText(username + ": " + message + "\n");
                tbMessage.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Senden der Nachricht: " + ex.Message);
            }
        }

        private void ReceiveMessage()
        {
            try
            {
                receiver = new UdpClient(localPort);
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);

                while (true)
                {
                    byte[] receivedBytes = receiver.Receive(ref endPoint);
                    string receivedMessage = Encoding.UTF8.GetString(receivedBytes);
                    Dispatcher.Invoke(() =>
                    {
                        tbChatWindow.AppendText(receivedMessage + "\n");
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Empfangen der Nachricht: " + ex.Message);
            }
        }

        private void btnStartChat_Click(object sender, RoutedEventArgs e)
        {
            StartChat();
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            SendMessage(tbMessage.Text);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            receiveThread.Abort();
            receiver.Close();
            sender.Close();
        }


    }
}





//using System;
//using System.Diagnostics;
//using System.Net;
//using System.Net.Sockets;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows;

//namespace vsy42
//{
//    public partial class MainWindow : Window
//    {
//        //https://github.com/EldarMuradov/LocalChat/tree/master
//        public MainWindow()
//        {
//            InitializeComponent();
//        }
//        public static int LocalPort;
//        public static int RemotePort;
//        public static IPAddress Ip;
//        public static string Name = "";

//        private void btnStartChat_Click(object sender, RoutedEventArgs e)
//        {
//            Ip = IPAddress.Parse("127.0.0.1");

//            Name = tbUsername.Text;

//            LocalPort = Convert.ToInt32(tbReceivePort.Text);

//            RemotePort = Convert.ToInt32(tbSendPort.Text);

//            Thread thread = new(ReceiveMessage);
//            thread.Start();

//            SendMessage();

//            Console.ReadLine();
//        }

//        public void SendMessage()
//        {
//            using UdpClient sender = new();
//            while (true)
//            {
//                var message = $"{Name}: {tbMessage.Text}";
//                byte[] data = Encoding.UTF8.GetBytes(message);
//                sender.Send(data, data.Length, new IPEndPoint(Ip, RemotePort));
//                tbChatWindow.Text = $"{message} \n\n";
//            }
//        }

//        public void ReceiveMessage()
//        {
//            using UdpClient receiver = new(LocalPort);
//            IPEndPoint ip = null;
//            while (true)
//            {
//                var result = receiver.Receive(ref ip);
//                var message = Encoding.UTF8.GetString(result);
//                tbChatWindow.Text = $"{message} \n\n";
//            }
//        }

//        private void btnSend_Click(object sender, RoutedEventArgs e)
//        {

//        }


//    }
//}

