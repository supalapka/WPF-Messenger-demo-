using System;
using System.Data.Entity;

namespace PhoneBook2.Model
{
    class MyDbContext : DbContext
    {
        public MyDbContext():base("connectionString"){ }

        public DbSet<User> Users { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Chat> Chats { get; set; }
    }
}
