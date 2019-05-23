using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HesterConsultants.AppCode.Entities
{
    // to do
    public class Message
    {
        public int MessageId { get; set; }
        public string Subject { get; set; }
        public DateTime Date { get; set; }
        public Message RepliedToMessage { get; set; }
        public string Author { get; set; }
    }
}