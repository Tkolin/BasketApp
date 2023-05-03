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
    /// Логика взаимодействия для StudentPage.xaml
    /// </summary>
    public partial class StudentPage : System.Windows.Controls.Page
    {
        Coach coach;
        public StudentPage()
        {
            InitializeComponent();
        }
        public StudentPage(Coach coach)
        {
            InitializeComponent();
            this.coach = coach; 
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            getStudents();
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
                BasketBDEntities.GetContext().Student.Remove(((Student)dataGrid.SelectedItem));
                BasketBDEntities.GetContext().SaveChanges();
                MessageBox.Show("Данные удалены! ");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

            getStudents();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new StudentManagementPage());
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItems.Count == 0)
                return;

            NavigationService.Navigate(new StudentManagementPage(((Student)dataGrid.SelectedItem)));
        }

        private void tBoxSearchStud_TextChanged(object sender, TextChangedEventArgs e)
        {
            getStudents();
        }

        private void tBoxSearchCoach_TextChanged(object sender, TextChangedEventArgs e)
        {
            getStudents();
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            getStudents();
        }
        private void cBoxGroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            getStudents();
        }

        public void getStudents()
        {
            List<Student> students = BasketBDEntities.GetContext().Student.ToList();

            if(coach != null)
                students = students.Where(c => c.Group.CoachID == coach.ID ).ToList();

            if (tBoxSearchCoach.Text.Length > 0)
            {
                string ser = tBoxSearchCoach.Text.ToLower();
                    students = students.Where(c => c.Group.Coach.FirstName.ToLower().Contains(ser) ||
                     c.Group.Coach.LastName.ToLower().Contains(ser) || 
                     c.Group.Coach.Patronimic.ToLower().Contains(ser))
                     .ToList();
            }
            if (tBoxSearchStud.Text.Length > 0)
            {
                string ser = tBoxSearchStud.Text.ToLower();
                students = students.Where(c => c.FirstName.ToLower().Contains(ser) ||
                c.LastName.ToLower().Contains(ser)||c.Patronimic.ToLower().Contains(ser))
                    .ToList();
            }
            if (cBoxGroup.SelectedItem != null)
            {
                Group grp = cBoxGroup.SelectedItem as Group;
                students = students.Where(c => c.GroupID == grp.ID).ToList();
            }

            
            dataGrid.ItemsSource = students;
        }

        private void btnVisit_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem == null)
                return;

            NavigationService.Navigate(new VisitPage(dataGrid.SelectedItem as Student));
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
                dStart = new DateTime(dStart.Year, dStart.Month, 1);
            }

            if (dPickDateEnd.SelectedDate != null)
                dEnd = dPickDateEnd.SelectedDate.Value;
            else
                dEnd = DateTime.Now;

            if (dataGrid.SelectedItem == null || dStart > dEnd)
                return;
            Student student = dataGrid.SelectedItem as Student;
            //
            Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
            app.Visible = true;
            app.WindowState = XlWindowState.xlMaximized;
            Workbook wb = app.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);
            Worksheet ws = wb.Worksheets[1];

            //форматирование текста
            ws.StandardWidth = 9;

            ws.Range["A2:F2"].Merge();
            ws.Range["A2"].Value = "Отчёт по посещаемости: "+student.LastName+" "+student.FirstName.First() + ". " + student.Patronimic.First()+".";
            ws.Range["A2"].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            ws.Range["A2"].Font.Size = 16; ws.Range["A2"].Font.Bold = true;

            ws.Range["A3"].Value = "Група:"; ws.Range["B3:C3"].Merge();
            ws.Range["B3"].Value = student.Group.Name;
            ws.Range["D3"].Value = "Тренер:"; ws.Range["E3:F3"].Merge();
            ws.Range["E3"].Value = student.Group.Coach.LastName + " " + student.Group.Coach.FirstName.ToUpper().First() + "."
                + student.Group.Coach.Patronimic.ToUpper().First() + ".";

            ws.Range["A4"].Value = "Дата";
            ws.Range["B4"].Value = "от:";
            ws.Range["C4"].Value = dStart.Day + "." + dStart.Month + "." + dStart.Year;
            ws.Range["D4"].Value = "до:";
            ws.Range["E4"].Value = dEnd.Day + "." + dEnd.Month + "." + dEnd.Year;

            ws.Range["A6:F6"].Font.Bold = true; ws.Range["A6:F6"].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            ws.Range["A6:F6"].RowHeight = 30; ws.Range["A6:F6"].WrapText = true;
            ws.Range["A6"].Value = "Номер записи";
            ws.Range["B6"].Value = "Дата посещения";
            ws.Range["C6"].Value = "Оценка";
            //

            List<Visit> visits = BasketBDEntities.GetContext().Visit.Where(
                r => r.Date >= dStart &&
                r.Date <= dEnd &&
                r.Student.ID == student.ID).ToList();

            int numberStart = 7;
            int numberEnd = numberStart;
            foreach (Visit t in visits)
            {
                ws.Range["A" + numberEnd].Value = numberEnd - numberStart + 1;
                DateTime dtime = (DateTime)t.Date;
                ws.Range["B" + numberEnd].Value = dtime.Day + "." + dtime.Month + "." + dtime.Year;
                if (t.Presence != null)
                ws.Range["C" + numberEnd].Value = t.Presence.Name;
                numberEnd++;
            }
            app.Calculation = XlCalculation.xlCalculationAutomatic;
            ws.Calculate();
        }
    }
}

