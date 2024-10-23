using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Timers;

namespace SocketAPI
{
    class Program
    {
        //out and in from server
        static void Main(string[] args)
        {
            try
            {
                curdirect = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\")) + "/export";
                if (File.Exists(curdirect + "/outData")) File.Delete(curdirect + "/outData");
                if (File.Exists(curdirect + "/inData")) File.Delete(curdirect + "/inData");
                var tcpEndPoint = new IPEndPoint(IPAddress.Parse(Console.ReadLine()), Convert.ToInt32("4307"));
                var tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                string s = "log" + Console.ReadLine();
                tcpSocket.Connect(tcpEndPoint);
                socket = tcpSocket;
                string a = Socet(s);
                if (a == "err")
                {
                    Console.WriteLine("Error connect");
                    return;
                }
                outData(a);
                inDataTimer.Elapsed += inData;
                inDataTimer.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error connect");
                Console.WriteLine(e);
            }
            Spanel();
        }
        static void Spanel()
        {
            Console.WriteLine(Socet(Console.ReadLine()));
            Spanel();
        }
        private static Timer inDataTimer = new Timer(100);
        private static Socket socket;
        private static string curdirect;

        static string Socet(string str)
        {
            try
            {
                var data = Encoding.UTF8.GetBytes(str);
                socket.Send(data);
                var buffer = new byte[512];
                var size = 0;
                var answer = new StringBuilder();
                do
                {
                    size = socket.Receive(buffer);
                    answer.Append(Encoding.UTF8.GetString(buffer, 0, size));
                }
                while (socket.Available > 0);
                return answer + "";
            }
            catch
            {
                Console.WriteLine("Error curret connect");
                inDataTimer.Stop();
                return "err";
            }
        }
        //Data
        static void inData(Object source, ElapsedEventArgs e)
        {
            if(File.Exists(curdirect + "/inData"))
            {
                string indata = File.ReadAllText(curdirect + "/inData");
                File.Delete(curdirect + "/inData");
                Console.WriteLine("in: \n" + indata);
                string outdata = Socet(indata);
                Console.WriteLine("out: \n" + outdata);
                outData(outdata);
            }
        }
        static void outData(string outdata)
        {
            if (!File.Exists(curdirect + "/outData"))
            {
                File.WriteAllText(curdirect + "/outData", outdata);
            }
            else
            {
                Console.WriteLine("out Data fauled");
            }
        }
    }
}
