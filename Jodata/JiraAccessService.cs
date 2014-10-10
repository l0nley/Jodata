using System.Data.Services;

namespace Jodata
{
  public class JiraAccessService : DataService<JiraEntities>
  {
    public static void InitializeService(IDataServiceConfiguration config)
    {
      config.SetEntitySetAccessRule("*", EntitySetRights.AllRead); 
    }
  }
}