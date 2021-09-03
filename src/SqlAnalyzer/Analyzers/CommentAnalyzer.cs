using Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;
using Microsoft.SqlServer.Management.SqlParser.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlAnalyzer.DTO;

namespace SqlAnalyzer.Analyzers
{
    public class CommentAnalyzer : IAnalyzer
    {
        private const string Message = "Commented code should by removed";
        private readonly char[] multiComment = "\r\n\t /*".ToCharArray();
        private readonly char[] singleComment = "\r\n\t -".ToCharArray();

        public string Name => "Comment analyzer";

        public IEnumerable<DiagnosticMessage> Analyze(SqlScript script)
        {
            foreach (var comment in script.Tokens.Where(k => k.Id == (int)Tokens.LEX_END_OF_LINE_COMMENT || k.Id == (int)Tokens.LEX_MULTILINE_COMMENT))
            {
                var text = comment.Text.Trim(comment.Id == (int)Tokens.LEX_MULTILINE_COMMENT ? multiComment : singleComment);
                if (string.IsNullOrEmpty(text))
                    continue;
                var result = Parser.Parse(text);
                if (!result.Errors.Any())
                {
                    CommentVisitor visitor = new CommentVisitor();
                    visitor.Visit(result.Script);
                    if (visitor.Count > 0)
                    {
                        yield return DiagnosticMessage.Warning(new Span(comment.StartLocation.Offset, comment.Text.Length), Message);
                    }
                }
            }
        }
    }

    class CommentVisitor : SqlCodeObjectRecursiveVisitor
    {
        internal int Count { get; private set; } = 0;
        public override void Visit(SqlUnqualifiedJoinTableExpression codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlUnpivotTableExpression codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlUnpivotClause codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlUniqueConstraint codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlUnaryScalarExpression codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlUdtStaticMethodExpression codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlUpdateBooleanExpression codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlUdtStaticDataMemberExpression codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlUdtInstanceDataMemberExpression codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlTriggerEvent codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlTriggerDefinition codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlTriggerAction codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlTopSpecification codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlTemporalPeriodDefinition codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlUdtInstanceMethodExpression codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlTargetTableExpression codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlUpdateMergeAction codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlUserDefinedScalarFunctionCallExpression codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlBackupLogStatement codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlBackupDatabaseStatement codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlBackupCertificateStatement codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlAlterViewStatement codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlAlterTriggerStatement codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlAlterProcedureStatement codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlUpdateSpecification codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlAlterLoginStatement codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlXmlNamespacesDeclaration codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlWhereClause codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlViewDefinition codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlVariableDeclaration codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlVariableColumnAssignment codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlValuesInsertMergeActionSource codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlAlterFunctionStatement codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlBackupMasterKeyStatement codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlTableUdtInstanceMethodExpression codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlTableValuedFunctionRefExpression codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlSetClause codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlSelectVariableAssignmentExpression codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlSelectStarExpression codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlSelectSpecificationInsertSource codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlSelectSpecification codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlSelectScalarExpression codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlSimpleCaseExpression codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlSelectIntoClause codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlSearchedWhenClause codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlSearchedCaseExpression codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlScript codeObject) { Count = 0; base.Visit(codeObject); }
        public override void Visit(SqlScalarVariableRefExpression codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlScalarVariableAssignment codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlScalarSubQueryExpression codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlSelectClause codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlTableVariableRefExpression codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlSimpleGroupByItem codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlSimpleOrderByItem codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlTableRefExpression codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlTableHint codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlTableFunctionReturnType codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlTableDefinition codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlTableConstructorInsertSource codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlTableConstructorExpression codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlSimpleOrderByClause codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlTableClrFunctionDefinition codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlStatisticsNoRecomputeIndexOption codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlStatisticsIncrementalIndexOption codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlSortInTempDbIndexOption codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlSortedDataReorgIndexOption codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlSortedDataIndexOption codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlSimpleWhenClause codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlStatisticsOnlyIndexOption codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlBackupServiceMasterKeyStatement codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlBackupTableStatement codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlBreakStatement codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlInlineTableVariableDeclareStatement codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlIfElseStatement codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlGrantStatement codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlExecuteStringStatement codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlDropViewStatement codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlInsertStatement codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlDropUserStatement codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlDropTriggerStatement codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlDropTableStatement codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlDropSynonymStatement codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlDropSequenceStatement codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlDropSecurityPolicyStatement codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlDropSchemaStatement codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlDropTypeStatement codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlDropRuleStatement codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlMergeStatement codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlRestoreDatabaseStatement codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlVariableDeclareStatement codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlUseStatement codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlUpdateStatement codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlTryCatchStatement codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlSetStatement codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlSetAssignmentStatement codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlStatement codeObject) 
        {
            if (!codeObject.Tokens.All(k => k.Id == (int)Tokens.TOKEN_LABEL))
                Count++;
            base.Visit(codeObject); 
        }
        public override void Visit(SqlSelectStatement codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlReturnStatement codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlRestoreTableStatement codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlRestoreServiceMasterKeyStatement codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlRestoreMasterKeyStatement codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlRestoreLogStatement codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlRestoreInformationStatement codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlRevokeStatement codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlDropProcedureStatement codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlDropLoginStatement codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlDropFunctionStatement codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlCreateTableStatement codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlCreateSynonymStatement codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlCreateSchemaStatement codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlCreateRoleStatement codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlCreateProcedureStatement codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlCreateLoginWithPasswordStatement codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlCreateTriggerStatement codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlCreateLoginFromWindowsStatement codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlCreateLoginFromAsymKeyStatement codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlCreateIndexStatement codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlCreateFunctionStatement codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlContinueStatement codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlCompoundStatement codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlCommentStatement codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlCreateLoginFromCertificateStatement codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlCreateUserDefinedDataTypeStatement codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlCreateUserDefinedTableTypeStatement codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlCreateUserDefinedTypeStatement codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlDropDefaultStatement codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlDropDatabaseStatement codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlDropAggregateStatement codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlDenyStatement codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlDeleteStatement codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlDBCCStatement codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlCursorDeclareStatement codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlCreateViewStatement codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlCreateUserWithoutLoginStatement codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlCreateUserStatement codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlCreateUserFromExternalProviderStatement codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlCreateUserFromLoginStatement codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlCreateUserWithImplicitAuthenticationStatement codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlCreateUserFromCertificateStatement codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlCreateUserFromAsymKeyStatement codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlScalarRelationalFunctionDefinition codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlWhileStatement codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlScalarRefExpression codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlScalarClrFunctionDefinition codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlDmlSpecificationTableSource codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlDerivedTableExpression codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlDeleteSpecification codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlDeleteMergeAction codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlDefaultValuesInsertSource codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlDefaultValuesInsertMergeActionSource codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlDmlTriggerDefinition codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlDdlTriggerDefinition codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlDataType codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlDataCompressionIndexOption codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlCursorVariableRefExpression codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlCursorVariableAssignment codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlCursorOption codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlCubeGroupByItem codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlDataTypeSpecification codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlCreateUserOption codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlDropExistingIndexOption codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlExecuteAsClause codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlFullTextColumn codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlFullTextBooleanExpression codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlFromClause codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlForXmlRawClause codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlForXmlPathClause codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlForXmlExplicitClause codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlForXmlDirective codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlForXmlAutoClause codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlForeignKeyConstraint codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlForBrowseClause codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlFilterClause codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlFillFactorIndexOption codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlExistsBooleanExpression codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlForXmlClause codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlFunctionDefinition codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlConvertExpression codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlConditionClause codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlBuiltinScalarFunctionCallExpression codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlBooleanFilterExpression codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlBooleanExpression codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlBinaryScalarExpression codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlBinaryQueryExpression codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlBinaryFilterExpression codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlCastExpression codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlBinaryBooleanExpression codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlAtTimeZoneExpression codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlAssignment codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlAllowRowLocksIndexOption codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlAllowPageLocksIndexOption codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlAllAnyComparisonBooleanExpression codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlBetweenBooleanExpression codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlConstraint codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlChangeTrackingContext codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlClrAssemblySpecifier codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlComputedColumnDefinition codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlCompressionPartitionRange codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlComparisonBooleanExpression codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlCommonTableExpression codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlColumnRefExpression codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlColumnIdentity codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlCheckConstraint codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlColumnDefinition codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlColumnAssignment codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlCollation codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlCollateScalarExpression codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlClrMethodSpecifier codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlClrFunctionBodyDefinition codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlClrClassSpecifier codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlDefaultConstraint codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlGlobalScalarVariableRefExpression codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlGrandTotalGroupByItem codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlGrandTotalGroupingSet codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlCompressionDelayIndexOption codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlBucketCountIndexOption codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlResumableIndexOption codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlOptimizeForSequentialKeyIndexOption codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlOnlineIndexOption codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlMaxDurationIndexOption codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlScalarExpression codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlQueryExpression codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlNotBooleanExpression codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlMultistatementTableRelationalFunctionDefinition codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlMultistatementFunctionBodyDefinition codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlModuleViewMetadataOption codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlTableExpression codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlModuleSchemaBindingOption codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlOffsetFetchClause codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlOrderByItem codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlRowConstructorExpression codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlRollupGroupByItem codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlQueryWithClause codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlQuerySpecification codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlQualifiedJoinTableExpression codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlProcedureDefinition codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlOrderByClause codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlStorageSpecification codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlPivotTableExpression codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlPivotClause codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlParameterDeclaration codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlPadIndexOption codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlOutputIntoClause codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlOutputClause codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlPrimaryKeyConstraint codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlModuleReturnsNullOnNullInputOption codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlModuleRecompileOption codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlModuleOption codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlInlineIndexConstraint codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlIndexOption codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlIndexHint codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlIndexedColumn codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlInBooleanExpressionQueryValue codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlInBooleanExpressionCollectionValue codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlInlineFunctionBodyDefinition codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlInBooleanExpression codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlIdentityFunctionCallExpression codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlHavingClause codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlGroupingSetItemsCollection codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlGroupBySets codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlGroupByClause codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlIgnoreDupKeyIndexOption codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlInlineTableRelationalFunctionDefinition codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlInlineTableVariableDeclaration codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlInsertMergeAction codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlModuleNativeCompilationOption codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlModuleExecuteAsOption codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlModuleEncryptionOption codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlModuleCalledOnNullInputOption codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlInsertSource codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlMergeSpecification codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlMergeActionClause codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlMaxDegreeOfParallelismIndexOption codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlLoginPassword codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlLikeBooleanExpression codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlLargeDataStorageInformation codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlIsNullBooleanExpression codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlIntoClause codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlInsertSpecification codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlScalarFunctionReturnType codeObject) { Count++; base.Visit(codeObject); }
        public override void Visit(SqlAggregateFunctionCallExpression codeObject) { Count++; base.Visit(codeObject); }
    }
}
