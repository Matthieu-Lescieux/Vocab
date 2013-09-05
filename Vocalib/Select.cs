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
    public partial class Select : Form
    {
        #region Champs privés
        private List<Word> words;
        private bool init;
        #endregion

        #region Constructeur
        public Select(List<Word> Words, bool Init)
        {
            InitializeComponent();

            words = Words;
            init = Init;
            List<string> languages = new List<string>();
            languages = GetLanguagesFromList(Words);
            InitComboBoxLanguage(GetLanguagesFromList(Words).ToArray(), comboBoxLanguage);
        }
        #endregion

        #region Méthodes privées
        /// <summary>
        /// Extrait les langues d'une liste de mot.
        /// </summary>
        /// <param name="words">Spécifie une liste de mots dans laquelle chercher les langues.</param>
        /// <returns>Retourne les langues sous la forme d'une liste de chaînes de caractères.</returns>
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

        /// <summary>
        /// Extrait les thèmes, appartenant à une langue spécifiée, d'une liste de mots.
        /// </summary>
        /// <param name="words">Spécifie une liste de mots dans laquelle chercher les thèmes.</param>
        /// <param name="inLanguage">Spécifie la langue à laquelle appartiennent les thèmes.</param>
        /// <returns>Retourne les thèmes sous la forme d'une liste de chaînes de caractères.</returns>
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
            if (init)
            {
                foreach (Word word in words)
                {
                    if (word.Language == comboBoxLanguage.Text && word.Category == comboBoxCategory.Text)
                        word.Count = 5;
                }
                this.Close();
            }
            else
            {
                Word.Select(comboBoxLanguage.Text, comboBoxCategory.Text);
                this.Close();
            }
        }
        #endregion
    }
}
