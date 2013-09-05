using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Vocalib
{
    /// <summary>
    /// Représente une liste de mots pouvant être sérialisée en xml.
    /// </summary>
    [Serializable()]
    public class Words : List<Word>
    {
        #region Propriétés publiques
        public string Xml
        {
            get
            {
                try
                {
                    MemoryStream xmlMemoryStream = new MemoryStream(1024);
                    XmlTextWriter xmlTextWriter = new XmlTextWriter(xmlMemoryStream, Encoding.UTF8);
                    XmlSerializer xmlSerialiser = new XmlSerializer(this.GetType());
                    xmlSerialiser.Serialize(xmlTextWriter, this);
                    xmlTextWriter.Flush();
                    xmlMemoryStream.Position = 0;
                    StreamReader xmlStreamReader = new StreamReader(xmlMemoryStream);
                    return xmlStreamReader.ReadToEnd();
                }
                catch (Exception)
                {
                    return string.Empty;
                }
            }
            set
            {
                try
                {
                    using (XmlReader xmlReader = XmlReader.Create(new StringReader(value)))
                    {
                        this.Clear();
                        XmlSerializer xmlSerialiser = new XmlSerializer(this.GetType());
                        Words words = xmlSerialiser.Deserialize(xmlReader) as Words;
                        this.AddRange(words);
                    }
                }
                catch (Exception)
                {

                }
            }
        }
        #endregion

        #region Méthodes statiques publiques
        /// <summary>
        /// Compare deux mots par langue puis par thème et enfin par écriture en Français.
        /// </summary>
        /// <param name="word1">Spécifie le premier mot à comparer.</param>
        /// <param name="word2">Spécifie le second mot à comparer.</param>
        /// <returns>Retourne un entier relatif.</returns>
        public static int CompareWords(Word word1, Word word2)
        {
            int result = String.Compare(word1.Language, word2.Language);
            if (result == 0)
            {
                result = String.Compare(word1.Category, word2.Category);
                if (result == 0)
                    result = String.Compare(word1.Writing, word2.Writing);
            }
            return result;
        }
        #endregion
    }
}
