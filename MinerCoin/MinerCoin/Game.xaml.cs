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
        int counter = 0;
        public Game()
        {
            InitializeComponent();
            
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var result = _loginDB.LoadResultGame(User.userId);
            ResultBox.Clear();
            ResultBox.Text += counter++ + result;
            gameResult.Scores = Convert.ToInt16(ResultBox.Text);
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            _loginDB.delete(User.userId);
            _loginDB.SaveGameResult(User.userId, gameResult);
        }
    }
}
