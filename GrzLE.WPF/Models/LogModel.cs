using System;
using System.Collections.Generic;
using System.Text;

namespace GrzLE.WPF.Models
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
