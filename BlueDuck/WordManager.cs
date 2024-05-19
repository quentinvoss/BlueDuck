using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Navigation;
using Microsoft.Windows.Themes;

namespace BlueDuck
{
    /*Each word is stored seperatly.
      Due to this fact you can easily add other translations later.
      Another advantage is that you can learn all meanings simultaneously.*/
    public class Word
    {
        public string WordString { get; set; } = "";
        public string Language {  get; set; } = "";
        public int Id { get; set; } = 0;
        public List<int> TranslationIds { get; set; } = new List<int>();

        //The tags will later be used to categorize the words for learning.
        public List<string> Tags { get; set; } = new List<string>();

    }
    internal class WordManager
    {
        //The data is stored in .json files, because that makes it easy to load, save and edit it.
        private readonly string filepath = "vocabulary.json";
        private List<Word> vocabulary;
        //To not create any duplicate Ids the last ones generated are stored.
        private List<int> lastLangIds;

        public WordManager()
        {
            vocabulary = new List<Word>();
            lastLangIds = new List<int>();
            Load();
        }

        /*There will later be an option to add multiple translations at once.
          That is why the word form language B is being imported as a list with one item.*/
        public void AddWord(string langAWord, string languageA, List<string> langBWords, string languageB)
        {
            List<int> langBWordsIds = new List<int>();

            //It is checked if the Word already exists.
            if (GetWordIndex(langAWord) == -1)
            {
                int langAWordId = GenerateId(languageA);

                //The Ids for the translations are being looked up or being created.
                foreach(string str in langBWords)
                {
                    langBWordsIds.Add(GetWordIndex(str) > -1 ? vocabulary[GetWordIndex(str)].Id : GenerateId(languageB));
                }

                Word word = new Word() { WordString = langAWord, Id = langAWordId, Language = languageA, TranslationIds = langBWordsIds };
                vocabulary.Add(word);
            }
            else
            {
                /*The Ids for the translations are being looked up or being created.
                  If the translation Id is not already connected with the word, it gets connected.*/
                foreach (string str in langBWords)
                {
                    int id = (GetWordIndex(str) > -1 ? vocabulary[GetWordIndex(str)].Id : GenerateId(languageB));
                    langBWordsIds.Add(id);
                    if (!vocabulary[GetWordIndex(langAWord)].TranslationIds.Contains(id)) { vocabulary[GetWordIndex(langAWord)].TranslationIds.Add(id); }
                }
            }

            //The translations get the Ids of the original word connected to them.
            for(int i = 0; i < langBWords.Count();i++)
            {
                //It is checked, if the Word already exists.
                if (GetWordIndex(langBWords[i]) > -1)
                {
                    //If the Id of the original word is not already connected with the translation, it gets connected.
                    int id = vocabulary[GetWordIndex(langAWord)].Id;
                    if (!vocabulary[GetWordIndex(langBWords[i])].TranslationIds.Contains(id)) { vocabulary[GetWordIndex(langBWords[i])].TranslationIds.Add(id);}
                }
                else
                {
                    Word word = new Word() { WordString = langBWords[i], Id = langBWordsIds[i], Language = languageB, TranslationIds = new List<int>() { vocabulary[GetWordIndex(langAWord)].Id } };
                    vocabulary.Add(word);
                }
            }
        }

        private void Load()
        {
            if (File.Exists("doNotDelete.json")){
                string json = File.ReadAllText("doNotDelete.json");
                lastLangIds = JsonSerializer.Deserialize<List<int>>(json);
            }
            else
            {
                lastLangIds = new List<int>() { -1, 1073741823 };
            }
            if (File.Exists(filepath))
            {
                string json = File.ReadAllText(filepath);
                vocabulary = JsonSerializer.Deserialize<List<Word>>(json);
            }
        }

        public void Save()
        {
            string json = JsonSerializer.Serialize(vocabulary, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filepath, json);
            json = JsonSerializer.Serialize(lastLangIds, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText("doNotDelete.json", json);
        }

        private int GetWordIndex(string word)
        {
            for(int i = 0; i< vocabulary.Count(); i++)
            {
                if (word.Equals(vocabulary[i].WordString))
                {
                    return i;
                }
            }
            return -1;
        }

        private int GenerateId(string language)
        {
            lastLangIds[(language == "german" ? 0 : 1)]++;
            return lastLangIds[(language == "german" ? 0 : 1)];
        }
    }
}
