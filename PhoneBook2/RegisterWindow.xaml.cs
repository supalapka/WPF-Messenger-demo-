using Microsoft.Win32;
using PhoneBook2.Components;
using PhoneBook2.Model;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace PhoneBook2
{
    public partial class RegisterWindow : Page//, INotifyPropertyChanged
    {
        User user = new User();
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new MyDbContext())
            {
                user.login = LoginTextBox.Text;  //fix
                user.password = PassBox1.Password;  //fix

                context.Users.Add(user);
                context.SaveChanges();
            }

            this.NavigationService.Navigate(new Uri("LoginWindow.xaml", UriKind.RelativeOrAbsolute));


        }

        private void PassChanged(object sender, RoutedEventArgs e)
        {
            if (PassBox1.Password != "" && PassBox1.Password == PassBox2.Password)
                RegButton.IsEnabled = true;
            else RegButton.IsEnabled = false;
        }

        private void Pass2Changed(object sender, RoutedEventArgs e)
        {
            if (PassBox1.Password != "" && PassBox1.Password == PassBox2.Password)
                RegButton.IsEnabled = true;
            else RegButton.IsEnabled = false;
        }

        private void Back_Button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("LoginWindow.xaml", UriKind.RelativeOrAbsolute));
        }

        private void LoadImageButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "PNG|*.png|JPG|*.jpg";
            openFileDialog.ShowDialog();

            BitmapImage bitmapImage = new BitmapImage(new Uri(openFileDialog.FileName, UriKind.RelativeOrAbsolute));
            user.image = MyFunctionsClass.ConvertImageToByte(bitmapImage);


        }
    }
}
