using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Jodata
{
  internal class QueryContext
  {
    // Executes the expression tree that is passed to it.
    internal static object Execute(Expression expression, bool isEnumerable)
    {
      // The expression must represent a query over the data source.
      if (!IsQueryOverDataSource(expression))
      {
        throw new InvalidProgramException("No query over the data source was specified.");
      }

      // Find the call to Where() and get the lambda expression predicate.
      InnermostWhereFinder whereFinder = new InnermostWhereFinder();
      MethodCallExpression whereExpression = whereFinder.GetInnermostWhere(expression);
      LambdaExpression lambdaExpression = (LambdaExpression)((UnaryExpression) (whereExpression.Arguments[1])).Operand;

      // Send the lambda expression through the partial evaluator.
      lambdaExpression = (LambdaExpression) Evaluator.PartialEval(lambdaExpression);

      UserFinder uf = new UserFinder(lambdaExpression.Body);
      List<string> names = uf.Usernames;
      if (names.Count == 0)
      {
        throw new InvalidQueryException("You must specify atleast one name for this query.");
      }

      IQueryable<UserSession> queryableSessions = sessions.AsQueryable<UserSession>();

      // Copy the expression tree that was passed in, changing only the first
      // argument of the innermost MethodCallExpression.

      ExpressionTreeModifier treeCopier = new ExpressionTreeModifier(queryableSessions);
      Expression newExpressionTree = treeCopier.Visit(expression);

      // This step creates an IQueryable that executes by replacing Queryable methods with Enumerable methods.
      return isEnumerable ? queryableSessions.Provider.CreateQuery(newExpressionTree) : queryableSessions.Provider.Execute(newExpressionTree);
    }

    private static bool IsQueryOverDataSource(Expression expression)
    {
      // If expression represents an unqueried IQueryable data source instance,
      // expression is of type ConstantExpression, not MethodCallExpression.
      return expression is MethodCallExpression;
    }
  }
}