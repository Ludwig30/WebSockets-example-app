using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ClassLibraryJobs
{
    public class Message
    {        
        public required string Action { get; set; }
        public int Duration { get; set; }
        public string Id { get; set; }

        public byte[] Serialize()
        {
            string json = $"{{\"Action\":\"{Action}\",\"Duration\":{Duration},\"Id\":\"{Id}\"}}";
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            bytes = bytes.Where(b => b != 0).ToArray();
            foreach (byte b in bytes)
            {
                Console.Write($"{b:X2} ");
            }
            Console.WriteLine();
            return bytes;
        }

        public static Message? Deserialize(byte[] bytes)
        {
            bytes = bytes.Where (b => b != 0).ToArray();
            string json = Encoding.UTF8.GetString(bytes);
            return JsonSerializer.Deserialize<Message>(json);
        }
    }
}
