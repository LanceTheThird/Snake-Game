using System;

namespace WpfTestApp.Model
{
    public static class Constants
    {
        public static int Step { get; } = 40;
        public static int Scale { get; } = 5;
        public static int Timestep { get; } = 150;
        public static int Height { get; } = 400;
        public static int Width { get; } = 800;
        public static int QuaterAngle { get; } = 90;
        public static int SnakeStartSize { get; } = 3;
        public static int FoodNumber { get; } = 0;
        public static int StartTop { get; } = 1 * Step;
        public static int StartLeft { get; } = 15 * Step;
        public static int AccelerationBuffer { get; } = 3 * Step;
        public static int Easy { get; } = 10;
        public static int LevelsCount { get; } = 7;
        public static string Head { get; } = "Red";
        public static string Body { get; } = "Blue";
        public static string Food { get; } = "Yellow";
        public static string Wall { get; } = "Brown";
        public static string RecordString { get; } = "Your record is {0} points";

        public static string ConnectionErrorMessage { get; } =
            "There was an error during update of your database. Please, contact your administrator";

        public static string BourderTouchError { get; } = "Don`t tuch borders!";
        public static string GameOver { get; } = "Game over. Do you want exit?";
        public static string Confirmation { get; } = "Confirmation";

        public static Random Random = new Random();
    }
}
