using System.Text.RegularExpressions;

namespace CarChecker.Shared
{
    public enum VehiclePart
    {
        BodyFront,
        BodyFrontLeft,
        BodyFrontRight,
        BodyRear,
        BodyRearLeft,
        BodyRearRight,
        Bonnet,
        DoorFrontLeft,
        DoorFrontRight,
        DoorRearLeft,
        DoorRearRight,
        Grill,
        HeadLightLeft,
        HeadLightRight,
        MirrorLeft,
        MirrorRight,
        Roof,
        TailLightLeft,
        TailLightRight,
        Undercarriage,
        WheelArchFrontLeft,
        WheelArchFrontRight,
        WheelArchRearLeft,
        WheelArchRearRight,
        WheelFrontLeft,
        WheelFrontRight,
        WheelRearLeft,
        WheelRearRight,
        WindowBack,
        WindowFrontLeft,
        WindowFrontRight,
        WindowRearLeft,
        WindowRearRight,
        Windshield,
    }

    public static class VehiclePartExtensions
    {
        private static Regex InnerCapital = new Regex("(.)([A-Z])");

        public static string DisplayName(this VehiclePart part)
        {
            return InnerCapital.Replace(part.ToString(), m => $"{m.Groups[1].Value} {m.Groups[2].Value}");
        }
    }
}
