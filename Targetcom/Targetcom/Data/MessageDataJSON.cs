using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Targetcom.Data
{
    public class MessageDataJSON
    {
        public Sender Sender { get; set; } = new Sender();
        public string Message { get; set; }
        public string SendedStamp { get; set; }      
    }
    public class Sender
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string HashedPassword { get; set; }
    }
}
