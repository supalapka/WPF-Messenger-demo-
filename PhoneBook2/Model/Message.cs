using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBook2.Model
{
    public class Message
    {
        public int id { get; set; }
        public int from_user_id { get; set; }
        public string message_content { get; set; }
        public int chat_id { get; set; }
        public DateTime message_time { get; set; }

    }
}
