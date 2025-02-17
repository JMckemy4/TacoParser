﻿using System;
using System.Linq;
using System.IO;
using GeoCoordinatePortable;

namespace LoggingKata
{
    class Program
    {
        static readonly ILog logger = new TacoLogger();
        const string csvPath = "TacoBell-US-AL.csv";

        static void Main(string[] args)
        {
            // TODO:  Find the two Taco Bells that are the furthest from one another.
            // HINT:  You'll need two nested forloops ---------------------------

            logger.LogInfo("Log initialized");

            // use File.ReadAllLines(path) to grab all the lines from your csv file
            // Log and error if you get 0 lines and a warning if you get 1 line
            string[] lines = File.ReadAllLines(csvPath);
            if (lines.Length == 0)
            {
                logger.LogError("No data found in the CSV file.");
                return;
            }
            else if (lines.Length == 1)
            {
                logger.LogWarning("Only one line of data found in the CSV file.");
            }

            logger.LogInfo($"Lines: {lines[0]}");

            // Create a new instance of your TacoParser class
            var parser = new TacoParser();

            // Grab an IEnumerable of locations using the Select command: var locations = lines.Select(parser.Parse);
            var locations = lines.Select(line => parser.Parse(line)).ToArray();

            // DON'T FORGET TO LOG YOUR STEPS

            // Now that your Parse method is completed, START BELOW ----------

            // TODO: Create two `ITrackable` variables with initial values of `null`. These will be used to store your two taco bells that are the farthest from each other.
            // Create a `double` variable to store the distance
            ITrackable maxLocation1 = null;
            ITrackable maxLocation2 = null;
            double maxDistance = 0;
            // Include the Geolocation toolbox, so you can compare locations: `using GeoCoordinatePortable;`

            //HINT NESTED LOOPS SECTION---------------------
            // Do a loop for your locations to grab each location as the origin (perhaps: `locA`)
            for (int i = 0; i < locations.Length; i++)
            {
                var locA = locations[i];
                var corA = new GeoCoordinate(locA.Location.Latitude, locA.Location.Longitude);

                // Now, do another loop on the locations with the scope of your first loop, so you can grab the "destination" location (perhaps: `locB`)
                for (int j = 0; j < locations.Length; j++)
                {
                    var locB = locations[j];
                    var corB = new GeoCoordinate(locB.Location.Latitude, locB.Location.Longitude);

                    // Calculate the distance between locA and locB
                    double distanceInMeters = corA.GetDistanceTo(corB);

                    // Update the furthest locations and distance if needed
                    if (distanceInMeters > maxDistance)
                    {
                        maxDistance = distanceInMeters;
                        maxLocation1 = locA;
                        maxLocation2 = locB;
                    }
                }
            }

            if (maxLocation1 != null && maxLocation2 != null)
            {

                double maxDistanceInMiles = maxDistance / 1609.34;
                logger.LogInfo($"The two Taco Bell locations furthest from each other are: ");
                logger.LogInfo($"Location 1: {maxLocation1.Name}");
                logger.LogInfo($"Location 2: {maxLocation2.Name}");
                logger.LogInfo($"Distance between them: {maxDistanceInMiles:F2} miles");
            }
            else
            {
                logger.LogWarning("Unable to determine the furthest Taco Bell locations.");
            }
        }
    }
}
