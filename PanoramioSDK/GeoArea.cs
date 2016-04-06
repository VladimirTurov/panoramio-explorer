using System;

namespace PanoramioSDK
{
    public class GeoArea
    {
        public GeoArea(double minimalLongitude,
                       double minimalLatitude,
                       double maximalLongitude,
                       double maximalLatitude)
        {
            if (minimalLongitude > maximalLongitude)
                throw new ArgumentException("Wrong longitude range specified: maximal is less than minimal");
            if (minimalLatitude > maximalLatitude)
                throw new ArgumentException("Wrong latitude range specified: maximal is less than minimal");

            MinimalLongitude = minimalLongitude;
            MinimalLatitude = minimalLatitude;
            MaximalLongitude = maximalLongitude;
            MaximalLatitude = maximalLatitude;
        }

        public double MinimalLongitude { get; private set; }
        public double MinimalLatitude { get; private set; }
        public double MaximalLongitude { get; private set; }
        public double MaximalLatitude { get; private set; }
    }
}