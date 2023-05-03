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
    /// Логика взаимодействия для VisitManagementPage.xaml
    /// </summary>
    public partial class VisitManagementPage : Page
    {

        public Visit visit;
        public bool edit;
        public VisitManagementPage()
        {
            InitializeComponent();
            visit = new Visit();
            edit = false;
        }

        public VisitManagementPage(Visit visit)
        {
            InitializeComponent();
            this.visit = visit;
            edit = true;
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            cBoxStudent.SelectedValuePath = "ID";
            cBoxStudent.ItemsSource = BasketBDEntities.GetContext().Student.ToList();

            cBoxStatus.DisplayMemberPath = "Name";
            cBoxStudent.SelectedValuePath = "ID";
            cBoxStatus.ItemsSource = BasketBDEntities.GetContext().Presence.ToList();

            if (edit)
            {
                tBoxID.Text = visit.Number.ToString();
                cBoxStudent.SelectedItem = visit.Student;
                dPicerDate.SelectedDate = visit.Date;
                cBoxStatus.SelectedItem = visit.Presence;


            }

        }
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (cBoxStudent.SelectedItem == null || dPicerDate.SelectedDate == null)
                return;

            visit.Student = cBoxStudent.SelectedItem as Student;
            visit.Date = dPicerDate.SelectedDate;                               
            visit.Presence = cBoxStatus.SelectedItem as Presence;


            if (!edit)
            {
                try
                {
                    BasketBDEntities.GetContext().Visit.Add(visit);
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка добавления");
                }
            }
            try
            {
                BasketBDEntities.GetContext().SaveChanges();
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка сохранения");
            }

        }

        private void btnDateNow_Click(object sender, RoutedEventArgs e)
        {
            dPicerDate.SelectedDate = DateTime.Now;
        }
    }
}
