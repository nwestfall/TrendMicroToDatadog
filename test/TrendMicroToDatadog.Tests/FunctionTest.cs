using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xunit;
using Amazon.Lambda.TestUtilities;
using Amazon.Lambda.SNSEvents;

using TrendMicroToDatadog;

namespace TrendMicroToDatadog.Tests
{
    public class FunctionTest
    {
        [Fact]
        public async Task TestSystemEvent()
        {
            var snsEvent = new SNSEvent
            {
                Records = new List<SNSEvent.SNSRecord>
                {
                    new SNSEvent.SNSRecord
                    {
                        Sns = new SNSEvent.SNSMessage()
                        {
                            Message = "[{\"ActionBy\":\"System\",\"Description\":\"Description Omitted\",\"EventID\":13904,\"EventType\":\"SystemEvent\",\"LogDate\":\"2020-04-08T18:09:50.669Z\",\"ManagerNodeID\":63577,\"ManagerNodeName\":\"job2-798\",\"Number\":250,\"Origin\":3,\"OriginString\":\"Manager\",\"Severity\":1,\"SeverityString\":\"Info\",\"Tags\":\"\",\"TargetID\":294,\"TargetName\":\"ec2-ip.compute-1.amazonaws.com (ECS Instance - EC2ContainerService-Name) [i-number]\",\"TargetType\":\"Host\",\"TenantID\":1,\"TenantName\":\"Company\",\"Title\":\"Computer Created\"},{\"ActionBy\":\"System\",\"Description\":\"Description Omitted\",\"EventID\":13905,\"EventType\":\"SystemEvent\",\"LogDate\":\"2020-04-08T18:10:19.295Z\",\"ManagerNodeID\":63577,\"ManagerNodeName\":\"job2-798\",\"Number\":1909,\"Origin\":3,\"OriginString\":\"Manager\",\"Severity\":1,\"SeverityString\":\"Info\",\"Tags\":\"\",\"TargetID\":1,\"TargetName\":\"company - 1\",\"TargetType\":\"AmazonAccount\",\"TenantID\":1,\"TenantName\":\"Company\",\"Title\":\"AWS Account Synchronization Finished\"}]"
                        }
                    }
                }
            };

            var logger = new TestLambdaLogger();
            var context = new TestLambdaContext
            {
                Logger = logger
            };

            var function = new Function();
            await function.FunctionHandler(snsEvent, context);

            Assert.Contains("Done processing event records", logger.Buffer.ToString());
        }

        [Fact]
        public async Task TestMultipleSystemEvent()
        {
            var snsEvent = new SNSEvent
            {
                Records = new List<SNSEvent.SNSRecord>
                {
                    new SNSEvent.SNSRecord
                    {
                        Sns = new SNSEvent.SNSMessage()
                        {
                            Message = "[{\"ActionBy\":\"System\",\"Description\":\"Description Omitted\",\"EventID\":13924,\"EventType\":\"SystemEvent\",\"LogDate\":\"2020-04-08T19:06:59.198Z\",\"ManagerNodeID\":63580,\"ManagerNodeName\":\"job4-798\",\"Number\":250,\"Origin\":3,\"OriginString\":\"Manager\",\"Severity\":1,\"SeverityString\":\"Info\",\"Tags\":\"\",\"TargetID\":298,\"TargetName\":\"ec2-ip.compute-1.amazonaws.com (ECS Instance - EC2ContainerService-Name) [i-number]\",\"TargetType\":\"Host\",\"TenantID\":1,\"TenantName\":\"Company\",\"Title\":\"Computer Created\"},{\"ActionBy\":\"System\",\"Description\":\"Description Omitted\",\"EventID\":13925,\"EventType\":\"SystemEvent\",\"LogDate\":\"2020-04-08T19:06:59.231Z\",\"ManagerNodeID\":63580,\"ManagerNodeName\":\"job4-798\",\"Number\":251,\"Origin\":3,\"OriginString\":\"Manager\",\"Severity\":1,\"SeverityString\":\"Info\",\"Tags\":\"\",\"TargetID\":1,\"TargetName\":\"ec2-ip.compute-1.amazonaws.com (ECS Instance - EC2ContainerService-Name) [i-number]\",\"TargetType\":\"Host\",\"TenantID\":1,\"TenantName\":\"Company\",\"Title\":\"Computer Deleted\"}]"
                        }
                    }
                }
            };

            var logger = new TestLambdaLogger();
            var context = new TestLambdaContext
            {
                Logger = logger
            };

            var function = new Function();
            await function.FunctionHandler(snsEvent, context);

            Assert.Contains("Done processing event records", logger.Buffer.ToString());
        }

        [Fact]
        public async Task TestAntiMalwareEvent()
        {
            var snsEvent = new SNSEvent
            {
                Records = new List<SNSEvent.SNSRecord>
                {
                    new SNSEvent.SNSRecord
                    {
                        Sns = new SNSEvent.SNSMessage()
                        {
                            Message = "[{\"ATSEDetectionLevel\":0,\"ContainerID\":\"\",\"CreationTime\":\"2020-04-08T18:12:07.000Z\",\"EngineType\":1207959846,\"EngineVersion\":\"11.0.0.1006\",\"ErrorCode\":0,\"EventID\":34,\"EventType\":\"AntiMalwareEvent\",\"FileSHA1\":\"CC06E1863B91CB51482381B478E617F9B09A9941\",\"HostAgentGUID\":\"C338C05E-D1DD-7224-023C-E8DB2CB81DEF\",\"HostAgentVersion\":\"12.0.0.967\",\"HostCloudType\":\"amazon\",\"HostGUID\":\"CEF681E1-8445-D382-AD2A-BBEBF6B7E4F4\",\"HostGroupID\":101,\"HostGroupName\":\"name (d-number)\",\"HostID\":135,\"HostInstanceID\":\"ws-wrkpdg0tf\",\"HostLastIPUsed\":\"127.0.0.1\",\"HostOS\":\"Microsoft Windows Server 2016 (64 bit)  Build 14393\",\"HostOwnerID\":\"1\",\"HostSecurityPolicyID\":1,\"HostSecurityPolicyName\":\"Base Policy\",\"Hostname\":\"127.0.0.1 (EC2AMAZ-id) [ws-wrkpdg0tf]\",\"InfectedFilePath\":\"d:\\\\temp2\\\\file.zip(file.aspx)\",\"LogDate\":\"2020-04-08T18:12:11.000Z\",\"MajorVirusType\":1,\"MajorVirusTypeString\":\"Trojan\",\"MalwareName\":\"HTML_ACE.WTS\",\"MalwareType\":1,\"ModificationTime\":\"2020-04-08T18:11:26.000Z\",\"Origin\":0,\"OriginString\":\"Agent\",\"PatternVersion\":\"15.791.00\",\"Protocol\":0,\"Reason\":\"Default Real-Time Scan Configuration\",\"ScanAction1\":2,\"ScanAction2\":0,\"ScanResultAction1\":0,\"ScanResultAction2\":0,\"ScanResultString\":\"Deleted\",\"ScanType\":0,\"ScanTypeString\":\"Real Time\",\"Tags\":\"\",\"TenantID\":1,\"TenantName\":\"Company\"}]"
                        }
                    }
                }
            };

            var logger = new TestLambdaLogger();
            var context = new TestLambdaContext
            {
                Logger = logger
            };

            var function = new Function();
            await function.FunctionHandler(snsEvent, context);

            Assert.Contains("Done processing event records", logger.Buffer.ToString());
        }
    }
}
