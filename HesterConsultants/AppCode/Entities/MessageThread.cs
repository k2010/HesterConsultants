using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HesterConsultants.AppCode.Entities
{
    // to do
    public class MessageThread
    {
        public int ThreadId { get; set; }
        public IList<Message> Messages { get; set; }
    }
}