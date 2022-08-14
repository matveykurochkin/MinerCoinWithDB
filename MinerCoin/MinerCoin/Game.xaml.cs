using System;
using System.Windows;
using System.Windows.Media;


namespace MinerCoin
{
    public partial class Game : Window
    {
        GameResult gameResult = new GameResult();
        private LoginDB _loginDB = new LoginDB();
        private int counter = 1;
        Random rndColor = new Random();

        private ResultBoxModel _model = new ResultBoxModel();

        public Game()
        {
            InitializeComponent();
            _model.Result += _loginDB.LoadResultGame(User.userId);

            var Theme = _loginDB.LoadTheme(User.userId);
            var Red = Convert.ToByte(Theme[0]);
            var Green = Convert.ToByte(Theme[1]);
            var Blue = Convert.ToByte(Theme[2]);
            Background = new SolidColorBrush(Color.FromRgb(Red, Green, Blue));
            if (UserInfo.isDelete)
            {
                Close();
            }

            this.DataContext = _model;
        }

        private void Save()
        {
            _loginDB.SaveGameResult(User.userId, gameResult);
            gameResult.Scores = 0;
            counter = 1;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var result = _loginDB.LoadResultGame(User.userId);
            var bonus = _loginDB.LoadBonuses(User.userId);
            _model.Result = 0;
            _model.Result += counter++ + result + bonus;
            gameResult.Scores = _model.Result;
            gameResult.Bonus = bonus;
            Save();

        }

        private void FirstBonusButton_Click(object sender, RoutedEventArgs e)
        {
            var result = _loginDB.LoadResultGame(User.userId);
            var bonus = _loginDB.LoadBonuses(User.userId);

            if (result >= 100)
            {
                bonus += 2;
                _model.Result = 0;
                _model.Result += counter + result - 101;
                gameResult.Scores = _model.Result;
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
            if (result >= 50)
            {
                var Red = Convert.ToByte(rndColor.Next(0, 255));
                var Green = Convert.ToByte(rndColor.Next(0, 255));
                var Blue = Convert.ToByte(rndColor.Next(0, 255));
                Background = new SolidColorBrush(Color.FromRgb(Red, Green, Blue));
                _model.Result = 0;
                _model.Result += counter + result - 51;
                gameResult.Scores = _model.Result;
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

        private void AccountInformation_Click(object sender, RoutedEventArgs e)
        {
            UserInfo info = new UserInfo();
            info.Show();
        }
    }
}
