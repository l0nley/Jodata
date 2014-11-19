using System.Collections.Generic;
using Newtonsoft.Json;

namespace Jodata.JiraEntities
{
  public class WorklogEntries
  {
    [JsonProperty("worklogs")]
    public IList<Worklog> Worklogs { get; set; }
  }
}