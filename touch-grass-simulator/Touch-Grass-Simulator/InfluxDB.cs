using System;
using System.Collections.Generic;
using Grpc.Net.Compression;
using InfluxDB3.Client;
using InfluxDB3.Client.Config;
using InfluxDB3.Client.Write;

public class InfluxDB
{
    private InfluxDBClient dbClient;

    public InfluxDB()
    {
        ClientConfig config = new ClientConfig
        {
            Host = "https://eu-central-1-1.aws.cloud2.influxdata.com",
            Token = Environment.GetEnvironmentVariable("INFLUXDB_TOKEN"),
            Database = "Grass Touching",
            AllowHttpRedirects = true,
            DisableServerCertificateValidation = true,
            WriteOptions = new WriteOptions
            {
                Precision = WritePrecision.S,
                GzipThreshold = 4096,
                NoSync = false
            },
            QueryOptions = new QueryOptions
            {
                Deadline = DateTime.UtcNow.AddSeconds(10),
                MaxReceiveMessageSize = 4096,
                MaxSendMessageSize = 4096,
            }
        };

        dbClient = new InfluxDBClient(config);

        const string database = "Grass Touching";

        var points = new[]
        {
            PointData.Measurement("census")
                .SetTag("location", "Klamath")
                .SetField("bees", 23),
            PointData.Measurement("census")
                .SetTag("location", "Portland")
                .SetField("ants", 30),
            PointData.Measurement("census")
                .SetTag("location", "Klamath")
                .SetField("bees", 28),
            PointData.Measurement("census")
                .SetTag("location", "Portland")
                .SetField("ants", 32),
            PointData.Measurement("census")
                .SetTag("location", "Klamath")
                .SetField("bees", 29),
            PointData.Measurement("census")
                .SetTag("location", "Portland")
                .SetField("ants", 40)
        };

        foreach (var point in points)
        {
            dbClient.WritePointAsync(point: point, database: database);
        }
    }
}