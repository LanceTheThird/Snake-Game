using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfTestApp.Model
{
    class BestScore
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public int Level { get; set; }
        public int Score { get; set; }

        public BestScore(string userName, int level, int score)
        {
            UserName = userName;
            Level = level;
            Score = score;
        }

        public BestScore()
        { }
        
    }
}
