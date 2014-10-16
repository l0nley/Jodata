using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Jodata.JiraEntities;
using Jodata.Translator;
using Newtonsoft.Json;

namespace Jodata
{
  public class ServiceEntities : BaseEntity
  {
    public IQueryable<ProjectDescription> Projects
    {
      get
      {
        return (new ProjectDescription[0]).AsQueryable();
        var result = JiraHelper.RunQuery("project");
        return JsonConvert.DeserializeObject<List<ProjectDescription>>(result).AsQueryable();
      }
    }

    public IQueryable<Issue> Issues
    {
      get
      {
        return new JiraQueryProvider().CreateQuery<Issue>(Expression.Parameter(typeof(IQueryable<Issue>), "issue"));
      }
    } 
  }
}