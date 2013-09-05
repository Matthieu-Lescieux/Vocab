using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Vocalib
{
    public partial class Quiz : Form
    {
        #region Champs privés
        private Random random;
        private bool changeTranslation;
        private List<Word> words;
        private List<Word> askedWords;
        private Word askedWord;
        private string selectedLanguage = String.Empty;
        private string selectedCategory = String.Empty;
        #endregion

        #region Constructeur
        public Quiz(List<Word> Words)
        {
            InitializeComponent();

            random = new Random();
            words = Words;
            List<string> languages = new List<string>();
            languages = GetLanguagesFromList(Words);
            InitComboBoxLanguage(GetLanguagesFromList(Words).ToArray(), comboBoxLanguage);
        }
        #endregion

        #region Méthodes privées
        //Retourne toutes les langues dans une liste de chaines.
        private List<string> GetLanguagesFromList(List<Word> words)
        {
            List<string> languages = new List<string>();
            foreach (Word word in words)
            {
                bool dontExist = true;
                foreach (string language in languages)
                {
                    if (word.Language.Equals(language))
                        dontExist = false;
                }
                if (dontExist)
                    languages.Add(word.Language);
            }
            return languages;
        }

        //Retourne toutes las catégories appartenant à la langue spécifiée dans une liste de chaines.
        private List<string> GetCategoriesFromList(List<Word> words, string inLanguage)
        {
            List<string> categories = new List<string>();
            foreach (Word word in words)
            {
                bool dontExist = true;
                foreach (string category in categories)
                {
                    if (word.Category == category)
                        dontExist = false;
                }
                if (dontExist && word.Language == inLanguage)
                    categories.Add(word.Category);
            }
            return categories as List<string>;
        }

        private bool GetTranslation()
        {
            bool result = false;
            switch (comboBoxTranslation.SelectedIndex)
            {
                case -1:
                    switch (random.Next(0, 2))
                    {
                        case 0:
                            result = false;
                            break;
                        case 1:
                            result = true;
                            break;
                    }
                    break;
                case 0:
                    switch (random.Next(0, 2))
                    {
                        case 0:
                            result = false;
                            break;
                        case 1:
                            result = true;
                            break;
                    }
                    break;
                case 1:
                    result = false;
                    break;
                case 2 :
                    result = true;
                    break;
                default:
                    result = false;
                    break;
            }
            return result;
        }

        //Initialise la ComboBox spécifiée avec les langues spécifiées
        private void InitComboBoxLanguage(string[] languages, ComboBox comboBox)
        {
            comboBox.Items.Clear();
            comboBox.Items.AddRange(languages);
            comboBox.Enabled = true;
        }

        //Initialise la ComboBox spécifiée avec les catégories spécifiées
        private void InitComboBoxCategory(string[] categories, ComboBox comboBox)
        {
            comboBox.Items.Clear();
            comboBox.Items.AddRange(categories);
            comboBox.Enabled = true;
        }

        private string LoadQuestion(string language, string category)
        {
            try
            {
                changeTranslation = GetTranslation();
                askedWord = askedWords[random.Next(0, askedWords.Count)];
                return "Traduire " + (!changeTranslation ? askedWord.Writing + (askedWord.Plural != String.Empty ? " (Précisez le pluriel)" : "") : askedWord.Translation) + " en " + (!changeTranslation ? askedWord.Language : "Français") + ".";
            }
            catch (Exception)
            {
                textBoxAnswer.Enabled = false;
                buttonNext.Enabled = false;
                return "Vous avez appris tous les mots de cette catégorie.";
            }
        }

        private bool CheckAnswer(string answer)
        {
            bool result = true;
            if ((!changeTranslation && askedWord.Translation + (askedWord.Plural != String.Empty ? ", " + askedWord.Plural : "") == answer) || (changeTranslation && askedWord.Writing == answer))
            {
                result = true;
            }
            else
                result = false;
            textBoxAnswer.Text = String.Empty;
            return result;
        }
        #endregion

        #region Méthodes de réponse aux évenements
        private void comboBoxLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            InitComboBoxCategory(GetCategoriesFromList(words, comboBoxLanguage.Text).ToArray(), comboBoxCategory);
        }

        private void comboBoxCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            buttonValidate.Enabled = true;
        }

        private void buttonValidate_Click(object sender, EventArgs e)
        {
            selectedLanguage = comboBoxLanguage.Text;
            selectedCategory = comboBoxCategory.Text;
            askedWords = new List<Word>();

            foreach (Word word in words)
            {
                if (word.Language == selectedLanguage && word.Category == selectedCategory && word.Count > 0)
                    askedWords.Add(word);
            }
            labelQuestion.Text = LoadQuestion(selectedLanguage, selectedCategory);
            comboBoxLanguage.Enabled = false;
            comboBoxCategory.Enabled = false;
            textBoxAnswer.Enabled = true;
            buttonNext.Enabled = true;
            buttonValidate.Enabled = false;
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            if (!CheckAnswer(textBoxAnswer.Text))
            {
                if (askedWord.Count < 5)
                    words[words.IndexOf(askedWord)].Count++;
                MessageBox.Show("Vous avez fait une erreur, la bonne réponse était " + (!changeTranslation ? askedWord.Translation + (askedWord.Plural != String.Empty ? ", " + askedWord.Plural : "") : askedWord.Writing) + ".");
                labelAnswer.Text = "Mauvaise réponse !";
            }
            else
            {
                words[words.IndexOf(askedWord)].Count--;
                labelAnswer.Text = (askedWord.Count == 0 ? "Félicitations, vous connaissez ce mot !" : "Bonne réponse, encore " + askedWord.Count.ToString() + " fois !"); 
            }
            if (askedWord.Count <= 0)
                askedWords.Remove(askedWord);
            labelQuestion.Text = LoadQuestion(selectedLanguage, selectedCategory);
        }
        #endregion
    }
}
