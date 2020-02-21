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
        string path = Directory.GetCurrentDirectory();
        string[] pathSplit;
        string finalPath;
        List<string> wordList;

        public MainWindow()
        {
            InitializeComponent();

            pathSplit = path.Split("\\bin");

            finalPath = pathSplit[0] + "\\assets\\wordList.txt";

            wordList = File.ReadAllLines(finalPath).ToList();

            MessageBox.Show(wordList[0]);
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
