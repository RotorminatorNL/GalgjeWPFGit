using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace GalgjeWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        // WordCollection speaks for itself, you could call it the library of words.
        List<WordCollection> wordCollection = new List<WordCollection>();

        // The Random function will be used to get a random generated number between 0 and the total amount of words (minus 1, because index starts at 0).
        Random rnd = new Random();

        public MainWindow()
        {
            InitializeComponent();

            // The string pathWordListFile stores the path to wordList.txt
            string pathWordListFile = $"{Directory.GetCurrentDirectory()}\\..\\..\\..\\assets\\wordList.txt";

            // The List<string> wordList stores everything inside wordList.txt and splits it on the ','.
            List<string> wordList = File.ReadAllText(pathWordListFile).Split(", ", StringSplitOptions.RemoveEmptyEntries).ToList();

            // Storing every word in WordCollection
            foreach (string word in wordList)
            {
                WordCollection newWord = new WordCollection
                {
                    Word = word
                };

                wordCollection.Add(newWord);
            }

            CreateGame();
        }

        // Begin BtnClose (customized close button (because the default one is ugly))
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
        // End BtnClose

        // Begin BtnMinimize (customized minimize button (because the default one is ugly))
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
        // End BtnMinimize

        /// <summary>
        /// This function calls every function to create and start a game
        /// </summary>
        public void CreateGame()
        {
            // rndWord is only here to keep it clean.
            string rndWord = wordCollection[rnd.Next(0, (wordCollection.Count - 1))].Word;

            List<string> listLetters = WordToLetters(rndWord);

            LettersOnScreen(listLetters);
        }

        /// <summary>
        /// This function will splits the incoming word (string) and returns the letters (List<string>)
        /// </summary>
        /// <param name="word">Enter the word that you want to split</param>
        public List<string> WordToLetters(string word)
        {
            List<string> listLetters = new List<string>();
            foreach (var letter in word.ToCharArray())
            {
                listLetters.Add(letter.ToString());
            }
            return listLetters;
        }

        /// <summary>
        /// This function will create a field foreach letter
        /// </summary>
        /// <param name="listLetters">The list of letters of the word that needs to be guessed</param>
        public void LettersOnScreen(List<string> listLetters)
        {
            Border newBorder;
            Label newLabel;

            var bc = new BrushConverter();

            for (int i = 0; i < listLetters.Count; i++)
            {
                newBorder = new Border();
                newLabel = new Label();

                newBorder.Name = $"letterBorder_ID{i}";
                newBorder.Background = (Brush)bc.ConvertFrom("#BDBDBD");
                newBorder.BorderBrush = (Brush)bc.ConvertFrom("#757575");
                newBorder.BorderThickness = new Thickness(2);
                newBorder.CornerRadius = new CornerRadius(10);
                newBorder.Height = 75;
                newBorder.Width = 75;
                newBorder.Margin = new Thickness(15,0,15,0);

                newLabel.Content = listLetters[i].ToUpper();
                newLabel.FontFamily = new FontFamily("Century Gothic");
                newLabel.FontSize = 50;
                newLabel.SetValue(HorizontalContentAlignmentProperty, HorizontalAlignment.Center);
                newLabel.SetValue(VerticalContentAlignmentProperty, VerticalAlignment.Center);
                newLabel.Padding = new Thickness(0,0,0,0);
                newLabel.Opacity = 0;

                newBorder.Child = newLabel;

                dpLetters.Children.Add(newBorder);
            }
        }
    }
}
