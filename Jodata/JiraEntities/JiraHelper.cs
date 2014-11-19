using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Jodata.JiraEntities
{
  public static class JiraHelper
  {
    static JiraHelper()
    {
      Username = "";
      Password = "";
      BaseUrl = "https://jira.epam.com/jira/rest/api/latest/";
    }

    public static string Username { get; private set; }
    public static string Password { get; private set; }
    public static string BaseUrl { get; private set; }

    public static string RunQuery(
      string resource = null,
      string argument = null,
      string data = null,
      string method = "GET")
    {
      var url = string.Format("{0}{1}/", BaseUrl, resource);

      if (argument != null)
      {
        url = string.Format("{0}{1}/", url, argument);
      }

      var request = (HttpWebRequest)WebRequest.Create(url);
      request.ContentType = "application/json";
      request.Method = method;

      if (data != null)
      {
        using (var writer = new StreamWriter(request.GetRequestStream()))
        {
          writer.Write(data);
        }
      }

      var base64Credentials = GetEncodedCredentials();
      request.Headers.Add("Authorization", "Basic " + base64Credentials);
      string result;

      using (var response = (HttpWebResponse)request.GetResponse())
      {
        using (var stream = response.GetResponseStream())
        {
          if (stream == null)
          {
            throw new HttpListenerException(502, "Gateway timeout");
          }

          using (var reader = new StreamReader(stream))
          {
            result = reader.ReadToEnd();
          }
        }
      }

      return result;
    }

    public static List<Issue> GetIssues(
      string jql,
      List<string> fields = null,
      int startAt = 0,
      int maxResult = 50)
    {
      fields = fields ?? new List<string> { "summary", "status", "assignee", "labels", "issuelinks", "parent", "issuetype", "created", "updated", "resolutiondate", "changelog" };

      var request = new SearchRequest
      {
        Fields = fields,
        JQL = jql,
        MaxResults = maxResult,
        StartAt = startAt
      };

      var data = JsonConvert.SerializeObject(request);
      var result = RunQuery("search", data: data, method: "POST");

      var response = JsonConvert.DeserializeObject<SearchResponse>(result);

      return response.IssueDescriptions;
    }

    public static Dictionary<string, object> GetIssueFieldValues(string issueId)
    {
      var request = (HttpWebRequest) WebRequest.Create("https://jira.epam.com/jira/rest/api/latest/issue/" + issueId);
      request.ContentType = "application/json";
      request.Method = "GET";

      var base64Credentials = GetEncodedCredentials();
      request.Headers.Add("Authorization", "Basic " + base64Credentials);

      using (var response = (HttpWebResponse)request.GetResponse())
      {
        using (var stream = response.GetResponseStream())
        {
          if (stream == null)
          {
            throw new HttpListenerException(502, "Gateway timeout");
          }

          using (var reader = new StreamReader(stream))
          {
            var result = reader.ReadToEnd();
            var obj = JObject.Parse(result);
            var fields = (JObject) obj["fields"];
            var props = fields.Properties();
           return props.ToDictionary(l => l.Name, l => fields[l.Name].Value<object>());
          }
        }
      }
    }

    private static string GetEncodedCredentials()
    {
      var mergedCredentials = string.Format("{0}:{1}", Username, Password);
      var byteCredentials = Encoding.UTF8.GetBytes(mergedCredentials);
      return Convert.ToBase64String(byteCredentials);
    }
  }
}