using Microsoft.Office.Interop.Excel;
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
    public partial class GroupPage : System.Windows.Controls.Page
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
            cBoxCoach.ItemsSource = BasketBDEntities.GetContext().Coach.ToList();
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
                MessageBox.Show(ex.Message.ToString(),"Конфликт записей!");
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
            List<Group> groups = BasketBDEntities.GetContext().Group.ToList();


            if (tBoxSearchName.Text.Length != null)
            {
                string ser = tBoxSearchName.Text.ToLower();
                groups = groups.Where(g => g.Name.ToLower().Contains(ser)).ToList();
            }
            if (cBoxCoach.SelectedItem != null)
            {
                Coach coach = cBoxCoach.SelectedItem as Coach;
                groups = groups.Where(g => g.CoachID == coach.ID).ToList();
            }


            return groups;
        }


        private void tBoxSearchName_TextChanged(object sender, TextChangedEventArgs e)
        {
            dataGrid.ItemsSource = getGroup();
        }

        private void cBoxCoach_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dataGrid.ItemsSource = getGroup();
        }

        private void btnGoToRecordPage_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem == null)
                return;

            NavigationService.Navigate(new RecordPage(((Group)dataGrid.SelectedValue)));
        }

        private void btnVisitPage_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem == null)
                return;
            NavigationService.Navigate(new VisitPage( (Group)dataGrid.SelectedValue));
        }
        private void btnReset2_Click(object sender, RoutedEventArgs e)
        {
            dPickDateEnd.SelectedDate = null;
            dPickDateStart.SelectedDate = null;
        }
        private void btnOtchetForVisit_Click(object sender, RoutedEventArgs e)
        {
            //
            DateTime dStart;
            DateTime dEnd;

            if (dPickDateStart.SelectedDate != null)
                dStart = dPickDateStart.SelectedDate.Value;
            else
            {
                dStart = DateTime.Now;
                dStart = new DateTime(dStart.Year,dStart.Month,1);
            }                

            if (dPickDateEnd.SelectedDate != null)
                dEnd = dPickDateEnd.SelectedDate.Value;
            else   
                dEnd = DateTime.Now;

            if (dataGrid.SelectedItem == null || dStart > dEnd)
                return;
            Group group = dataGrid.SelectedItem as Group;
            //
            Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
            app.Visible = true;
            app.WindowState = XlWindowState.xlMaximized;
            Workbook wb = app.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);
            Worksheet ws = wb.Worksheets[1];

            //форматирование текста
            ws.StandardWidth = 9;

            ws.Range["A2:F2"].Merge();
            ws.Range["A2"].Value = "Отчёт по посещаемости";
            ws.Range["A2"].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            ws.Range["A2"].Font.Size = 16; ws.Range["A2"].Font.Bold = true;

            ws.Range["A3"].Value = "Група:"; ws.Range["B3:C3"].Merge();
            ws.Range["B3"].Value = group.Name;
            ws.Range["D3"].Value = "Тренер:"; ws.Range["E3:F3"].Merge();
            ws.Range["E3"].Value = group.Coach.LastName +" "+ group.Coach.FirstName.ToUpper().First() +"."+ group.Coach.Patronimic.ToUpper().First() + ".";

            ws.Range["A4"].Value = "Дата";
            ws.Range["B4"].Value = "от:";
            ws.Range["C4"].Value = dStart.Day + "." + dStart.Month + "." + dStart.Year;
            ws.Range["D4"].Value =  "до:";
            ws.Range["E4"].Value = dEnd.Day + "." + dEnd.Month + "." + dEnd.Year;

            ws.Range["A6:F6"].Font.Bold = true; ws.Range["A6:F6"].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            ws.Range["A6:F6"].RowHeight = 30; ws.Range["A6:F6"].WrapText = true;
            ws.Range["A6"].Value = "Номер записи";
            ws.Range["B6"].Value = "Имя студента";
            ws.Range["C6"].Value = "Фамилия студента";
            ws.Range["D6"].Value = "Группа";
            ws.Range["E6"].Value = "Дата посещения";
            ws.Range["F6"].Value = "Оценка";
            //

            List<Visit> visits = BasketBDEntities.GetContext().Visit.Where(
                r => r.Date >= dStart &&
                r.Date <= dEnd &&
                r.Student.GroupID == group.ID).ToList();

            int numberStart = 7;
            int numberEnd = numberStart;
            foreach (Visit t in visits)
            {

                ws.Range["A" + numberEnd].Value = numberEnd - numberStart + 1;
                ws.Range["B" + numberEnd].Value = t.Student.FirstName;
                ws.Range["C" + numberEnd].Value = t.Student.LastName;
                ws.Range["D" + numberEnd].Value = t.Student.Group.Name;
                DateTime dtime = (DateTime)t.Date;
                ws.Range["E" + numberEnd].Value = dtime.Day + "." +dtime.Month +"." +dtime.Year;
                if (t.Presence != null)                    
                ws.Range["F" + numberEnd].Value = t.Presence.Name;
                numberEnd++;
            }
            app.Calculation = XlCalculation.xlCalculationAutomatic;
            ws.Calculate();
        }
    }
}
