using System;
using System.Data;
using System.Data.SqlClient;

namespace MinerCoin
{
    internal class LoginDB : IDisposable
    {
        private SqlConnection _cn;
        private bool disposedValue;

        void EnsureConnected()
        {
            if (_cn != null) return;

            var builder = new SqlConnectionStringBuilder();
            builder.InitialCatalog = "UserDataBase";
            builder.DataSource = "DESKTOP-4R4Q7SE";
            builder.IntegratedSecurity = true;
            var connStr = builder.ConnectionString;

            _cn = new SqlConnection(connStr);
            _cn.Open();
        }

        internal void SaveGameResult(int userId, GameResult gameResult)
        {

        }

        internal int Login(string userName, string userPassword)
        {
            EnsureConnected();
            using (var cmd = _cn.CreateCommand())
            {
                cmd.CommandText = "select top 1 UserId from UserInfo where UserName = @userName and UserPassword = @userPassword";
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.AddWithValue("userName", userName);
                cmd.Parameters.AddWithValue("userPassword", userPassword);

                using (var dataReader = cmd.ExecuteReader())
                {
                    if (dataReader.Read())
                    {
                        var userId = Convert.ToInt32(dataReader["UserId"]);
                        return userId;
                    }
                }

                return -1;
            }
        }

        internal void Register(string userName, string userPassword)
        {
            EnsureConnected();
            using (var cmd = _cn.CreateCommand())
            {
                cmd.CommandText = @"INSERT INTO [dbo].[UserInfo]
                                           (
                                            [UserName]
                                           ,[UserPassword])
                                    VALUES
                                           (
                                            @username
                                           ,@userpassword)";

                cmd.CommandType = CommandType.Text;

                cmd.Parameters.AddWithValue("userName", userName);
                cmd.Parameters.AddWithValue("userPassword", userPassword);
                var dataReader = cmd.ExecuteNonQuery();
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _cn?.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
