using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace RepairWilliam121.Pages
{
    public partial class AdminPage : Page
    {
        public AdminPage()
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
                        UserName = r.User != null ? r.User.Username : "Неизвестно",
                        EquipmentName = r.Equipment != null ? r.Equipment.EquipmentName : "Неизвестно",
                        FaultTypeName = r.RepairType != null ? r.RepairType.RepairTypeName : "Неизвестно",
                        StatusName = r.Status != null ? r.Status.StatusName : "Неизвестно",
                        r.Description
                    })
                    .ToList();

                RequestsDataGrid.ItemsSource = requests;
            }
        }

        private void ChangeStatusButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var request = button?.DataContext;

            if (request != null)
            {
                int requestId = (int)request.GetType().GetProperty("ID").GetValue(request);

                NavigationService.Navigate(new StatusChangePage(requestId));
            }
            else
            {
                MessageBox.Show("Не удалось получить информацию о заявке.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void StatisticsButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new StatisticsPage());
        }
    }
}

