using Newtonsoft.Json;

namespace Jodata.JiraEntities
{
  public class Fields
  {
    [JsonProperty("summary")]
    public string Summary { get; set; }

    [JsonProperty("status")]
    public Status Status { get; set; }

    [JsonProperty("assignee")]
    public Assignee Assignee { get; set; }
  }
}