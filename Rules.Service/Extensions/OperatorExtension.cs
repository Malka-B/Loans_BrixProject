using System;
using System.Linq.Expressions;

namespace Rules.Service.Extensions
{
    public static class Extension
    {
        public static BinaryExpression Operator(this string logic, ParameterExpression param, ConstantExpression valueToCompare)
        {
            switch (logic)
            {
                case ">": return Expression.GreaterThan(param, valueToCompare);
                case "<": return Expression.LessThan(param, valueToCompare);
                case "==": return Expression.Equal(param, valueToCompare);
                case ">=": return Expression.GreaterThanOrEqual(param, valueToCompare);
                case "<=": return Expression.LessThanOrEqual(param, valueToCompare);
                case "!=": return Expression.NotEqual(param, valueToCompare);
                default: throw new Exception("invalid logic");
            }
        }
    }
}
