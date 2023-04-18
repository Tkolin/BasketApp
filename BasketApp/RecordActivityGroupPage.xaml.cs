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
    /// Логика взаимодействия для RecordActivityGroupPage.xaml
    /// </summary>
    public partial class RecordActivityGroupPage : Page
    {
        Group group;
        bool edit;
        public RecordActivityGroupPage()
        {
            InitializeComponent();
            this.group = new Group();
            edit = false;
        }
        public RecordActivityGroupPage(Group group)
        {
            InitializeComponent();
            this.group = group;
            edit = true;
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (dPickEnd.SelectedDate == null || dPickStart.SelectedDate == null ||
               cBoxGroup.SelectedItem == null || tBoxName.Text.Length < 0)
                return;


            foreach(Student student in BasketBDEntities.GetContext().Student
                .Where(r=>r.GroupID == group.ID))
            {
                Record record = new Record();

                record.Name = tBoxName.Text;
                record.DateEnd = dPickEnd.SelectedDate;
                record.DateStart = dPickStart.SelectedDate;
                record.StudentID = student.ID;
                try
                {
                    BasketBDEntities.GetContext().Record.Add(record);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка задания у " + student.FirstName + " " + student.LastName);
                }

            }
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

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            cBoxGroup.DisplayMemberPath = "Name";
            cBoxGroup.SelectedValuePath = "ID";
            cBoxGroup.ItemsSource = BasketBDEntities.GetContext().Group.ToList();
        
            if (edit)
            {
                cBoxGroup.SelectedItem = group;
                cBoxGroup.IsEnabled = false;
            }
        }
    }
}
