//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.13.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from ./Constraint.g4 by ANTLR 4.13.1

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

using Antlr4.Runtime.Misc;
using IParseTreeListener = Antlr4.Runtime.Tree.IParseTreeListener;
using IToken = Antlr4.Runtime.IToken;

/// <summary>
/// This interface defines a complete listener for a parse tree produced by
/// <see cref="ConstraintParser"/>.
/// </summary>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.13.1")]
[System.CLSCompliant(false)]
public interface IConstraintListener : IParseTreeListener {
	/// <summary>
	/// Enter a parse tree produced by <see cref="ConstraintParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterExpr([NotNull] ConstraintParser.ExprContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ConstraintParser.expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitExpr([NotNull] ConstraintParser.ExprContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ConstraintParser.implyExpr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterImplyExpr([NotNull] ConstraintParser.ImplyExprContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ConstraintParser.implyExpr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitImplyExpr([NotNull] ConstraintParser.ImplyExprContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ConstraintParser.orExpr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterOrExpr([NotNull] ConstraintParser.OrExprContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ConstraintParser.orExpr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitOrExpr([NotNull] ConstraintParser.OrExprContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ConstraintParser.andExpr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterAndExpr([NotNull] ConstraintParser.AndExprContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ConstraintParser.andExpr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitAndExpr([NotNull] ConstraintParser.AndExprContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ConstraintParser.primaryExpr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterPrimaryExpr([NotNull] ConstraintParser.PrimaryExprContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ConstraintParser.primaryExpr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitPrimaryExpr([NotNull] ConstraintParser.PrimaryExprContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ConstraintParser.trueExpr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterTrueExpr([NotNull] ConstraintParser.TrueExprContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ConstraintParser.trueExpr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitTrueExpr([NotNull] ConstraintParser.TrueExprContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ConstraintParser.falseExpr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterFalseExpr([NotNull] ConstraintParser.FalseExprContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ConstraintParser.falseExpr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitFalseExpr([NotNull] ConstraintParser.FalseExprContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ConstraintParser.parenExpr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterParenExpr([NotNull] ConstraintParser.ParenExprContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ConstraintParser.parenExpr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitParenExpr([NotNull] ConstraintParser.ParenExprContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ConstraintParser.notParenExpr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterNotParenExpr([NotNull] ConstraintParser.NotParenExprContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ConstraintParser.notParenExpr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitNotParenExpr([NotNull] ConstraintParser.NotParenExprContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ConstraintParser.statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterStatement([NotNull] ConstraintParser.StatementContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ConstraintParser.statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitStatement([NotNull] ConstraintParser.StatementContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ConstraintParser.eqValStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterEqValStatement([NotNull] ConstraintParser.EqValStatementContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ConstraintParser.eqValStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitEqValStatement([NotNull] ConstraintParser.EqValStatementContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ConstraintParser.notEqValStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterNotEqValStatement([NotNull] ConstraintParser.NotEqValStatementContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ConstraintParser.notEqValStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitNotEqValStatement([NotNull] ConstraintParser.NotEqValStatementContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ConstraintParser.inListStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterInListStatement([NotNull] ConstraintParser.InListStatementContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ConstraintParser.inListStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitInListStatement([NotNull] ConstraintParser.InListStatementContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ConstraintParser.notInListStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterNotInListStatement([NotNull] ConstraintParser.NotInListStatementContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ConstraintParser.notInListStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitNotInListStatement([NotNull] ConstraintParser.NotInListStatementContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ConstraintParser.lst"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterLst([NotNull] ConstraintParser.LstContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ConstraintParser.lst"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitLst([NotNull] ConstraintParser.LstContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ConstraintParser.val"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterVal([NotNull] ConstraintParser.ValContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ConstraintParser.val"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitVal([NotNull] ConstraintParser.ValContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ConstraintParser.strVal"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterStrVal([NotNull] ConstraintParser.StrValContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ConstraintParser.strVal"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitStrVal([NotNull] ConstraintParser.StrValContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="ConstraintParser.nullVal"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterNullVal([NotNull] ConstraintParser.NullValContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="ConstraintParser.nullVal"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitNullVal([NotNull] ConstraintParser.NullValContext context);
}
