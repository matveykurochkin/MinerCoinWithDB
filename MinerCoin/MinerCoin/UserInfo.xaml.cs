using System;
using System.Windows;
using System.Windows.Media;



namespace MinerCoin
{
    public partial class UserInfo : Window
    {
        private LoginDB _loginDB = new LoginDB();
        public static bool isDelete = false; 
        public UserInfo()
        {
            InitializeComponent();
            var result = _loginDB.LoadResultGame(User.userId);
            var bonus = _loginDB.LoadBonuses(User.userId);
            var Theme = _loginDB.LoadTheme(User.userId);

            NameAcc.Content = User.Name;
            ScoresOnAcc.Content = result;
            BonusesOnAcc.Content = bonus;

            var Red = Convert.ToByte(Theme[0]);
            var Green = Convert.ToByte(Theme[1]);
            var Blue = Convert.ToByte(Theme[2]);
            Background = new SolidColorBrush(Color.FromRgb(Red, Green, Blue));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
          var deleteAcc = MessageBox.Show("Удалить аккаунт?",
                "Delete Account",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);
            if (deleteAcc == MessageBoxResult.Yes)
            {
                _loginDB.deleteAccount(User.userId);
                MessageBox.Show("Удаление заершено!",
                "Delete completed",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
                isDelete = true;
                Close();
                
            }
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
