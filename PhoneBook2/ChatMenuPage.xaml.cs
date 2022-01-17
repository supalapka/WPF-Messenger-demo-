using PhoneBook2.Components;
using PhoneBook2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace PhoneBook2
{
    public partial class ChatMenuPage : Page
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public static User currentChatUser;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public static Chat currentChat;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public static List<Chat> chats;
        List<Message> messagesList;
        List<Message> newMessages;

        private static bool readyToUpdates = false;

        public ChatMenuPage()
        {
            InitializeComponent();
            LoadChatsMenu();
            LoadChatingPanel();
        }

        private void LoadChatsMenu()
        {
            MessagePreviewGrid.Children.Clear();
            readyToUpdates = false;
            using (var context = new MyDbContext())
            {
                if (chats == null)
                    chats = context.Chats.Where(c => c.user_id1 == UserConfig.user_id || c.user_id2 == UserConfig.user_id).ToList();
                foreach (var chat in chats)
                {
                    RowDefinition row = new RowDefinition() { Height = new GridLength(100) };
                    MessagePreviewGrid.RowDefinitions.Add(row);
                    if (currentChat == null)
                    {
                        currentChat = chat;
                        messagesList = context.Messages.Where(m => m.chat_id == currentChat.id).ToList();
                    }
                    Button button = new Button()
                    {
                        Height = 100,
                        BorderBrush = Brushes.Transparent,
                        Background = Brushes.Transparent,
                        HorizontalContentAlignment = HorizontalAlignment.Left
                    };
                    Grid grid = new Grid();
                    ColumnDefinition columnDefinition1 = new ColumnDefinition() { Width = new GridLength(100) };
                    ColumnDefinition columnDefinition2 = new ColumnDefinition();
                    grid.ColumnDefinitions.Add(columnDefinition1);
                    grid.ColumnDefinitions.Add(columnDefinition2);
                    int chatMemberId;
                    if (UserConfig.user_id != chat.user_id1) //get another chat member user's ID
                        chatMemberId = chat.user_id1;
                    else chatMemberId = chat.user_id2;

                    var user = context.Users.Where(i => i.Id == chatMemberId).FirstOrDefault();
                    Image image = new Image() { Source = MyFunctionsClass.ConvertByteToImageSource(user.image), Margin = new Thickness(5), Stretch = Stretch.UniformToFill };

                    WrapPanel wrapPanel = new WrapPanel() { Orientation = Orientation.Vertical };
                    Label userName = new Label()
                    {
                        Content = user.login,
                        Foreground = Brushes.White,
                        FontSize = 30,
                    };
                    Label message = new Label()
                    {
                        Foreground = Brushes.Gray,
                        FontSize = 20,
                    };
                    var msg = context.Messages.OrderByDescending(i => i.id).Where(m => m.chat_id == chat.id).FirstOrDefault();
                    WrapPanel wrapPanel1 = new WrapPanel() { Orientation = Orientation.Horizontal };
                    Label msgFromName = new Label() { FontSize = 20, Foreground = Brushes.LightBlue };

                    if (msg != null)
                    {
                        if (msg.from_user_id == UserConfig.user_id)
                            msgFromName.Content = "You:";
                        else msgFromName.Content = user.login + ":";
                        message.Content = msg.message_content;

                    }

                    button.Click += delegate
                    {
                        currentChat = chat;
                        currentChatUser = user;
                        readyToUpdates = false;
                        LoadChatingPanel();
                    };

                    wrapPanel.Children.Add(userName);
                    wrapPanel1.Children.Add(msgFromName);
                    wrapPanel1.Children.Add(message);
                    wrapPanel.Children.Add(wrapPanel1);

                    Grid.SetColumn(image, grid.Children.Count);
                    grid.Children.Add(image);
                    Grid.SetColumn(wrapPanel, 1);
                    grid.Children.Add(wrapPanel);
                    button.Content = grid;
                    Grid.SetRow(button, MessagePreviewGrid.Children.Count);
                    MessagePreviewGrid.Children.Add(button);
                }
            }
        }

        public async void LoadChatingPanel()
        {
            readyToUpdates = false; // messages updating error preventer

            stackPanelMessages.Children.Clear();
            if (currentChatUser != null)
            {
                using (var ctx = new MyDbContext())
                {
                    labelName.Content = currentChatUser.login;
                    messagesList = ctx.Messages.Where(m => m.chat_id == currentChat.id).ToList();
                    foreach (var message in messagesList)
                    {
                        AddMessage(message.message_content, message.from_user_id);
                    }
                    readyToUpdates = true; // messages updating error preventer
                }
            }
            try
            {
                await Task.Run(() => //check for messages updating in database
                {
                    int tmp;
                    using (var ctx = new MyDbContext())
                    {
                        for (; ; )
                        {
                            Thread.Sleep(300);
                            if (currentChat == null || !readyToUpdates)
                                continue;
                            if (ctx.Messages.Where(m => m.chat_id == currentChat.id).ToList() == null)
                                continue;
                            else if (messagesList == null)
                                messagesList = ctx.Messages.Where(m => m.chat_id == currentChat.id).ToList();

                            if (messagesList.Count > 0)
                                if (messagesList.First().chat_id != currentChat.id)//update messages from current chat
                                    messagesList = ctx.Messages.Where(m => m.chat_id == currentChat.id).ToList();


                            newMessages = ctx.Messages.Where(m => m.chat_id == currentChat.id).ToList();
                            if (messagesList.Count != newMessages.Count)
                            {
                                messagesList = newMessages;
                                this.Dispatcher.Invoke(() =>
                                {
                                    AddMessage(messagesList.Last().message_content, messagesList.Last().from_user_id);
                                }, priority: DispatcherPriority.Normal);
                            }
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        [STAThread]
        void AddMessage(string message, int from_id)  //adding message textblock to xaml window
        {
            var bc = new BrushConverter();
            Border borderMessage = new Border()
            {
                CornerRadius = new CornerRadius(20),
                MaxWidth = 500,
                Margin = new Thickness(30, 10, 20, 0),
            };
            if (from_id == UserConfig.user_id)
                borderMessage.Background = (Brush)bc.ConvertFrom("#ff2b5278");
            else
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                borderMessage.Background = (Brush)bc.ConvertFrom("#ff182533");
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            TextBlock textBlockMessage = new TextBlock()
            {
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(10),
                Foreground = Brushes.White,
                FontSize = 20,
                Text = message,
            };
            borderMessage.Child = textBlockMessage;
            stackPanelMessages.Children.Add(borderMessage);
        }

        private async void SendMessage_button_Click(object sender, RoutedEventArgs e)  //save message to database
        {
            Message message = new Message()
            {
                chat_id = currentChat.id,
                from_user_id = UserConfig.user_id,
                message_content = messageTextBox.Text,
                message_time = DateTime.Now
            };

            //AddMessage(message.message_content, message.from_user_id);
            var ctx = new MyDbContext();
            ctx.Messages.Add(message);
            ctx.SaveChanges();

            messageTextBox.Clear();
        }


    }
}
