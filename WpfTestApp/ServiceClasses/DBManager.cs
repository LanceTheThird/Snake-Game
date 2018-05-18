using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using WpfTestApp.Interfaces;
using WpfTestApp.Model;

namespace WpfTestApp.ServiceClasses
{
    internal class DBManager : IPathManager
    {
        public void UnloadPath(ObservableCollection<TimeTickData> tickDatas, int currentLevel)
        {
            var sql = $"SELECT * FROM TimeTickTable WHERE TimeTickTable.Level={currentLevel}";
            var con = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (var connection = new SqlConnection(con))
            {
                connection.Open();
                var adapter = new SqlDataAdapter(sql, connection);
                var ds = new DataSet();
                adapter.Fill(ds);

                var dt = ds.Tables[0];
                for (var i = 0; i < tickDatas.Count; i++)
                {
                    var newRow = dt.NewRow();
                    newRow["Level"] = currentLevel;
                    newRow["Tick"] = i;
                    newRow["FoodLeft"] = tickDatas[i].FoodLeft;
                    newRow["FoodTop"] = tickDatas[i].FoodTop;
                    newRow["Height"] = tickDatas[i].Height;
                    newRow["Width"] = tickDatas[i].Width;
                    newRow["Direction"] = DirectionNumber(tickDatas[i].Direction);
                    newRow["LevelPassed"] = DateTime.Now;
                    dt.Rows.Add(newRow);
                }

                try
                {
                    var cb = new SqlCommandBuilder(adapter);
                    adapter.UpdateCommand = cb.GetUpdateCommand();
                    adapter.Update(ds);
                }
                catch (SqlException)
                {
                    MessageBox.Show(Constants.ConnectionErrorMessage);
                }

                ds.Clear();
            }
        }

        public ObservableCollection<TimeTickData> LoadPath(int currentLevel)
        {     
            var sqlExpression =
                $"SELECT * FROM TimeTickTable WHERE TimeTickTable.LevelPassed = (SELECT MAX(LevelPassed) FROM TimeTickTable WHERE Level = {currentLevel}) ORDER BY TimeTickTable.Tick";
            var con = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            var tickDatas = new ObservableCollection<TimeTickData>();
            using (var connection = new SqlConnection(con))
            {              
                connection.Open();
                var command = new SqlCommand(sqlExpression, connection);
                var reader = command.ExecuteReader();
                
                if (reader.HasRows) 
                {
                    
                    while (reader.Read()) 
                    {
                        var iter = new TimeTickData
                        {
                            FoodLeft = reader.GetInt32(2),
                            FoodTop = reader.GetInt32(3),
                            Height = reader.GetInt32(4),
                            Width = reader.GetInt32(5),
                            Direction = NumberDirection(reader.GetInt16(6))
                        };


                        tickDatas.Add(iter);
                    }
                }

                reader.Close();
            }

            return tickDatas;
        }

        public bool IsThereAPath(int currentLevel)
        {
            var sqlExpression =
                $"SELECT * FROM TimeTickTable WHERE TimeTickTable.LevelPassed = (SELECT MAX(LevelPassed) FROM TimeTickTable WHERE Level = {currentLevel}) ORDER BY TimeTickTable.Tick";
            var con = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            var result = false;
            using (var connection = new SqlConnection(con))
            {
                connection.Open();
                var command = new SqlCommand(sqlExpression, connection);
                var reader = command.ExecuteReader();

                if (reader.HasRows) 
                {
                    result = true;
                }
                reader.Close();
            }

            return result;
        }

        private static int DirectionNumber(Direction direction)
        {
            switch (direction)
            {
                case Direction.Down:
                    return 1;
                case Direction.Up:
                    return 2;
                case Direction.Right:
                    return 3;
                default:
                    return 4;
            }
        }

        private static Direction NumberDirection(int direction)
        {
            switch (direction)
            {
                case 1:
                    return Direction.Down;
                case 2:
                    return Direction.Up;
                case 3:
                    return Direction.Right;
                default:
                    return Direction.Left;
            }
        }
    }
}

