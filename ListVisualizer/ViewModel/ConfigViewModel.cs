using ListVisualizer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListVisualizer.ViewModel
{
    public class ConfigViewModel : ObservableObject
    {
        private readonly AppConfiguration config;

        public ConfigViewModel()
        {
            config = AppConfiguration.GetAppConfiguration();
        }

        public string ConnectionString
        {
            get { return config.ConnectionString; }
            set { config.ConnectionString = value; NotifyPropertyChanged(nameof(ConnectionString)); }
        }

        public bool DeveloperMode
        {
            get { return config.DeveloperMode; }
            set { config.DeveloperMode = value; NotifyPropertyChanged(nameof(DeveloperMode)); }
        }
    }
}
