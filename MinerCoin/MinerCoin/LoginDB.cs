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
                return 0;
            }
        }   
        internal void SaveGameResult(int userId, GameResult gameResult)
        {
            EnsureConnected();
            using (var cmd = _cn.CreateCommand())
            {
                cmd.CommandText = @"
                        MERGE INTO UserBonuses AS tgt  
                        USING (SELECT @UserId as UserId, @Bonus as Bonus) as src 
                        ON tgt.UserId = src.UserId
                        WHEN MATCHED
                            THEN UPDATE SET UserBonuses = src.Bonus                 
                        WHEN NOT MATCHED BY TARGET THEN 
                        INSERT (UserId, UserBonuses) VALUES (src.UserId, src.Bonus);
	

                        MERGE INTO ResultGame AS tgt  
                        USING (SELECT @UserId as UserId, @Scores as Scores) as src 
                        ON tgt.UserId = src.UserId
                        WHEN MATCHED
                            THEN UPDATE SET Scores = src.Scores                 
                        WHEN NOT MATCHED BY TARGET THEN 
                        INSERT (UserId, Scores) VALUES (src.UserId, src.Scores);
";

                cmd.CommandType = CommandType.Text;

                cmd.Parameters.AddWithValue("UserId", userId);
                cmd.Parameters.AddWithValue("Bonus", gameResult.Bonus);
                cmd.Parameters.AddWithValue("Scores", gameResult.Scores);
                var dataReader = cmd.ExecuteNonQuery();
            }
        }

        internal void SaveTheme(int userId, GameResult gameResult)
        {
            EnsureConnected();
            using (var cmd = _cn.CreateCommand())
            {
                cmd.CommandText = @"INSERT INTO [dbo].[UserTheme]
                                           ([UserId],[Red],[Green],[Blue])
                                    VALUES
                                           (
                                            @UserId,@Red,@Green,@Blue)";

                cmd.CommandType = CommandType.Text;

                cmd.Parameters.AddWithValue("UserId", userId);
                cmd.Parameters.AddWithValue("Red", gameResult.Red);
                cmd.Parameters.AddWithValue("Green", gameResult.Green);
                cmd.Parameters.AddWithValue("Blue", gameResult.Blue);
                var dataReader = cmd.ExecuteNonQuery();
            }
        }

        internal void deleteTheme(int userId)
        {
            EnsureConnected();
            using (var cmd = _cn.CreateCommand())
            {
                cmd.CommandText = @"DELETE UserTheme
                                        WHERE UserId = @userId";
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("UserId", userId);
                var dataReader = cmd.ExecuteNonQuery();
            }
        }
        internal int[] LoadTheme(int userId)
        {
            EnsureConnected();
            int[] colorArr = new int[] {123,122,155};
            using (var cmd = _cn.CreateCommand())
            {
                cmd.CommandText = @"SELECT Red, Green, Blue
                                        FROM UserTheme
                                        WHERE UserId = @userId;";
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("UserId", userId);
                using (var dataReader = cmd.ExecuteReader())
                {
                    if (dataReader.Read())
                    {
                        var ColorRed = Convert.ToInt32(dataReader["Red"]);
                        var ColorGreen = Convert.ToInt32(dataReader["Green"]);
                        var ColorBlue = Convert.ToInt32(dataReader["Blue"]);
                        colorArr[0] = ColorRed;
                        colorArr[1] = ColorGreen;
                        colorArr[2] = ColorBlue;
                        return colorArr;
                    }
                }
                return colorArr;
            }
        }

        internal void deleteAccount(int userId)
        {
            EnsureConnected();
            using (var cmd = _cn.CreateCommand())
            {
                cmd.CommandText = @"DELETE UserInfo
                                        WHERE UserId = @userId";
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("UserId", userId);
                var dataReader = cmd.ExecuteNonQuery();
            }
        }

        internal int LoadBonuses(int userId)
        {
            EnsureConnected();
            using (var cmd = _cn.CreateCommand())
            {
                cmd.CommandText = @"SELECT UserBonuses
                                        FROM UserBonuses
                                        WHERE UserId = @userId;";
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.AddWithValue("UserId", userId);


                using (var dataReader = cmd.ExecuteReader())
                {
                    if (dataReader.Read())
                    {
                        var userBonus = Convert.ToInt32(dataReader["UserBonuses"]);
                        return userBonus;
                    }
                }
                return 0;
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

        internal bool CheckUserNameInDB(string userName)
        {
            EnsureConnected();
            using (var cmd = _cn.CreateCommand())
            {
                cmd.CommandText = "select top 1 UserName from UserInfo where UserName = @userName";
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.AddWithValue("userName", userName);

                using (var dataReader = cmd.ExecuteReader())
                {
                    if (dataReader.Read())
                    {
                        return true;
                    }
                }
            }
            return false;
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
