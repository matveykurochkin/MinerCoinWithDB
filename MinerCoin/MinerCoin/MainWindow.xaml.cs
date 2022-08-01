using System.Windows;

namespace MinerCoin
{
    public partial class MainWindow : Window
    {
        private LoginDB _loginDB = new LoginDB();
        public int _userId;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            _loginDB?.Dispose();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            if (CheckUserName.Text != "" && CheckPassword.Text != "")
            {
                _loginDB.Register(CheckUserName.Text, CheckPassword.Text);
                MessageBox.Show("Регистрация выполнена!",
                         "Information",
                         MessageBoxButton.OK,
                         MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Регистрация не выполненна! Не заполнены необходимые поля!",
                              "Error",
                             MessageBoxButton.OK,
                             MessageBoxImage.Error);
            }
        }

        private void CheckLogin_Click(object sender, RoutedEventArgs e)
        {
            var userId = _loginDB.Login(CheckUserName.Text, CheckPassword.Text);
            if (userId > 0)
            {
                MessageBox.Show("Вход выполнен!",
                    "Information",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
                User.userId = userId;

                Game newGame = new Game();
                newGame.Show();
            }
            else
            {
                MessageBox.Show("Вход не выполнен! Проверьте правильность ввода данных или зарегестрируйтесь!",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                RegisterButton.Visibility = Visibility.Visible;
            }
        }
    }
}
