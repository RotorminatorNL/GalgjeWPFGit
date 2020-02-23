using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Path = System.Windows.Shapes.Path;

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

            CreateLetters();

            // The string pathWordListFile stores the path to wordList.txt
            string pathWordListFile = $"{Directory.GetCurrentDirectory()}\\..\\..\\..\\assets\\wordList.txt";

            // The List<string> wordList stores everything inside wordList.txt and splits it on the ', '.
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

        /// <summary>
        /// This function will generate the letters (which you'll press to guess) on screen
        /// </summary>
        public void CreateLetters()
        {
            string[] alphabet = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            int column = 0;
            int lastColumn = 2;

            Border newBorder;
            Label newLabel;
            var bc = new BrushConverter();

            for (int i = 0; i < alphabet.Length; i++)
            {
                newLabel = new Label
                {
                    Content = alphabet[i],
                    FontFamily = new FontFamily("Century Gothic"),
                    FontSize = 50,
                    Padding = new Thickness(0)
                };

                newLabel.SetValue(HorizontalContentAlignmentProperty, HorizontalAlignment.Center);
                newLabel.SetValue(VerticalContentAlignmentProperty, VerticalAlignment.Center);

                newBorder = new Border
                {
                    Name = $"letter_{alphabet[i]}",
                    Background = (Brush)bc.ConvertFrom("#BDBDBD"),
                    BorderBrush = (Brush)bc.ConvertFrom("#757575"),
                    BorderThickness = new Thickness(2),
                    CornerRadius = new CornerRadius(10),
                    Height = 75,
                    Width = 75,
                    Margin = new Thickness(10),
                    Child = newLabel
                };

                if (column == 10)
                {
                    column = 0;
                }

                if (i < 10)
                {
                    Grid.SetColumn(newBorder, column++);
                    Grid.SetRow(newBorder, 0);
                }
                else if (i < 20)
                {
                    Grid.SetColumn(newBorder, column++);
                    Grid.SetRow(newBorder, 1);
                } else
                {
                    Grid.SetColumn(newBorder, lastColumn++);
                    Grid.SetRow(newBorder, 2);
                }

                newBorder.MouseLeftButtonUp += new MouseButtonEventHandler(PressedLetter);

                grdChooseLetter.Children.Add(newBorder);
            }
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

            CreateFields(listLetters);
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
        public void CreateFields(List<string> listLetters)
        {
            Border newBorder;
            Label newLabel;
            var bc = new BrushConverter();

            for (int i = 0; i < listLetters.Count; i++)
            {
                newLabel = new Label
                {
                    Content = listLetters[i].ToUpper(),
                    FontFamily = new FontFamily("Century Gothic"),
                    FontSize = 50,
                    Padding = new Thickness(0),
                    Opacity = 0
                };
            
                newLabel.SetValue(HorizontalContentAlignmentProperty, HorizontalAlignment.Center);
                newLabel.SetValue(VerticalContentAlignmentProperty, VerticalAlignment.Center);

                newBorder = new Border
                {

                    Name = $"guessWordLetter_{listLetters[i]}",
                    Background = (Brush)bc.ConvertFrom("#BDBDBD"),
                    BorderBrush = (Brush)bc.ConvertFrom("#757575"),
                    BorderThickness = new Thickness(2),
                    CornerRadius = new CornerRadius(10),
                    Height = 75,
                    Width = 75,
                    Margin = new Thickness(15, 0, 15, 0),
                    Child = newLabel
                };

                dpLetters.Children.Add(newBorder);
            }
        }

        /// <summary>
        /// This function will be activated if the player presses one of the generated letters
        /// </summary>
        public void PressedLetter(object sender, EventArgs e)
        {
            Border bdrPressedLetter = (Border)sender;
            Label lblPressedLetter = (Label)bdrPressedLetter.Child;
            Border bdrLetterToGuess;
            Label lblLetterToGuess;

            var bc = new BrushConverter();

            bool bGoodGuess = false;

            for (int i = 0; i < dpLetters.Children.Count; i++)
            {
                bdrLetterToGuess = (Border)dpLetters.Children[i];
                lblLetterToGuess = (Label)bdrLetterToGuess.Child;

                if ((string)lblPressedLetter.Content == (string)lblLetterToGuess.Content)
                {
                    bdrPressedLetter.Background = (Brush)bc.ConvertFrom("#00E676");
                    bdrLetterToGuess.Background = (Brush)bc.ConvertFrom("#FAFAFA");
                    lblLetterToGuess.Opacity = 1;
                    bGoodGuess = true;
                    WordGuessed();
                } 
            }

            if (bGoodGuess == false)
            {
                if (bdrPressedLetter.Background != Brushes.Red)
                {
                    bdrPressedLetter.Background = Brushes.Red;
                    HangmanProgress();
                }
            }
        }

        /// <summary>
        /// This function shows the progression of the hangman per mistake
        /// </summary>
        public void HangmanProgress()
        {
            int iRedLetters = -1;

            for (int i = 0; i < grdChooseLetter.Children.Count; i++)
            {
                Border bdrLetter = (Border)grdChooseLetter.Children[i];
                if (bdrLetter.Background == Brushes.Red)
                {
                    iRedLetters++;
                }
            }

            if (iRedLetters < 5 && iRedLetters != -1)
            {
                Rectangle rect = (Rectangle)grdHangman.Children[iRedLetters];
                rect.Opacity = 1;
            }
            else if (iRedLetters < 7 && iRedLetters != -1)
            {
                Ellipse elp = (Ellipse)grdHangman.Children[iRedLetters];
                elp.Opacity = 1;
            }
            else if (iRedLetters != -1)
            {
                Path pth = (Path)grdHangman.Children[iRedLetters];
                pth.Opacity = 1;

                if (iRedLetters == (grdHangman.Children.Count - 1))
                {
                    MessageBox.Show("Je bent dood!");
                    ResetGame();
                }
            }
        }

        /// <summary>
        /// This function checks if the player has guessed the word and if he/she has a new game will start
        /// </summary>
        public void WordGuessed()
        {
            int iGoodGuess = 0;

            for (int i = 0; i < dpLetters.Children.Count; i++)
            {
                Border bdrLetterToGuess = (Border)dpLetters.Children[i];
                Label lblLetterToGuess = (Label)bdrLetterToGuess.Child;

                if (lblLetterToGuess.Opacity == 1)
                {
                    iGoodGuess++;
                }

                if (iGoodGuess == dpLetters.Children.Count)
                {
                    MessageBox.Show("Je hebt het woord geraden!");
                    ResetGame();
                }
            }
        }

        /// <summary>
        /// This function resets the game
        /// </summary>
        public void ResetGame()
        {
            var bc = new BrushConverter();

            for (int i = 0; i < grdChooseLetter.Children.Count; i++)
            {
                Border bdrChooseLetters = (Border)grdChooseLetter.Children[i];

                bdrChooseLetters.Background = (Brush)bc.ConvertFrom("#BDBDBD");
            }

            for (int i = 0; i < grdHangman.Children.Count; i++)
            {
                if (i < 5)
                {
                    Rectangle rect = (Rectangle)grdHangman.Children[i];
                    rect.Opacity = 0;
                }
                else if (i < 7)
                {
                    Ellipse elp = (Ellipse)grdHangman.Children[i];
                    elp.Opacity = 0;
                }
                else
                {
                    Path pth = (Path)grdHangman.Children[i];
                    pth.Opacity = 0;
                }
            }

            dpLetters.Children.Clear();

            CreateGame();
        }
    }
}
