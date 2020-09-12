using System;

namespace SqlGenerator.DDL
{
    public abstract class ColumnDefinition<T> : ICreate where T : Enum
    {
        public string Name { get; }
        public T Type { get; }
        public int TypeLength { get; }

        protected ColumnDefinition(string name, T type, int typeLength = 0)
        {
            Name = name;
            Type = type;
            TypeLength = typeLength;
        }

        public abstract string GetTypeValue(T type);

        protected virtual string GetTypeLength()
        {
            return TypeLength == 0 ? string.Empty : $"({TypeLength})";
        }
    }
}