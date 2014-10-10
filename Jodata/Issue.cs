using Newtonsoft.Json;

namespace Jodata
{
  /// <summary>
  /// A class representing a JIRA issue
  /// </summary>
  public class Issue : BaseEntity
  {
    private string _keyString;

    [JsonProperty("expand")]
    public string Expand { get; set; }

    [JsonProperty("id")]
    public int Id { get; set; }

    #region Special key solution
    [JsonProperty("key")]
    public string ProxyKey
    {
      get
      {
        return Key.ToString();
      }

      set
      {
        _keyString = value;
      }
    }

    [JsonIgnore]
    public IssueKey Key
    {
      get
      {
        return IssueKey.Parse(_keyString);
      }
    }
    #endregion Special key solution

    [JsonProperty("fields")]
    public Fields Fields { get; set; }
  }
}