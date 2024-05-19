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
        private WordManager wordManager = new WordManager();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void AddButtonClick(object sender, RoutedEventArgs e)
        {
            //It is checked if the input is complete
            if(LangABox.Text != "" && LangBBox.Text != "")
            {
                List<string> langBwords = new List<string> { LangBBox.Text };
                wordManager.AddWord(LangABox.Text, "italian", langBwords, "german");
                LangABox.Text = null;
                LangBBox.Text = null;
                emptyErrorMessage.Visibility = Visibility.Hidden;
            }
            else { emptyErrorMessage.Visibility = Visibility.Visible; }
        }

        private void SaveButtonClick(object sender, RoutedEventArgs e)
        {
            wordManager.Save();
        }
    }
}