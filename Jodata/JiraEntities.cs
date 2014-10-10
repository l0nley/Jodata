using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Newtonsoft.Json;

namespace Jodata
{
  public class JiraEntities : BaseEntity
  {
    public IQueryable<ProjectDescription> Projects
    {
      get
      {
        var result = RunQuery("project");
        return JsonConvert.DeserializeObject<List<ProjectDescription>>(result).AsQueryable();
      }
    }
  }
}