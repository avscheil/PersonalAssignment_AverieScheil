namespace PersonalAssignment_AverieScheil.Models
{
    // this reads the index of long / lat
    public sealed class HeaderMap
    {
        // init makes a property setter available only during object construction, enforcing immutability after initialization 
        public int LatIndex { get; init; }
        public int LngIndex { get; init; }
        public Dictionary<string, int> Metrics { get; init; } = new();
    }

    // this is a naming exception
    public sealed class CsvSchemaException : Exception
    {
        public CsvSchemaException(string message) : base(message) { }
    }
}
