using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Web;

namespace ServerShips
{
    public class ClientObject
    {
        protected internal string Id { get; private set; }
        protected internal NetworkStream Stream { get; private set; }
        public TcpClient client;
        public ServerObject server;

        public ClientObject(TcpClient tcpClient, ServerObject serverObject)
        {
            Id = Guid.NewGuid().ToString();
            client = tcpClient;
            server = serverObject;
            serverObject.AddConnection(this);
        }

        public void Process()
        {
            try
            {
                Stream = client.GetStream();
                string message;
                while (true)
                {
                    try
                    {
                        var bytes = GetBytes();
                        var s1 = Encoding.Unicode.GetString(bytes);
                        var s2 = Encoding.UTF8.GetString(bytes);
                        var s3 = Encoding.UTF32.GetString(bytes);
                        //message = GetMessage(Encoding.Unicode);
                        //string message2 = GetMessage(Encoding.UTF8);
                        //string message3 = GetMessage(Encoding.UTF32);
                        if (!string.IsNullOrEmpty(s1))
                        {
                            Support.LogWrite(s1);
                            client.GetStream().Write(Encoding.Unicode.GetBytes("Пысюн"));

                            //byte[] answer = Encoding.Unicode.GetBytes("Привет!");
                            //Stream.Write(answer, 0, answer.Length);
                        }
                    }
                    catch
                    {
                        Support.LogWrite("Был отключен: " + this.client.Client.RemoteEndPoint);

                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                // в случае выхода из цикла закрываем ресурсы
                server.RemoveConnection(this.Id);
                Close();
            }
        }

        // чтение входящего сообщения и преобразование в строку
        private string GetMessage(Encoding e)
        {
            byte[] data = new byte[64]; // буфер для получаемых данных
            StringBuilder builder = new StringBuilder();
            int bytes = 0;
            do
            {
                bytes = Stream.Read(data, 0, data.Length);
                builder.Append(e.GetString(data, 0, bytes));
            }
            while (Stream.DataAvailable);

            return builder.ToString();
        }

        private byte[] GetBytes()
        {
            byte[] data = new byte[64]; // буфер для получаемых данных
            List<byte> listBytes = new List<byte>();
            int bytes = 0;
            MemoryStream stream = new MemoryStream();
            do
            {
                bytes = Stream.Read(data, 0, data.Length);                
                stream.Append(data);
            }
            while (Stream.DataAvailable);

            return stream.ToArray();
        }

        // закрытие подключения
        protected internal void Close()
        {
            if (Stream != null)
                Stream.Close();
            if (client != null)
                client.Close();
        }
    }
}