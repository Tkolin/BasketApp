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
    /// Логика взаимодействия для CoachManagementPage.xaml
    /// </summary>
    public partial class CoachManagementPage : Page
    {
        private Coach coach;
        public CoachManagementPage()
        {
            InitializeComponent();
            this.coach = new Coach();
        }
        public CoachManagementPage(Coach coach)
        {
            InitializeComponent();
            this.coach = coach;
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (coach != null)
            {
                tBoxFirstName.Text = coach.FirstName;
                tBoxLastName.Text = coach.LastName;
                tBoxPatronymic.Text = coach.Patronimic;
                tBoxPhoneNumber.Text = coach.PhoneNumber;
                cBoxPosition.SelectedItem = coach.Position;
                cBoxAccaunt.SelectedItem = coach.User;           
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (tBoxFirstName.Text.Length < 0||tBoxLastName.Text.Length < 0||cBoxPosition.SelectedItem == null)
            {
                //MessegBox.Show
                return;
            }
            coach.FirstName = tBoxFirstName.Text;
            coach.LastName = tBoxLastName.Text;
            coach.Patronimic = tBoxPatronymic.Text;
            coach.PhoneNumber = tBoxPhoneNumber.Text;
            coach.PositionID = ((Position)cBoxPosition.SelectedItem).ID;
            coach.UserID = ((User)cBoxAccaunt.SelectedItem).ID;

            if(coach.ID.ToString().Length < 0)
            {
                try
                {
                    BasketBDEntities.GetContext().Coach.Add(coach);
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }  
            try
            {
                BasketBDEntities.GetContext().SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

    }
}
