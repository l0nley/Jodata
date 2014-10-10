using System;
using System.Linq;
using System.Linq.Expressions;

namespace Jodata
{
  public class JiraQueryProvider : IQueryProvider
  {
    public IQueryable CreateQuery(Expression expression)
    {
      var elementType = TypeSystem.GetElementType(expression.Type);
      try
      {
        return (IQueryable) Activator.CreateInstance(typeof(JiraQuery<>).MakeGenericType(elementType), new object[] { this, expression });
      }
      catch (System.Reflection.TargetInvocationException tie)
      {
        throw tie.InnerException;
      }
    }

    public IQueryable<TResult> CreateQuery<TResult>(Expression expression)
    {
      return new JiraQuery<TResult>(this, expression);
    }

    public object Execute(Expression expression)
    {
      return QueryContext.Execute(expression, false);
    }

    // Queryable's "single value" standard query operators call this method.
    // It is also called from QueryableDataSet.GetEnumerator().
    public TResult Execute<TResult>(Expression expression)
    {
      var isEnumerable = typeof(TResult).Name == "IEnumerable`1";
      return (TResult)QueryContext.Execute(expression, isEnumerable);
    }
  }
}



}