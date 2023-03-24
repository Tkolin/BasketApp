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
    /// Логика взаимодействия для GroupManagementPage.xaml
    /// </summary>
    public partial class GroupManagementPage : Page
    {
        public Group group;
        public bool edit;
        public GroupManagementPage()
        {
            InitializeComponent();
            this.group = new Group();
            edit = false;
        }
        public GroupManagementPage(Group group)
        {
            InitializeComponent();
            this.group = group;
            edit = true;
        }
        private void btnAddStudForGroup_Click(object sender, RoutedEventArgs e)
        {
            if (saveGroup())
            {
                NavigationService.Navigate(new GroupAddStudentPage(group));
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            cBoxCoach.DisplayMemberPath = "LastName";
            cBoxCoach.SelectedValuePath = "ID";
            cBoxCoach.ItemsSource = BasketBDEntities.GetContext().Coach.ToList();


            if(edit)
            {
                tBoxName.Text = group.Name;
                cBoxCoach.SelectedItem = group.Coach;
                dataGrid.ItemsSource = BasketBDEntities.GetContext().Student.Where(s => s.GroupID == group.ID).ToList();
            }


        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if(saveGroup())
             NavigationService.GoBack();
        }
        public bool saveGroup()
        {
            if (cBoxCoach.SelectedItem == null || tBoxName.Text.Length < 0)
                return false;

            group.Coach = cBoxCoach.SelectedItem as Coach;
            group.Name = tBoxName.Text;
            if (!edit)
            {
                try
                {
                    BasketBDEntities.GetContext().Group.Add(group);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error");
                }
            }
            try
            {
                BasketBDEntities.GetContext().SaveChanges();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
            return true;
        }
    }
}
