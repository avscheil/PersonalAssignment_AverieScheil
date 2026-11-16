using System.Collections.Concurrent;
using PersonalAssignment_AverieScheil.Models;

namespace PersonalAssignment_AverieScheil.Services
{
    public class DatasetStore
    {
        private readonly ConcurrentDictionary<Guid, Dataset> _data = new();
        public void Save(Dataset d) => _data[d.Id] = d;
        public Dataset? Get(Guid id, string ownerId) =>
            _data.TryGetValue(id, out var d) && d.OwnerId == ownerId ? d : null;

        public HeatPayload? GetHeat(Guid id, string ownerId, string metric)
        {
            var d = Get(id, ownerId);
            if(d is  null || !d.Metrics.Contains(metric)) return null;

            var points = new List<double[]>();
            var values = new List<double>();

            foreach(var b in d.Bins)
            {
                var m = b.Mean(metric);
                if(m is null) continue;
                points.Add(new[] { b.Lat, b.Lng, m.Value });
                values.Add(m.Value);
            }

            values.Sort();

            return new HeatPayload
            {
                Metric = metric,
                P5 = Percentile(values, 5),
                P95 = Percentile(values, 95),
                Points = points,
            };
        }

        public double[]? GetBounds(Guid id, string ownerId)
        {
            var d = Get(id, ownerId);
            if(d is null || d.Bins.Count == 0) return null;

            return new[]
            {
                d.Bins.Min(b => b.Lat), d.Bins.Min(b => b.Lng),
                d.Bins.Max(b => b.Lat), d.Bins.Max(b => b.Lng)
            };
        }

        private static double Percentile(List<double> sorted, double p)
        {
            if(sorted.Count == 0) return 0;
            if(sorted.Count == 1) return sorted[0];
            
            var rank = p / 100.0 * (sorted.Count - 1);
            var lo = (int)Math.Floor(rank);
            var hi = (int)Math.Ceiling(rank);

            return lo == hi? sorted[lo] : sorted[lo] + (rank - lo) * (sorted[hi] - sorted[lo]);
        }
    }
}
