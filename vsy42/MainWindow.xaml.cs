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
        private IPAddress targetIP;

        public MainWindow()
        {
            InitializeComponent();
            try
            {
                sender = new UdpClient();
                receiveThread = new Thread(ReceiveMessage);
                receiveThread.IsBackground = true;
                lblPublicIP.Content = $"Your Public-IP:  {GetPublicIP()}";
                lblLocalIP.Content = $"Your Local-IP: {GetLocalIP()}";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Initialisieren: " + ex.Message);
            }
        }


        private void StartChat()
        {
            try
            {
                username = tbUsername.Text;
                localPort = int.Parse(tbReceivePort.Text);
                remotePort = int.Parse(tbSendPort.Text);
                targetIP = IPAddress.Parse(tbIpAddress.Text);

                // Starte den Empfangsthread
                receiveThread.Start();

                // Starte den Sendevorgang
                SendMessage($"I joined the chat, Receiver: {localPort}, Sender: {remotePort}, Target-IP: {targetIP}.");
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
                sender.Send(data, data.Length, new IPEndPoint(targetIP, remotePort));
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
            try
            {
                receiveThread.Abort();
                receiver.Close();
                sender.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Schließen der Verbindungen: " + ex.Message);
            }
        }

        private string GetPublicIP()
        {
            try
            {
                WebClient client = new WebClient();
                return client.DownloadString("http://api.ipify.org");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Abrufen der öffentlichen IP-Adresse: " + ex.Message);
                return "N/A";
            }
        }

        public string GetLocalIP()
        {
            try
            {
                string hostName = Dns.GetHostName();
                IPAddress[] addresses = Dns.GetHostAddresses(hostName);

                foreach (IPAddress address in addresses)
                {
                    if (address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        return address.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Abrufen der lokalen Netzwerk-IP-Adresse: " + ex.Message);
            }

            return "N/A";
        }

    }
}

