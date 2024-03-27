using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace vsy42
{
    public class Reciever
    {
        public static void ReceiveMessage()
        {
            using UdpClient receiver = new(MainWindow.LocalPort);
            IPEndPoint ip = null;
            while (true)
            {
                var result = receiver.Receive(ref ip);
                var message = Encoding.UTF8.GetString(result);
                Console.WriteLine(message);
            }
        }
    }
}
