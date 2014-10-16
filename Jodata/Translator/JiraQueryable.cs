using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Jodata.Translator
{
  public class JiraQueryable<T> : IQueryable<T>
  {
    public JiraQueryable(IQueryProvider provider, Expression expression, string argument = null, string data = null)
    {
      Provider = provider;
      Expression = expression;
      Argument = argument;
      Data = data;
    }

    public Expression Expression { get; private set; }
    public string Argument { get; set; }
    public string Data { get; set; }

    public Type ElementType
    {
      get { return typeof(T); }
    }

    public IQueryProvider Provider { get; private set; }

    public IEnumerator<T> GetEnumerator()
    {
      return Provider.Execute<IEnumerable<T>>(Expression).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }
  }
}