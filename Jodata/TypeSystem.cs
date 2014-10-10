using System;
using System.Collections.Generic;
using System.Linq;

namespace Jodata
{
  internal static class TypeSystem
  {
    internal static Type GetElementType(Type seqType)
    {
      var ienum = FindIEnumerable(seqType);
      return ienum == null ? seqType : ienum.GetGenericArguments()[0];
    }

    private static Type FindIEnumerable(Type seqType)
    {
      while (true)
      {
        if (seqType == null || seqType == typeof(string))
        {
          return null;
        }

        if (seqType.IsArray)
        {
          return typeof(IEnumerable<>).MakeGenericType(seqType.GetElementType());
        }

        if (seqType.IsGenericType)
        {
          var type = seqType;
          foreach (var ienum in seqType.GetGenericArguments().Select(arg => typeof(IEnumerable<>).MakeGenericType(arg)).Where(ienum => ienum.IsAssignableFrom(type)))
          {
            return ienum;
          }
        }

        var ifaces = seqType.GetInterfaces();

        if (ifaces.Length > 0)
        {
          foreach (var ienum in ifaces.Select(FindIEnumerable).Where(ienum => ienum != null))
          {
            return ienum;
          }
        }

        if (seqType.BaseType == null || seqType.BaseType == typeof(object))
        {
          return null;
        }

        seqType = seqType.BaseType;
      }
    }
  }
}