using NUnit.Framework;
using SqlGenerator;

namespace SqlGeneratorTest
{
    [TestFixture]
    public class InsertClauseTests
    {
        [Test]
        public void BuildInsertClause()
        {
            (string, object) value1 = ("Name1", "Value");
            (string, object) value2 = ("Name2", 12);
            var result = new InsertClause("Table", new []{value1, value2}).GetQuery();
            Assert.That(result, Is.EqualTo("INSERT INTO Table (Name1, Name2) VALUES (Value, 12)"));
        }

        [Test]
        public void BuildInsertStatement()
        {
            (string, object) value1 = ("Name1", "Value");
            (string, object) value2 = ("Name2", 12);
            var result = InsertStatement.Insert("Table", new []{value1, value2}).GetQuery();
            Assert.That(result, Is.EqualTo("INSERT INTO Table (Name1, Name2) VALUES (Value, 12)"));
        }
    }
}