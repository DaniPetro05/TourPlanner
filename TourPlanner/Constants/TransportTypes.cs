namespace TourPlanner.Constants;

public static class TransportTypes
{
    public const string DrivingCar = "driving-car";
    public const string CyclingRegular = "cycling-regular";
    public const string FootWalking = "foot-walking";

    public static readonly string[] All =
    {
        DrivingCar,
        CyclingRegular,
        FootWalking
    };
}