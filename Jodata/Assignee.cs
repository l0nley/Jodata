using Newtonsoft.Json;

namespace Jodata
{
  public class Assignee : BaseEntity
  {
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("emailAddress")]
    public string EmailAddress { get; set; }

    [JsonProperty("displayName")]
    public string DisplayName { get; set; }

    [JsonProperty("active")]
    public bool Active { get; set; }
  }
}