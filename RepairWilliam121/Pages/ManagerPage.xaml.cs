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
    /// Логика взаимодействия для ManagerPage.xaml
    /// </summary>
    public partial class ManagerPage : Page
    {
        public ManagerPage()
        {
            InitializeComponent();
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            using (var db = new RepairDBEntities())
            {
                var requests = db.Requests
                    .Select(r => new
                    {
                        r.RequestID,
                        r.RequestDate,
                        DeadLine = r.DeadLine,
                        UserName = r.User != null ? r.User.Username : "Неизвестно",
                        EquipmentName = r.Equipment != null ? r.Equipment.EquipmentName : "Неизвестно",
                        FaultTypeName = r.RepairType != null ? r.RepairType.RepairTypeName : "Неизвестно",
                        StatusName = r.Status != null ? r.Status.StatusName : "Неизвестно",
                        Employee = r.EmployeeID != null ? r.Employee.EmployeeName : "Не назначен",
                        r.Description
                    })
                    .ToList();

                RequestsDataGrid.ItemsSource = requests;
            }
        }


        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            var selectedRequest = RequestsDataGrid.SelectedItem;

            if (selectedRequest != null)
            {
                int requestId = (int)((dynamic)selectedRequest).ID;

                NavigationService.Navigate(new EmployeeTaskAddPage(requestId));
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите заявку для назначения сотрудника.");
            }
        }
    }
}
