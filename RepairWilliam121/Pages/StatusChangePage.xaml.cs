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
    /// Логика взаимодействия для StatusChangePage.xaml
    /// </summary>
    public partial class StatusChangePage : Page
    {
        private int requestId;

        public StatusChangePage(int requestId)
        {
            InitializeComponent();
            this.requestId = requestId;
            LoadRequestDetails();
        }
        private void LoadRequestDetails()
        {
            using (var db = new RepairDBEntities())
            {
                var request = db.Requests
                    .Where(r => r.RequestID == requestId)
                    .Select(r => new
                    {
                        r.RequestDate,
                        UserName = r.User.Username,
                        EquipmentName = r.Equipment.EquipmentName,
                        FaultTypeName = r.RepairType.RepairTypeName,
                        r.StatusID,
                        r.Description
                    })
                    .FirstOrDefault();

                if (request != null)
                {
                    DateTextBlock.Text = request.RequestDate.ToString();
                    UserNameTextBlock.Text = request.UserName;
                    EquipmentNameTextBlock.Text = request.EquipmentName;
                    FaultTypeNameTextBlock.Text = request.FaultTypeName;
                    ProblemDescriptionTextBlock.Text = request.Description;

                    var statuses = db.Status.ToList();
                    StatusComboBox.ItemsSource = statuses;
                    StatusComboBox.DisplayMemberPath = "Name";
                    StatusComboBox.SelectedValuePath = "ID";
                    StatusComboBox.SelectedValue = request.StatusID;
                }
                else
                {
                    MessageBox.Show("Заявка не найдена.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new RepairDBEntities())
            {
                var request = db.Requests.FirstOrDefault(r => r.RepairTypeID == requestId);
                if (request != null)
                {
                    request.StatusID = (int)StatusComboBox.SelectedValue;
                    db.SaveChanges();
                    MessageBox.Show("Статус успешно изменен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    NavigationService.GoBack();
                }
                else
                {
                    MessageBox.Show("Не удалось найти заявку для изменения статуса.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
