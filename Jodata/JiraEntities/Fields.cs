using System.Collections.Generic;
using System.Reflection.Emit;
using Newtonsoft.Json;

namespace Jodata.JiraEntities
{
  public class Fields
  {
    [JsonProperty("summary")]
    public string Summary { get; set; }

    [JsonProperty("status")]
    public Status Status { get; set; }

    [JsonProperty("issuetype")]
    public IssueType IssueType { get; set; }

    [JsonProperty("assignee")]
    public Assignee Assignee { get; set; }

    [JsonProperty("timetracking")]
    public string TimeTracking { get; set; }

    [JsonProperty("labels")]
    public IList<string> Labels { get; set; }

    [JsonIgnore]
    public string LabelsRaw
    {
      get { return string.Join(",", Labels); }
    }
  }

  public class IssueType
  {
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("id")]
    public int Id { get; set; }
  }
}