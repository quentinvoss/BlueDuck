using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Navigation;
using Microsoft.Windows.Themes;
using System.Printing;

namespace BlueDuck
{
    /*Each word is stored seperatly.
      Due to this fact you can easily add other translations later.
      Another advantage is that you can learn all meanings simultaneously.*/
    public class Word
    {
        public string WordString { get; set; } = "";
        public string Language { get; set; } = "";
        public int Id { get; set; } = 0;
        public List<int> TranslationIds { get; set; } = new List<int>();

        //The tags are used to categorize the words for learning.
        public List<string> Tags { get; set; } = new List<string>();

    }

    //With this class we can easily load and store data that is needed to run the programm.
    internal class LoadData
    {
        public List<int> lastLangIds { get; set; } = new List<int>() { -1, 1073741823 };
        public List<string> tags { get; set; } = new List<string>() { "Verb", "Irregular", "-isc-" };
    }
    internal class WordManager
    {
        //The data is stored in .json files, because that makes it easy to load, save and edit it.
        private readonly string filepath = "vocabulary.json";
        private List<Word> vocabulary;
        //To not create any duplicate Ids the last ones generated are stored.
        public LoadData loadData;

        public WordManager()
        {
            vocabulary = new List<Word>();
            loadData = new LoadData();
            Load();
        }

        /*There will later be an option to add multiple translations at once.
          That is why the word form language B is being imported as a list with one item.*/
        public void AddWord(string langAWord, string languageA, List<string> langBWords, string languageB, List<string> tags)
        {
            List<int> langBWordsIds = new List<int>();

            //It is checked if the Word already exists.
            if (GetWordIndex(langAWord, languageA) == -1)
            {
                int langAWordId = GenerateId(languageA);

                //The Ids for the translations are being looked up or being created.
                foreach (string str in langBWords)
                {
                    langBWordsIds.Add(GetWordIndex(str, languageB) > -1 ? vocabulary[GetWordIndex(str, languageB)].Id : GenerateId(languageB));
                }

                Word word = new Word() { WordString = langAWord, Id = langAWordId, Language = languageA, TranslationIds = langBWordsIds, Tags = tags };
                vocabulary.Add(word);
            }
            else
            {
                /*The Ids for the translations are being looked up or being created.
                  If the translation Id is not already connected with the word, it gets connected.*/
                foreach (string str in langBWords)
                {
                    int id = (GetWordIndex(str, languageB) > -1 ? vocabulary[GetWordIndex(str, languageB)].Id : GenerateId(languageB));
                    langBWordsIds.Add(id);
                    if (!vocabulary[GetWordIndex(langAWord, languageA)].TranslationIds.Contains(id)) { vocabulary[GetWordIndex(langAWord, languageA)].TranslationIds.Add(id); }

                    //The tags get added to the word.
                    foreach (string tag in tags)
                    {
                        if (!vocabulary[GetWordIndex(langAWord, languageA)].Tags.Contains(tag)) { vocabulary[GetWordIndex(langAWord, languageA)].Tags.Add(tag); }
                    }
                }
            }

            //The translations get the Ids of the original word connected to them.
            for (int i = 0; i < langBWords.Count; i++)
            {
                //It is checked, if the Word already exists.
                if (GetWordIndex(langBWords[i], languageB) > -1)
                {
                    //If the Id of the original word is not already connected with the translation, it gets connected.
                    int id = vocabulary[GetWordIndex(langAWord, languageA)].Id;
                    if (!vocabulary[GetWordIndex(langBWords[i], languageB)].TranslationIds.Contains(id)) { vocabulary[GetWordIndex(langBWords[i], languageB)].TranslationIds.Add(id); }
                    
                    //The tags get added to the translation.
                    foreach (string tag in tags)
                    {
                        if (!vocabulary[GetWordIndex(langBWords[i], languageB)].Tags.Contains(tag)) { vocabulary[GetWordIndex(langBWords[i], languageB)].Tags.Add(tag); }
                    }
                }
                else
                {
                    Word word = new Word() { WordString = langBWords[i], Id = langBWordsIds[i], Language = languageB, TranslationIds = new List<int>() { vocabulary[GetWordIndex(langAWord, languageA)].Id }, Tags = tags };
                    vocabulary.Add(word);
                }
            }
        }

        private void Load()
        {
            if (File.Exists("doNotDelete.json"))
            {
                string json = File.ReadAllText("doNotDelete.json");
                loadData = JsonSerializer.Deserialize<LoadData>(json);

            }
            if (File.Exists(filepath))
            {
                string json = File.ReadAllText(filepath);
                vocabulary = JsonSerializer.Deserialize<List<Word>>(json);
            }
        }

        private JsonSerializerOptions JSO = new JsonSerializerOptions { WriteIndented = true };
        public void Save()
        {
            string json = JsonSerializer.Serialize(vocabulary, JSO);
            File.WriteAllText(filepath, json);
            json = JsonSerializer.Serialize(loadData, JSO);
            File.WriteAllText("doNotDelete.json", json);
        }

        //The function checks if the word already exist in its language.
        private int GetWordIndex(string wordString, string language)
        {
            for (int i = 0; i < vocabulary.Count; i++)
            {
                if (wordString.Equals(vocabulary[i].WordString) && vocabulary[i].Language == language)
                {
                    return i;
                }
            }
            return -1;
        }

        private int GenerateId(string language)
        {
            loadData.lastLangIds[(language == "german" ? 0 : 1)]++;
            return loadData.lastLangIds[(language == "german" ? 0 : 1)];
        }

        public Dictionary<int,Word> WordsWithTag(string tag)
        {
            Dictionary<int,Word> Output = new Dictionary<int,Word>();

            foreach(Word word in vocabulary)
            {
                if (word.Tags.Contains(tag))
                {
                    Output.Add(word.Id, word);
                }
            }

            return Output;

        }
    }
}
