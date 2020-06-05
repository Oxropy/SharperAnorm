using NUnit.Framework;
using SqlGenerator;

namespace SqlGeneratorTest
{
    [TestFixture]
    public class InsertClauseTests
    {
        [Test]
        public void BuildInsertValue()
        {
            var value = new InsertValue("Name", "Value");
            Assert.That(value.Field, Is.EqualTo("Name"));
            Assert.That(value.Value, Is.EqualTo("Value"));
        }
        
        [Test]
        public void BuildInsertClause()
        {
            var value1 = new InsertValue("Name1", "Value");
            var value2 = new InsertValue("Name2", 12);
            var result = new InsertClause("Table", new []{value1, value2}).GetQuery();
            Assert.That(result, Is.EqualTo("INSERT INTO Table (Name1, Name2) VALUES (Value, 12)"));
        }
    }
}