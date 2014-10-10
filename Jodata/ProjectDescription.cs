using System.Data.Services.Common;
using System.Linq;
using Newtonsoft.Json;

namespace Jodata
{
  [DataServiceKey("Id")]
  public class ProjectDescription : BaseEntity
  {
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("key")]
    public string Key { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    public IQueryable<Issue> Issues
    {
      get
      {
        return new JiraQueryProvider(Key);
      }
    }
  }
}