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
    /// Логика взаимодействия для VisitPage.xaml
    /// </summary>
    public partial class VisitPage : Page
    {
        private Group group;
        private Coach coach;
        private Student student;
        public VisitPage()
        {
            InitializeComponent();
        }
        public VisitPage(Visit visit)
        {
            InitializeComponent();
        }
        public VisitPage(Group group)
        {
            InitializeComponent();
            this.group = group;
        }

        public VisitPage(Coach coach)
        {
            InitializeComponent();
            this.coach = coach;
        }
        public VisitPage(Student student)
        {
            InitializeComponent();
            this.student = student;
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            cBoxPresenc.ItemsSource = BasketBDEntities.GetContext().Presence.ToList();
            cBoxPresenc.DisplayMemberPath = "Name";
            cBoxPresenc.SelectedValuePath = "ID";

            if (group != null)
                tBlockStudentName.Text += group.Name;
            else if(coach != null)
                tBlockStudentName.Text += coach.LastName + " " + coach.FirstName;
            else if (student != null)
            {
                tBlockStudentName.Text += student.LastName + " " + student.FirstName;
                btnAdd.Visibility = Visibility.Collapsed;
                btnDel.Visibility = Visibility.Collapsed;
                btnEdit.Visibility = Visibility.Collapsed;
                tBlocPart.Visibility = Visibility.Collapsed;
                cBoxPresenc.Visibility = Visibility.Collapsed;
            }
            else           
                tBlockStudentName.Text += student.LastName + " " + student.FirstName;

            dataGrid.ItemsSource = getRecord();

        }
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem == null)
                return;

            NavigationService.Navigate(new VisitManagementPage(((Visit)dataGrid.SelectedItem)));
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new VisitManagementPage());
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItems.Count == 0)
                return;

            try
            {
                BasketBDEntities.GetContext().Visit.Remove(((Visit)dataGrid.SelectedItem));
                BasketBDEntities.GetContext().SaveChanges();
                MessageBox.Show("Данные удалены! ");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

            dataGrid.ItemsSource = getRecord();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            dPickDateEnd.SelectedDate = null;
            dPickDateStart.SelectedDate = null;
            tBoxStud.Text = null;
        }

        public List<Visit> getRecord()
        {
            List<Visit> visits = BasketBDEntities.GetContext().Visit.ToList();

            if (group != null)
                visits = visits.Where(r => r.Student.GroupID == group.ID).ToList();
            else if (coach != null)
                visits = visits.Where(r => r.Student.Group.CoachID == coach.ID).ToList();
            else if (student != null)
                visits = visits.Where(r => r.StudentID == student.ID).ToList();

            if (dPickDateStart.SelectedDate != null)
                visits = visits.Where(r => r.Date >= dPickDateStart.SelectedDate.Value).ToList();
            if (dPickDateEnd.SelectedDate != null)
                visits = visits.Where(r => r.Date <= dPickDateEnd.SelectedDate.Value).ToList();
            if (tBoxStud.Text.Length > 0)
                visits = visits.Where(r => 
                r.Student.FirstName.ToLower().Contains(tBoxStud.Text.ToLower()) ||
                r.Student.LastName.ToLower().Contains(tBoxStud.Text.ToLower()) ||
                r.Student.Patronimic.ToLower().Contains(tBoxStud.Text.ToLower()))
                    .ToList();

            return visits;
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGrid.SelectedItem == null)
                return;
            cBoxPresenc.SelectedItem = ((Visit)dataGrid.SelectedItem).Presence;
        }

        private void dPickDateStart_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            dataGrid.ItemsSource = getRecord();
        }

        private void dPickDateEnd_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            dataGrid.ItemsSource = getRecord();
        }

        private void tBoxName_TextChanged(object sender, TextChangedEventArgs e)
        {
            dataGrid.ItemsSource = getRecord();
        }

        private void cBoxPresenc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGrid.SelectedItem == null)
                return;
            ((Visit)dataGrid.SelectedItem).Presence = cBoxPresenc.SelectedItem as Presence;
            try
            {
                BasketBDEntities.GetContext().SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


            dataGrid.ItemsSource = getRecord();
        }
    }
}
