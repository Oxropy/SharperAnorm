using System.Collections.Generic;
using System.Text;

namespace SqlGenerator
{
    public enum BaseType
    {
        Text,
        Numberic,
        Integer
    }

    public class ColumnDefinition : ICreate
    {
        public string Name { get; }
        public BaseType Type { get; }
        public int TypeLength { get; }

        public ColumnDefinition(string name, BaseType type, int typeLength = 0)
        {
            Name = name;
            Type = type;
            TypeLength = typeLength;
        }

        public void BuildQuery(StringBuilder sb)
        {
            sb.Append(Name);
            sb.Append(" ");
            sb.Append(GetTypeValue(Type, TypeLength));
        }

        public static string GetTypeValue(BaseType type, int length)
        {
            return type.ToString();
        }
    }

    public class CreateClause : IQueryPart
    {
        public string Name { get; }
        public bool IfNotExist { get; }
        public IEnumerable<ICreate> Create { get; }

        public CreateClause(string name, bool ifnotExist, IEnumerable<ICreate> create)
        {
            this.Name = name;
            this.IfNotExist = ifnotExist;
            this.Create = create;
        }

        public void BuildQuery(StringBuilder sb)
        {
            sb.Append("CREATE TABLE ");
            if (IfNotExist)
            {
                sb.Append("IF NOT EXISTS ");
            }

            sb.Append(Name);
            sb.Append(" (");
            QueryHelper.BuildJoinedExpression(sb, ", ", Create);
            sb.Append(")");
        }
    }
}