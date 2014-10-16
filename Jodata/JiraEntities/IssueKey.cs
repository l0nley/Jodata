using System;

namespace Jodata.JiraEntities
{
  /// <summary>
  /// A class representing a JIRA issue key [PROJECT KEY]-[ISSUE ID]
  /// </summary>
  public class IssueKey
  {
    public IssueKey()
    {
    }

    public IssueKey(string projectKey, int issueId)
    {
       ProjectKey = projectKey;
       IssueId = issueId;
    }

    public string ProjectKey { get; set; }

    public int IssueId { get; set; }

    public static IssueKey Parse(string issueKeyString)
    {
      if (issueKeyString == null)
      {
        throw new ArgumentNullException("issueKeyString");
      }

      var split = issueKeyString.Split('-');

      if (split.Length != 2)
      {
        throw new ArgumentException("The string entered is not a JIRA key!");
      }

      int issueId;
      if (!int.TryParse(split[1], out issueId))
      {
        throw new ArgumentException("The string entered could not be parsed, issue id is non-integer!");
      }

      return new IssueKey(split[0], issueId);
    }

    public override string ToString()
    {
      return string.Format("{0}-{1}",  ProjectKey,  IssueId);
    }
  }
}