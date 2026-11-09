using System.Text.RegularExpressions;
using PersonalAssignment_AverieScheil.Models;

namespace PersonalAssignment_AverieScheil.Services
{
    public class HeaderResolver
    {
        // this class is to recognize column names in an uploaded CSV file
        public static readonly string[] MetricKeys =
            {"water_stress", "leaf_density", "nitrogen_content"};

        private static readonly Dictionary<string, string[]> Synonyms = new()
        {
            ["latitude"] = new[] { "lat", "latitude", "ycoord", "y" },
            ["longitude"] = new[] { "lon", "lng", "long", "longitude", "xcoord", "x" },
            ["water_stress"] = new[] { "waterstress", "wstress", "cwsi", "waterstressindex" },
            ["leaf_density"] = new[] { "leafdensity", "lai", "leafareaindex" },
            ["nitrogen_content"] = new[] { "nitrogen", "nitrogencontent", "npct", "nitrogenppm" },
        };

        public static string Canon(string? header) =>
            Regex.Replace(header?? "", "[^a-zA-Z0-9]", "").ToLowerInvariant();

        public static HeaderMap Resolve(string[]? headers)
        {
            if (headers is null || headers.Length == 0)
                throw new CsvSchemaException("The file has no header row.");

            var canon = headers.Select(Canon).ToArray();

            int Find(string key)
            {
                foreach(var alias in Synonyms[key])
                {
                    var i = Array.IndexOf(canon, alias);
                    if (i >= 0) return i;
                }
                return -1;
            }

            var lat = Find("latitude");
            var lng = Find("longitude");

            if (lat < 0 || lng < 0)
                throw new CsvSchemaException(
                    $"Could not find latitude/longituded columns. Found: {string.Join(",", headers)}");

            var metrics = new Dictionary<string, int>();

            foreach(var key in MetricKeys)
            {
                var i = Find(key);
                if (i >= 0) metrics[key] = i;
            }

            if(metrics.Count == 0)
                throw new CsvSchemaException(
                    $"No recognized metric columns. Found: {string.Join(", ", headers)}");

            return new HeaderMap { LatIndex = lat, LngIndex = lng, Metrics = metrics };
        }        
    }
}
