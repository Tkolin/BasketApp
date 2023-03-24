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
        bool edit;
        public CoachManagementPage()
        {
            InitializeComponent();
            this.coach = new Coach();
            edit = false; 
        }
        public CoachManagementPage(Coach coach)
        {
            InitializeComponent();
            this.coach = coach;
            edit = true;
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {


            cBoxAccaunt.SelectedValuePath = "ID";
            cBoxAccaunt.DisplayMemberPath = "Login";
            cBoxAccaunt.ItemsSource = BasketBDEntities.GetContext().User.ToList();


            cBoxPosition.SelectedValuePath = "ID";
            cBoxPosition.DisplayMemberPath = "Name";
            cBoxPosition.ItemsSource = BasketBDEntities.GetContext().Position.ToList();

            if (edit)
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
                MessageBox.Show("Не все поля были заполнены!", "Внимание!");
                return;
            }

            coach.FirstName = tBoxFirstName.Text;
            coach.LastName = tBoxLastName.Text;
            coach.Patronimic = tBoxPatronymic.Text;
            coach.PhoneNumber = tBoxPhoneNumber.Text;
            coach.PositionID = ((Position)cBoxPosition.SelectedItem).ID;
            coach.UserID = ((User)cBoxAccaunt.SelectedItem).ID;

            if(!edit)
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
            NavigationService.GoBack();
        }

    }
}
