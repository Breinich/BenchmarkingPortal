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
using IErrorNode = Antlr4.Runtime.Tree.IErrorNode;
using ITerminalNode = Antlr4.Runtime.Tree.ITerminalNode;
using IToken = Antlr4.Runtime.IToken;
using ParserRuleContext = Antlr4.Runtime.ParserRuleContext;

/// <summary>
/// This class provides an empty implementation of <see cref="IConstraintListener"/>,
/// which can be extended to create a listener which only needs to handle a subset
/// of the available methods.
/// </summary>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.13.1")]
[System.Diagnostics.DebuggerNonUserCode]
[System.CLSCompliant(false)]
public partial class ConstraintBaseListener : IConstraintListener {
	/// <summary>
	/// Enter a parse tree produced by <see cref="ConstraintParser.expr"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterExpr([NotNull] ConstraintParser.ExprContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="ConstraintParser.expr"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitExpr([NotNull] ConstraintParser.ExprContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="ConstraintParser.implyExpr"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterImplyExpr([NotNull] ConstraintParser.ImplyExprContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="ConstraintParser.implyExpr"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitImplyExpr([NotNull] ConstraintParser.ImplyExprContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="ConstraintParser.orExpr"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterOrExpr([NotNull] ConstraintParser.OrExprContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="ConstraintParser.orExpr"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitOrExpr([NotNull] ConstraintParser.OrExprContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="ConstraintParser.andExpr"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterAndExpr([NotNull] ConstraintParser.AndExprContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="ConstraintParser.andExpr"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitAndExpr([NotNull] ConstraintParser.AndExprContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="ConstraintParser.primaryExpr"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterPrimaryExpr([NotNull] ConstraintParser.PrimaryExprContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="ConstraintParser.primaryExpr"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitPrimaryExpr([NotNull] ConstraintParser.PrimaryExprContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="ConstraintParser.trueExpr"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterTrueExpr([NotNull] ConstraintParser.TrueExprContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="ConstraintParser.trueExpr"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitTrueExpr([NotNull] ConstraintParser.TrueExprContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="ConstraintParser.falseExpr"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterFalseExpr([NotNull] ConstraintParser.FalseExprContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="ConstraintParser.falseExpr"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitFalseExpr([NotNull] ConstraintParser.FalseExprContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="ConstraintParser.parenExpr"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterParenExpr([NotNull] ConstraintParser.ParenExprContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="ConstraintParser.parenExpr"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitParenExpr([NotNull] ConstraintParser.ParenExprContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="ConstraintParser.notParenExpr"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterNotParenExpr([NotNull] ConstraintParser.NotParenExprContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="ConstraintParser.notParenExpr"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitNotParenExpr([NotNull] ConstraintParser.NotParenExprContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="ConstraintParser.statement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterStatement([NotNull] ConstraintParser.StatementContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="ConstraintParser.statement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitStatement([NotNull] ConstraintParser.StatementContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="ConstraintParser.eqValStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterEqValStatement([NotNull] ConstraintParser.EqValStatementContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="ConstraintParser.eqValStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitEqValStatement([NotNull] ConstraintParser.EqValStatementContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="ConstraintParser.notEqValStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterNotEqValStatement([NotNull] ConstraintParser.NotEqValStatementContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="ConstraintParser.notEqValStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitNotEqValStatement([NotNull] ConstraintParser.NotEqValStatementContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="ConstraintParser.inListStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterInListStatement([NotNull] ConstraintParser.InListStatementContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="ConstraintParser.inListStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitInListStatement([NotNull] ConstraintParser.InListStatementContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="ConstraintParser.notInListStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterNotInListStatement([NotNull] ConstraintParser.NotInListStatementContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="ConstraintParser.notInListStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitNotInListStatement([NotNull] ConstraintParser.NotInListStatementContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="ConstraintParser.lst"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterLst([NotNull] ConstraintParser.LstContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="ConstraintParser.lst"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitLst([NotNull] ConstraintParser.LstContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="ConstraintParser.val"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterVal([NotNull] ConstraintParser.ValContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="ConstraintParser.val"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitVal([NotNull] ConstraintParser.ValContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="ConstraintParser.strVal"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterStrVal([NotNull] ConstraintParser.StrValContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="ConstraintParser.strVal"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitStrVal([NotNull] ConstraintParser.StrValContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="ConstraintParser.nullVal"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterNullVal([NotNull] ConstraintParser.NullValContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="ConstraintParser.nullVal"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitNullVal([NotNull] ConstraintParser.NullValContext context) { }

	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void EnterEveryRule([NotNull] ParserRuleContext context) { }
	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void ExitEveryRule([NotNull] ParserRuleContext context) { }
	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void VisitTerminal([NotNull] ITerminalNode node) { }
	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void VisitErrorNode([NotNull] IErrorNode node) { }
}
