using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SqlGenerator;
using SqlGenerator.DDL;
using SqlGenerator.DML;
using SqlGenerator.DML.FieldAndTable;
using SqlGenerator.DML.Truthy;

namespace SqlGeneratorTest
{
    [TestFixture]
    public class PostgreSqlGeneratorTests
    {
        #region Values for Tests

        private readonly IGenerator _generator = new PostgreSqlGenerator();
        private static TableName LeftTable { get; } = new TableName("Left");
        private static TableName RightTable { get; } = new TableName("Right");
        private static FieldReferenceExpression FieldReferenceWithoutTable { get; } = new FieldReferenceExpression("Field");
        private static FieldReferenceExpression FieldReferenceWithTable { get; } = new FieldReferenceExpression("Field", "Table");
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
        private static SortOrderClause SortOrderAsc { get; } = new SortOrderClause(FieldReferenceWithTable);
        private static SortOrderClause SortOrderDesc { get; } = new SortOrderClause(FieldReferenceWithTable, SortOrder.Descending);

        #endregion

        #region Classes for Test

        private class NewQueryPart : IQueryPart { }

        private class NewTableName : ITableName { }

        private class NewExpression : IExpression { }

        private class NewValue : IValue { }

        private class NewTruthy : ITruthy { }

        private class NewCreate : ICreate { }

        private class NewQuery : IQuery { }

        private class NewTableAndFieldValues : TableAndFieldValues
        {
            public NewTableAndFieldValues() : base("Table", new[] { new FieldValue("field", new LiteralExpression(12)) }) { }
        }

        #endregion

        #region Build

        #region Create

        [Test]
        public void BuildTextColumnDefinitionWithoutLength()
        {
            var columnDefinition = new BaseTypeColumnDefinition("Column", BaseType.Text);
            Assert.That(_generator.GetQuery(columnDefinition), Is.EqualTo($"{columnDefinition.Name} Text"));
        }

        [Test]
        public void BuildTextColumnDefinitionWithLength()
        {
            var columnDefinition = new BaseTypeColumnDefinition("Column", BaseType.Text, 5);
            Assert.That(_generator.GetQuery(columnDefinition), Is.EqualTo($"{columnDefinition.Name} Text({columnDefinition.TypeLength})"));
        }

        [Test]
        public void BuildNumericColumnDefinitionWithoutLength()
        {
            var columnDefinition = new BaseTypeColumnDefinition("Column", BaseType.Numeric);
            Assert.That(_generator.GetQuery(columnDefinition), Is.EqualTo($"{columnDefinition.Name} Numeric"));
        }

        [Test]
        public void BuildNumericColumnDefinitionWithLength()
        {
            var columnDefinition = new BaseTypeColumnDefinition("Column", BaseType.Numeric, 5);
            Assert.That(_generator.GetQuery(columnDefinition), Is.EqualTo($"{columnDefinition.Name} Numeric({columnDefinition.TypeLength})"));
        }

        [Test]
        public void BuildIntegerColumnDefinitionWithoutLength()
        {
            var columnDefinition = new BaseTypeColumnDefinition("Column", BaseType.Integer);
            Assert.That(_generator.GetQuery(columnDefinition), Is.EqualTo($"{columnDefinition.Name} Integer"));
        }

        [Test]
        public void BuildIntegerColumnDefinitionWithLength()
        {
            var columnDefinition = new BaseTypeColumnDefinition("Column", BaseType.Integer, 5);
            Assert.That(_generator.GetQuery(columnDefinition), Is.EqualTo($"{columnDefinition.Name} Integer({columnDefinition.TypeLength})"));
        }

        [Test]
        public void BuildCreateClause()
        {
            BaseTypeColumnDefinition[] columns = { "Column1".ColDefinition(BaseType.Text), "Column2".ColDefinition(BaseType.Integer, 5) };
            var create = new CreateClause("Table", false, columns);
            Assert.That(_generator.GetQuery(create), Is.EqualTo($"CREATE TABLE {create.Table} ({columns[0].Name} Text, {columns[1].Name} Integer({columns[1].TypeLength}))"));
        }

        [Test]
        public void BuildCreateClauseIfNotExists()
        {
            BaseTypeColumnDefinition[] columns = { "Column1".ColDefinition(BaseType.Text), "Column2".ColDefinition(BaseType.Integer, 5) };
            var create = new CreateClause("Table", true, columns);
            Assert.That(_generator.GetQuery(create), Is.EqualTo($"CREATE TABLE IF NOT EXISTS {create.Table} ({columns[0].Name} Text, {columns[1].Name} Integer({columns[1].TypeLength}))"));
        }

        #endregion

        #region Drop

        [Test]
        public void BuildDropClause()
        {
            var drop = new DropClause("Table");
            Assert.That(_generator.GetQuery(drop), Is.EqualTo($"DROP TABLE {drop.Table}"));
        }

        #endregion

        #region Insert

        [Test]
        public void BuildInsertClause()
        {
            var value1 = new FieldValue("Name1", LiteralString);
            var value2 = new FieldValue("Name2", LiteralNumber);
            var insert = new InsertClause("Table", new[] { value1, value2 });
            Assert.That(_generator.GetQuery(insert), Is.EqualTo($"INSERT INTO {insert.Table} ({value1.Field}, {value2.Field}) VALUES ('{((LiteralExpression) value1.Value).Literal}', {((LiteralExpression) value2.Value).Literal})"));
        }

        [Test]
        public void BuildInsertStatement()
        {
            var value1 = new FieldValue("Name1", LiteralString);
            var value2 = new FieldValue("Name2", LiteralNumber);
            InsertStatement insert = InsertStatement.Insert("Table", new[] { value1, value2 });
            Assert.That(_generator.GetQuery(insert),
                Is.EqualTo(
                    $"INSERT INTO {insert.InsertClause.Table} ({value1.Field}, {value2.Field}) VALUES ('{((LiteralExpression) value1.Value).Literal}', {((LiteralExpression) value2.Value).Literal})"));
        }

        #endregion

        #region Update

        [Test]
        public void BuildUpdateClause()
        {
            var value1 = new FieldValue("Name1", LiteralString);
            var value2 = new FieldValue("Name2", LiteralNumber);
            var update = new UpdateClause("Table", new[] { value1, value2 });
            Assert.That(_generator.GetQuery(update), Is.EqualTo($"UPDATE {update.Table} SET ({value1.Field} = '{((LiteralExpression) value1.Value).Literal}', {value2.Field} = {((LiteralExpression) value2.Value).Literal})"));
        }

        [Test]
        public void BuildUpdateStatement()
        {
            var value1 = new FieldValue("Name1", LiteralString);
            var value2 = new FieldValue("Name2", LiteralNumber);
            var update = new UpdateClause("Table", new[] { value1, value2 });
            var statement = new UpdateStatement(update);
            Assert.That(_generator.GetQuery(statement), Is.EqualTo($"UPDATE {update.Table} SET ({value1.Field} = '{((LiteralExpression) value1.Value).Literal}', {value2.Field} = {((LiteralExpression) value2.Value).Literal})"));
        }

        [Test]
        public void BuildUpdateStatementWithWhere()
        {
            var value1 = new FieldValue("Name1", LiteralString);
            var value2 = new FieldValue("Name2", LiteralNumber);
            var update = new UpdateClause("Table", new[] { value1, value2 });
            var statement = new UpdateStatement(update);
            var where = new WhereClause(TestTruthy);
            statement = statement.WithWhere(where);
            Assert.That(_generator.GetQuery(statement), Is.EqualTo($"UPDATE {update.Table} SET ({value1.Field} = '{((LiteralExpression) value1.Value).Literal}', {value2.Field} = {((LiteralExpression) value2.Value).Literal}) WHERE {TestTruthyExpresison}"));
        }

        #endregion
        
        #region Delete

        [Test]
        public void BuildDeleteClause()
        {
            var delete = new DeleteClause(new TableName("Table"));
            Assert.That(_generator.GetQuery(delete), Is.EqualTo($"DELETE FROM {delete.Table}"));
        }

        [Test]
        public void BuildDeleteStatement()
        {
            var deleteClause = new DeleteClause(new TableName("Table"));
            var delete = new DeleteStatement(deleteClause);
            Assert.That(_generator.GetQuery(delete), Is.EqualTo($"DELETE FROM {delete.Delete.Table}"));
        }

        [Test]
        public void BuildDeleteStatementWithWhere()
        {
            var delete = new DeleteClause(new TableName("Table"));
            var statement = new DeleteStatement(delete);
            var where = new WhereClause(TestTruthy);
            statement = statement.WithWhere(where);
            Assert.That(_generator.GetQuery(statement), Is.EqualTo($"DELETE FROM {delete.Table} WHERE {TestTruthyExpresison}"));
        }

        #endregion

        #region Select

        [Test]
        public void BuildFieldAlias()
        {
            var fieldAlias = new FieldAliasExpression(LiteralString, "test");
            Assert.That(_generator.GetQuery(fieldAlias), Is.EqualTo($"('{LiteralString.Literal}') AS {fieldAlias.Alias}"));
        }

        [Test]
        public void BuildSelectClause()
        {
            var field1 = new FieldAliasExpression(LiteralString, "firstField");
            var field2 = new FieldAliasExpression(LiteralNumber, "SecondField");
            var select = new SelectClause(new[] { field1, field2 });
            Assert.That(_generator.GetQuery(select), Is.EqualTo($"SELECT ('{LiteralString.Literal}') AS {field1.Alias}, ({LiteralNumber.Literal}) AS {field2.Alias}"));
        }

        [Test]
        public void BuildSelectStatement()
        {
            var field1 = new FieldAliasExpression(LiteralString, "firstField");
            var field2 = new FieldAliasExpression(LiteralNumber, "SecondField");
            var select = new SelectClause(new[] { field1, field2 });
            var from = new FromClause(LeftTable);
            var statement = new SelectStatement(select, from);
            Assert.That(_generator.GetQuery(statement), Is.EqualTo($"SELECT ('{LiteralString.Literal}') AS {field1.Alias}, ({LiteralNumber.Literal}) AS {field2.Alias} FROM {LeftTable.Table}"));
        }

        [Test]
        public void BuildSelectStatementWithWhere()
        {
            var field1 = new FieldAliasExpression(LiteralString, "firstField");
            var field2 = new FieldAliasExpression(LiteralNumber, "SecondField");
            var select = new SelectClause(new[] { field1, field2 });
            var from = new FromClause(LeftTable);
            var statement = new SelectStatement(select, from);
            var where = new WhereClause(TestTruthy);
            statement = statement.WithWhere(where);
            Assert.That(_generator.GetQuery(statement),
                Is.EqualTo($"SELECT ('{LiteralString.Literal}') AS {field1.Alias}, ({LiteralNumber.Literal}) AS {field2.Alias} FROM {LeftTable.Table} WHERE {TestTruthyExpresison}"));
        }

        [Test]
        public void BuildSelectStatementWithOrderBy()
        {
            var field1 = new FieldAliasExpression(LiteralString, "firstField");
            var field2 = new FieldAliasExpression(LiteralNumber, "SecondField");
            var select = new SelectClause(new[] { field1, field2 });
            var from = new FromClause(LeftTable);
            var statement = new SelectStatement(select, from);
            var orderBy = new OrderByClause(new[] { SortOrderAsc });
            statement = statement.WithOrderBy(orderBy);
            Assert.That(_generator.GetQuery(statement),
                Is.EqualTo(
                    $"SELECT ('{LiteralString.Literal}') AS {field1.Alias}, ({LiteralNumber.Literal}) AS {field2.Alias} FROM {LeftTable.Table} ORDER BY {FieldReferenceWithTable.TableName}.{FieldReferenceWithTable.FieldName} ASC"));
        }

        [Test]
        public void BuildSelectStatementWithGroupBy()
        {
            var field1 = new FieldAliasExpression(LiteralString, "firstField");
            var field2 = new FieldAliasExpression(LiteralNumber, "SecondField");
            var select = new SelectClause(new ISelection[] { field1, field2, FieldReferenceWithTable });
            var from = new FromClause(LeftTable);
            var statement = new SelectStatement(select, from);
            var groupBy = new GroupByClause(new[] { FieldReferenceWithTable });
            statement = statement.WithGroupBy(groupBy);
            Assert.That(_generator.GetQuery(statement),
                Is.EqualTo(
                    $"SELECT ('{LiteralString.Literal}') AS {field1.Alias}, ({LiteralNumber.Literal}) AS {field2.Alias}, {FieldReferenceWithTable.TableName}.{FieldReferenceWithTable.FieldName} FROM {LeftTable.Table} GROUP BY {FieldReferenceWithTable.TableName}.{FieldReferenceWithTable.FieldName}"));
        }

        #endregion

        #region From

        #region Table name

        [Test]
        public void BuildTableNameWithoutAlias()
        {
            Assert.That(_generator.GetQuery(LeftTable), Is.EqualTo(LeftTable.Table));
        }

        [Test]
        public void BuildTableNameWithAlias()
        {
            var table = new TableName("Table", "Alias");
            Assert.That(_generator.GetQuery(table), Is.EqualTo($"{table.Table} {table.Alias}"));
        }

        #endregion

        #region Join condition

        [Test]
        public void BuildInnerJoinCondition()
        {
            var join = new JoinCondition(LeftTable, JoinClause.Inner, RightTable, TestTruthy);
            Assert.That(_generator.GetQuery(join), Is.EqualTo($"{LeftTable.Table} INNER JOIN {RightTable.Table} ON ({TestTruthyExpresison})"));
        }

        [Test]
        public void BuildLeftJoinCondition()
        {
            var join = new JoinCondition(LeftTable, JoinClause.Left, RightTable, TestTruthy);
            Assert.That(_generator.GetQuery(join), Is.EqualTo($"{LeftTable.Table} LEFT JOIN {RightTable.Table} ON ({TestTruthyExpresison})"));
        }

        [Test]
        public void BuildRightJoinCondition()
        {
            var join = new JoinCondition(LeftTable, JoinClause.Right, RightTable, TestTruthy);
            Assert.That(_generator.GetQuery(join), Is.EqualTo($"{LeftTable.Table} RIGHT JOIN {RightTable.Table} ON ({TestTruthyExpresison})"));
        }

        [Test]
        public void BuildFullJoinCondition()
        {
            var join = new JoinCondition(LeftTable, JoinClause.Full, RightTable, TestTruthy);
            Assert.That(_generator.GetQuery(join), Is.EqualTo($"{LeftTable.Table} FULL JOIN {RightTable.Table} ON ({TestTruthyExpresison})"));
        }

        [Test]
        public void BuildLeftOuterJoinCondition()
        {
            var join = new JoinCondition(LeftTable, JoinClause.LeftOuter, RightTable, TestTruthy);
            Assert.That(_generator.GetQuery(join), Is.EqualTo($"{LeftTable.Table} LEFT OUTER JOIN {RightTable.Table} ON ({TestTruthyExpresison})"));
        }

        [Test]
        public void BuildRightOuterJoinCondition()
        {
            var join = new JoinCondition(LeftTable, JoinClause.RightOuter, RightTable, TestTruthy);
            Assert.That(_generator.GetQuery(join), Is.EqualTo($"{LeftTable.Table} RIGHT OUTER JOIN {RightTable.Table} ON ({TestTruthyExpresison})"));
        }

        [Test]
        public void BuildFullOuterJoinCondition()
        {
            var join = new JoinCondition(LeftTable, JoinClause.FullOuter, RightTable, TestTruthy);
            Assert.That(_generator.GetQuery(join), Is.EqualTo($"{LeftTable.Table} FULL OUTER JOIN {RightTable.Table} ON ({TestTruthyExpresison})"));
        }

        #endregion

        #region From Clause

        [Test]
        public void BuildFromClause()
        {
            var from = new FromClause(LeftTable);
            Assert.That(_generator.GetQuery(from), Is.EqualTo($"FROM {LeftTable.Table}"));
        }

        #endregion

        #endregion

        #region Where

        #region Comparison operator

        [Test]
        public void BuildEqual()
        {
            var comparison = new ComparisonExpression(LiteralString, ComparisonOperator.Equal, LiteralString);
            Assert.That(_generator.GetQuery(comparison), Is.EqualTo($"'{LiteralString.Literal}' = '{LiteralString.Literal}'"));
        }

        [Test]
        public void BuildNotEqual()
        {
            var comparison = new ComparisonExpression(LiteralString, ComparisonOperator.NotEqual, LiteralString);
            Assert.That(_generator.GetQuery(comparison), Is.EqualTo($"'{LiteralString.Literal}' != '{LiteralString.Literal}'"));
        }

        [Test]
        public void BuildGreaterThan()
        {
            var comparison = new ComparisonExpression(LiteralString, ComparisonOperator.GreaterThan, LiteralString);
            Assert.That(_generator.GetQuery(comparison), Is.EqualTo($"'{LiteralString.Literal}' > '{LiteralString.Literal}'"));
        }

        [Test]
        public void BuildGreaterThanOrEqual()
        {
            var comparison = new ComparisonExpression(LiteralString, ComparisonOperator.GreaterThanOrEqual, LiteralString);
            Assert.That(_generator.GetQuery(comparison), Is.EqualTo($"'{LiteralString.Literal}' >= '{LiteralString.Literal}'"));
        }

        [Test]
        public void BuildLowerThan()
        {
            var comparison = new ComparisonExpression(LiteralString, ComparisonOperator.LowerThan, LiteralString);
            Assert.That(_generator.GetQuery(comparison), Is.EqualTo($"'{LiteralString.Literal}' < '{LiteralString.Literal}'"));
        }

        [Test]
        public void BuildLowerThanOrEqual()
        {
            var comparison = new ComparisonExpression(LiteralString, ComparisonOperator.LowerThanOrEqual, LiteralString);
            Assert.That(_generator.GetQuery(comparison), Is.EqualTo($"'{LiteralString.Literal}' <= '{LiteralString.Literal}'"));
        }

        [Test]
        public void BuildCompareUnknownOperator()
        {
            var comparison = new ComparisonExpression(LiteralString, (ComparisonOperator) 10, LiteralString);
            Assert.That(() => comparison.GetQuery(_generator), Throws.InstanceOf<NotSupportedException>());
        }

        #endregion

        #region Junction

        [Test]
        public void BuildAndExpressions()
        {
            var junction = new Junction(TestTruthy, JunctionOp.And, TestTruthy);
            Assert.That(_generator.GetQuery(junction), Is.EqualTo($"({TestTruthyExpresison}) AND ({TestTruthyExpresison})"));
        }

        [Test]
        public void BuildOrExpressions()
        {
            var junction = new Junction(TestTruthy, JunctionOp.Or, TestTruthy);
            Assert.That(_generator.GetQuery(junction), Is.EqualTo($"({TestTruthyExpresison}) OR ({TestTruthyExpresison})"));
        }

        [Test]
        public void BuildJunctionUnknownOperator()
        {
            var junction = new Junction(TestTruthy, (JunctionOp) 10, TestTruthy);
            Assert.That(() => _generator.GetQuery(junction), Throws.InstanceOf<NotSupportedException>());
        }

        #endregion

        #region Junctions

        [Test]
        public void BuildAndsExpressions()
        {
            var junctions = new Junctions(JunctionOp.And, TestTruthy, ParameterStringTruthy, ParameterNumberTruthy);
            Assert.That(_generator.GetQuery(junctions), Is.EqualTo($"(({TestTruthyExpresison}) AND ({ParameterStringTruthyExpresison}) AND ({ParameterNumberTruthyExpresison}))"));
        }

        [Test]
        public void BuildOrsExpressions()
        {
            var junctions = new Junctions(JunctionOp.Or, TestTruthy, ParameterStringTruthy, ParameterNumberTruthy);
            Assert.That(_generator.GetQuery(junctions), Is.EqualTo($"(({TestTruthyExpresison}) OR ({ParameterStringTruthyExpresison}) OR ({ParameterNumberTruthyExpresison}))"));
        }

        [Test]
        public void BuildJunctionsUnknownOperator()
        {
            var junctions = new Junctions((JunctionOp) 10, TestTruthy, TestTruthy, TestTruthy);
            Assert.That(() => _generator.GetQuery(junctions), Throws.InstanceOf<NotSupportedException>());
        }

        #endregion

        #region Is Null

        [Test]
        public void BuildIsNull()
        {
            var isNull = new IsNullExpression(TestTruthy);
            Assert.That(_generator.GetQuery(isNull), Is.EqualTo($"({TestTruthyExpresison}) IS Null"));
        }

        #endregion

        #region In

        [Test]
        public void BuildIn()
        {
            var inExpression = new InExpression(LiteralString, LiteralString);
            Assert.That(_generator.GetQuery(inExpression), Is.EqualTo($"'{LiteralString.Literal}' IN ('{LiteralString.Literal}')"));
        }

        #endregion

        #region Like

        [Test]
        public void BuildLike()
        {
            var like = new LikeExpression(LiteralString, ParameterString);
            Assert.That(_generator.GetQuery(like), Is.EqualTo($"'{LiteralString.Literal}' LIKE '{PostgreSqlGenerator.ParameterPrefix}{ParameterString.Name}'"));
        }

        [Test]
        public void BuildLikeWithLierteral()
        {
            var like = new LikeExpression(LiteralString, LiteralNumber);
            Assert.That(_generator.GetQuery(like), Is.EqualTo($"'{LiteralString.Literal}' LIKE '{LiteralNumber.Literal}'"));
        }

        [Test]
        public void BuildLikeWithStringLiteral()
        {
            var like = new LikeExpression(LiteralString, LiteralString);
            Assert.That(_generator.GetQuery(like), Is.EqualTo($"'{LiteralString.Literal}' LIKE '{LiteralString.Literal}'"));
        }

        #endregion

        #region Not

        [Test]
        public void BuildNot()
        {
            var not = new NotExpression(TestTruthy);
            Assert.That(_generator.GetQuery(not), Is.EqualTo($"NOT ({TestTruthyExpresison})"));
        }

        #endregion

        #region Where Clause

        [Test]
        public void BuildWhereClause()
        {
            var where = new WhereClause(TestTruthy);
            Assert.That(_generator.GetQuery(where), Is.EqualTo($"WHERE {TestTruthyExpresison}"));
        }

        #endregion

        #endregion

        #region Order by

        [Test]
        public void BuildSortOrderClauseAscending()
        {
            var sortOrder = new SortOrderClause(FieldReferenceWithoutTable);
            Assert.That(_generator.GetQuery(sortOrder), Is.EqualTo($"{FieldReferenceWithoutTable.FieldName} ASC"));
        }

        [Test]
        public void BuildSortOrderClauseDescending()
        {
            Assert.That(_generator.GetQuery(SortOrderDesc), Is.EqualTo($"{FieldReferenceWithTable.TableName}.{FieldReferenceWithTable.FieldName} DESC"));
        }

        [Test]
        public void BuildOrderByClause()
        {
            var sortOrder = new OrderByClause(new[] { SortOrderAsc, SortOrderDesc });
            Assert.That(_generator.GetQuery(sortOrder),
                Is.EqualTo($"ORDER BY {FieldReferenceWithTable.TableName}.{FieldReferenceWithTable.FieldName} ASC, {FieldReferenceWithTable.TableName}.{FieldReferenceWithTable.FieldName} DESC"));
        }

        #endregion

        #region Group by

        [Test]
        public void BuildGroupByClause()
        {
            var groupBy = new GroupByClause(new[] { FieldReferenceWithTable, FieldReferenceWithTable });
            Assert.That(_generator.GetQuery(groupBy),
                Is.EqualTo($"GROUP BY {FieldReferenceWithTable.TableName}.{FieldReferenceWithTable.FieldName}, {FieldReferenceWithTable.TableName}.{FieldReferenceWithTable.FieldName}"));
        }

        #endregion

        #region Value

        #region Connect Query

        [Test]
        public void BuildConnectQuery()
        {
            var connectQuery = new ConnectQueryExpression(LiteralNumber, LiteralString);
            Assert.That(_generator.GetQuery(connectQuery), Is.EqualTo($"{LiteralNumber.Literal} '{LiteralString.Literal}'"));
        }

        #endregion

        #region Field Reference

        [Test]
        public void BuildFieldReferenceWithoutTable()
        {
            var fieldReference = new FieldReferenceExpression("Name");
            Assert.That(_generator.GetQuery(fieldReference), Is.EqualTo(fieldReference.FieldName));
        }

        [Test]
        public void BuildFieldReferenceWithTable()
        {
            var fieldReference = new FieldReferenceExpression("Name", "Table");
            Assert.That(_generator.GetQuery(fieldReference), Is.EqualTo($"{fieldReference.TableName}.{fieldReference.FieldName}"));
        }

        #endregion

        #region Function Call

        [Test]
        public void BuildFunctionCallSingleParameter()
        {
            var functionCall = new FunctionCallExpression("Name", new[] { LiteralString });
            Assert.That(_generator.GetQuery(functionCall), Is.EqualTo($"{functionCall.FunctionName}('{new[] { LiteralString }[0].Literal}')"));
        }

        [Test]
        public void BuildFunctionCallMultipleParameter()
        {
            var functionCall = new FunctionCallExpression("Name", new IExpression[] { LiteralString, LiteralNumber, ParameterString });
            Assert.That(_generator.GetQuery(functionCall),
                Is.EqualTo($"{functionCall.FunctionName}('{LiteralString.Literal}', {LiteralNumber.Literal}, {PostgreSqlGenerator.ParameterPrefix}{ParameterString.Name})"));
        }

        #endregion

        #region Literal

        [Test]
        public void BuildStringLiteral()
        {
            Assert.That(_generator.GetQuery(LiteralString), Is.EqualTo($"'{LiteralString.Literal}'"));
        }
        
        [Test]
        public void BuildStringLiteraWithSingleQuote()
        {
            var stringLiteral = new LiteralExpression("It's a test for '");
            Assert.That(_generator.GetQuery(stringLiteral), Is.EqualTo("'It''s a test for '''"));
        }

        [Test]
        public void BuildNumberLiteral()
        {
            Assert.That(_generator.GetQuery(LiteralNumber), Is.EqualTo($"{LiteralNumber.Literal}"));
        }

        #endregion

        #region Parameter

        [Test]
        public void BuildNumberParameter()
        {
            Assert.That(_generator.GetQuery(ParameterNumber), Is.EqualTo($"{PostgreSqlGenerator.ParameterPrefix}{ParameterNumber.Name}"));
        }

        [Test]
        public void BuildStringParameter()
        {
            Assert.That(_generator.GetQuery(ParameterString), Is.EqualTo($"{PostgreSqlGenerator.ParameterPrefix}{ParameterString.Name}"));
        }

        #endregion
        
        #endregion

        #region ArgumentOutOfRangeException

        [Test]
        public void QueryPartUnknownBuild()
        {
            Assert.That(() => _generator.GetQuery(new NewQueryPart()), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void TableNameUnknownBuild()
        {
            Assert.That(() => _generator.GetQuery(new NewTableName()), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void ExpressionUnknownBuild()
        {
            Assert.That(() => _generator.GetQuery(new NewExpression()), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void ValueUnknownBuild()
        {
            Assert.That(() => _generator.GetQuery(new NewValue()), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void TruthyUnknownBuild()
        {
            Assert.That(() => _generator.GetQuery(new NewTruthy()), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void CreateUnknownBuild()
        {
            Assert.That(() => _generator.GetQuery(new NewCreate()), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void QueryUnknownBuild()
        {
            Assert.That(() => _generator.GetQuery(new NewQuery()), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void TableAndFieldValuesUnknownBuild()
        {
            Assert.That(() => _generator.GetQuery(new NewTableAndFieldValues()), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        #endregion

        #endregion

        #region GetParameters

        #region Create

        [Test]
        public void GetParametersBaseTypeColumnDefinition()
        {
            var columnDefinition = new BaseTypeColumnDefinition("Column", BaseType.Text);
            IEnumerable<ParameterExpression> parameter = _generator.GetParameters(columnDefinition);
            Assert.That(parameter.Any(), Is.False);
        }

        [Test]
        public void GetParametersCreateClause()
        {
            BaseTypeColumnDefinition[] columns = { "Column1".ColDefinition(BaseType.Text), "Column2".ColDefinition(BaseType.Integer, 5) };
            var create = new CreateClause("Table", false, columns);
            IEnumerable<ParameterExpression> parameter = _generator.GetParameters(create);
            Assert.That(parameter.Any(), Is.False);
        }

        #endregion

        #region Drop

        [Test]
        public void GetParametersDropClause()
        {
            var drop = new DropClause("Table");
            IEnumerable<ParameterExpression> parameter = _generator.GetParameters(drop);
            Assert.That(parameter.Any(), Is.False);
        }

        #endregion

        #region Insert

        [Test]
        public void GetParametersInsertClause()
        {
            var value1 = new FieldValue("Name1", ParameterNumber);
            var value2 = new FieldValue("Name2", ParameterString);
            var insert = new InsertClause("Table", new[] { value1, value2 });
            ParameterExpression[] parameter = _generator.GetParameters(insert).ToArray();
            Assert.That(parameter[0], Is.EqualTo(ParameterNumber));
            Assert.That(parameter[1], Is.EqualTo(ParameterString));
        }

        [Test]
        public void GetParametersInsertStatement()
        {
            var value1 = new FieldValue("Name1", ParameterString);
            var value2 = new FieldValue("Name2", ParameterNumber);
            InsertStatement insert = InsertStatement.Insert("Table", new[] { value1, value2 });
            ParameterExpression[] parameter = _generator.GetParameters(insert).ToArray();
            Assert.That(parameter[0], Is.EqualTo(ParameterString));
            Assert.That(parameter[1], Is.EqualTo(ParameterNumber));
        }

        #endregion

        #region Update

        [Test]
        public void GetParametersUpdateClause()
        {
            var value1 = new FieldValue("Name1", ParameterNumber);
            var value2 = new FieldValue("Name2", ParameterString);
            var update = new UpdateClause("Table", new[] { value1, value2 });
            ParameterExpression[] parameter = _generator.GetParameters(update).ToArray();
            Assert.That(parameter[0], Is.EqualTo(ParameterNumber));
            Assert.That(parameter[1], Is.EqualTo(ParameterString));
        }

        [Test]
        public void GetParametersUpdateStatement()
        {
            var value1 = new FieldValue("Name1", ParameterString);
            var value2 = new FieldValue("Name2", ParameterNumber);
            var update = new UpdateClause("Table", new[] { value1, value2 });
            var statement = new UpdateStatement(update);
            ParameterExpression[] parameter = _generator.GetParameters(statement).ToArray();
            Assert.That(parameter[0], Is.EqualTo(ParameterString));
            Assert.That(parameter[1], Is.EqualTo(ParameterNumber));
        }

        [Test]
        public void GetParametersUpdateStatementWithWhere()
        {
            var value1 = new FieldValue("Name1", ParameterString);
            var update = new UpdateClause("Table", new[] { value1 });
            var statement = new UpdateStatement(update);
            var where = new WhereClause(ParameterNumberTruthy);
            statement = statement.WithWhere(where);
            ParameterExpression[] parameter = _generator.GetParameters(statement).ToArray();
            Assert.That(parameter[0], Is.EqualTo(ParameterString));
            Assert.That(parameter[1], Is.EqualTo(ParameterNumber));
        }

        #endregion
        
        #region Delete

        [Test]
        public void GetParametersDeleteClause()
        {
            var delete = new DeleteClause(new TableName("Table"));
            IEnumerable<ParameterExpression> parameter = _generator.GetParameters(delete);
            Assert.That(parameter.Any(), Is.False);
        }

        [Test]
        public void GetParametersDeleteStatementWithoutWhere()
        {
            var delete = new DeleteClause(new TableName("Table"));
            var statement = new DeleteStatement(delete);
            IEnumerable<ParameterExpression> parameter = _generator.GetParameters(statement);
            Assert.That(parameter.Any, Is.False);
        }

        [Test]
        public void GetParametersDeleteStatementWithWhere()
        {
            var delete = new DeleteClause(new TableName("Table"));
            var statement = new DeleteStatement(delete);
            var where = new WhereClause(ParameterNumberTruthy);
            statement = statement.WithWhere(where);
            ParameterExpression[] parameter = _generator.GetParameters(statement).ToArray();
            Assert.That(parameter[0], Is.EqualTo(ParameterNumber));
        }

        #endregion

        #region Select

        [Test]
        public void GetParametersSelectClause()
        {
            var field1 = new FieldAliasExpression(ParameterString, "firstField");
            var field2 = new FieldAliasExpression(ParameterNumber, "SecondField");
            var select = new SelectClause(new[] { field1, field2 });
            ParameterExpression[] parameter = _generator.GetParameters(select).ToArray();
            Assert.That(parameter[0], Is.EqualTo(ParameterString));
            Assert.That(parameter[1], Is.EqualTo(ParameterNumber));
        }

        [Test]
        public void GetParametersSelectStatement()
        {
            var field1 = new FieldAliasExpression(ParameterString, "firstField");
            var field2 = new FieldAliasExpression(ParameterNumber, "SecondField");
            var select = new SelectClause(new[] { field1, field2 });
            var from = new FromClause(LeftTable);
            var statement = new SelectStatement(select, from);
            ParameterExpression[] parameter = _generator.GetParameters(statement).ToArray();
            Assert.That(parameter[0], Is.EqualTo(ParameterString));
            Assert.That(parameter[1], Is.EqualTo(ParameterNumber));
        }

        [Test]
        public void GetParametersSelectStatementWithWhere()
        {
            var field1 = new FieldAliasExpression(ParameterString, "firstField");
            var select = new SelectClause(new[] { field1 });
            var from = new FromClause(LeftTable);
            var statement = new SelectStatement(select, from);
            var where = new WhereClause(ParameterNumberTruthy);
            statement = statement.WithWhere(where);
            ParameterExpression[] parameter = _generator.GetParameters(statement).ToArray();
            Assert.That(parameter[0], Is.EqualTo(ParameterString));
            Assert.That(parameter[1], Is.EqualTo(ParameterNumber));
        }

        [Test]
        public void GetParametersSelectStatementWithOrderBy()
        {
            var field1 = new FieldAliasExpression(ParameterString, "firstField");
            var select = new SelectClause(new[] { field1 });
            var from = new FromClause(LeftTable);
            var statement = new SelectStatement(select, from);
            var orderBy = new OrderByClause(new[] { new SortOrderClause(ParameterNumber) });
            statement = statement.WithOrderBy(orderBy);
            ParameterExpression[] parameter = _generator.GetParameters(statement).ToArray();
            Assert.That(parameter[0], Is.EqualTo(ParameterString));
            Assert.That(parameter[1], Is.EqualTo(ParameterNumber));
        }

        [Test]
        public void GetParametersSelectStatementWithGroupBy()
        {
            var field1 = new FieldAliasExpression(ParameterString, "firstField");
            var select = new SelectClause(new ISelection[] { field1 });
            var from = new FromClause(LeftTable);
            var statement = new SelectStatement(select, from);
            var groupBy = new GroupByClause(new[] { ParameterNumber });
            statement = statement.WithGroupBy(groupBy);
            ParameterExpression[] parameter = _generator.GetParameters(statement).ToArray();
            Assert.That(parameter[0], Is.EqualTo(ParameterString));
            Assert.That(parameter[1], Is.EqualTo(ParameterNumber));
        }

        #endregion

        #region From

        #region Table name

        [Test]
        public void GetParametersTableName()
        {
            IEnumerable<ParameterExpression> parameter = _generator.GetParameters(LeftTable);
            Assert.That(parameter.Any(), Is.False);
        }

        #endregion

        #region Join condition

        [Test]
        public void GetParametersInnerJoinCondition()
        {
            var join = new JoinCondition(LeftTable, JoinClause.Inner, RightTable, ParameterNumberTruthy);
            ParameterExpression[] parameter = _generator.GetParameters(join).ToArray();
            Assert.That(parameter[0], Is.EqualTo(ParameterNumber));
        }

        [Test]
        public void GetParametersLeftJoinCondition()
        {
            var join = new JoinCondition(LeftTable, JoinClause.Left, RightTable, ParameterNumberTruthy);
            ParameterExpression[] parameter = _generator.GetParameters(join).ToArray();
            Assert.That(parameter[0], Is.EqualTo(ParameterNumber));
        }

        [Test]
        public void GetParametersRightJoinCondition()
        {
            var join = new JoinCondition(LeftTable, JoinClause.Right, RightTable, ParameterNumberTruthy);
            ParameterExpression[] parameter = _generator.GetParameters(join).ToArray();
            Assert.That(parameter[0], Is.EqualTo(ParameterNumber));
        }

        [Test]
        public void GetParametersFullJoinCondition()
        {
            var join = new JoinCondition(LeftTable, JoinClause.Full, RightTable, ParameterNumberTruthy);
            ParameterExpression[] parameter = _generator.GetParameters(join).ToArray();
            Assert.That(parameter[0], Is.EqualTo(ParameterNumber));
        }

        [Test]
        public void GetParametersLeftOuterJoinCondition()
        {
            var join = new JoinCondition(LeftTable, JoinClause.LeftOuter, RightTable, ParameterNumberTruthy);
            ParameterExpression[] parameter = _generator.GetParameters(join).ToArray();
            Assert.That(parameter[0], Is.EqualTo(ParameterNumber));
        }

        [Test]
        public void GetParametersRightOuterJoinCondition()
        {
            var join = new JoinCondition(LeftTable, JoinClause.RightOuter, RightTable, ParameterNumberTruthy);
            ParameterExpression[] parameter = _generator.GetParameters(join).ToArray();
            Assert.That(parameter[0], Is.EqualTo(ParameterNumber));
        }

        [Test]
        public void GetParametersFullOuterJoinCondition()
        {
            var join = new JoinCondition(LeftTable, JoinClause.FullOuter, RightTable, ParameterNumberTruthy);
            ParameterExpression[] parameter = _generator.GetParameters(join).ToArray();
            Assert.That(parameter[0], Is.EqualTo(ParameterNumber));
        }

        #endregion

        #region From Clause

        [Test]
        public void GetParametersFromClauseWithJoin()
        {
            var join = new JoinCondition(LeftTable, JoinClause.Inner, RightTable, ParameterNumberTruthy);
            var from = new FromClause(join);
            ParameterExpression[] parameter = _generator.GetParameters(from).ToArray();
            Assert.That(parameter[0], Is.EqualTo(ParameterNumber));
        }

        #endregion

        #endregion

        #region Where

        #region Comparison operator

        [Test]
        public void GetParametersEqual()
        {
            var comparison = new ComparisonExpression(ParameterNumber, ComparisonOperator.Equal, ParameterString);
            ParameterExpression[] parameter = _generator.GetParameters(comparison).ToArray();
            Assert.That(parameter[0], Is.EqualTo(ParameterNumber));
            Assert.That(parameter[1], Is.EqualTo(ParameterString));
        }

        #endregion

        #region Junction

        [Test]
        public void GetParametersAndExpressions()
        {
            var junction = new Junction(ParameterNumberTruthy, JunctionOp.And, ParameterStringTruthy);
            ParameterExpression[] parameter = _generator.GetParameters(junction).ToArray();
            Assert.That(parameter[0], Is.EqualTo(ParameterNumber));
            Assert.That(parameter[1], Is.EqualTo(ParameterString));
        }

        #endregion

        #region Junctions

        [Test]
        public void GetParametersAndsExpressions()
        {
            var junctions = new Junctions(JunctionOp.And, ParameterNumberTruthy, ParameterStringTruthy);
            ParameterExpression[] parameter = _generator.GetParameters(junctions).ToArray();
            Assert.That(parameter[0], Is.EqualTo(ParameterNumber));
            Assert.That(parameter[1], Is.EqualTo(ParameterString));
        }

        #endregion

        #region Is Null

        [Test]
        public void GetParametersIsNull()
        {
            var isNull = new IsNullExpression(ParameterNumber);
            ParameterExpression[] parameter = _generator.GetParameters(isNull).ToArray();
            Assert.That(parameter[0], Is.EqualTo(ParameterNumber));
        }

        #endregion

        #region In

        [Test]
        public void GetParametersIn()
        {
            var inExpression = new InExpression(ParameterStringTruthy, ParameterNumberTruthy);
            ParameterExpression[] parameter = _generator.GetParameters(inExpression).ToArray();
            Assert.That(parameter[0], Is.EqualTo(ParameterString));
            Assert.That(parameter[1], Is.EqualTo(ParameterNumber));
        }

        #endregion

        #region Like

        [Test]
        public void GetParametersLike()
        {
            var like = new LikeExpression(ParameterString, ParameterNumber);
            ParameterExpression[] parameter = _generator.GetParameters(like).ToArray();
            Assert.That(parameter[0], Is.EqualTo(ParameterString));
            Assert.That(parameter[1], Is.EqualTo(ParameterNumber));
        }

        #endregion

        #region Not

        [Test]
        public void GetParametersNot()
        {
            var not = new NotExpression(ParameterStringTruthy);
            ParameterExpression[] parameter = _generator.GetParameters(not).ToArray();
            Assert.That(parameter[0], Is.EqualTo(ParameterString));
        }

        #endregion

        #region Where Clause

        [Test]
        public void GetParametersWhereClause()
        {
            var where = new WhereClause(ParameterStringTruthy);
            ParameterExpression[] parameter = _generator.GetParameters(where).ToArray();
            Assert.That(parameter[0], Is.EqualTo(ParameterString));
        }

        #endregion

        #endregion

        #region Order by

        [Test]
        public void GetParametersSortOrderClauseAscendingWithoutTableName()
        {
            var sortOrder = new SortOrderClause(ParameterNumber);
            ParameterExpression[] parameter = _generator.GetParameters(sortOrder).ToArray();
            Assert.That(parameter[0], Is.EqualTo(ParameterNumber));
        }

        [Test]
        public void GetParametersOrderByClause()
        {
            var sortOrderNumber = new SortOrderClause(ParameterNumber);
            var sortOrderString = new SortOrderClause(ParameterString);
            var sortOrder = new OrderByClause(new[] { sortOrderNumber, sortOrderString });
            ParameterExpression[] parameter = _generator.GetParameters(sortOrder).ToArray();
            Assert.That(parameter[0], Is.EqualTo(ParameterNumber));
            Assert.That(parameter[1], Is.EqualTo(ParameterString));
        }

        #endregion

        #region Group by

        [Test]
        public void GetParametersGroupByClause()
        {
            var groupBy = new GroupByClause(new[] { ParameterNumber, ParameterString });
            ParameterExpression[] parameter = _generator.GetParameters(groupBy).ToArray();
            Assert.That(parameter[0], Is.EqualTo(ParameterNumber));
            Assert.That(parameter[1], Is.EqualTo(ParameterString));
        }

        #endregion

        #region Value

        #region Connect Query

        [Test]
        public void GetParametersConnectQuery()
        {
            var connectQuery = new ConnectQueryExpression(ParameterNumber, ParameterString);
            ParameterExpression[] parameter = _generator.GetParameters(connectQuery).ToArray();
            Assert.That(parameter[0], Is.EqualTo(ParameterNumber));
            Assert.That(parameter[1], Is.EqualTo(ParameterString));
        }

        #endregion

        #region Field Reference

        [Test]
        public void GetParametersFieldReference()
        {
            var fieldReference = new FieldReferenceExpression("Name");
            IEnumerable<ParameterExpression> parameter = _generator.GetParameters(fieldReference);
            Assert.That(parameter.Any(), Is.False);
        }

        #endregion

        #region Function Call

        [Test]
        public void GetParametersFunctionCallSingleParameter()
        {
            var functionCall = new FunctionCallExpression("Name", new[] { ParameterString });
            ParameterExpression[] parameter = _generator.GetParameters(functionCall).ToArray();
            Assert.That(parameter[0], Is.EqualTo(ParameterString));
        }

        [Test]
        public void GetParametersFunctionCallMultipleParameter()
        {
            var functionCall = new FunctionCallExpression("Name", new IExpression[] { ParameterString, ParameterNumber });
            ParameterExpression[] parameter = _generator.GetParameters(functionCall).ToArray();
            Assert.That(parameter[0], Is.EqualTo(ParameterString));
            Assert.That(parameter[1], Is.EqualTo(ParameterNumber));
        }

        #endregion

        #region Parameter

        [Test]
        public void GetParametersNumberParameter()
        {
            ParameterExpression[] parameter = _generator.GetParameters(ParameterNumber).ToArray();
            Assert.That(parameter[0].Name, Is.EqualTo(ParameterNumber.Name));
            Assert.That(parameter[0].Parameter, Is.EqualTo(ParameterNumber.Parameter));
        }

        [Test]
        public void GetParametersStringParameter()
        {
            ParameterExpression[] parameter = _generator.GetParameters(ParameterString).ToArray();
            Assert.That(parameter[0].Name, Is.EqualTo($"{ParameterString.Name}"));
            Assert.That(parameter[0].Parameter, Is.EqualTo($"{ParameterString.Parameter}"));
        }

        #endregion

        #region Literal

        [Test]
        public void GetParametersLiteral()
        {
            IEnumerable<ParameterExpression> parameter = _generator.GetParameters(LiteralString);
            Assert.That(parameter.Any(), Is.False);
        }

        #endregion

        #endregion
        
        #region ArgumentOutOfRangeException

        [Test]
        public void QueryPartUnknownGetParmeters()
        {
            Assert.That(() => _generator.GetParameters(new NewQueryPart()), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void TableNameUnknownGetParmeters()
        {
            Assert.That(() => _generator.GetParameters(new NewTableName()), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void ExpressionUnknownGetParmeters()
        {
            Assert.That(() => _generator.GetParameters(new NewExpression()), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void ValueUnknownGetParmeters()
        {
            Assert.That(() => _generator.GetParameters(new NewValue()), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void TruthyUnknownGetParmeters()
        {
            Assert.That(() => _generator.GetParameters(new NewTruthy()), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void QueryUnknownGetParmeters()
        {
            Assert.That(() => _generator.GetParameters(new NewQuery()), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        #endregion

        #endregion
    }
}