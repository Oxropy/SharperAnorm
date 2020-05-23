using NUnit.Framework;
using SqlGenerator;

namespace SqlGeneratorTest
{
    [TestFixture]
    public class CreateClauseTests
    {
        [Test]
        public void BuildTextColumnDefinitionWithoutLength()
        {
            var result = new BaseTypeColumnDefinition("Column", BaseType.Text).GetQuery();
            Assert.That(result, Is.EqualTo("Column Text"));
        }
        
        [Test]
        public void BuildTextColumnDefinitionWithLength()
        {
            var result = new BaseTypeColumnDefinition("Column",BaseType.Text, 5).GetQuery();
            Assert.That(result, Is.EqualTo("Column Text(5)"));
        }
        
        [Test]
        public void BuildNumericColumnDefinitionWithoutLength()
        {
            var result = new BaseTypeColumnDefinition("Column", BaseType.Numeric).GetQuery();
            Assert.That(result, Is.EqualTo("Column Numeric"));
        }
        
        [Test]
        public void BuildNumericColumnDefinitionWithLength()
        {
            var result = new BaseTypeColumnDefinition("Column", BaseType.Numeric, 5).GetQuery();
            Assert.That(result, Is.EqualTo("Column Numeric(5)"));
        }
        
        [Test]
        public void BuildIntegerColumnDefinitionWithoutLength()
        {
            var result = new BaseTypeColumnDefinition("Column", BaseType.Integer).GetQuery();
            Assert.That(result, Is.EqualTo("Column Integer"));
        }
        
        [Test]
        public void BuildIntegerColumnDefinitionWithLength()
        {
            var result = new BaseTypeColumnDefinition("Column", BaseType.Integer, 5).GetQuery();
            Assert.That(result, Is.EqualTo("Column Integer(5)"));
        }
        
        [Test]
        public void BuildCreateClause()
        {
            var result = new CreateClause("Table", false, new []{"Column1".ColDefinition(BaseType.Text), "Column2".ColDefinition(BaseType.Integer, 5)}).GetQuery();
            Assert.That(result, Is.EqualTo("CREATE TABLE Table (Column1 Text, Column2 Integer(5))"));
        }
        
        [Test]
        public void BuildCreateClauseIfNotExists()
        {
            var result = new CreateClause("Table", true, new []{"Column1".ColDefinition(BaseType.Text), "Column2".ColDefinition(BaseType.Integer, 5)}).GetQuery();
            Assert.That(result, Is.EqualTo("CREATE TABLE IF NOT EXISTS Table (Column1 Text, Column2 Integer(5))"));
        }
    }
}