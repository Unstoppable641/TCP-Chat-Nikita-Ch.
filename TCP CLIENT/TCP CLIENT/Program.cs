using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Sockets;

namespace TCP_CLIENT
{
    // TCP CHAT 
    class Program
    {
        static void Main(string[] args)
        {
            var client = new TcpClient("10.4.24.10", 9000);
            var nickName = "PETR POROSHENKO .:THE ORIGINAL PRESIDENT OF UKRAINE:.";

            //отправление сообщения на сервер
            Task.Run(() =>
            {
                while (true)
                {
                    var buffer = new byte[4096];
                    var readedBytes = client.GetStream().Read(buffer, 0, buffer.Length);
                    var serializedMessage = Encoding.UTF8.GetString(buffer, 0, readedBytes);
                    var incomMessage = JsonConvert.DeserializeObject<Message>(serializedMessage);
                    Console.WriteLine($"{incomMessage.Sender} ---- {incomMessage.Text}");
                }
            });
            while (true)
            {
                var text = Console.ReadLine();
                if (string.IsNullOrEmpty(text))
                    continue;

                var message = new Message() { Text = text, Sender = nickName};
                var SerMess = JsonConvert.SerializeObject(message);
                var messageArray = Encoding.UTF8.GetBytes(SerMess);
                client.GetStream().Write(messageArray, 0, messageArray.Length);
                client.GetStream().Flush();
            }
        }
    }
    public class Message
    {
        public string Sender { get; set; }
        public string Text { get; set; }
    }
}
