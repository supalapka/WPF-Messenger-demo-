using PhoneBook2.Components;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PhoneBook2
{

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Logout_Button_Click(object sender, RoutedEventArgs e)
        {
            UserConfig.ResetProperties();
            Uri  uri= new Uri("LoginWindow.xaml", UriKind.Relative);
            MainFrame.Source = uri;
        }

        private void Contacts_button_Click(object sender, RoutedEventArgs e)
        {
            Uri uri = new Uri("ContactsListPage.xaml", UriKind.Relative);
            MainFrame.Source = uri;
        }

        private void CreateGroup_button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Chats_button_Click(object sender, RoutedEventArgs e)
        {
            Uri uri = new Uri("ChatMenuPage.xaml", UriKind.Relative);
            MainFrame.Source = uri;
        }
    }
}
