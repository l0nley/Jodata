using System.Collections.Generic;
using Newtonsoft.Json;

namespace Jodata.JiraEntities
{
  /// <summary>
  /// A class representing a JIRA REST search response
  /// </summary>
  public class SearchResponse
  {
    [JsonProperty("expand")]
    public string Expand { get; set; }

    [JsonProperty("startAt")]
    public int StartAt { get; set; }

    [JsonProperty("maxResults")]
    public int MaxResults { get; set; }

    [JsonProperty("total")]
    public int Total { get; set; }

    [JsonProperty("issues")]
    public List<Issue> IssueDescriptions { get; set; }
  }
}