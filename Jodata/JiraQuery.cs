using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Jodata
{
  public class JiraQuery<T> : BaseEntity, IQueryable<T>
  {
    public JiraQuery(IQueryProvider provider, Expression expression, string argument = null, string data = null)
    {
      Provider = provider;
      Expression = expression;
      Argument = argument;
      Data = data;
    }

    public IEnumerator<T> GetEnumerator()
    {
      return Provider.Execute<IEnumerable<T>>(Expression).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    public Expression Expression { get; private set; }
    public string Argument { get; set; }
    public string Data { get; set; }

    public Type ElementType
    {
      get { return typeof(T); }
    }

    public IQueryProvider Provider { get; private set; }
  }


}