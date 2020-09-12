namespace SqlGenerator.DDL
{
    public class FieldValue
    {
        public readonly string Field;
        public readonly IValue Value;

        public FieldValue(string field, IValue value)
        {
            Field = field;
            Value = value;
        }
    }
}