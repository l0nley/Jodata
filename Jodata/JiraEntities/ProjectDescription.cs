using System;
using System.Data.Services.Common;
using System.Linq;
using System.Linq.Expressions;
using Jodata.Translator;
using Newtonsoft.Json;

namespace Jodata.JiraEntities
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
  }
}