using System;
using System.Collections.Generic;
using System.Text;

namespace SqlGenerator
{
    public static class QueryHelper
    {
        public static void BuildJoinedExpression(StringBuilder sb, string seperator, IEnumerable<IQueryPart> parts)
        {
            BuildJoinedExpression(sb, seperator, parts, (part, builder) => part.Build(builder));
        }

        public static void BuildJoinedExpression(StringBuilder sb, string seperator, IEnumerable<IQueryPart> parts, Action<IQueryPart, StringBuilder> sourroundPart)
        {
            BuildSeperated(sb, seperator, parts, sourroundPart);
        }
        
        public static void BuildSeperated<T>(StringBuilder sb, string seperator, IEnumerable<T> parts, Action<T, StringBuilder> appendPart)
        {
            using var part = parts.GetEnumerator();
            if (!part.MoveNext())
            {
                return;
            }

            appendPart(part.Current, sb);

            while (part.MoveNext())
            {
                sb.Append(seperator);
                appendPart(part.Current, sb);
            }
        }
        
        public static void AppendQueryPart(StringBuilder sb, IQueryPart? part)
        {
            if (part == null)
            {
                return;
            }

            sb.Append(" ");
            part.Build(sb);
        }
    }
}