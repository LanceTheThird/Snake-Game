
namespace WpfTestApp.Model.Stepping
{
    internal class CoordinateStepping
    {
        public CoordinateStep[] Stepping = new CoordinateStep[4];

        public CoordinateStepping()
        {
            Stepping[0] = new CoordinateStep(Constants.Step, 0);
            Stepping[1] = new CoordinateStep(-Constants.Step, 0);
            Stepping[2] = new CoordinateStep(0, Constants.Step);
            Stepping[3] = new CoordinateStep(0, -Constants.Step);
        }

    }
}
