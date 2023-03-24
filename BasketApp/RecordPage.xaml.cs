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
    /// Логика взаимодействия для RecordPage.xaml
    /// </summary>
    public partial class RecordPage : Page
    {
        private Group group;
        private Student student;
        private Coach coach;
        public RecordPage()
        {
            InitializeComponent();
        }
        public RecordPage(Group group)
        {
            InitializeComponent();
            this.group = group;
        }
        public RecordPage(Student student)
        {
            InitializeComponent();
            this.student = student;
        }
        public RecordPage(Coach coach)
        {
            InitializeComponent();
            this.coach = coach;
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            cBoxPresenc.ItemsSource = BasketBDEntities.GetContext().Presence.ToList();
            cBoxPresenc.DisplayMemberPath = "Name";
            cBoxPresenc.SelectedValuePath = "ID";

            if (group != null)
                tBlockStudentName.Text += group.Name;
            if (coach != null)
                tBlockStudentName.Text += coach.LastName + " " + coach.FirstName;
            if (student != null)
            {
                tBlockStudentName.Text += student.LastName + " " + student.FirstName;
                btnAdd.Visibility = Visibility.Collapsed;
                btnDel.Visibility = Visibility.Collapsed;
                btnEdit.Visibility = Visibility.Collapsed;
                tBlocPart.Visibility = Visibility.Collapsed;
                cBoxPresenc.Visibility = Visibility.Collapsed;                          
            }
            dataGrid.ItemsSource = getRecord(); 

        }
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem == null)
                return;

            NavigationService.Navigate(new RecordManagementPage(((Record)dataGrid.SelectedItem)));
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RecordManagementPage());
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItems.Count == 0)
                return;

            try
            {
                BasketBDEntities.GetContext().Record.Remove(((Record)dataGrid.SelectedItem));
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
            tBoxName.Text = null;
        }

        public List<Record> getRecord()
        {
            List<Record> records = BasketBDEntities.GetContext().Record.ToList();

            if (group != null) {
                List<Student> _student = BasketBDEntities.GetContext().Student.Where(s => s.GroupID == group.ID).ToList();                                
                records = records.Where(r => _student.Equals(r.Student)).ToList();        
            }
            if (coach != null) {
                List<Student> _student = BasketBDEntities.GetContext().Student.Where(s => s.Group.CoachID == coach.ID).ToList();
                records = records.Where(r => _student.Equals(r.Student)).ToList();
            }
            if (student != null)
                records = records.Where(r => r.StudentID == student.ID).ToList();


            if (dPickDateStart.SelectedDate != null)
                records = records.Where(r => r.DateStart > dPickDateStart.SelectedDate.Value).ToList();
            if (dPickDateEnd.SelectedDate != null)
                records = records.Where(r => r.DateEnd < dPickDateEnd.SelectedDate.Value).ToList();
            if (tBoxName.Text.Length > 0)
                records = records.Where(r => r.Name.ToLower().Contains(tBoxName.Text.ToLower())).ToList();

            return records;
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGrid.SelectedItem == null)
                return;
            cBoxPresenc.SelectedItem = ((Record)dataGrid.SelectedItem).Presence;
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
            ((Record)dataGrid.SelectedItem).Presence = cBoxPresenc.SelectedItem as Presence;
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
