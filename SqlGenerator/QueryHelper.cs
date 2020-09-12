#nullable enable
using System;
using System.Collections.Generic;
using System.Text;

namespace SqlGenerator
{
    public static class QueryHelper
    {
        public static void BuildJoinedExpression(StringBuilder sb, string seperator, IEnumerable<IQueryPart> parts, IGenerator generator)
        {
            BuildJoinedExpression(sb, seperator, parts, generator.Build);
        }

        public static void BuildJoinedExpression(StringBuilder sb, string seperator, IEnumerable<IQueryPart> parts, Action<IQueryPart, StringBuilder> sourroundPart)
        {
            BuildSeperated(sb, seperator, parts, sourroundPart);
        }

        public static void BuildSeperated<T>(StringBuilder sb, string seperator, IEnumerable<T> parts, Action<T, StringBuilder> appendPart)
        {
            using IEnumerator<T> part = parts.GetEnumerator();
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

        public static void AppendQueryPart(IGenerator generator, StringBuilder sb, IQueryPart? part)
        {
            if (part == null)
            {
                return;
            }

            sb.Append(" ");
            generator.Build(part, sb);
        }
    }
}