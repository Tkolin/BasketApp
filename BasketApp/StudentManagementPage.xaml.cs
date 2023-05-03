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
    /// Логика взаимодействия для StudentManagementPage.xaml
    /// </summary>
    public partial class StudentManagementPage : Page
    {
        Student student;
        bool edit;
        public StudentManagementPage()
        {
            InitializeComponent();
            this.student = new Student();
            edit = false;
        }
        public StudentManagementPage(Student student)
        {
            InitializeComponent();
            this.student = student;
            edit = true;
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            cBoxAccaunt.DisplayMemberPath = "Login";
            cBoxAccaunt.SelectedValuePath = "ID";
            cBoxAccaunt.ItemsSource = BasketBDEntities.GetContext().User.ToList();

            cBoxGroup.DisplayMemberPath = "Name";
            cBoxGroup.SelectedValuePath = "ID";
            cBoxGroup.ItemsSource = BasketBDEntities.GetContext().Group.ToList();


            cBoxGender.DisplayMemberPath = "Name";
            cBoxGender.SelectedValuePath = "ID";
            cBoxGender.ItemsSource = BasketBDEntities.GetContext().Gender.ToList();

            if (edit)
            {
                tBoxFirstName.Text = student.FirstName;
                tBoxLastName.Text = student.LastName;
                tBoxPatronymic.Text = student.Patronimic;
                dPickBirth.SelectedDate = student.DateBirth;
                cBoxAccaunt.SelectedItem = student.User;
                cBoxGroup.SelectedItem = student.Group;

                tBoxPhoneNumber.Text = student.PhoneNumber;
                cBoxGender.SelectedItem = student.Gender;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (tBoxFirstName.Text.Length < 0 || tBoxLastName.Text.Length < 0 ||
                dPickBirth.SelectedDate == null)


             student.FirstName = tBoxFirstName.Text;
             student.LastName  = tBoxLastName.Text;
             student.Patronimic= tBoxPatronymic.Text;
             student.DateBirth = dPickBirth.SelectedDate;
             student.User = cBoxAccaunt.SelectedItem as User;
             student.Group = cBoxGroup.SelectedItem as Group;

            student.PhoneNumber = tBoxPhoneNumber.Text;
            student.Gender = cBoxGender.SelectedItem as Gender;

            if (!edit)
            {
                try
                {
                    BasketBDEntities.GetContext().Student.Add(student);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error");
                }
            }
            try
            {
                BasketBDEntities.GetContext().SaveChanges();
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
