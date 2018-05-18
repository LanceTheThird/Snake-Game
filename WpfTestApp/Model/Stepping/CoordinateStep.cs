
namespace WpfTestApp.Model.Stepping
{
    internal class CoordinateStep
    {
        public int Left { get; set; }
        public int Top { get; set; }

        public CoordinateStep( int left, int top)
        {
            Left = left;
            Top = top;
        }
    }
}
