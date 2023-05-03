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
    /// Логика взаимодействия для GroupAddStudentPage.xaml
    /// </summary>
    public partial class GroupAddStudentPage : Page
    {
        public Group group;
        public GroupAddStudentPage(Group group)
        {
            InitializeComponent();
            this.group = group;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            tBlockGrpoub.Text += group.Name.ToString();

            cBoxLastName.SelectedValuePath = "ID";
            cBoxLastName.ItemsSource = BasketBDEntities.GetContext().Student.ToList();

            Binding binding = new Binding();
            Binding binding1 = new Binding();
            Binding binding2 = new Binding();

            binding.ElementName = "cBoxLastName";
            binding.Path = new PropertyPath("SelectedItem.FirstName");
            tBoxFirstName.SetBinding(TextBox.TextProperty, binding);

            binding1.ElementName = "cBoxLastName";
            binding1.Path = new PropertyPath("SelectedItem.Patronimic");
            tBoxPatronymic.SetBinding(TextBox.TextProperty, binding1);

            binding2.ElementName = "cBoxLastName";
            binding2.Path = new PropertyPath("SelectedItem.User.Login");
            tBoxAccount.SetBinding(TextBox.TextProperty, binding2);
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (cBoxLastName.SelectedItem == null)
                return;

            Student stud = (Student)cBoxLastName.SelectedItem;
            stud.Group = group;

            try
            {
                BasketBDEntities.GetContext().SaveChanges();
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
