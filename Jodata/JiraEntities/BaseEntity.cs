using Newtonsoft.Json;

namespace Jodata.JiraEntities
{
  public class BaseEntity
  {
    [JsonProperty("self")]
    public string Self { get; set; }

   [JsonProperty("key")]
    public string Key { get; set; }
  }
}