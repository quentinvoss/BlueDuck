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

namespace BlueDuck.Views
{
    /// <summary>
    /// Interaction logic for WordInput.xaml
    /// </summary>
    public partial class WordInput : UserControl
    {
        private Dictionary<CheckBox, string> checkBoxTags = new Dictionary<CheckBox, string>();
        private WordManager wordManager = new WordManager();
        public WordInput()
        {
            InitializeComponent();
            checkBoxTags.Add(Verb, "Verb");
            checkBoxTags.Add(Irregular, "Irregular");
            checkBoxTags.Add(Isc, "-isc-");
        }
        private void AddButtonClick(object sender, RoutedEventArgs e)
        {
            //It is checked if the input is complete
            if (LangABox.Text != "" && LangBBox.Text != "" && Lesson.Text != "")
            {
                List<string> langBwords = new List<string> { LangBBox.Text };
                List<string> tags = new List<string>();
                tags.Add(Lesson.Text);
                if (!wordManager.loadData.tags.Contains(Lesson.Text)) { wordManager.loadData.tags.Add(Lesson.Text); }
                foreach (KeyValuePair<CheckBox, string> pair in checkBoxTags)
                {
                    if (pair.Key.IsChecked == true)
                    {
                        tags.Add(pair.Value);
                        pair.Key.IsChecked = false;
                    }
                }
                wordManager.AddWord(LangABox.Text, "italian", langBwords, "german", tags);
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
