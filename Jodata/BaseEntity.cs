using System;
using System.Data.Services;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace Jodata
{
  [IgnoreProperties("Username", "Password", "BaseUrl")]
  public class BaseEntity
  {
    public BaseEntity()
    {
      Username = "Uladzimir_Harabtsou";
      Password = "";
      BaseUrl = "https://jira.epam.com/jira/rest/api/latest/";
    }

    [JsonProperty("self")]
    public string Self { get; set; }

    [JsonIgnore]
    public string Username { get; private set; }
    [JsonIgnore]
    public string Password { get; private set; }
    [JsonIgnore]
    public string BaseUrl { get; private set; }


    protected string RunQuery(
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

    private string GetEncodedCredentials()
    {
      var mergedCredentials = string.Format("{0}:{1}", Username, Password);
      var byteCredentials = Encoding.UTF8.GetBytes(mergedCredentials);
      return Convert.ToBase64String(byteCredentials);
    }
  }

  
}