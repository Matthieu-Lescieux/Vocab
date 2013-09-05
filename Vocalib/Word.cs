using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Xml.Serialization;

namespace Vocalib
{
    /// <summary>
    /// Représente un mot.
    /// </summary>
    [Serializable()]
    public class Word
    {
        #region Champs privés
        private static string selectedLanguage = String.Empty;
        private static string selectedCategory = String.Empty;
        private string writing = string.Empty;
        private string translation = string.Empty;
        private string plural = string.Empty;
        private string language = String.Empty;
        private string category = String.Empty;
        private int  count = 5;
        #endregion

        #region Constructeur
        public Word()
        {
            language = selectedLanguage;
            category = selectedCategory;
        }
        #endregion

        #region Méthodes statiques privées
        public static void Select(string Language, string Category)
        {
            selectedLanguage = Language;
            selectedCategory = Category;
        }
        #endregion

        #region Propriétés publiques
        [DisplayName("Traduction")]
        [Category("Identification")]
        [Description("Traduction du mot en langue étrangère")]
        [XmlAttribute()]
        public string Translation
        {
            get { return translation; }
            set { translation = value; }
        }

        [DisplayName("Pluriel")]
        [Category("Identification")]
        [Description("Pluriel du mot en langue étrangère")]
        [XmlAttribute()]
        public string Plural
        {
            get { return plural; }
            set { plural = value; }
        }

        [DisplayName("Mot français")]
        [Category("Identification")]
        [Description("Ecriture du mot en Français")]
        [XmlAttribute()]
        public string Writing
        {
            get { return writing; }
            set { writing = value; }
        }

        [DisplayName("Langue")]
        [Category("Indexation")]
        [Description("Langue dans laquelle ce mot correspond à cette traduction")]
        [XmlAttribute()]
        public string Language
        {
            get { return language; }
            set { language = value; }
        }

        [DisplayName("Thème")]
        [Category("Indexation")]
        [Description("Thème auquel appartient ce mot")]
        [XmlAttribute()]
        public string Category
        {
            get { return category; }
            set { category = value; }
        }

        [DisplayName("Compteur")]
        [Category("Paramètres")]
        [Description("Nombre d'apparitions du mot avant qu'il soit connu")]
        [ReadOnly(true)]
        [XmlAttribute()]
        public int Count
        {
            get { return count; }
            set { count = value; }
        }
        #endregion
    }
}