using System.Windows;

namespace MinerCoin
{
    public partial class MainWindow : Window
    {
        private LoginDB _loginDB = new LoginDB();
        private int _userId;
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
            _loginDB.Register(CheckUserName.Text, CheckPassword.Text);
            MessageBox.Show("Успех");
        }

        private void CheckLogin_Click(object sender, RoutedEventArgs e)
        {           
            var userId =  _loginDB.Login(CheckUserName.Text, CheckPassword.Text);
            if (userId > 0)
            {
                MessageBox.Show("Успех");
                _userId = userId;
            }
            else
            {
                MessageBox.Show("Error");
            }
        }
    }
}
