using System;
using System.Collections.Generic;

namespace SqlGenerator
{
    public abstract class ColumnDefinition<T> : ICreate where T : Enum
    {
        public string Name { get; }
        public T Type1 { get; }
        private int TypeLength { get; }

        protected ColumnDefinition(string name, T type, int typeLength = 0)
        {
            Name = name;
            Type1 = type;
            TypeLength = typeLength;
        }

        public abstract string GetTypeValue(T type);

        protected virtual string GetTypeLength()
        {
            return TypeLength == 0 ? string.Empty : $"({TypeLength})";
        }
    }

    public class CreateClause : IQueryPart
    {
        public string Table { get; }
        public bool IfNotExist { get; }
        public IEnumerable<ICreate> Create { get; }

        public CreateClause(string table, bool ifNotExist, IEnumerable<ICreate> create)
        {
            Table = table;
            IfNotExist = ifNotExist;
            Create = create;
        }
    }
}