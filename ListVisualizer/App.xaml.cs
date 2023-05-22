using ListVisualizer.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ListVisualizer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            InitializeConfiguration();
        }

        private void InitializeConfiguration()
        {
            // Initialize app configuration
            AppConfiguration conf = AppConfiguration.GetAppConfiguration();
            conf.ConnectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=PearReviewDb;Integrated Security=True;TrustServerCertificate=true;";
            // conf.DeveloperMode = ...
        }
    }
}
