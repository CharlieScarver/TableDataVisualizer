using ListVisualizer.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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

        private void HandleCheck(object sender, SelectionChangedEventArgs e)
        {
        }

        private List<bool> checkboxStates = new List<bool>();

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            UpdateCheckboxStates();
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            UpdateCheckboxStates();
        }

        private void UpdateCheckboxStates()
        {
            checkboxStates.Clear();

            foreach (var item in columnsList.Items)
            {
                var listBoxItem = (ListBoxItem)columnsList.ItemContainerGenerator.ContainerFromItem(item);
                var checkBox = FindVisualChild<CheckBox>(listBoxItem);
                if (checkBox != null)
                {
                    checkboxStates.Add(checkBox.IsChecked ?? false);
                }
            }
        }

        private TChild? FindVisualChild<TChild>(DependencyObject obj) where TChild : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);

                if (child != null && child is TChild)
                    return (TChild)child;

                var childOfChild = FindVisualChild<TChild>(child);

                if (childOfChild != null)
                    return childOfChild;
            }

            return null;
        }

    }
}
