namespace PersonalAssignment_AverieScheil.Models
{
    // result of one upload
    public sealed class Dataset
    {
        public Guid Id { get; init; }
        public string OwnerId { get; init; } = "";
        public string FileName { get; init; } = "";
        public DateTime CreatedUtc { get; init; }= DateTime.UtcNow;
        public List<string> Metrics { get; init; } = new();
        public List<Bin> Bins { get; init; } = new();
        public int RowsRead { get; init; }
        public int RowsSkipped { get; init; }
    }

    // json; one metric, ready to draw 
    public sealed class HeatPayload
    {
        public string Metric { get; init; } = "";
        public double P5 { get; init; }
        public double P95 { get; init; }
        public List<double[]> Points { get; init; } = new();
    }

    // metadata, no payload; first before heat data 
    public sealed class DatasetInfo
    {
        public Guid Id { get; init; }
        public List<string> Metrics { get; init; } = new();
        public int RowsRead { get; init; }
        public int RowsSkipped { get; init; }
        public double[]? Bounds { get; init; } // nesw; ? = checking before using 
    }
}
