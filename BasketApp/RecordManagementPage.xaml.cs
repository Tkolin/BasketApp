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
    /// Логика взаимодействия для RecordManagementPage.xaml
    /// </summary>
    public partial class RecordManagementPage : Page
    {
        public Record record;
        public bool edit;
        public RecordManagementPage()
        {
            InitializeComponent();
            record = new Record();
            edit = false;
        }
        public RecordManagementPage(Record record)
        {
            InitializeComponent();
            this.record = record;
            edit = true;
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            cBoxStudent.DisplayMemberPath = "LastName";
            cBoxStudent.SelectedValuePath = "ID";
            cBoxStudent.ItemsSource = BasketBDEntities.GetContext().Student.ToList();


            if (edit)
            {
                tBoxName.Text = record.Name;
                cBoxStudent.SelectedItem = record.Student;
                dPickEnd.SelectedDate = record.DateEnd;
                dPickStart.SelectedDate = record.DateStart;
            }

        }
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (cBoxStudent.SelectedItem == null || dPickStart.SelectedDate == null ||
                dPickEnd.SelectedDate == null || tBoxName.Text.Length <0 || dPickStart.SelectedDate>dPickEnd.SelectedDate)
                return;

            record.Name = tBoxName.Text;
            record.Student = cBoxStudent.SelectedItem as Student;
            record.DateStart = dPickStart.SelectedDate;
            record.DateEnd = dPickEnd.SelectedDate;
            if (!edit)
            {
                try
                {
                    BasketBDEntities.GetContext().Record.Add(record);
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
    }
}
