using System;

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

        public override string GetTypeValue(BaseType type)
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
}