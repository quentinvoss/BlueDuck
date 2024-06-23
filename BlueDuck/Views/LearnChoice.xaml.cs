using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
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

namespace BlueDuck.Views
{
    /// <summary>
    /// Interaction logic for LearnChoice.xaml
    /// </summary>
    public partial class LearnChoice : UserControl
    {
        WordManager wordManager = new WordManager();
        public LearnChoice()
        {
            InitializeComponent();
            tagSelector.Items.Clear();
            foreach (string tag in wordManager.loadData.tags)
            {
                tagSelector.Items.Add(tag);
            }
        }

        private void LearnClick(object sender, RoutedEventArgs e)
        {
            if (tagSelector.SelectedItem != null)
            {
                string tag = tagSelector.SelectedItem.ToString();
                Messenger.Instance.SetLearnTag(tag);
            }
            else
            {
                noSelectionErrorMessage.Visibility = Visibility.Visible;
            }
        }
    }
}
