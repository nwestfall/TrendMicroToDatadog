using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TrendMicroToDatadog.Models
{
    public class DataDogEventModel
    {
        public const string PRIORITY_NORMAL = "normal";
        public const string PRIORITY_LOW = "low";
        public const string ALERT_TYPE_ERROR = "error";
        public const string ALERT_TYPE_WARNING = "warning";
        public const string ALERT_TYPE_INFO = "info";
        public const string ALERT_TYPE_SUCCESS = "success";

        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("text")]
        public string Text { get; set; }
        [JsonPropertyName("date_happened")]
        public long? DateHappened { get; set; }
        [JsonPropertyName("priority")]
        public string Priority { get; set; }
        [JsonPropertyName("host")]
        public string Host { get; set; }
        [JsonPropertyName("tags")]
        public IList<string> Tags { get; set; }
        [JsonPropertyName("alert_type")]
        public string AlertType { get; set; }
        [JsonPropertyName("aggregation_key")]
        public string AggregationKey { get; set; }
        [JsonPropertyName("source_type_name")]
        public string SourceTypeName { get; set; }
        [JsonPropertyName("related_event_id")]
        public int RelatedEventID { get; set; }
        [JsonPropertyName("device_name")]
        public string DeviceName { get; set; }
    }
}