using Apocalypse.Models;
using System.Globalization;

var cities = new City[10] {
    new City{ Name = "London, UK", Coordinate = new Coordinate { Latitude = 51.5074, Longitude = -0.1278 }},
    new City{ Name = "Paris, France", Coordinate = new Coordinate { Latitude = 48.8566, Longitude = 2.3522 }},
    new City{ Name = "Berlin, Germany", Coordinate = new Coordinate { Latitude = 52.5200, Longitude = 13.40500 }},
    new City{ Name = "Madrid, Spain", Coordinate = new Coordinate { Latitude = 40.4168, Longitude = -3.7038 }},
    new City{ Name = "Rome, Italy", Coordinate = new Coordinate { Latitude = 41.9028, Longitude = 12.4964 }},
    new City{ Name = "Amsterdam, Netherlands", Coordinate = new Coordinate { Latitude = 52.3676, Longitude = 4.9041 }},
    new City{ Name = "Vienna, Austria", Coordinate = new Coordinate { Latitude = 48.2082, Longitude = 16.3738 }},
    new City{ Name = "Stockholm, Sweden", Coordinate = new Coordinate { Latitude = 59.3293, Longitude = 18.0686 }},
    new City{ Name = "Athens,Greece", Coordinate = new Coordinate { Latitude = 37.9838, Longitude = 23.7275 }},
    new City{ Name = "Dublin, Ireland", Coordinate = new Coordinate { Latitude = 53.3498, Longitude = -6.2603 }},
};

var filePath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "/Assets/drop_locations-1.txt";

using var reader = new StreamReader(filePath);

string? supplyDropLineItem;

while ((supplyDropLineItem = reader.ReadLine()) != null)
{
    var line = supplyDropLineItem.Split();
    var supplyDrop = new SupplyDrop { 
        Id = line[0], 
        Coordinate = new Coordinate { 
            Latitude = Convert.ToDouble(line[1], CultureInfo.InvariantCulture), 
            Longitude = Convert.ToDouble(line[2] , CultureInfo.InvariantCulture) 
        }
    };

    for (int i = 0; i < cities.Length; i++) {
        var city = cities[i];
        var distance = CalculateDistance(city.Coordinate, supplyDrop.Coordinate);
        if (distance < city.DistanceToNearestDrop) {
            cities[i].NearestSupplyDrop = supplyDrop;
            cities[i].DistanceToNearestDrop = distance;
        }
    }
}

for (int i = 0; i < cities.Length; i++) {
    Console.WriteLine($"City: { cities[i].Name }, Nearest supply drop: { cities[i].NearestSupplyDrop.Id }, Distance: { Math.Round(cities[i].DistanceToNearestDrop,2) }km");
}




static double CalculateDistance(Coordinate cityCoordinate, Coordinate supplyDropCoordinate)
{

    //Using Havarsine formula since the earth is a sphere
    const double earthRadius = 6371;

    var cityLatitude = CalculateRadians(cityCoordinate.Latitude);
    var cityLongitude = CalculateRadians(cityCoordinate.Longitude);
    var supplyDropLatitude = CalculateRadians(supplyDropCoordinate.Latitude);
    var supplyDropLongitude = CalculateRadians(supplyDropCoordinate.Longitude);

    var latitudeDelta = supplyDropLatitude - cityLatitude;
    var longitudeDelta = supplyDropLongitude - cityLatitude;

    var havarsine = Math.Pow(Math.Sin(latitudeDelta / 2), 2) +
                    Math.Cos(cityLatitude) * Math.Cos(supplyDropLatitude) *
                    Math.Pow(Math.Sin(longitudeDelta / 2), 2);

    return 2 * earthRadius * Math.Asin(Math.Sqrt(havarsine));
}

static double CalculateRadians(double angle) => angle * Math.PI / 180;