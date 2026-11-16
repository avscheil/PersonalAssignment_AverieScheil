using PersonalAssignment_AverieScheil.Models;

namespace PersonalAssignment_AverieScheil.Services
{
    public sealed class BinAccumulator
    {
        private readonly double _cell;
        private readonly Dictionary<(long, long), Bin> _bins = new();

        public BinAccumulator(double cellSizeDegrees = 0.001) => _cell = cellSizeDegrees;

        public void Add(double lat, double lng, string metric, double value)
        {
            var ky = (long)Math.Floor(lat / _cell);
            var kx = (long)Math.Floor(lng / _cell);

            if(!_bins.TryGetValue((ky, kx), out var bin))
            {
                bin = new Bin
                {
                    Lat = (ky + 0.5) * _cell, // bin center
                    Lng = (kx + 0.5) * _cell,
                };
                _bins[(ky, kx)] = bin;
            }
            bin.Accumulate(metric, value);
        }
        public List<Bin> Build() => _bins.Values.ToList();
    }
}
