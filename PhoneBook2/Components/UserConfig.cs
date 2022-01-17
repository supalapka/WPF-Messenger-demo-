using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBook2.Components
{
    public static class UserConfig
    {
        public static int user_id;
      //  public static string username;


        public static void ResetProperties()
        {
            ChatMenuPage.currentChat = null;
            ChatMenuPage.currentChatUser = null;
            ChatMenuPage.chats = null;
        }
    }
}
