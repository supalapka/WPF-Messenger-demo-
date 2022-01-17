using System.Collections.Generic;

namespace PhoneBook2.Model
{
   public class User
    {
        public int Id { get; set; }
        public string login { get; set; }
        public string password { get; set; }
        public byte[] image { get; set; }
    }
}
