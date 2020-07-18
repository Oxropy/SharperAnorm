using NUnit.Framework;
using SqlGenerator;

namespace SqlGeneratorTest
{
    [TestFixture]
    public class DeleteClauseTests
    {
        private readonly IGenerator _generator = new PostgreSqlGenerator();

        [Test]
        public void BuildDeleteClause()
        {
            var result = new DeleteClause("Table").GetQuery(_generator);
            Assert.That(result, Is.EqualTo("DELETE FROM Table"));
        }
    }
}