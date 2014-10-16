using System.Collections.Generic;
using Newtonsoft.Json;

namespace Jodata.JiraEntities
{
  /// <summary>
  /// A class representing a JIRA REST search request
  /// </summary>
  public class SearchRequest
  {
    public SearchRequest()
    {
      Fields = new List<string>();
    }

    [JsonProperty("jql")]
    public string JQL { get; set; }

    [JsonProperty("startAt")]
    public int StartAt { get; set; }

    [JsonProperty("maxResults")]
    public int MaxResults { get; set; }

    [JsonProperty("fields")]
    public List<string> Fields { get; set; }
  }
}