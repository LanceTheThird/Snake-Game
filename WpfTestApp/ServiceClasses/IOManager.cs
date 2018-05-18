using System;
using System.Collections.ObjectModel;
using System.IO;
using Newtonsoft.Json;
using WpfTestApp.Interfaces;
using WpfTestApp.ViewModels;

namespace WpfTestApp.ServiceClasses
{
    internal class IOManager : IBlocksLoader, IPathManager, IGuiLoader
    {
        private static StreamReader NeedLevel(int currentLevel)
        {
            return new StreamReader(string.Format(Environment.CurrentDirectory + @"\data\levels\path{0}.txt",
                currentLevel));
        }

        private static string NeedPath(int currentLevel)
        {
            return string.Format(Environment.CurrentDirectory + @"\data\paths\path{0}.txt", currentLevel);
        }

        public ObservableCollection<Block> LoadBlocks(int currentLevel)
        {
            using (var r = NeedLevel(currentLevel))
            {
                var json = r.ReadToEnd();
                return JsonConvert.DeserializeObject<ObservableCollection<Block>>(json);
            }
        }

        private static StreamReader GetDescriptions()
        {
            return new StreamReader(Environment.CurrentDirectory + @"\data\Descr\LevelDescr.txt");
        }

        public ObservableCollection<string> LoadDescription()
        {
            using (var r = GetDescriptions())
            {
                var json = r.ReadToEnd();
                return JsonConvert.DeserializeObject<ObservableCollection<string>>(json);
            }
        }

        public void UnloadPath(ObservableCollection<TimeTickData> tickDatas, int currentLevel)
        {
            using (var file = File.CreateText(NeedPath(currentLevel)))
            {
                var serializer = new JsonSerializer();
                serializer.Serialize(file, tickDatas);
            }
        }

        public ObservableCollection<TimeTickData> LoadPath(int currentLevel)
        {
            using (var r = new StreamReader(NeedPath(currentLevel)))
            {
                var json = r.ReadToEnd();
                return JsonConvert.DeserializeObject<ObservableCollection<TimeTickData>>(json);
            }
        }

        public bool IsThereAPath(int currentLevel)
        {
            try
            {
                var streamReader = new StreamReader(NeedPath(currentLevel));
            }
            catch (FileNotFoundException)
            {
                return false;
            }

            return true;
        }

        public string GetShot(int currentLevel)
        {
            return string.Format(Environment.CurrentDirectory + @"\data\screenshots\UserShotLevel{0}.png",
                currentLevel);
        }

        public string GetBack(int currentLevel)
        {
            return string.Format(Environment.CurrentDirectory + @"\data\Backs\{0}.jpg", currentLevel);
        }

        public string GetListBack()
        {
            return string.Format(Environment.CurrentDirectory + @"\data\Backs\ListBack.jpg");
        }

        public string GetDetailsBack()
        {
            return string.Format(Environment.CurrentDirectory + @"\data\Backs\DetailsBack.jpg");
        }

        private static StreamReader GetTitle()
        {
            return new StreamReader(Environment.CurrentDirectory + @"\data\Descr\Title.txt");
        }

        public string GetBody(int currentLevel)
        {
            return string.Format(Environment.CurrentDirectory + @"\data\Icons\Body{0}.txt", currentLevel);
        }

        public string LoadTitle()
        {
            using (var r = GetTitle())
            {
                var json = r.ReadToEnd();
                return json;
            }
        }
    }
}