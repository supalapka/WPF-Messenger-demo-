using Microsoft.Win32;
using PhoneBook2.Components;
using PhoneBook2.Model;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace PhoneBook2
{
    public partial class AddContactWindow : Page
    {
        public static Contact contact = new Contact();
        public static bool isEditing = false;


        public AddContactWindow()
        {
            InitializeComponent();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("ContactsListPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new MyDbContext())
            {

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                User user = context.Users.Where(u => u.login == NumberTextBox.Text).FirstOrDefault();
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
                contact.owner_id = UserConfig.user_id;
                contact.user_id = user.Id;
                context.Contacts.Add(contact);
                context.SaveChanges();
            }
            this.NavigationService.Navigate(new Uri("ContactsListPage.xaml", UriKind.RelativeOrAbsolute));
        } 

    }
}
