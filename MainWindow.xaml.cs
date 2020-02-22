﻿using System;
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
        
        // WordCollection speaks for itself, you could call it the library of words.
        List<WordCollection> wordCollection = new List<WordCollection>();

        // The Random function will be used to get a random generated number between 0 and the total amount of words (minus 1, because index starts at 0).
        Random rnd = new Random();

        // The string rndWord stores the random word. 
        string rndWord;

        public MainWindow()
        {
            InitializeComponent();

            // The string pathWordListFile stores the path to wordList.txt
            string pathWordListFile = $"{Directory.GetCurrentDirectory()}\\..\\..\\..\\assets\\wordList.txt";

            // The List<string> wordList stores everything inside wordList.txt and splits it on the ','.
            List<string> wordList = File.ReadAllText(pathWordListFile).Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();

            // Storing every word in WordCollection
            foreach (string word in wordList)
            {
                WordCollection newWord = new WordCollection
                {
                    Word = word
                };

                wordCollection.Add(newWord);
            }

            // The Random function and rndWord in action.
            rndWord = wordCollection[rnd.Next(0, (wordCollection.Count - 1))].Word;
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
    }
}
