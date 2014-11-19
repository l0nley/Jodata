using System;
using System.Collections;
using Newtonsoft.Json;

namespace Jodata.JiraEntities
{
  public class Worklog
  {
    [JsonProperty("timeSpent")]
    public string TimeSpent { get; set; }

    [JsonProperty("timeSpentSeconds")]
    public int TimeSpentSeconds { get; set; }

    [JsonProperty("updateAuthor")]
    public UpdateAuthor UpdateAuthor { get; set; }

    [JsonProperty("started")]
    public DateTime Started { get; set; }
  }
}
