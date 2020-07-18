using NUnit.Framework;
using SqlGenerator;

namespace SqlGeneratorTest
{
    [TestFixture]
    public class UpdateClauseTests
    {
        private readonly IGenerator _generator = new PostgreSqlGenerator();

        [Test]
        public void BuildUpdateClause()
        {
            (string, object) value1 = ("Name1", "Value");
            (string, object) value2 = ("Name2", 12);
            var result = new UpdateClause("Table", new[] {value1, value2}).GetQuery(_generator);
            Assert.That(result, Is.EqualTo("UPDATE Table SET (Name1 = Value, Name2 = 12)"));
        }
    }
}