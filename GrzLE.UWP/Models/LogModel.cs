using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrzLE.UWP.Models
{
    class LogModel
    {
        public string Sender { get; }
        public string Message { get; }
        public DateTime Time { get; }

        public LogModel(string sender, string message)
        {
            Sender = sender;
            Message = message;
            Time = DateTime.Now;
        }

        public override string ToString()
            => $"[{Time}] [{Sender}] {Message}";
    }
}
