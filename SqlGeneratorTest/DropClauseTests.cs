using NUnit.Framework;
using SqlGenerator;

namespace SqlGeneratorTest
{
    [TestFixture]
    public class DropClauseTests
    {
        private readonly IGenerator _generator = new PostgreSqlGenerator();

        [Test]
        public void BuildDropClause()
        {
            var result = new DropClause("Table").GetQuery(_generator);
            Assert.That(result, Is.EqualTo("DROP TABLE Table"));
        }
    }
}