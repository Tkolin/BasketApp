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
    /// Логика взаимодействия для MainMenyPage.xaml
    /// </summary>
    public partial class MainMenyPage : Page
    {
        User user;
        Coach coachl;
        Student student;
        public MainMenyPage(User user)
        {
            InitializeComponent();
            this.user = user;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            switch (user.RoleID)
            {
                case 1:
                    student = BasketBDEntities.GetContext().Student.Where(s => s.User.ID == user.ID).First();
                    NavigationService.Navigate(new RecordPage(student));
                    break;
                case 2:
                    coachl = BasketBDEntities.GetContext().Coach.Where(s => s.User.ID == user.ID).First();
                    btnCoach.Visibility= Visibility.Collapsed;
                    break;
                case 3:
                     
                    break;
            }
        }

        private void btnCoach_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new   CoachPage() );
        }

        private void btnGroup_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new   GroupPage());

        }

        private void btnRecord_Click(object sender, RoutedEventArgs e)
        {
            if(student != null)
                NavigationService.Navigate(new RecordPage(student));
            else if (coachl != null)
                NavigationService.Navigate(new RecordPage(coachl));
            else
                NavigationService.Navigate(new RecordPage());
        }

        private void btnStudent_Click(object sender, RoutedEventArgs e)
        {
            if (coachl != null)
                NavigationService.Navigate(new StudentPage(coachl));
            else
                NavigationService.Navigate(new StudentPage());

        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void btnVisit_Click(object sender, RoutedEventArgs e)
        {
            if (student != null)
                NavigationService.Navigate(new VisitPage(student));
            else if (coachl != null)
                NavigationService.Navigate(new VisitPage(coachl));
            else
                NavigationService.Navigate(new VisitPage());
        }
    }
}
