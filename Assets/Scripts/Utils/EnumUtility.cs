using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
public static class EnumUtility
{
  public static List<object> GetOrderedValues(Type enumType)
  {
    if (!enumType.IsEnum)
      throw new ArgumentException("Type must be an Enum", nameof(enumType));
    return enumType
        .GetFields(BindingFlags.Public | BindingFlags.Static)
        .OrderBy(field => field.MetadataToken)
        .Select(field => field.GetValue(null))
        .ToList();
  }
}