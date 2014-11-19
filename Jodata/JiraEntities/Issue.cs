using System;
using System.Data.Services.Common;
using System.Runtime.Remoting.Messaging;
using Newtonsoft.Json;

namespace Jodata.JiraEntities
{
  [DataServiceKey("Id")]
  public class Issue : BaseEntity
  {

    [JsonProperty("expand")]
    public string Expand { get; set; }

    [JsonProperty("fields")]
    public Fields Fields { get; set; }


    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonIgnore]
    public int Progress
    {
      get
      {
        //var fields = JiraHelper.GetIssueFieldValues(Key);
        //return fields.ContainsKey("customfield_20500") ? Convert.ToInt32(fields["customfield_20500"]) : 1;
        return 1;
      }

      set
      {
      }
    }

    [JsonIgnore]
    public string WorklogQuery { get; set; }
  }
}