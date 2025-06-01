using System;
using System.Net.Sockets;
using System.Text;

namespace Player2
{
    public class Client
    {
        private NetworkStream stream;

        public void Connect(string host, int port)
        {
            var client = new TcpClient();
            client.Connect(host, port);
            stream = client.GetStream();
            Console.WriteLine("Conectado ao servidor!");
        }

        public void Send(string msg)
        {
            var data = Encoding.ASCII.GetBytes(msg);
            stream.Write(data, 0, data.Length);
        }

        public string Receive()
        {
            var buf = new byte[32];
            int len = stream.Read(buf, 0, buf.Length);
            return Encoding.ASCII.GetString(buf, 0, len);
        }
    }
}
