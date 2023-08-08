using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace UDPTest
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage:");
                Console.WriteLine("UDPTest s 192.168.0.1 2222");
                Console.WriteLine("     Server LocalIP LocalPort");
                Console.WriteLine("UDPTest c 192.168.0.2 3333 192.168.0.1 2222 200 1");
                Console.WriteLine("     Client LocalIP LocalPort RemoteIP RemotePort Delay Data");
                return;
            }

            string mode = args[0];
            string localIp = args[1];
            ushort localPort = ushort.Parse(args[2]);
            bool isSrv = mode == "s";

            if (isSrv)
            {
                var srv = new UdpClient(new IPEndPoint(IPAddress.Parse(localIp), localPort));

                IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Loopback, 0);

                for (;;)
                {
                    var bytes = srv.Receive(ref RemoteIpEndPoint);
                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}]{RemoteIpEndPoint,22} - {bytes[0]} ({bytes[1],3})");
                }
            }
            else
            {
                byte seq = 0;
                string remoteIp = args[3];
                ushort remotePort = ushort.Parse(args[4]);
                var delay = int.Parse(args[5]);
                var data = byte.Parse(args[6]);

                var cln = new UdpClient(new IPEndPoint(IPAddress.Parse(localIp), localPort));
                cln.Connect(new IPEndPoint(IPAddress.Parse(remoteIp), remotePort));

                for (;;)
                {
                    cln.Send(new byte[] { data, seq }, 2);
                    Console.Write(data);
                    seq++;
                    Thread.Sleep(delay);
                }
            }
        }
    }
}
