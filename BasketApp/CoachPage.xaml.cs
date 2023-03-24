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
    /// Логика взаимодействия для CoachPage.xaml
    /// </summary>
    public partial class CoachPage : Page
    {
        public CoachPage()
        {
            InitializeComponent();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = getCoaches();

            cBoxGroup.SelectedValuePath = "ID";
            cBoxGroup.DisplayMemberPath = "Name";
            cBoxGroup.ItemsSource = BasketBDEntities.GetContext().Position.ToList();
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
                BasketBDEntities.GetContext().Coach.Remove(((Coach)dataGrid.SelectedValue));
                BasketBDEntities.GetContext().SaveChanges();
                MessageBox.Show("Данные удалены! ");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

            dataGrid.ItemsSource = getCoaches();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CoachManagementPage());
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItems.Count == 0)
                return;

            NavigationService.Navigate(new CoachManagementPage(((Coach)dataGrid.SelectedValue)));
        }

        private void tBoxSearchCoach_TextChanged(object sender, TextChangedEventArgs e)
        {
            dataGrid.ItemsSource = getCoaches();
        }
        private void cBoxGroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dataGrid.ItemsSource = getCoaches();
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            tBoxSearchCoach.Text = null;
            cBoxGroup.SelectedItem = null;

            dataGrid.ItemsSource = getCoaches();
        }


        public List<Coach> getCoaches()
        {
            List<Coach> coaches = BasketBDEntities.GetContext().Coach.ToList();


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


            return coaches;
        }


    }
}
