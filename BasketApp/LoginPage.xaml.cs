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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BasketApp
{
    /// <summary>
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnAuth_Click(object sender, RoutedEventArgs e)
        {
            if (login() == null)
            {
                MessageBox.Show("Проверьте логин/пароль!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            User users = login();
            NavigationService.Navigate(new MainMenyPage(users));
 
        }

        private User login()
        {
            List<User> users = BasketBDEntities.GetContext().User.ToList();
            foreach (User checkUser in users)
            {
                if(checkUser.Login.Replace(" ", "").Equals(TBoxLogin.Text.Replace(" ", "")) &&
                    checkUser.Password.Replace(" ", "").Equals(TBoxPassword.Text.Replace(" ", "")))
                {
                    return checkUser;
                }
            }
            return null;
        }
    }
}
