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

namespace RepairWilliam121.Pages
{
    /// <summary>
    /// Логика взаимодействия для EmployeeTaskAddPage.xaml
    /// </summary>
    public partial class EmployeeTaskAddPage : Page
    {
        private int requestId;

        public EmployeeTaskAddPage(int requestId)
        {
            InitializeComponent();
            this.requestId = requestId;
            LoadRequestDetails();
            LoadEmployees();
        }

        private void LoadRequestDetails()
        {
            using (var db = new RepairDBEntities())
            {
                var request = db.Requests
                    .Where(r => r.RepairTypeID == requestId)
                    .Select(r => new
                    {
                        r.RequestID,
                        r.Description,
                        r.DeadLine,
                        r.DaysSpent
                    })
                    .FirstOrDefault();

                if (request != null)
                {
                    RequestIdTextBlock.Text = request.RequestID.ToString();
                    ProblemDescriptionTextBlock.Text = request.Description;
                    DeadlineDatePicker.SelectedDate = request.DeadLine;
                    DaysSpentTextBox.Text = request.DaysSpent.ToString();
                }
                else
                {
                    MessageBox.Show("Заявка не найдена.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void LoadEmployees()
        {
            using (var db = new RepairDBEntities())
            {
                var employees = db.Employees.Select(s => new
                {
                    s.EmployeeID,
                    FullName = s.EmployeeName + " " + s.EmployeeSurname
                }).ToList();

                EmployeeComboBox.ItemsSource = employees;
                EmployeeComboBox.DisplayMemberPath = "FullName";
                EmployeeComboBox.SelectedValuePath = "ID";
            }
        }

        private void AssignButton_Click(object sender, RoutedEventArgs e)
        {
            if (EmployeeComboBox.SelectedValue != null)
            {
                using (var db = new RepairDBEntities())
                {
                    var request = db.Requests.FirstOrDefault(r => r.RepairTypeID == requestId);
                    if (request != null)
                    {
                        request.EmployeeID = (int)EmployeeComboBox.SelectedValue;

                        if (DeadlineDatePicker.SelectedDate.HasValue)
                        {
                            request.DeadLine = DeadlineDatePicker.SelectedDate.Value;
                        }
                        else
                        {
                            MessageBox.Show("Пожалуйста, выберите дедлайн.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }

                        if (int.TryParse(DaysSpentTextBox.Text, out int daysSpent))
                        {
                            request.DaysSpent = daysSpent;
                        }
                        else
                        {
                            MessageBox.Show("Пожалуйста, введите корректное количество дней.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }

                        db.SaveChanges();
                        MessageBox.Show("Сотрудник назначен успешно!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        NavigationService.GoBack();
                    }
                    else
                    {
                        MessageBox.Show("Не удалось найти заявку для назначения сотрудника.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите сотрудника.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
