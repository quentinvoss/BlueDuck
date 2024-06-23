using BlueDuck.ViewModels;
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
    /// Interaction logic for LearnSpace.xaml
    /// </summary>
    public partial class LearnSpace : UserControl
    {
        Messenger messenger;
        public LearnSpace()
        {
            InitializeComponent();
            DataContext = new LearnChoiceViewModel();
            Messenger.TagChoiceHandler += new TagChoiceDelegate(ChangeToLearningView);
        }

        void ChangeToLearningView(TagChoiceArgs e)
        {
            DataContext = new LearningViewModel();
        }
    }
}
