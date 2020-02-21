using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace GalgjeWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /*
         * Waarom WordCollection als eigen class, omdat ik dan 1x de list wordCollection hoef te vullen 
         * en vervolgens de list in verschillende/meerdere classes kan gebruiken. 
        */
        List<WordCollection> wordCollection = new List<WordCollection>();

        /*
         * De Random functie is nodig voor een randomized getal (index) tussen 0 en het totaal aantal woorden van WordCollection.
        */
        Random rnd = new Random();

        /* 
         * Dat randomized getal (index) staat verbonden met een woord in de WordCollection.
         * rndWord slaat dat woord op.
        */ 
        string rndWord;


        public MainWindow()
        {
            InitializeComponent();

            string pathWordListFile = $"{Directory.GetCurrentDirectory()}\\..\\..\\..\\assets\\wordList.txt";
            List<string> wordList = File.ReadAllText(pathWordListFile).Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();

            foreach (string word in wordList)
            {
                WordCollection newWord = new WordCollection
                {
                    Word = word
                };

                wordCollection.Add(newWord);
            }

            int rndIndex = rnd.Next(0, (wordCollection.Count - 1));
            rndWord = wordCollection[rndIndex].Word;
        }

        /// <summary>
        /// This explain the functionality of this method
        /// </summary>
        /// <param name="woord">This woord parameters is being used for splitting a sentence</param>
        private void DitIsEenVoorbeeld(string woord)
        {

        }



        private void BtnClose_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void BtnClose_MouseEnter(object sender, MouseEventArgs e)
        {
            BtnClose.Opacity = 0.5;
        }

        private void BtnClose_MouseLeave(object sender, MouseEventArgs e)
        {
            BtnClose.Opacity = 1;
        }

        private void BtnMinimize_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void BtnMinimize_MouseEnter(object sender, MouseEventArgs e)
        {
            BtnMinimize.Background = Brushes.LightGray;
            txtMinimize.Foreground = Brushes.White;
        }

        private void BtnMinimize_MouseLeave(object sender, MouseEventArgs e)
        {
            BrushConverter bc = new BrushConverter();
            BtnMinimize.Background = (Brush)bc.ConvertFrom("#EEEEEE");
            txtMinimize.Foreground = Brushes.LightGray;
        }
    }
}
