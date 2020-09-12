using System.Linq;
using System.Text;
using NUnit.Framework;
using SqlGenerator;
using SqlGenerator.DDL;
using SqlGenerator.DML;
using SqlGenerator.DML.FieldAndTable;
using SqlGenerator.DML.Truthy;
using static SqlGenerator.QueryBuilderExtensions;

namespace SqlGeneratorTest
{
    [TestFixture]
    public class PostgreSqlQueryBuilderTests
    {
        private readonly IGenerator _generator = new PostgreSqlGenerator();
        private static TableName LeftTable { get; } = new TableName("Left");
        private static TableName RightTable { get; } = new TableName("Right");
        private static FieldReferenceExpression FieldReferenceWithoutTable { get; } = new FieldReferenceExpression("Field");
        private static LiteralExpression LiteralString { get; } = new LiteralExpression("Value");
        private static LiteralExpression LiteralNumber { get; } = new LiteralExpression(12);
        private static ParameterExpression ParameterString { get; } = new ParameterExpression("stringValue", "Value");
        private static ParameterExpression ParameterNumber { get; } = new ParameterExpression("numberValue", 12);
        private static ITruthy TestTruthy { get; } = new ComparisonExpression(LiteralString, ComparisonOperator.Equal, LiteralNumber);
        private static string TestTruthyExpresison { get; } = "'Value' = 12";
        private static ITruthy ParameterNumberTruthy { get; } = new ComparisonExpression(LiteralNumber, ComparisonOperator.Equal, ParameterNumber);
        private static string ParameterNumberTruthyExpresison { get; } = "12 = :numberValue";
        private static ITruthy ParameterStringTruthy { get; } = new ComparisonExpression(LiteralString, ComparisonOperator.Equal, ParameterString);
        private static string ParameterStringTruthyExpresison { get; } = "'Value' = :stringValue";
        private static SortOrderClause SortOrderAsc { get; } = new SortOrderClause(FieldReferenceWithoutTable);
        private static SortOrderClause SortOrderDesc { get; } = new SortOrderClause(FieldReferenceWithoutTable, SortOrder.Descending);

        #region Builder Extensions

        [Test]
        public void GetQuery()
        {
            Assert.That(LiteralString.GetQuery(_generator), Is.EqualTo(_generator.GetQuery(LiteralString)));
            Assert.That(LiteralNumber.GetQuery(_generator), Is.EqualTo(_generator.GetQuery(LiteralNumber)));
            Assert.That(TestTruthy.GetQuery(_generator), Is.EqualTo(_generator.GetQuery(TestTruthy)));
        }

        [Test]
        public void CreateLiteralString()
        {
            LiteralExpression literal = Val("Value");
            Assert.That(_generator.GetQuery(literal), Is.EqualTo($"'{LiteralString.Literal}'"));
        }

        [Test]
        public void CreateLiteralNumber()
        {
            LiteralExpression literal = Val(12);
            Assert.That(_generator.GetQuery(literal), Is.EqualTo($"{LiteralNumber.Literal}"));
        }

        [Test]
        public void CreateParameter()
        {
            ParameterExpression parameter = "stringValue".Para("Value");
            Assert.That(_generator.GetQuery(parameter), Is.EqualTo($"{PostgreSqlGenerator.ParameterPrefix}{ParameterString.Name}"));
        }

        [Test]
        public void CreateFieldReferenceWithoutTable()
        {
            FieldReferenceExpression fieldReference = "Name".Col();
            Assert.That(_generator.GetQuery(fieldReference), Is.EqualTo(fieldReference.FieldName));
        }

        [Test]
        public void CreateFieldReferenceWithTable()
        {
            FieldReferenceExpression fieldReference = "Name".Col("Table");
            Assert.That(_generator.GetQuery(fieldReference), Is.EqualTo($"{fieldReference.TableName}.{fieldReference.FieldName}"));
        }

        [Test]
        public void CreateFunctionCallSingleParameter()
        {
            FunctionCallExpression functionCall = "Name".Call(LiteralString);
            Assert.That(_generator.GetQuery(functionCall), Is.EqualTo($"{functionCall.FunctionName}('{LiteralString.Literal}')"));
        }

        [Test]
        public void CreateFunctionCallMultipleParameter()
        {
            FunctionCallExpression functionCall = "Name".Call(LiteralString, LiteralNumber);
            Assert.That(_generator.GetQuery(functionCall), Is.EqualTo($"{functionCall.FunctionName}('{LiteralString.Literal}', {LiteralNumber.Literal})"));
        }

        #region Build

        [Test]
        public void AddFromToSelectClause()
        {
            SelectClause select = Select("Name".Col());
            ITableName table = Table("Table");
            FromClause from = From(table);
            IQueryPart selectClause = select.AddFrom(from);
            Assert.That(_generator.GetQuery(selectClause), Is.EqualTo($"SELECT {((FieldReferenceExpression) select.Sel.First()).FieldName} FROM {((TableName) table).Table}"));
        }

        [Test]
        public void AddWhereToDeleteClause()
        {
            DeleteClause delete = Delete(new TableName("Table"));
            WhereClause where = Where(TestTruthy);
            IQueryPart whereClause = delete.AddWhere(where);
            Assert.That(_generator.GetQuery(whereClause), Is.EqualTo($"DELETE FROM {delete.Table} WHERE {TestTruthyExpresison}"));
        }

        [Test]
        public void AddOrderByToWhereClause()
        {
            WhereClause where = Where(TestTruthy);
            FieldReferenceExpression fieldReference = "Field".Col();
            OrderByClause orderBy = OrderBy(fieldReference.SortOrder(SortOrder.Ascending));
            IQueryPart orderByedWhere = where.AddOrderBy(orderBy);
            Assert.That(_generator.GetQuery(orderByedWhere), Is.EqualTo($"WHERE {TestTruthyExpresison} ORDER BY {fieldReference.FieldName} ASC"));
        }

        [Test]
        public void AddGroupByToWhereClause()
        {
            WhereClause where = Where(TestTruthy);
            FieldReferenceExpression fieldReference = "Field".Col();
            GroupByClause groupBy = GroupBy(fieldReference);
            IQueryPart groupByesWhere = where.AddGroupBy(groupBy);
            Assert.That(_generator.GetQuery(groupByesWhere), Is.EqualTo($"WHERE {TestTruthyExpresison} GROUP BY {fieldReference.FieldName}"));
        }

        #endregion

        #region Create

        [Test]
        public void BuildColumnDefinitionWithoutLength()
        {
            BaseTypeColumnDefinition columnDefinition = "Column".ColDefinition(BaseType.Text);
            Assert.That(_generator.GetQuery(columnDefinition), Is.EqualTo($"{columnDefinition.Name} Text"));
        }

        [Test]
        public void BuildColumnDefinitionWithLength()
        {
            BaseTypeColumnDefinition columnDefinition = "Column".ColDefinition(BaseType.Text, 5);
            Assert.That(_generator.GetQuery(columnDefinition), Is.EqualTo($"{columnDefinition.Name} Text({columnDefinition.TypeLength})"));
        }

        [Test]
        public void BuildCreateClause()
        {
            CreateClause createClause = Create("Table", false, "Column1".ColDefinition(BaseType.Integer), "Column2".ColDefinition(BaseType.Text));
            Assert.That(_generator.GetQuery(createClause), Is.EqualTo($"CREATE TABLE {createClause.Table} (Column1 Integer, Column2 Text)"));
        }

        #endregion

        #region Drop

        [Test]
        public void BuildDropClause()
        {
            DropClause dropClause = Drop("Table");
            Assert.That(_generator.GetQuery(dropClause), Is.EqualTo($"DROP TABLE {dropClause.Table}"));
        }

        #endregion

        #region Insert

        [Test]
        public void BuildInsertClauseWithoutValues()
        {
            InsertClause insertClause = Insert("Table");
            Assert.That(_generator.GetQuery(insertClause), Is.EqualTo($"INSERT INTO {insertClause.Table} () VALUES ()"));
        }

        [Test]
        public void BuildInsertClauseWithValues()
        {
            InsertClause insertClause = Insert("Table", new FieldValue("Column1", LiteralString), new FieldValue("Column2", LiteralNumber));
            Assert.That(_generator.GetQuery(insertClause), Is.EqualTo($"INSERT INTO {insertClause.Table} (Column1, Column2) VALUES ('{LiteralString.Literal}', {LiteralNumber.Literal})"));
        }

        #endregion

        #region Update

        [Test]
        public void BuildUpdateClauseWithoutValues()
        {
            UpdateClause updateClause = Update("Table");
            Assert.That(_generator.GetQuery(updateClause), Is.EqualTo($"UPDATE {updateClause.Table} SET ()"));
        }

        [Test]
        public void BuildUpdateClauseWithValues()
        {
            UpdateClause updateClause = Update("Table", new FieldValue("Column1", LiteralString), new FieldValue("Column2", LiteralNumber));
            Assert.That(_generator.GetQuery(updateClause), Is.EqualTo($"UPDATE {updateClause.Table} SET (Column1 = '{LiteralString.Literal}', Column2 = {LiteralNumber.Literal})"));
        }

        #endregion

        #region Delete

        [Test]
        public void BuildDeleteClause()
        {
            DeleteClause deleteClause = Delete(new TableName("Table"));
            Assert.That(_generator.GetQuery(deleteClause), Is.EqualTo($"DELETE FROM {deleteClause.Table}"));
        }

        #endregion

        #region Select

        [Test]
        public void BuildSelectClause()
        {
            SelectClause selectClause = Select("Column1".Col(), "Column2".Col());
            Assert.That(_generator.GetQuery(selectClause), Is.EqualTo("SELECT Column1, Column2"));
        }

        [Test]
        public void BuildFieldAliasWithFieldReference()
        {
            ISelection alias = "Column".Col().As("Field");
            Assert.That(_generator.GetQuery(alias), Is.EqualTo("(Column) AS Field"));
        }

        [Test]
        public void BuildFieldAliasWithStringLiteral()
        {
            ISelection alias = Val("Lit").As("Field");
            Assert.That(_generator.GetQuery(alias), Is.EqualTo("('Lit') AS Field"));
        }

        [Test]
        public void BuildFieldAliasWithNumberLiteral()
        {
            ISelection alias = Val(12).As("Field");
            Assert.That(_generator.GetQuery(alias), Is.EqualTo("(12) AS Field"));
        }

        #endregion

        #region From

        [Test]
        public void BuildTableNameWthoutAlias()
        {
            TableName table = Table("Table");
            Assert.That(_generator.GetQuery(table), Is.EqualTo("Table"));
        }

        [Test]
        public void BuildTableNameWthAlias()
        {
            TableName table = Table("Table", "Alias");
            Assert.That(_generator.GetQuery(table), Is.EqualTo("Table Alias"));
        }

        [Test]
        public void BuildFromClause()
        {
            FromClause fromClause = From(LeftTable);
            Assert.That(_generator.GetQuery(fromClause), Is.EqualTo($"FROM {LeftTable.Table}"));
        }

        [Test]
        public void BuildFromClauseWithJoin()
        {
            FromClause fromClause = From(LeftTable.InnerJoin(RightTable, TestTruthy));
            Assert.That(_generator.GetQuery(fromClause), Is.EqualTo($"FROM {LeftTable.Table} INNER JOIN {RightTable.Table} ON ({TestTruthyExpresison})"));
        }

        [Test]
        public void BuildInnerJoin()
        {
            ITableName join = LeftTable.InnerJoin(RightTable, TestTruthy);
            Assert.That(_generator.GetQuery(join), Is.EqualTo($"{LeftTable.Table} INNER JOIN {RightTable.Table} ON ({TestTruthyExpresison})"));
        }

        [Test]
        public void BuildLeftJoin()
        {
            ITableName join = LeftTable.LeftJoin(RightTable, TestTruthy);
            Assert.That(_generator.GetQuery(join), Is.EqualTo($"{LeftTable.Table} LEFT JOIN {RightTable.Table} ON ({TestTruthyExpresison})"));
        }

        [Test]
        public void BuildRightJoin()
        {
            ITableName join = LeftTable.RightJoin(RightTable, TestTruthy);
            Assert.That(_generator.GetQuery(join), Is.EqualTo($"{LeftTable.Table} RIGHT JOIN {RightTable.Table} ON ({TestTruthyExpresison})"));
        }

        [Test]
        public void BuildFullJoin()
        {
            ITableName join = LeftTable.FullJoin(RightTable, TestTruthy);
            Assert.That(_generator.GetQuery(join), Is.EqualTo($"{LeftTable.Table} FULL JOIN {RightTable.Table} ON ({TestTruthyExpresison})"));
        }

        [Test]
        public void BuildLeftOuterJoin()
        {
            ITableName join = LeftTable.LeftOuterJoin(RightTable, TestTruthy);
            Assert.That(_generator.GetQuery(join), Is.EqualTo($"{LeftTable.Table} LEFT OUTER JOIN {RightTable.Table} ON ({TestTruthyExpresison})"));
        }

        [Test]
        public void BuildRightOuterJoin()
        {
            ITableName join = LeftTable.RightOuterJoin(RightTable, TestTruthy);
            Assert.That(_generator.GetQuery(join), Is.EqualTo($"{LeftTable.Table} RIGHT OUTER JOIN {RightTable.Table} ON ({TestTruthyExpresison})"));
        }

        [Test]
        public void BuildFullOuterJoin()
        {
            ITableName join = LeftTable.FullOuterJoin(RightTable, TestTruthy);
            Assert.That(_generator.GetQuery(join), Is.EqualTo($"{LeftTable.Table} FULL OUTER JOIN {RightTable.Table} ON ({TestTruthyExpresison})"));
        }

        #endregion

        #region Where

        [Test]
        public void BuildWhere()
        {
            WhereClause where = Where(TestTruthy);
            Assert.That(_generator.GetQuery(where), Is.EqualTo($"WHERE {TestTruthyExpresison}"));
        }

        [Test]
        public void BuildSingleAnd()
        {
            ITruthy truthy = ParameterStringTruthy.And(ParameterNumberTruthy);
            Assert.That(_generator.GetQuery(truthy), Is.EqualTo($"({ParameterStringTruthyExpresison}) AND ({ParameterNumberTruthyExpresison})"));
        }

        [Test]
        public void BuildMultipleAnd()
        {
            ITruthy truthy = ParameterStringTruthy.And(ParameterNumberTruthy).And(TestTruthy);
            Assert.That(_generator.GetQuery(truthy), Is.EqualTo($"(({ParameterStringTruthyExpresison}) AND ({ParameterNumberTruthyExpresison})) AND ({TestTruthyExpresison})"));
        }

        [Test]
        public void BuildSingleOr()
        {
            ITruthy truthy = ParameterStringTruthy.Or(ParameterNumberTruthy);
            Assert.That(_generator.GetQuery(truthy), Is.EqualTo($"({ParameterStringTruthyExpresison}) OR ({ParameterNumberTruthyExpresison})"));
        }

        [Test]
        public void BuildMultipleOr()
        {
            ITruthy truthy = ParameterStringTruthy.Or(ParameterNumberTruthy).Or(TestTruthy);
            Assert.That(_generator.GetQuery(truthy), Is.EqualTo($"(({ParameterStringTruthyExpresison}) OR ({ParameterNumberTruthyExpresison})) OR ({TestTruthyExpresison})"));
        }

        [Test]
        public void BuildEqual()
        {
            ITruthy comparison = LiteralString.Eq(LiteralString);
            Assert.That(_generator.GetQuery(comparison), Is.EqualTo($"'{LiteralString.Literal}' = '{LiteralString.Literal}'"));
        }

        [Test]
        public void BuildNotEqual()
        {
            ITruthy comparison = LiteralString.Neq(LiteralString);
            Assert.That(_generator.GetQuery(comparison), Is.EqualTo($"'{LiteralString.Literal}' != '{LiteralString.Literal}'"));
        }

        [Test]
        public void BuildGreaterThan()
        {
            ITruthy comparison = LiteralString.Gt(LiteralString);
            Assert.That(_generator.GetQuery(comparison), Is.EqualTo($"'{LiteralString.Literal}' > '{LiteralString.Literal}'"));
        }

        [Test]
        public void BuildGreaterThanOrEqual()
        {
            ITruthy comparison = LiteralString.GtEq(LiteralString);
            Assert.That(_generator.GetQuery(comparison), Is.EqualTo($"'{LiteralString.Literal}' >= '{LiteralString.Literal}'"));
        }

        [Test]
        public void BuildLowerThan()
        {
            ITruthy comparison = LiteralString.Lt(LiteralString);
            Assert.That(_generator.GetQuery(comparison), Is.EqualTo($"'{LiteralString.Literal}' < '{LiteralString.Literal}'"));
        }

        [Test]
        public void BuildLowerThanOrEqual()
        {
            ITruthy comparison = LiteralString.LtEq(LiteralString);
            Assert.That(_generator.GetQuery(comparison), Is.EqualTo($"'{LiteralString.Literal}' <= '{LiteralString.Literal}'"));
        }

        [Test]
        public void BuildList()
        {
            ITruthy comparison = LiteralString.LtEq(LiteralString);
            Assert.That(_generator.GetQuery(comparison), Is.EqualTo($"'{LiteralString.Literal}' <= '{LiteralString.Literal}'"));
        }

        [Test]
        public void BuildIsNull()
        {
            ITruthy isNull = TestTruthy.IsNull();
            Assert.That(_generator.GetQuery(isNull), Is.EqualTo($"({TestTruthyExpresison}) IS Null"));
        }

        [Test]
        public void BuildNot()
        {
            ITruthy not = TestTruthy.Not();
            Assert.That(_generator.GetQuery(not), Is.EqualTo($"NOT ({TestTruthyExpresison})"));
        }

        [Test]
        public void BuildIn()
        {
            ITruthy inExpression = LiteralString.In(LiteralString);
            Assert.That(_generator.GetQuery(inExpression), Is.EqualTo($"'{LiteralString.Literal}' IN ('{LiteralString.Literal}')"));
        }

        [Test]
        public void BuildLike()
        {
            ITruthy like = LiteralString.Like(ParameterString);
            Assert.That(_generator.GetQuery(like), Is.EqualTo($"'{LiteralString.Literal}' LIKE '{PostgreSqlGenerator.ParameterPrefix}{ParameterString.Name}'"));
        }

        #endregion

        #region Order By

        [Test]
        public void BuildSortOrderAscending()
        {
            SortOrderClause sortOrder = FieldReferenceWithoutTable.SortOrder(SortOrder.Ascending);
            Assert.That(_generator.GetQuery(sortOrder), Is.EqualTo($"{FieldReferenceWithoutTable.FieldName} ASC"));
        }

        [Test]
        public void BuildSortOrderClauseDescending()
        {
            SortOrderClause sortOrder = FieldReferenceWithoutTable.SortOrder(SortOrder.Descending);
            Assert.That(_generator.GetQuery(sortOrder), Is.EqualTo($"{FieldReferenceWithoutTable.FieldName} DESC"));
        }

        [Test]
        public void BuildOrderByClause()
        {
            OrderByClause sortOrder = OrderBy(SortOrderAsc, SortOrderDesc);
            Assert.That(_generator.GetQuery(sortOrder), Is.EqualTo($"ORDER BY {FieldReferenceWithoutTable.FieldName} ASC, {FieldReferenceWithoutTable.FieldName} DESC"));
        }

        #endregion

        #region Group By

        [Test]
        public void BuildGroupByClause()
        {
            GroupByClause groupBy = GroupBy(FieldReferenceWithoutTable, LiteralNumber);
            Assert.That(_generator.GetQuery(groupBy), Is.EqualTo($"GROUP BY {FieldReferenceWithoutTable.FieldName}, {LiteralNumber.Literal}"));
        }

        #endregion

        #endregion

        #region Query Helper

        [Test]
        public void BuildJoinExpressionWithoutValuesWithout()
        {
            var sb = new StringBuilder();
            QueryHelper.BuildJoinedExpression(sb, (string) ",", new IQueryPart[0], _generator);
            Assert.That(sb.ToString(), Is.Empty);
        }

        #endregion
    }
}