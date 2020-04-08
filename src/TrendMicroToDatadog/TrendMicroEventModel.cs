using System;
using System.Text;

namespace TrendMicroToDatadog.Models
{
    public class TrendMicroEventModel
    {
        public const string EVENT_TYPE_ANTIMALWARE = "AntiMalwareEvent";
        public const string EVENT_TYPE_SYSTEM = "SystemEvent";
        public const string EVENT_TYPE_TEST = "TestSNS";

        public int ATSEDetectionLevel { get; set; }
        public string ContainerID { get; set; }
        public DateTime CreationTime { get; set; }
        public int EngineType { get; set; }
        public string EngineVersion { get; set; }
        public int ErrorCode { get; set; }
        public string FileSHA1 { get; set; }
        public string HostAgentGUID { get; set; }
        public string HostAgentVersion { get; set; }
        public string HostCloudType { get; set; }
        public string HostGUID { get; set; }
        public int HostGroupID { get; set; }
        public string HostGroupName { get; set; }
        public string HostOwnerID { get; set; }
        public string HostInstanceID { get; set; }
        public string HostLastIPUsed { get; set; }
        public string HostOS { get; set; }
        public int HostSecurityPolicyID { get; set; }
        public string HostSecurityPolicyName { get; set; }
        public string HostName { get; set; }
        public string InfectedFilePath { get; set; }
        public int MajorVirusType { get; set; }
        public string MajorVirusTypeString { get; set; }
        public string MalwareName { get; set; }
        public int MalwareType { get; set; }
        public DateTime ModificationTime { get; set; }
        public int Origin { get; set; }
        public string OriginString { get; set; }
        public string PatternVersion { get; set; }
        public int Protocol { get; set; }
        public string Reason { get; set; }
        public int ScanAction1 { get; set; }
        public int ScaneAction2 { get; set; }
        public int ScanResultAction1 { get; set; }
        public int ScanResultAction2 { get; set; }
        public string ScanResultString { get; set; }
        public int ScanType { get; set; }
        public string ScanTypeString { get; set; }
        public string ActionBy { get; set; }
        public string Description { get; set; }
        public int EventID { get; set; }
        public string EventType { get; set; }
        public DateTime LogDate { get; set; }
        public int ManagerNodeID { get; set; }
        public string ManagerNodeName { get; set; }
        public int Number { get; set; }
        public int Severity { get; set; }
        public string SeverityString { get; set; }
        public string Tags { get; set; }
        public int TargetID { get; set; }
        public string TargetName { get; set; }
        public string TargetType { get; set; }
        public int TenantID { get; set; }
        public string TenantName { get; set; }
        public string Title { get; set; }

        public string ToMalwareSummary()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"ATSE Detection Level: {ATSEDetectionLevel}");
            sb.AppendLine($"Container ID: {ContainerID}");
            sb.AppendLine($"Engine Type: {EngineType}");
            sb.AppendLine($"Engine Version: {EngineVersion}");
            sb.AppendLine($"Host Agent Version: {HostAgentVersion}");
            sb.AppendLine($"Host Instance ID: {HostInstanceID}");
            sb.AppendLine($"Host Name: {HostName}");
            sb.AppendLine($"FileSHA1: {FileSHA1}");
            sb.AppendLine($"Infected File Path: {InfectedFilePath}");
            sb.AppendLine($"Major Virus Type: {MajorVirusTypeString}");
            sb.AppendLine($"Malware Name: {MalwareName}");
            sb.AppendLine($"Reason: {Reason}");
            sb.AppendLine($"Scan Type: {ScanTypeString}");
            sb.AppendLine($"Scan Result: {ScanResultString}");
            return sb.ToString();
        }
    }
}