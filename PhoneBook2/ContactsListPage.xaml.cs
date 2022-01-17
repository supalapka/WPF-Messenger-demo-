using PhoneBook2.Components;
using PhoneBook2.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PhoneBook2
{
    /// <summary>
    /// Interaction logic for ContactsListPage.xaml
    /// </summary>
    public partial class ContactsListPage : Page
    {
        public ContactsListPage()
        {
            InitializeComponent();
            ShowContacts();
        }
       

        void ShowContacts()
        {
            contactsGrid.Children.Clear();
            List<Contact> contacts = new List<Contact>();
            var context = new MyDbContext();
            contacts = context.Contacts.Where(c => c.owner_id == UserConfig.user_id).ToList<Contact>();
            foreach (var contact in contacts)
            {
                RowDefinition rowDefinition = new RowDefinition() { Height = new GridLength(60) };
                contactsGrid.RowDefinitions.Add(rowDefinition);

                Grid grid = new Grid();
                ColumnDefinition columnDefinition = new ColumnDefinition() { Width = new GridLength(60) };
                ColumnDefinition columnDefinition2 = new ColumnDefinition();
                grid.ColumnDefinitions.Add(columnDefinition);
                grid.ColumnDefinitions.Add(columnDefinition2);

                var user = context.Users.Where(u => u.Id == contact.user_id).FirstOrDefault<User>();
                var image = new Image() { Source = MyFunctionsClass.ConvertByteToImageSource(user.image), Stretch = Stretch.UniformToFill};
                var labelName = new Label() { Content = user.login };

                Grid.SetColumn(image, 0);
                Grid.SetColumn(labelName, 1);

                ContextMenu contextMenu = new ContextMenu();
                MenuItem itemSendMessage = new MenuItem();
                MenuItem itemDelete = new MenuItem();
                itemSendMessage.Header = "Send message";
                itemDelete.Header = "Delete";

                itemDelete.Click += delegate
                {
                    context.Contacts.Remove(contact);
                    context.SaveChanges();
                    ShowContacts();
                };

                itemSendMessage.Click += delegate
                {
                    var chat = context.Chats.Where(c => (c.user_id1 == UserConfig.user_id && c.user_id2 == contact.user_id) ||
                                                        (c.user_id1 == contact.user_id && c.user_id2 == UserConfig.user_id)).FirstOrDefault();
                    if(chat == null)
                    {
                        chat = new Chat() {
                            user_id1 = UserConfig.user_id,
                        user_id2 = contact.user_id};
                        context.Chats.Add(chat);
                        context.SaveChanges();
                    }
                    ChatMenuPage.currentChatUser = user;
                    ChatMenuPage.currentChat = chat;
                    this.NavigationService.Navigate(new Uri("ChatMenuPage.xaml", UriKind.RelativeOrAbsolute));
                };

                contextMenu.Items.Add(itemSendMessage);
                contextMenu.Items.Add(itemDelete);

                grid.ContextMenu = contextMenu;
                grid.Children.Add(image);
                grid.Children.Add(labelName);
                Grid.SetRow(grid, contactsGrid.Children.Count);
                contactsGrid.Children.Add(grid);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ShowContacts();
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("AddContactWindow.xaml", UriKind.RelativeOrAbsolute));
        }
    }
}
