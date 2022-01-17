using PhoneBook2.Components;
using PhoneBook2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PhoneBook2
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Page
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Register_Button(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("RegisterWindow.xaml", UriKind.RelativeOrAbsolute));
        }

        private void Login_Button_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new MyDbContext())
            {
                var users = context.Users.Where(p => p.login == LoginTextBox.Text);

                if (users.Count() > 0)
                    foreach (User user in users)  //fix with single linq
                        if (user.password == passwordBox.Password)
                        {
                            UserConfig.user_id = user.Id;
                            this.NavigationService.Navigate(new Uri("MainMenuWindow.xaml", UriKind.RelativeOrAbsolute));
                        }
                        else MessageBox.Show("wrong");
                else MessageBox.Show("wrong");
            }
        }
    }
}
