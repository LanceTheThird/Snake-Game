using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using WpfTestApp.Interfaces;
using WpfTestApp.Model;

namespace WpfTestApp.ServiceClasses
{
    class DBBestScoreLoader : IBestScoreLoader
    {
        public void LoadBestScore( BestScore score)
        {
            var sql = $"SELECT * FROM BestScoresTable WHERE UserName='{score.UserName}' AND Level={score.Level}";
            var con = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (var connection = new SqlConnection(con))
            {
                connection.Open();
                var adapter = new SqlDataAdapter(sql, connection);
                var ds = new DataSet();
                adapter.Fill(ds);

                var dt = ds.Tables[0];
                var customerRow =ds.Tables[0].Select($" UserName='{score.UserName}' AND Level={score.Level}");
                if (customerRow.Length > 0)
                {                   
                    if ((int)customerRow[0]["Score"] < score.Score)
                    customerRow[0]["Score"] = score.Score;
                }
                else
                {
                    var newRow = dt.NewRow();
                    newRow["UserName"] = score.UserName;
                    newRow["Level"] = score.Level;
                    newRow["Score"] = score.Score;
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

        public BestScore UnloadBestScore(int currentLevel)
        {
            var userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            var sqlExpression =
                $"SELECT * FROM BestScoresTable WHERE Level = {currentLevel} AND UserName = '{userName}'";
            var con = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            var score = new BestScore();
            using (var connection = new SqlConnection(con))
            {
                connection.Open();
                var command = new SqlCommand(sqlExpression, connection);
                var reader = command.ExecuteReader();
                
                if (reader.HasRows)
                {

                    while (reader.Read())
                    {
                        score.Id = reader.GetInt32(0);
                        score.UserName = reader.GetString(1);
                        score.Level = reader.GetInt32(2);
                        score.Score = reader.GetInt32(3);                                                
                    }
                }

                reader.Close();
            }

            return score;
        }
    }
}
