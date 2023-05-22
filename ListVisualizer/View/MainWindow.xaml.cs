using System.Windows;

namespace ListVisualizer.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ConfigureButton_Click(object sender, RoutedEventArgs e)
        {
            ConfigureWindow config = new ConfigureWindow();
            config.Show();
        }
    }
}
