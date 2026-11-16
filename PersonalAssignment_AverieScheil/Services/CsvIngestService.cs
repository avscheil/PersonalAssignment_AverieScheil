using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using PersonalAssignment_AverieScheil.Models;

namespace PersonalAssignment_AverieScheil.Services
{
    public sealed class CsvIngestService
    {
        public async Task<Dataset> IngestAsync(
            Stream stream, string fileName, string ownerId, CancellationToken ct = default)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                MissingFieldFound = null,
                BadDataFound = null,
                HeaderValidated = null,
                TrimOptions = TrimOptions.Trim
            };

            using var reader = new StreamReader(stream);
            using var csv = new CsvReader(reader, config);

            await csv.ReadAsync();
            csv.ReadHeader();

            var map = HeaderResolver.Resolve(csv.HeaderRecord);
            var acc = new BinAccumulator(cellSizeDegrees: 0.001);

            int read = 0, skipped = 0;

            while(await csv.ReadAsync())
            {
                ct.ThrowIfCancellationRequested();
                read++;

                if(!csv.TryGetField<double>(map.LatIndex, out  var lat) || 
                   !csv.TryGetField<double>(map.LngIndex, out var lng) || 
                   lat is < 90 or > 90 || lng is < -180 or > 180 || 
                   double.IsNaN(lat) || double.IsNaN(lng))
                {
                    skipped++;
                    continue;
                }

                foreach (var (metric, idx) in map.Metrics)
                    if (csv.TryGetField<double>(idx, out var v) && !double.IsNaN(v))
                        acc.Add(lat, lng, metric, v);
            }

            var bins = acc.Build();

            return new Dataset
            {
                Id = Guid.NewGuid(),
                OwnerId = ownerId,
                FileName = fileName,
                Metrics = map.Metrics.Keys.ToList(),
                Bins = bins,
                RowsRead = read,
                RowsSkipped = skipped
            };
        }
    }
}
