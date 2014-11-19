using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

      var expressionString = expression.ToString();
      string jql;
      if (expressionString.Contains("WorklogQuery"))
      {
        var query = ParseNode(expression, string.Empty);
        var wkl = query.Split('\"').Skip(1).Take(1).First();
        var @params = wkl.Split(';').ToList();
        var dateStart = @params[0];
        var dateEnd = @params[1];
        var userName = @params[2];
        jql = string.Format("key in workedIssues(\"{0}\",\"{1}\",\"{2}\")", dateStart, dateEnd, userName);
        return JiraHelper.GetIssues(
          jql,
          new List<string>
          {
            "worklog",
            "issuetype",
            "parent",
             "assignee", 
             "labels", 
             "issuelinks",
             "summary",
          });
      }
      
      jql = ParseNode(expression);
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
        case ExpressionType.Conditional:
          return ParseConditional((ConditionalExpression) node, typeContext);
        case ExpressionType.And:
        case ExpressionType.AndAlso:
          return ParseAnd((BinaryExpression) node, typeContext);
        case ExpressionType.Or:
        case ExpressionType.OrElse:
          return ParseOr((BinaryExpression) node, typeContext);
        case ExpressionType.Convert:
          return ParseConvert((UnaryExpression) node, typeContext);

      }

      return string.Empty;
    }

    private static string ParseConvert(UnaryExpression node, string typeContext)
    {
      if (node.Operand.Type == typeof (DateTime))
      {
        var date = (DateTime)((ConstantExpression) node.Operand).Value;
        return "\"" + date.ToString("yyyy/MM/dd HH:mm") + "\"";
      }

      return string.Empty;
    }

    private static string ParseOr(BinaryExpression node, string typeContext)
    {
      var nodeLeft = ParseNode(node.Left, typeContext);
      var nodeRigt = ParseNode(node.Right, typeContext);
      var nodeLeftEmpty = string.IsNullOrEmpty(nodeLeft);
      var nodeRightEmpty = string.IsNullOrEmpty(nodeRigt);
      if (nodeLeftEmpty && nodeRightEmpty == false)
      {
        return nodeRigt;
      }

      if (nodeRightEmpty && nodeLeftEmpty == false)
      {
        return nodeLeft;
      }

      if (nodeLeftEmpty)
      {
        return string.Empty;
      }

      return "((" + nodeLeft + ") OR (" + nodeRigt + "))";
    }

    private static string ParseAnd(BinaryExpression node, string typeContext = "")
    {
      var nodeLeft = ParseNode(node.Left, typeContext);
      var nodeRigt = ParseNode(node.Right, typeContext);
      var nodeLeftEmpty = string.IsNullOrEmpty(nodeLeft);
      var nodeRightEmpty = string.IsNullOrEmpty(nodeRigt);
      if (nodeLeftEmpty && nodeRightEmpty == false)
      {
        return nodeRigt;
      }

      if (nodeRightEmpty && nodeLeftEmpty == false)
      {
        return nodeLeft;
      }

      if (nodeLeftEmpty)
      {
        return string.Empty;
      }

      return "(" + nodeLeft + ") AND (" + nodeRigt + ")";
    }

    private static string ParseConditional(ConditionalExpression node, string typeContext = "")
    {
      var iftrue = ParseNode(node.IfTrue);
      var iffalse = ParseNode(node.IfFalse);
      // NOTE Specific
      return iftrue == string.Empty ? iffalse : iftrue;
    }

    private static string ParseConstant(ConstantExpression node, string typeContext = "")
    {
      if (node.Value == null)
      {
        // NOTE Special null parsing;
        return string.Empty;
      }
      return typeContext + string.Format("\"{0}\"", node.Value);
    }

    private static string ParseMember(MemberExpression node, string typeContext = "")
    {
      if (node.Member.Name == "LabelsRaw")
      {
        // NOTE Special handling;
        return "labels";
      }

      if (node.Expression.NodeType == ExpressionType.Parameter)
      {
        return typeContext + node.Member.Name;
      }

      if (node.Expression.Type == typeof (IssueType))
      {
        return "type";
      }

      if (node.Expression.Type == typeof(Assignee) && node.Member.Name == "Name")
      {
        return "assignee";
      }

      if (node.Expression.Type == typeof (Status))
      {
        return "status";
      }

      if (node.Member.Name == "Resolved")
      {
        return "resolved";
      }

      if (node.Member.Name == "Progress")
      {
        return "cf[20500]";
      }

      return "UNKNOWN";
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
        case "Select":
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