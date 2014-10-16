using System.Data.Services.Common;
using Newtonsoft.Json;

namespace Jodata.JiraEntities
{
  [DataServiceKey("Id")]
  public class Issue : BaseEntity
  {
    [JsonProperty("expand")]
    public string Expand { get; set; }

    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("key")]
    public string Key { get; set; }

    [JsonProperty("fields")]
    public Fields Fields { get; set; }
  }
}