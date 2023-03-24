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
    /// Логика взаимодействия для StudentPage.xaml
    /// </summary>
    public partial class StudentPage : Page
    {
        public StudentPage()
        {
            InitializeComponent();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = getStudents();
        }
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItems.Count == 0)
                return;

            try
            {
                BasketBDEntities.GetContext().Student.Remove(((Student)dataGrid.SelectedItem));
                BasketBDEntities.GetContext().SaveChanges();
                MessageBox.Show("Данные удалены! ");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

            dataGrid.ItemsSource = getStudents();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new StudentManagementPage());
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItems.Count == 0)
                return;

            NavigationService.Navigate(new StudentManagementPage(((Student)dataGrid.SelectedItem)));
        }

        private void tBoxSearchStud_TextChanged(object sender, TextChangedEventArgs e)
        {
            dataGrid.ItemsSource = getStudents();
        }

        private void tBoxSearchCoach_TextChanged(object sender, TextChangedEventArgs e)
        {
            dataGrid.ItemsSource = getStudents();
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = getStudents();
        }
        private void cBoxGroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dataGrid.ItemsSource = getStudents();
        }

        public List<Student> getStudents()
        {
            List<Student> coaches = BasketBDEntities.GetContext().Student.ToList();


            if (tBoxSearchCoach.Text.Length > 0)
            {
                string ser = tBoxSearchCoach.Text.ToLower();
                coaches = coaches.Where(c => c.FirstName.ToLower().Contains(ser) || c.LastName.ToLower().Contains(ser) || c.Patronimic.ToLower().Contains(ser)).ToList();
            }
            if (cBoxGroup.SelectedItem != null)
            {
                Position grp = cBoxGroup.SelectedItem as Position;
                coaches = coaches.Where(c => c.PositionID == grp.ID).ToList();
            }
            if (cBoxGroup.SelectedItem != null)
            {
                Position grp = cBoxGroup.SelectedItem as Position;
                coaches = coaches.Where(c => c.PositionID == grp.ID).ToList();
            }

            return coaches;
        }


    }
}
