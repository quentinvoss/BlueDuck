using BlueDuck.ViewModels;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BlueDuck
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new WordInputViewModel();
        }

        private void WordInputClick(object sender, RoutedEventArgs e)
        {
            DataContext = new WordInputViewModel();
        }

        private void LearningClick(object sender, RoutedEventArgs e)
        {
            DataContext = new LearnSpaceViewModel();
        }
    }
}