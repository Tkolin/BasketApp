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
            if (group != null)
                tBlockStudentName.Text += group.Name;
            if (coach != null)
                tBlockStudentName.Text += coach.LastName + " " + coach.FirstName;
            if (student != null)
                tBlockStudentName.Text += student.LastName + " " + student.FirstName;

        }
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {

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

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {

        }

        public List<Record> getRecord()
        {
            List<Record> records = BasketBDEntities.GetContext().Record.ToList();

            if (group != null)
                records = records.Where().ToList();
            if (coach != null)
                records = records.Where.ToList();
            if (student != null)
                records = records.Where.ToList();


            if ( dPickDateStart.SelectedDate != null)
                records = records.Where.ToList();
            if (dPickDateEnd.SelectedDate != null)
                records = records.Where.ToList();
            if (tBoxName.Text.Length > 0)
                records = records.Where.ToList();

            return records;
        }

    }
}
