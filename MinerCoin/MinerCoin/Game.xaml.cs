using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MinerCoin
{
    public partial class Game : Window
    {
        GameResult gameResult = new GameResult();
        private LoginDB _loginDB = new LoginDB();
        private int counter = 1;
        Random rndColor = new Random();
        public Game()
        {
            InitializeComponent();
            ResultBox.Text += _loginDB.LoadResultGame(User.userId);

            var ThemeR = _loginDB.LoadThemeR(User.userId);
            var ThemeG = _loginDB.LoadThemeG(User.userId);
            var ThemeB = _loginDB.LoadThemeB(User.userId);
            if (ThemeR > -1 && ThemeG > -1 && ThemeR > -1)
            {
                var Red = Convert.ToByte(ThemeR);
                var Green = Convert.ToByte(ThemeG);
                var Blue = Convert.ToByte(ThemeB);
                Background = new SolidColorBrush(Color.FromRgb(Red, Green, Blue));
            }
        }

        private void Save()
        {
            _loginDB.delete(User.userId);
            _loginDB.SaveGameResult(User.userId, gameResult);
            _loginDB.deleteBonus(User.userId);
            _loginDB.SaveBonus(User.userId, gameResult);
            gameResult.Scores = 0;
            counter = 1;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var result = _loginDB.LoadResultGame(User.userId);
            var bonus = _loginDB.LoadBonuses(User.userId);
            ResultBox.Clear();
            ResultBox.Text += counter++ + result + bonus;
            gameResult.Scores = Convert.ToInt16(ResultBox.Text);
            gameResult.Bonus = bonus;
            Save();

        }

        private void FirstBonusButton_Click(object sender, RoutedEventArgs e)
        {
            var result = _loginDB.LoadResultGame(User.userId);
            var bonus = _loginDB.LoadBonuses(User.userId);

            if (result >= 10)
            {
                bonus += 2;
                ResultBox.Clear();
                ResultBox.Text += counter + result - 11;
                gameResult.Scores = Convert.ToInt16(ResultBox.Text);
                gameResult.Bonus = bonus;
                Save();
            }
            else
            {
                ResultBox.Clear();
                ResultBox.Text += "Не хватает Coin!";
            }
        }

        private void ThemeButton_Click(object sender, RoutedEventArgs e)
        {
            var result = _loginDB.LoadResultGame(User.userId);
            var bonus = _loginDB.LoadBonuses(User.userId);
            if (result >= 10)
            {
                var Red = Convert.ToByte(rndColor.Next(0, 255));
                var Green = Convert.ToByte(rndColor.Next(0, 255));
                var Blue = Convert.ToByte(rndColor.Next(0, 255));
                Background = new SolidColorBrush(Color.FromRgb(Red, Green, Blue));
                ResultBox.Clear();
                ResultBox.Text += counter + result - 11;
                gameResult.Scores = Convert.ToInt16(ResultBox.Text);
                gameResult.Red = Red;
                gameResult.Green = Green;
                gameResult.Blue = Blue;
                _loginDB.deleteTheme(User.userId);
                _loginDB.SaveTheme(User.userId, gameResult);
                gameResult.Bonus = bonus;
                Save();
            }
            else
            {
                ResultBox.Clear();
                ResultBox.Text += "Не хватает Coin!";
            }
        }
    }
}
