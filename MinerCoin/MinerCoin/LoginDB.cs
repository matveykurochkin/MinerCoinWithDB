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

        internal void delete(int userId)
        {
            EnsureConnected();
            using (var cmd = _cn.CreateCommand())
            {
                cmd.CommandText = @"DELETE ResultGame
                                        WHERE UserId = @userId";
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("UserId", userId);
                var dataReader = cmd.ExecuteNonQuery();
            }
        }

        internal int LoadResultGame(int userId)
        {
            EnsureConnected();
            using (var cmd = _cn.CreateCommand())
            {
                cmd.CommandText = @"SELECT Scores
                                        FROM ResultGame
                                        WHERE UserId = @userId;";
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.AddWithValue("UserId", userId);


                using (var dataReader = cmd.ExecuteReader())
                {
                    if (dataReader.Read())
                    {
                        var scores = Convert.ToInt32(dataReader["Scores"]);
                        return scores;
                    }
                }
                return -1;
            }
        }

        internal void SaveGameResult(int userId, GameResult gameResult)
        {
            EnsureConnected();
            using (var cmd = _cn.CreateCommand())
            {
                cmd.CommandText = @"INSERT INTO [dbo].[ResultGame]
                                           (
                                            [UserId]
                                           ,[Scores])
                                    VALUES
                                           (
                                            @UserId
                                           ,@Scores)";

                cmd.CommandType = CommandType.Text;

                cmd.Parameters.AddWithValue("UserId", userId);
                cmd.Parameters.AddWithValue("Scores", gameResult.Scores);
                var dataReader = cmd.ExecuteNonQuery();
            }
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
