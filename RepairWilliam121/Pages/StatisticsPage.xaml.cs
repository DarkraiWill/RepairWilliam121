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
    public partial class StatisticsPage : Page
    {
        public StatisticsPage()
        {
            InitializeComponent();
            LoadStatistics();
        }

        private void LoadStatistics()
        {
            using (var db = new RepairDBEntities())
            {
                var completedRequestsCount = db.Requests.Count(r => r.StatusID == 4);


                var faultTypeStatistics = db.Requests
                    .GroupBy(r => r.RepairTypeID)
                    .Select(g => new
                    {
                        FaultTypeID = g.Key,
                        Count = g.Count()
                    })
                    .ToList();

                CompletedRequestsCountTextBlock.Text = completedRequestsCount.ToString();
                FaultTypeStatisticsDataGrid.ItemsSource = faultTypeStatistics;
            }
        }
    }
}
