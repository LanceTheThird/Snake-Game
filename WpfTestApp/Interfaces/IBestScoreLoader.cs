using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfTestApp.Model;

namespace WpfTestApp.Interfaces
{
    interface IBestScoreLoader
    {
        void LoadBestScore(BestScore score);
        BestScore UnloadBestScore(int currentLevel);
    }
}
