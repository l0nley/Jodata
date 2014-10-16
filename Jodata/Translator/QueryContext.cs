using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Jodata.JiraEntities;

namespace Jodata.Translator
{
  internal class QueryContext
  {
    // Executes the expression tree that is passed to it.
    internal static object Execute(Expression expression, bool isEnumerable)
    {
      while (expression.CanReduce)
      {
        expression = expression.Reduce();
      }

      var jql = ParseNode(expression);

      return ParseAndExecute(jql);
    }

    private static string ParseNode(Expression node, string typeContext = "")
    {
      switch (node.NodeType)
      {
        case ExpressionType.Call:
          return ParseMethodCall((MethodCallExpression) node, typeContext);
        case ExpressionType.Quote:
          return ParseQuoteExpression((UnaryExpression) node, typeContext);
        case ExpressionType.Lambda:
          return ParseLambda((LambdaExpression) node, typeContext);
        case ExpressionType.Equal:
        case ExpressionType.NotEqual:
        case ExpressionType.GreaterThan:
        case ExpressionType.LessThan:
        case ExpressionType.GreaterThanOrEqual:
        case ExpressionType.LessThanOrEqual:
          return ParseEqual((BinaryExpression) node, typeContext);
        case ExpressionType.MemberAccess:
          return ParseMember((MemberExpression) node, typeContext);
        case ExpressionType.Constant:
          return ParseConstant((ConstantExpression) node, typeContext);
      }

      return string.Empty;
    }

    private static string ParseConstant(ConstantExpression node, string typeContext = "")
    {
      return typeContext + string.Format("\"{0}\"", node.Value);
    }

    private static string ParseMember(MemberExpression node, string typeContext = "")
    {
      return typeContext+node.Member.Name;
    }

    private static string ParseEqual(BinaryExpression node, string typeContext = "")
    {
      var s = string.Empty;
      switch (node.NodeType)
      {
        case ExpressionType.Equal:
          s = "=";
          break;
        case ExpressionType.NotEqual:
          s = "!=";
          break;
        case ExpressionType.GreaterThan:
          s = ">";
          break;
        case ExpressionType.LessThan:
          s = "<";
          break;
        case ExpressionType.GreaterThanOrEqual:
          s = ">=";
          break;
        case ExpressionType.LessThanOrEqual:
          s = "<=";
          break;
      }

      return ParseNode(node.Left, typeContext) + s + ParseNode(node.Right, typeContext);
    }

    private static string ParseLambda(LambdaExpression lambda, string typeContext = "")
    {
      return "(" + ParseNode(lambda.Body, typeContext) + ")";
    }

    private static string ParseQuoteExpression(UnaryExpression unaryExpression, string typeContext = "")
    {
      return "(" + ParseNode(unaryExpression.Operand, typeContext) + ")";
    }

    private static string ParseMethodCall(MethodCallExpression expression, string typeContext = "")
    {
      switch (expression.Method.Name)
      {
        case "Where":
          // first expression for where is param, witch is typeContext
          // second is Quote
          return "(" + ParseNode(expression.Arguments[1], typeContext) + ")";
      }

      throw new NotImplementedException();
    }

    private static object ParseAndExecute(string query)
    {
      return JiraHelper.GetIssues(query);
    }
  }
}