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
using System.Data.OleDb;
using System.Windows.Media.Animation;
using System.Threading.Tasks;

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
            List<string> wordList = File.ReadAllText(pathWordListFile).Split("\r\n", StringSplitOptions.RemoveEmptyEntries).ToList();

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
        /// This function will generate the playing letters (which you'll press to guess) on screen
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
                    Cursor = Cursors.Hand,
                    CornerRadius = new CornerRadius(10),
                    Height = 85,
                    Width = 85,
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

                newBorder.MouseEnter += new MouseEventHandler(MouseEnterLetter);
                newBorder.MouseLeave += new MouseEventHandler(MouseLeaveLetter);
                newBorder.MouseLeftButtonUp += new MouseButtonEventHandler(PressedLetter);

                grdChooseLetter.Children.Add(newBorder);
            }
        }

        /// <summary>
        /// This function will change the state of the playing letter
        /// </summary>
        public void MouseEnterLetter(object sender, EventArgs e)
        {
            Border bdrPlayingLetter = (Border)sender;
            Label lblPlayingLetter = (Label)bdrPlayingLetter.Child;
            var bc = new BrushConverter();

            DoubleAnimation da = new DoubleAnimation
            {
                From = 85,
                To = 95,
                Duration = TimeSpan.FromMilliseconds(100)
            };

            DoubleAnimation daFontSize = new DoubleAnimation
            {
                From = 50,
                To = 60,
                Duration = TimeSpan.FromMilliseconds(100)
            };

            ThicknessAnimation ta = new ThicknessAnimation
            {
                From = new Thickness(10),
                To = new Thickness(5),
                Duration = TimeSpan.FromMilliseconds(100)
            };


            if (bdrPlayingLetter.Background == Brushes.Red)
            {
                // Do no thing
            }
            else if(bdrPlayingLetter.Background.ToString() == "#FF00E676")
            {
                // Do no thing
            }
            else
            {
                bdrPlayingLetter.Background = (Brush)bc.ConvertFrom("#EEEEEE");
                bdrPlayingLetter.BeginAnimation(Border.HeightProperty, da);
                bdrPlayingLetter.BeginAnimation(Border.WidthProperty, da);
                bdrPlayingLetter.BeginAnimation(Border.MarginProperty, ta);

                lblPlayingLetter.BeginAnimation(Label.FontSizeProperty, daFontSize);
            }
        }

        /// <summary>
        /// This function will reset the state of the playing letter
        /// </summary>
        public void MouseLeaveLetter(object sender, EventArgs e)
        {
            Border bdrPlayingLetter = (Border)sender;
            Label lblPlayingLetter = (Label)bdrPlayingLetter.Child;
            var bc = new BrushConverter();

            DoubleAnimation da = new DoubleAnimation
            {
                From = 95,
                To = 85,
                Duration = TimeSpan.FromMilliseconds(100)
            };

            DoubleAnimation daFontSize = new DoubleAnimation
            {
                From = 60,
                To = 50,
                Duration = TimeSpan.FromMilliseconds(100)
            };

            ThicknessAnimation ta = new ThicknessAnimation
            {
                From = new Thickness(5),
                To = new Thickness(10),
                Duration = TimeSpan.FromMilliseconds(100)
            };

            if (bdrPlayingLetter.Background == Brushes.Red && bdrPlayingLetter.Width != 85 || bdrPlayingLetter.Background.ToString() == "#FF00E676" && bdrPlayingLetter.Width != 85)
            {
                bdrPlayingLetter.BeginAnimation(Border.HeightProperty, da);
                bdrPlayingLetter.BeginAnimation(Border.WidthProperty, da);
                bdrPlayingLetter.BeginAnimation(Border.MarginProperty, ta);

                lblPlayingLetter.BeginAnimation(Label.FontSizeProperty, daFontSize);
            }
            else if (bdrPlayingLetter.Background != Brushes.Red && bdrPlayingLetter.Background.ToString() != "#FF00E676")
            {
                bdrPlayingLetter.Background = (Brush)bc.ConvertFrom("#BDBDBD");
                bdrPlayingLetter.BeginAnimation(Border.HeightProperty, da);
                bdrPlayingLetter.BeginAnimation(Border.WidthProperty, da);
                bdrPlayingLetter.BeginAnimation(Border.MarginProperty, ta);

                lblPlayingLetter.BeginAnimation(Label.FontSizeProperty, daFontSize);
            }
        }

        /// <summary>
        /// This function will be activated if the player presses one of the generated playing letters
        /// </summary>
        public void PressedLetter(object sender, EventArgs e)
        {
            Border bdrPressedLetter = (Border)sender;
            Label lblPressedLetter = (Label)bdrPressedLetter.Child;
            Border bdrLetterToGuess;
            Label lblLetterToGuess;

            var bc = new BrushConverter();
            bool bGoodGuess = false;
            bool bIsGameOver = IsGameOver();

            if (!bIsGameOver)
            {
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
                        lblPressedLetter.Opacity = 0.5;
                        HangmanProgress();
                    }
                }
            }
        }

        /// <summary>
        /// This function calls every function to create and start a game
        /// </summary>
        public void CreateGame()
        {
            var bc = new BrushConverter();
            bdrInfo.Background = (Brush)bc.ConvertFrom("#FBC02D");
            lblInfo.Content = "Klik op een letter om te beginnen";

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
                    bdrInfo.Background = Brushes.Red;
                    lblInfo.Content = "Oh nee! Je hebt het woord niet geraden.";
                    lblInfo.Foreground = Brushes.White;

                    int iAmountBad = Convert.ToInt32(lblAmountBad.Content);
                    iAmountBad++;
                    lblAmountBad.Content = iAmountBad.ToString();

                    bdrNextGame.Cursor = Cursors.Hand;
                    bdrNextGame.Opacity = 1;
                }
            }
        }

        /// <summary>
        /// This function checks if the player has guessed the word and if he/she has a new game will start
        /// </summary>
        public void WordGuessed()
        {
            var bc = new BrushConverter();
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
                    bdrInfo.Background = (Brush)bc.ConvertFrom("#00E676");
                    lblInfo.Content = "Heel goed! Je hebt het woord geraden.";
                    lblInfo.Foreground = Brushes.White;

                    int iAmountGood = Convert.ToInt32(lblAmountGood.Content);
                    iAmountGood++;
                    lblAmountGood.Content = iAmountGood.ToString();

                    bdrNextGame.Cursor = Cursors.Hand;
                    bdrNextGame.Opacity = 1;
                }
            }
            if (iGoodGuess == dpLetters.Children.Count)
            {
                for(int i = 0; i < dpLetters.Children.Count; i++)
                {
                    Border bdrLetterToGuess = (Border)dpLetters.Children[i];
                    ThicknessAnimation ta = new ThicknessAnimation
                    {
                        From = new Thickness(15,0,15,0),
                        To = new Thickness(0),
                        Duration = TimeSpan.FromSeconds(1)
                    };
                    bdrLetterToGuess.BeginAnimation(MarginProperty, ta);
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
                Border bdrPlayingLetters = (Border)grdChooseLetter.Children[i];
                Label lblPlayingLetters = (Label)bdrPlayingLetters.Child;

                bdrPlayingLetters.Background = (Brush)bc.ConvertFrom("#BDBDBD");
                lblPlayingLetters.Opacity = 1;
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

            bdrNextGame.Cursor = Cursors.Arrow;
            bdrNextGame.Opacity = 0.5;

            CreateGame();
        }

        /// <summary>
        /// This function checks if the player is still guessing or guessed or failed to guess the word
        /// </summary>
        /// <returns>False = the player is still guessing; True = the player guessed or failed to guess the word</returns>
        public bool IsGameOver()
        {
            int iGoodGuess = 0;
            int iRedLetters = -1;

            for (int i = 0; i < dpLetters.Children.Count; i++)
            {
                Border bdrLetterToGuess = (Border)dpLetters.Children[i];
                Label lblLetterToGuess = (Label)bdrLetterToGuess.Child;

                if (lblLetterToGuess.Opacity == 1)
                {
                    iGoodGuess++;
                }
            }

            if (iGoodGuess == dpLetters.Children.Count)
            {
                return true;
            }

            for (int i = 0; i < grdChooseLetter.Children.Count; i++)
            {
                Border bdrLetter = (Border)grdChooseLetter.Children[i];
                if (bdrLetter.Background == Brushes.Red)
                {
                    iRedLetters++;
                }
            }

            if (iRedLetters == (grdHangman.Children.Count - 1))
            {
                return true;
            }

            return false;
        }

        private void bdrNextGame_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            bool bIsGameOver = IsGameOver();
            if (bIsGameOver)
            {
                ResetGame();
            }
        }

        private void bdrStop_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
