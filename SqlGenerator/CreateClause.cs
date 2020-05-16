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
        private readonly string _name;
        private readonly BaseType _type;
        private readonly int _typeLength;

        public ColumnDefinition(string name, BaseType type, int typeLength = 0)
        {
            _name = name;
            _type = type;
            _typeLength = typeLength;
        }

        public void Build(StringBuilder sb)
        {
            sb.Append(_name);
            sb.Append(" ");
            sb.Append(GetTypeValue(_type, _typeLength));
        }

        private static string GetTypeValue(BaseType type, int length)
        {
            return type.ToString();
        }
    }

    public class CreateClause : IQueryPart
    {
        private readonly string _name;
        private readonly bool _ifNotExist;
        private readonly IEnumerable<ICreate> _create;

        public CreateClause(string name, bool ifnotExist, IEnumerable<ICreate> create)
        {
            _name = name;
            _ifNotExist = ifnotExist;
            _create = create;
        }

        public void Build(StringBuilder sb)
        {
            sb.Append("CREATE TABLE ");
            if (_ifNotExist)
            {
                sb.Append("IF NOT EXISTS ");
            }

            sb.Append(_name);
            sb.Append(" (");
            QueryHelper.BuildJoinedExpression(sb, ", ", _create);
            sb.Append(")");
        }
    }
}