using Newtonsoft.Json;

namespace Jodata.JiraEntities
{
  public class UpdateAuthor
  {
    [JsonProperty("name")]
    public string Name { get; set; }
  }
}