using System.Data.Services;
using System.Data.Services.Common;
using System.ServiceModel;

namespace Jodata
{
  [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
  public class JiraAccessService : DataService<ServiceEntities>
  {
    public static void InitializeService(DataServiceConfiguration config)
    {
      config.SetEntitySetAccessRule("*", EntitySetRights.AllRead); 
      config.SetServiceOperationAccessRule("*", ServiceOperationRights.AllRead);
      config.DataServiceBehavior.MaxProtocolVersion = DataServiceProtocolVersion.V3;
      config.UseVerboseErrors = true;
    }
  }
}