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
    /// Логика взаимодействия для GroupPage.xaml
    /// </summary>
    public partial class GroupPage : Page
    {
        public GroupPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = getGroup();

            cBoxCoach.SelectedValuePath = "ID";
            cBoxCoach.DisplayMemberPath = "LastName";
            cBoxCoach.ItemsSource = EnglishKlassBDEntities.GetContext().ClassGroup.ToList();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
        private void btnRecordActivityGroup_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItems.Count == 0)
                return;

            NavigationService.Navigate(new RecordActivityGroupPage(((Group)dataGrid.SelectedValue)));

        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItems.Count == 0)
                return;

            NavigationService.Navigate(new GroupManagementPage(((Group)dataGrid.SelectedValue)));
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new GroupManagementPage());
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItems.Count == 0)
                return;

            try
            {
                BasketBDEntities.GetContext().Group.Remove(((Group)dataGrid.SelectedValue));
                BasketBDEntities.GetContext().SaveChanges();
                MessageBox.Show("Данные удалены! ");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

            dataGrid.ItemsSource = getGroup();
        }


        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            tBoxSearchName.Text = null;
            cBoxCoach.SelectedItem = null;
        }

        public List<Group> getGroup()
        {
            List<Group> coaches = BasketBDEntities.GetContext().Group.ToList();


            if (cBoxGroup.Text.Length != null)
            {
                string ser = tBoxSearchCoach.Text.ToLower();
                coaches = coaches.Where(c => c.FirstName.ToLower().Contains(ser) || c.LastName.ToLower().Contains(ser) || c.Patronimic.ToLower().Contains(ser)).ToList();
            }
            if (tBoxSearchName.Text.Length > 0)
            {
                Group grp = cBoxGroup.SelectedItem as Group;
                coaches = coaches.Where(c => c.ID == grp.CoachID).ToList();
            }


            return coaches;
        }


        private void tBoxSearchName_TextChanged(object sender, TextChangedEventArgs e)
        {
            dataGrid.ItemsSource = getGroup();
        }

        private void cBoxCoach_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dataGrid.ItemsSource = getGroup();
        }
    }
}
