using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.Net.Http;

using Amazon.Lambda.Core;
using Amazon.Lambda.SNSEvents;
using TrendMicroToDatadog.Models;
using System.Text;
using System.Net;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.LambdaJsonSerializer))]

namespace TrendMicroToDatadog
{
    public class Function
    {
        const string SOURCE_NAME = "trendmicro_deepsecurity";

        readonly string DATADOG_URL;
        readonly string DATADOG_API_KEY;
        readonly HttpClient _datadogClient;

        /// <summary>
        /// Default constructor. This constructor is used by Lambda to construct the instance. When invoked in a Lambda environment
        /// the AWS credentials will come from the IAM role associated with the function and the AWS region will be set to the
        /// region the Lambda function is executed in.
        /// </summary>
        public Function()
        {
            DATADOG_URL = Environment.GetEnvironmentVariable("DATADOG_URL");
            DATADOG_API_KEY = Environment.GetEnvironmentVariable("DATADOG_API_KEY");

            _datadogClient = new HttpClient();
        }


        /// <summary>
        /// This method is called for every Lambda invocation. This method takes in an SNS event object and can be used 
        /// to respond to SNS messages.
        /// </summary>
        /// <param name="evnt"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task FunctionHandler(SNSEvent evnt, ILambdaContext context)
        {
            if(string.IsNullOrEmpty(DATADOG_API_KEY))
            {
                context.Logger.LogLine("No DATADOG_API_KEY set");
                throw new ArgumentNullException(nameof(DATADOG_API_KEY));
            }
            if(string.IsNullOrEmpty(DATADOG_URL))
            {
                context.Logger.LogLine("No DDATADOG_URL set");
                throw new ArgumentNullException(nameof(DATADOG_URL));
            }

            context.Logger.LogLine("Procesing event records");
            foreach(var record in evnt.Records)
            {
                await ProcessRecordAsync(record, context);
            }
            context.Logger.LogLine("Done processing event records");
        }

        private async Task ProcessRecordAsync(SNSEvent.SNSRecord record, ILambdaContext context)
        {
            context.Logger.LogLine($"Processed record {record.Sns.Message}");

            TrendMicroEventModel[] trendMicroEvents = (TrendMicroEventModel[])JsonSerializer.Deserialize(record.Sns.Message, typeof(TrendMicroEventModel[]));
            foreach(var trendMicroEvent in trendMicroEvents)
            {
                DataDogEventModel datadogEvent = new DataDogEventModel();
                datadogEvent.SourceTypeName = SOURCE_NAME;
                // Determine Host
                if(!string.IsNullOrEmpty(trendMicroEvent.HostName))
                    datadogEvent.Host = trendMicroEvent.HostName;
                else if(!string.IsNullOrEmpty(trendMicroEvent.TargetName))
                    datadogEvent.Host = trendMicroEvent.TargetName;
                // Determine Title
                if(!string.IsNullOrEmpty(trendMicroEvent.Title))
                    datadogEvent.Title = trendMicroEvent.Title;
                else
                    datadogEvent.Title = trendMicroEvent.EventType;
                // Determine when
                datadogEvent.DateHappened = trendMicroEvent.LogDate.ToUniversalTime().ToUnixtime() / 1000;
                // Set tags
                if(!string.IsNullOrEmpty(trendMicroEvent.Tags))
                {
                    var tags = trendMicroEvent.Tags.Split(',');
                    datadogEvent.Tags = tags;
                }
                datadogEvent.Tags.Append(SOURCE_NAME);
                // Set aggregation
                datadogEvent.AggregationKey = record.Sns.MessageId;
                // Related Event
                datadogEvent.RelatedEventID = trendMicroEvent.EventID;
                switch(trendMicroEvent.EventType)
                {
                    case TrendMicroEventModel.EVENT_TYPE_ANTIMALWARE: //AntiMalwareEvent
                        datadogEvent.Priority = DataDogEventModel.PRIORITY_NORMAL;
                        datadogEvent.AlertType = DataDogEventModel.ALERT_TYPE_ERROR;
                        datadogEvent.Text = trendMicroEvent.ToMalwareSummary();
                        break;
                    default: // SystemEvent
                        datadogEvent.Priority = DataDogEventModel.PRIORITY_LOW;
                        datadogEvent.AlertType = DataDogEventModel.ALERT_TYPE_INFO;
                        datadogEvent.Text = $"{trendMicroEvent.Title} for {trendMicroEvent.TargetName}.\n\nManager Node Name: {trendMicroEvent.ManagerNodeName}\nAction By: {trendMicroEvent.ActionBy}\nOrigin: {trendMicroEvent.OriginString}";
                        break;
                }

                // Post Event
                var datadogEventString = JsonSerializer.Serialize(datadogEvent, typeof(DataDogEventModel), new JsonSerializerOptions() { IgnoreNullValues = true });
                context.Logger.LogLine($"Posting Event to Datadog - {datadogEventString}");
                var response = await _datadogClient.PostAsync($"{DATADOG_URL}/v1/events?api_key={DATADOG_API_KEY}", new StringContent(datadogEventString, Encoding.UTF8, "application/json"));
                if(response.StatusCode != HttpStatusCode.Accepted)
                {
                    context.Logger.LogLine($"[{response.StatusCode}] - {await response.Content.ReadAsStringAsync()}");
                    throw new InvalidOperationException("Unable to post event to Datadog");
                }
            }

            await Task.CompletedTask;
        }
    }
}
