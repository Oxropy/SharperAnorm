using System;
using System.Collections.Generic;
using System.Text;

namespace SqlGenerator
{
    public enum BaseType
    {
        Text,
        Numeric,
        Integer
    }

    public class BaseTypeColumnDefinition : ColumnDefinition<BaseType>
    {
        public BaseTypeColumnDefinition(string name, BaseType type, int typeLength = 0) : base(name, type, typeLength)
        {
        }

        protected override string GetTypeValue(BaseType type)
        {
            return type switch
            {
                BaseType.Text => "Text",
                BaseType.Numeric => "Numeric",
                BaseType.Integer => "Integer",
                _ => throw new ArgumentOutOfRangeException()
            } + GetTypeLength();
        }
    }
    
    public abstract class ColumnDefinition<T> : ICreate where T : Enum
    {
        private readonly string _name;
        private readonly T _type;
        private readonly int _typeLength;

        protected ColumnDefinition(string name, T type, int typeLength = 0)
        {
            _name = name;
            _type = type;
            _typeLength = typeLength;
        }

        public void Build(StringBuilder sb)
        {
            sb.Append(_name);
            sb.Append(" ");
            sb.Append(GetTypeValue(_type));
        }

        protected abstract string GetTypeValue(T type);
        
        protected virtual string GetTypeLength()
        {
            return _typeLength == 0 ? string.Empty : $"({_typeLength})";
        }
    }

    public class CreateClause : IQueryPart
    {
        private readonly string _table;
        private readonly bool _ifNotExist;
        private readonly IEnumerable<ICreate> _create;

        public CreateClause(string table, bool ifNotExist, IEnumerable<ICreate> create)
        {
            _table = table;
            _ifNotExist = ifNotExist;
            _create = create;
        }

        public void Build(StringBuilder sb)
        {
            sb.Append("CREATE TABLE ");
            if (_ifNotExist)
            {
                sb.Append("IF NOT EXISTS ");
            }

            sb.Append(_table);
            sb.Append(" (");
            QueryHelper.BuildJoinedExpression(sb, ", ", _create);
            sb.Append(")");
        }
    }
}