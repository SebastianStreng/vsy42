using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace vsy42
{
    public class Sender
    {
        public static void SendMessage()
        {
            using UdpClient sender = new();
            Console.WriteLine("To send message press Enter");
            while (true)
            {
                string message = Console.ReadLine();

                message = $"{MainWindow.Name}: {message}";
                byte[] data = Encoding.UTF8.GetBytes(message);
                sender.Send(data, data.Length, new IPEndPoint(MainWindow.Ip, MainWindow.RemotePort));
            }
        }
    }
}
