using System;
using System.Collections.Generic;
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
    /// Interaction logic for Learning.xaml
    /// </summary>
    public partial class Learning : UserControl
    {
        private WordManager wordManager = new WordManager();
        private string Tag = Messenger.Instance.Tag;
        private Dictionary<int, Word> WordsToLearn = new Dictionary<int, Word>();
        private List<Word> WordsToLearnList = new List<Word>();
        private List<Word> FalseWords = new List<Word>();
        private List<Word> CurrentWords = new List<Word>();


        private string toTranslate = "";
        private string translated = "";



        //This is a function, because later there will be additional steps.
        

        private List<Word> WordsAssociatedWithId(int id)
        {
            List<Word> output = [WordsToLearn[id]];
            foreach (int i in WordsToLearn[id].TranslationIds)
            {
                output.Add(WordsToLearn[i]);
                foreach (int j in WordsToLearn[i].TranslationIds)
                {
                    if (!output.Contains(WordsToLearn[j])) { output.Add(WordsToLearn[j]); }
                }
            }
            return output;
        }

        //The words are scrambled to ensure the user doesn't memorise the order of the words and actually learns their translations.
        private List<Word> ScrambledWordsList(List<Word> words)
        {
            List<Word> output = new List<Word>();

            int i = DateTime.Now.GetHashCode();

            Random random = new Random(i);

            while (words.Count != 0)
            {
                Word temp = words[random.Next(words.Count)];

                output.Add(temp);

                words.Remove(temp);
            }

            return output;
        }

        private void SetNewWord()
        {
            //Because one word can have multíple translations, all of them are shown to the user.
            List<Word> words = WordsAssociatedWithId(WordsToLearnList[0].Id);
            CurrentWords.Clear();
            toTranslate = "";
            translated = "";
            foreach (Word word in words)
            {
                if (word.Language == "german")
                {
                    toTranslate += (word.WordString + "; ");
                }
                else if (word.Language == "italian")
                {
                    translated += (word.WordString + "; ");
                }
                CurrentWords.Add(word);
            }
            foreach (Word word in CurrentWords)
            {
                if (WordsToLearnList.Contains(word))
                {
                    WordsToLearnList.Remove(word);
                }
            }
            if (toTranslate != "") { toTranslate = toTranslate.Remove(toTranslate.Length - 2); }
            if (translated != "") { translated = translated.Remove(translated.Length - 2); }
            ToTranslateText.Text = toTranslate;
            TranslatedText.Text = translated;
            TranslatedText.Visibility = Visibility.Hidden;
            FalseButton.Visibility = Visibility.Hidden;
            CorrectButton.Visibility = Visibility.Hidden;
            ShowButton.Visibility = Visibility.Visible;
        }

        public Learning()
        {
            InitializeComponent();
            WordsToLearn = wordManager.WordsWithTag(Tag);
            WordsToLearnList = ScrambledWordsList(WordsToLearn.Values.ToList());
            SetNewWord();
        }

        private void ShowClick(object sender, RoutedEventArgs e)
        {
            ShowButton.Visibility = Visibility.Hidden;
            TranslatedText.Visibility = Visibility.Visible;
            FalseButton.Visibility = Visibility.Visible;
            CorrectButton.Visibility = Visibility.Visible;
        }

        //When the user went through all words, the words they got wrong are repeated.
        private bool SwitchLists()
        {
            if (FalseWords.Count == 0) { return false; }
            WordsToLearnList = ScrambledWordsList(FalseWords);
            FalseWords.Clear();
            return true;
        }

        private void CorrectClick(object sender, RoutedEventArgs e)
        {
            if (WordsToLearnList.Count == 0)
            {
                if (!SwitchLists())
                {
                    //The user has finished.
                    ShowButton.Visibility = Visibility.Hidden;
                    TranslatedText.Visibility = Visibility.Hidden;
                    ToTranslateText.Visibility = Visibility.Hidden;
                    FalseButton.Visibility = Visibility.Hidden;
                    CorrectButton.Visibility = Visibility.Hidden;
                    FinishedText.Visibility = Visibility.Visible;
                    return;
                }
            }
            SetNewWord();

        }

        private void FalseClick(object sender, RoutedEventArgs e)
        {
            FalseWords.AddRange(CurrentWords);
            if (WordsToLearnList.Count == 0)
            {
                SwitchLists();
            }
            SetNewWord();
        }
    }
}
