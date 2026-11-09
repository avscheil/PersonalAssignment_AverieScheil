namespace PersonalAssignment_AverieScheil.Models
{
    // overlaying a grid on the map, collapsing every row into one cell
    // One Bin = one cell = one dot the browser eventually draws
    // sum/count over avg because you're averaging incrementally, and you don't know the total in advance
    public sealed class Bin
    {
        public double Lat {  get; init; }
        public double Lng { get; init; }

        private readonly Dictionary<string, (double Sum, int Count)> _acc = new();

        public void Accumulate(string metric, double value)
        {
            _acc.TryGetValue(metric, out var cur);
            _acc[metric] = (cur.Sum + value, cur.Count + 1);
        }

        public double? Mean (string metric) =>
            _acc.TryGetValue(metric, out var c) && c.Count > 0
            ? c.Sum / c.Count
            : null;

        public IEnumerable<string> MetricNames => _acc.Keys;
    }
}
