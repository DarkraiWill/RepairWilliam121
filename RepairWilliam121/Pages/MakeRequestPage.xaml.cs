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
    /// Логика взаимодействия для MakeRequestPage.xaml
    /// </summary>
    public partial class MakeRequestPage : Page
    {
        private int userId;

        public MakeRequestPage(int userId)
        {
            InitializeComponent();
            this.userId = userId;
            LoadEquipment();
            LoadFaultTypes();
        }

        private void LoadEquipment()
        {
            using (var db = new RepairDBEntities())
            {
                var equipmentList = db.Equipments.Select(e => new
                {
                    e.EquipmentID,
                    e.EquipmentName
                }).ToList();

                EquipmentComboBox.ItemsSource = equipmentList;
                EquipmentComboBox.DisplayMemberPath = "Name";
                EquipmentComboBox.SelectedValuePath = "ID";
            }
        }

        private void LoadFaultTypes()
        {
            using (var db = new RepairDBEntities())
            {
                var faultTypesList = db.RepairTypes.Select(f => new
                {
                    f.RepairTypeID,
                    f.RepairTypeName
                }).ToList();

                FaultTypeComboBox.ItemsSource = faultTypesList;
                FaultTypeComboBox.DisplayMemberPath = "Name";
                FaultTypeComboBox.SelectedValuePath = "ID";
            }
        }

        private void CreateRequestButton_Click(object sender, RoutedEventArgs e)
        {
            if (EquipmentComboBox.SelectedValue == null || FaultTypeComboBox.SelectedValue == null)
            {
                MessageBox.Show("Пожалуйста, выберите оборудование и тип неисправности.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using (var db = new RepairDBEntities())
            {
                var newRequest = new Request
                {
                    RequestDate = DateTime.Now,
                    EquipmentID = (int)EquipmentComboBox.SelectedValue,
                    RepairTypeID = (int)FaultTypeComboBox.SelectedValue,
                    Description = ProblemDescriptionTextBox.Text,
                    StatusID = 1
                };

                db.Requests.Add(newRequest);
                db.SaveChanges();
            }

            MessageBox.Show("Заявка успешно создана!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            NavigationService.GoBack();
        }
    }
}
