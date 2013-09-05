using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Vocalib;

namespace Vocabulaire
{
    public partial class Main : Form
    {
        #region Champs privés
        private string nomFichier = string.Empty;
        private string selectedLanguage = String.Empty;
        private string selectedCategory = String.Empty;
        #endregion

        #region Constructeur
        public Main()
        {
            InitializeComponent();

            OpenLastFile();
        }
        #endregion

        #region Méthodes privées
        private void OpenLastFile()
        {
            try
            {
                using (System.IO.StreamReader streamReader = new System.IO.StreamReader(Application.UserAppDataPath + @"\Vocab.txt"))
                {
                    string fileName = streamReader.ReadLine();
                    mainBindingSource.DataSource = OuvrirFichier(fileName);
                    mainBindingNavigator.BindingSource = mainBindingSource;
                    mainDataGridView.DataSource = mainBindingNavigator.BindingSource;
                    nomFichier = fileName;
                }
            }
            catch (Exception)
            {

            }
        }

        private void SauverFichier(string NomDuFichier)
        {
            (mainBindingSource.DataSource as Words).Sort(Words.CompareWords);
            using (System.IO.StreamWriter streamWriter = new System.IO.StreamWriter(NomDuFichier, false))
            {
                streamWriter.Write((mainBindingSource.DataSource as Words).Xml);
            }
        }

        private Words OuvrirFichier(string NomDuFichier)
        {
            Words result = null;

            using (System.IO.StreamReader sr = new System.IO.StreamReader(NomDuFichier))
            {
                result = new Words();
                result.Xml = sr.ReadToEnd();
            }
            return result;
        }
        #endregion

        #region Méthodes de réponse aux évenements
        private void nouveauToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mainBindingSource.DataSource = new Words();
            mainBindingNavigator.BindingSource = mainBindingSource;
            mainDataGridView.DataSource = mainBindingNavigator.BindingSource;
        }

        private void ouvrirToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (nomFichier == String.Empty && mainBindingNavigator.BindingSource != null)
                enregistrersousToolStripMenuItem_Click(this, new EventArgs());
            else if (nomFichier != String.Empty && mainBindingNavigator.BindingSource != null)
                SauverFichier(nomFichier);

            using (OpenFileDialog fileOpen = new System.Windows.Forms.OpenFileDialog())
            {
                fileOpen.Filter = "Fichiers vocab|*.voc";
                fileOpen.InitialDirectory = Application.StartupPath;

                if (fileOpen.ShowDialog() == DialogResult.OK)
                {
                    mainBindingSource.DataSource = OuvrirFichier(fileOpen.FileName);
                    mainBindingNavigator.BindingSource = mainBindingSource;
                    mainDataGridView.DataSource = mainBindingNavigator.BindingSource;
                    nomFichier = fileOpen.FileName;
                }
            }
        }

        private void enregistrerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (nomFichier == string.Empty)
            {
                this.enregistrersousToolStripMenuItem_Click(sender, e);
            }
            else
            {
                SauverFichier(nomFichier);
            }
        }

        private void enregistrersousToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog fileSave = new SaveFileDialog())
            {
                fileSave.Filter = "Fichiers vocab|*.voc";
                fileSave.InitialDirectory = Application.StartupPath;

                if (fileSave.ShowDialog() == DialogResult.OK)
                {
                    nomFichier = fileSave.FileName;
                    SauverFichier(nomFichier);
                }
            }
        }

        private void quitterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void démarrerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mainBindingSource.DataSource as Words != null)
            {
                Quiz quiz = new Quiz(mainBindingSource.DataSource as Words);
                mainBindingNavigator.BindingSource = null;
                mainDataGridView.DataSource = mainBindingNavigator.BindingSource;
                quiz.ShowDialog();
            }
            else
                MessageBox.Show("Veuillez ouvrir ou créer un fichier Vocab.");
        }

        private void selectionnerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mainBindingSource.DataSource as Words != null)
            {
                Select init = new Select(mainBindingSource.DataSource as Words, false);
                init.ShowDialog();
            }
            else
                MessageBox.Show("Veuillez ouvrir ou créer un fichier Vocab.");
        }

        private void réinitialiserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mainBindingSource.DataSource as Words != null)
            {
                Select init = new Select(mainBindingSource.DataSource as Words, true);
                init.ShowDialog();
            }
            else
                MessageBox.Show("Veuillez ouvrir ou créer un fichier Vocab.");
        }

        private void afficherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mainBindingNavigator.BindingSource = mainBindingSource;
            mainDataGridView.DataSource = mainBindingNavigator.BindingSource;
        }

        private void mainBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            this.itemPropertyGrid.SelectedObject = (sender as BindingSource).Current;
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (nomFichier == String.Empty)
                {
                    DialogResult dialogResult = MessageBox.Show("Voulez-vous vraiment quitter sans sauvegarder ?", "Vocab", MessageBoxButtons.YesNoCancel);
                    if (dialogResult == System.Windows.Forms.DialogResult.Yes)
                        e.Cancel = false;
                    else if (dialogResult == System.Windows.Forms.DialogResult.No)
                        enregistrersousToolStripMenuItem_Click(this, new EventArgs());
                    else
                        e.Cancel = true;
                }
                else
                    SauverFichier(nomFichier);

                if (nomFichier != String.Empty)
                {
                    using (System.IO.StreamWriter streamWriter = new System.IO.StreamWriter(Application.UserAppDataPath + @"\Vocab.txt"))
                    {
                        streamWriter.WriteLine(nomFichier);
                    }
                }
            }
            catch (Exception)
            {
                
            }
        }
        #endregion
    }
}
