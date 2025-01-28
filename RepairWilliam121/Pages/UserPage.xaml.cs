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
    /// Логика взаимодействия для UserPage.xaml
    /// </summary>
    public partial class UserPage : Page
    {
        private int userId;

        public UserPage(int userId)
        {
            InitializeComponent();
            this.userId = userId;
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MakeRequestPage(userId));
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            using (var db = new RepairDBEntities())
            {
                var requests = db.Requests
                    .Where(r => r.User.UserID == userId)
                    .Select(r => new
                    {
                        r.RequestDate,
                        EquipmentName = r.Equipment.EquipmentName,
                        FaultTypeName = r.RepairType.RepairTypeName,
                        StatusName = r.Status.StatusName,
                        r.Description
                    })
                    .ToList();

                RequestsDataGrid.ItemsSource = requests;
            }
        }
    }
}
