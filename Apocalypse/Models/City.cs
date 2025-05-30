namespace Apocalypse.Models
{
    public class City
    {
        public string Name { get; set; }
        public Coordinate Coordinate { get; set; }

        public SupplyDrop NearestSupplyDrop { get; set; }
        public double DistanceToNearestDrop { get; set; } = double.MaxValue;
    }
}
