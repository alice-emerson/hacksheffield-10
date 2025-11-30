using System;
using System.Collections.Generic;
using Grpc.Net.Compression;
using InfluxDB3.Client;
using InfluxDB3.Client.Config;
using InfluxDB3.Client.Write;

public class InfluxDB
{
    public InfluxDBClient dbClient;

    public const string databaseTitle = "Grass Touching";

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
    }
}