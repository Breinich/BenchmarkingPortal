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

using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using DFA = Antlr4.Runtime.Dfa.DFA;

[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.13.1")]
[System.CLSCompliant(false)]
public partial class ConstraintParser : Parser {
	protected static DFA[] decisionToDFA;
	protected static PredictionContextCache sharedContextCache = new PredictionContextCache();
	public const int
		T__0=1, T__1=2, T__2=3, T__3=4, T__4=5, T__5=6, T__6=7, T__7=8, T__8=9, 
		T__9=10, T__10=11, T__11=12, T__12=13, NULL=14, STR=15, WS=16;
	public const int
		RULE_expr = 0, RULE_implyExpr = 1, RULE_orExpr = 2, RULE_andExpr = 3, 
		RULE_primaryExpr = 4, RULE_trueExpr = 5, RULE_falseExpr = 6, RULE_parenExpr = 7, 
		RULE_notParenExpr = 8, RULE_statement = 9, RULE_eqValStatement = 10, RULE_notEqValStatement = 11, 
		RULE_inListStatement = 12, RULE_notInListStatement = 13, RULE_lst = 14, 
		RULE_val = 15, RULE_strVal = 16, RULE_nullVal = 17;
	public static readonly string[] ruleNames = {
		"expr", "implyExpr", "orExpr", "andExpr", "primaryExpr", "trueExpr", "falseExpr", 
		"parenExpr", "notParenExpr", "statement", "eqValStatement", "notEqValStatement", 
		"inListStatement", "notInListStatement", "lst", "val", "strVal", "nullVal"
	};

	private static readonly string[] _LiteralNames = {
		null, "'->'", "'|'", "'&'", "'true'", "'false'", "'('", "')'", "'!('", 
		"'='", "'!='", "'['", "','", "']'", "'null'"
	};
	private static readonly string[] _SymbolicNames = {
		null, null, null, null, null, null, null, null, null, null, null, null, 
		null, null, "NULL", "STR", "WS"
	};
	public static readonly IVocabulary DefaultVocabulary = new Vocabulary(_LiteralNames, _SymbolicNames);

	[NotNull]
	public override IVocabulary Vocabulary
	{
		get
		{
			return DefaultVocabulary;
		}
	}

	public override string GrammarFileName { get { return "Constraint.g4"; } }

	public override string[] RuleNames { get { return ruleNames; } }

	public override int[] SerializedAtn { get { return _serializedATN; } }

	static ConstraintParser() {
		decisionToDFA = new DFA[_ATN.NumberOfDecisions];
		for (int i = 0; i < _ATN.NumberOfDecisions; i++) {
			decisionToDFA[i] = new DFA(_ATN.GetDecisionState(i), i);
		}
	}

		public ConstraintParser(ITokenStream input) : this(input, Console.Out, Console.Error) { }

		public ConstraintParser(ITokenStream input, TextWriter output, TextWriter errorOutput)
		: base(input, output, errorOutput)
	{
		Interpreter = new ParserATNSimulator(this, _ATN, decisionToDFA, sharedContextCache);
	}

	public partial class ExprContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public ImplyExprContext implyExpr() {
			return GetRuleContext<ImplyExprContext>(0);
		}
		public ExprContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_expr; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override void EnterRule(IParseTreeListener listener) {
			IConstraintListener typedListener = listener as IConstraintListener;
			if (typedListener != null) typedListener.EnterExpr(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override void ExitRule(IParseTreeListener listener) {
			IConstraintListener typedListener = listener as IConstraintListener;
			if (typedListener != null) typedListener.ExitExpr(this);
		}
	}

	[RuleVersion(0)]
	public ExprContext expr() {
		ExprContext _localctx = new ExprContext(Context, State);
		EnterRule(_localctx, 0, RULE_expr);
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 36;
			implyExpr();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class ImplyExprContext : ParserRuleContext {
		public OrExprContext leftOp;
		public OrExprContext rightOp;
		[System.Diagnostics.DebuggerNonUserCode] public OrExprContext[] orExpr() {
			return GetRuleContexts<OrExprContext>();
		}
		[System.Diagnostics.DebuggerNonUserCode] public OrExprContext orExpr(int i) {
			return GetRuleContext<OrExprContext>(i);
		}
		public ImplyExprContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_implyExpr; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override void EnterRule(IParseTreeListener listener) {
			IConstraintListener typedListener = listener as IConstraintListener;
			if (typedListener != null) typedListener.EnterImplyExpr(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override void ExitRule(IParseTreeListener listener) {
			IConstraintListener typedListener = listener as IConstraintListener;
			if (typedListener != null) typedListener.ExitImplyExpr(this);
		}
	}

	[RuleVersion(0)]
	public ImplyExprContext implyExpr() {
		ImplyExprContext _localctx = new ImplyExprContext(Context, State);
		EnterRule(_localctx, 2, RULE_implyExpr);
		try {
			State = 43;
			ErrorHandler.Sync(this);
			switch ( Interpreter.AdaptivePredict(TokenStream,0,Context) ) {
			case 1:
				EnterOuterAlt(_localctx, 1);
				{
				State = 38;
				orExpr();
				}
				break;
			case 2:
				EnterOuterAlt(_localctx, 2);
				{
				State = 39;
				_localctx.leftOp = orExpr();
				State = 40;
				Match(T__0);
				State = 41;
				_localctx.rightOp = orExpr();
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class OrExprContext : ParserRuleContext {
		public AndExprContext leftOp;
		public AndExprContext rightOp;
		[System.Diagnostics.DebuggerNonUserCode] public AndExprContext[] andExpr() {
			return GetRuleContexts<AndExprContext>();
		}
		[System.Diagnostics.DebuggerNonUserCode] public AndExprContext andExpr(int i) {
			return GetRuleContext<AndExprContext>(i);
		}
		public OrExprContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_orExpr; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override void EnterRule(IParseTreeListener listener) {
			IConstraintListener typedListener = listener as IConstraintListener;
			if (typedListener != null) typedListener.EnterOrExpr(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override void ExitRule(IParseTreeListener listener) {
			IConstraintListener typedListener = listener as IConstraintListener;
			if (typedListener != null) typedListener.ExitOrExpr(this);
		}
	}

	[RuleVersion(0)]
	public OrExprContext orExpr() {
		OrExprContext _localctx = new OrExprContext(Context, State);
		EnterRule(_localctx, 4, RULE_orExpr);
		try {
			State = 50;
			ErrorHandler.Sync(this);
			switch ( Interpreter.AdaptivePredict(TokenStream,1,Context) ) {
			case 1:
				EnterOuterAlt(_localctx, 1);
				{
				State = 45;
				andExpr();
				}
				break;
			case 2:
				EnterOuterAlt(_localctx, 2);
				{
				State = 46;
				_localctx.leftOp = andExpr();
				State = 47;
				Match(T__1);
				State = 48;
				_localctx.rightOp = andExpr();
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class AndExprContext : ParserRuleContext {
		public PrimaryExprContext leftOp;
		public PrimaryExprContext rightOp;
		[System.Diagnostics.DebuggerNonUserCode] public PrimaryExprContext[] primaryExpr() {
			return GetRuleContexts<PrimaryExprContext>();
		}
		[System.Diagnostics.DebuggerNonUserCode] public PrimaryExprContext primaryExpr(int i) {
			return GetRuleContext<PrimaryExprContext>(i);
		}
		public AndExprContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_andExpr; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override void EnterRule(IParseTreeListener listener) {
			IConstraintListener typedListener = listener as IConstraintListener;
			if (typedListener != null) typedListener.EnterAndExpr(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override void ExitRule(IParseTreeListener listener) {
			IConstraintListener typedListener = listener as IConstraintListener;
			if (typedListener != null) typedListener.ExitAndExpr(this);
		}
	}

	[RuleVersion(0)]
	public AndExprContext andExpr() {
		AndExprContext _localctx = new AndExprContext(Context, State);
		EnterRule(_localctx, 6, RULE_andExpr);
		try {
			State = 57;
			ErrorHandler.Sync(this);
			switch ( Interpreter.AdaptivePredict(TokenStream,2,Context) ) {
			case 1:
				EnterOuterAlt(_localctx, 1);
				{
				State = 52;
				primaryExpr();
				}
				break;
			case 2:
				EnterOuterAlt(_localctx, 2);
				{
				State = 53;
				_localctx.leftOp = primaryExpr();
				State = 54;
				Match(T__2);
				State = 55;
				_localctx.rightOp = primaryExpr();
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class PrimaryExprContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public TrueExprContext trueExpr() {
			return GetRuleContext<TrueExprContext>(0);
		}
		[System.Diagnostics.DebuggerNonUserCode] public FalseExprContext falseExpr() {
			return GetRuleContext<FalseExprContext>(0);
		}
		[System.Diagnostics.DebuggerNonUserCode] public ParenExprContext parenExpr() {
			return GetRuleContext<ParenExprContext>(0);
		}
		[System.Diagnostics.DebuggerNonUserCode] public NotParenExprContext notParenExpr() {
			return GetRuleContext<NotParenExprContext>(0);
		}
		[System.Diagnostics.DebuggerNonUserCode] public StatementContext statement() {
			return GetRuleContext<StatementContext>(0);
		}
		public PrimaryExprContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_primaryExpr; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override void EnterRule(IParseTreeListener listener) {
			IConstraintListener typedListener = listener as IConstraintListener;
			if (typedListener != null) typedListener.EnterPrimaryExpr(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override void ExitRule(IParseTreeListener listener) {
			IConstraintListener typedListener = listener as IConstraintListener;
			if (typedListener != null) typedListener.ExitPrimaryExpr(this);
		}
	}

	[RuleVersion(0)]
	public PrimaryExprContext primaryExpr() {
		PrimaryExprContext _localctx = new PrimaryExprContext(Context, State);
		EnterRule(_localctx, 8, RULE_primaryExpr);
		try {
			State = 64;
			ErrorHandler.Sync(this);
			switch (TokenStream.LA(1)) {
			case T__3:
				EnterOuterAlt(_localctx, 1);
				{
				State = 59;
				trueExpr();
				}
				break;
			case T__4:
				EnterOuterAlt(_localctx, 2);
				{
				State = 60;
				falseExpr();
				}
				break;
			case T__5:
				EnterOuterAlt(_localctx, 3);
				{
				State = 61;
				parenExpr();
				}
				break;
			case T__7:
				EnterOuterAlt(_localctx, 4);
				{
				State = 62;
				notParenExpr();
				}
				break;
			case STR:
				EnterOuterAlt(_localctx, 5);
				{
				State = 63;
				statement();
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class TrueExprContext : ParserRuleContext {
		public TrueExprContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_trueExpr; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override void EnterRule(IParseTreeListener listener) {
			IConstraintListener typedListener = listener as IConstraintListener;
			if (typedListener != null) typedListener.EnterTrueExpr(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override void ExitRule(IParseTreeListener listener) {
			IConstraintListener typedListener = listener as IConstraintListener;
			if (typedListener != null) typedListener.ExitTrueExpr(this);
		}
	}

	[RuleVersion(0)]
	public TrueExprContext trueExpr() {
		TrueExprContext _localctx = new TrueExprContext(Context, State);
		EnterRule(_localctx, 10, RULE_trueExpr);
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 66;
			Match(T__3);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class FalseExprContext : ParserRuleContext {
		public FalseExprContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_falseExpr; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override void EnterRule(IParseTreeListener listener) {
			IConstraintListener typedListener = listener as IConstraintListener;
			if (typedListener != null) typedListener.EnterFalseExpr(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override void ExitRule(IParseTreeListener listener) {
			IConstraintListener typedListener = listener as IConstraintListener;
			if (typedListener != null) typedListener.ExitFalseExpr(this);
		}
	}

	[RuleVersion(0)]
	public FalseExprContext falseExpr() {
		FalseExprContext _localctx = new FalseExprContext(Context, State);
		EnterRule(_localctx, 12, RULE_falseExpr);
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 68;
			Match(T__4);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class ParenExprContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public ExprContext expr() {
			return GetRuleContext<ExprContext>(0);
		}
		public ParenExprContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_parenExpr; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override void EnterRule(IParseTreeListener listener) {
			IConstraintListener typedListener = listener as IConstraintListener;
			if (typedListener != null) typedListener.EnterParenExpr(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override void ExitRule(IParseTreeListener listener) {
			IConstraintListener typedListener = listener as IConstraintListener;
			if (typedListener != null) typedListener.ExitParenExpr(this);
		}
	}

	[RuleVersion(0)]
	public ParenExprContext parenExpr() {
		ParenExprContext _localctx = new ParenExprContext(Context, State);
		EnterRule(_localctx, 14, RULE_parenExpr);
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 70;
			Match(T__5);
			State = 71;
			expr();
			State = 72;
			Match(T__6);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class NotParenExprContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public ExprContext expr() {
			return GetRuleContext<ExprContext>(0);
		}
		public NotParenExprContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_notParenExpr; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override void EnterRule(IParseTreeListener listener) {
			IConstraintListener typedListener = listener as IConstraintListener;
			if (typedListener != null) typedListener.EnterNotParenExpr(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override void ExitRule(IParseTreeListener listener) {
			IConstraintListener typedListener = listener as IConstraintListener;
			if (typedListener != null) typedListener.ExitNotParenExpr(this);
		}
	}

	[RuleVersion(0)]
	public NotParenExprContext notParenExpr() {
		NotParenExprContext _localctx = new NotParenExprContext(Context, State);
		EnterRule(_localctx, 16, RULE_notParenExpr);
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 74;
			Match(T__7);
			State = 75;
			expr();
			State = 76;
			Match(T__6);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class StatementContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public EqValStatementContext eqValStatement() {
			return GetRuleContext<EqValStatementContext>(0);
		}
		[System.Diagnostics.DebuggerNonUserCode] public NotEqValStatementContext notEqValStatement() {
			return GetRuleContext<NotEqValStatementContext>(0);
		}
		[System.Diagnostics.DebuggerNonUserCode] public InListStatementContext inListStatement() {
			return GetRuleContext<InListStatementContext>(0);
		}
		[System.Diagnostics.DebuggerNonUserCode] public NotInListStatementContext notInListStatement() {
			return GetRuleContext<NotInListStatementContext>(0);
		}
		public StatementContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_statement; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override void EnterRule(IParseTreeListener listener) {
			IConstraintListener typedListener = listener as IConstraintListener;
			if (typedListener != null) typedListener.EnterStatement(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override void ExitRule(IParseTreeListener listener) {
			IConstraintListener typedListener = listener as IConstraintListener;
			if (typedListener != null) typedListener.ExitStatement(this);
		}
	}

	[RuleVersion(0)]
	public StatementContext statement() {
		StatementContext _localctx = new StatementContext(Context, State);
		EnterRule(_localctx, 18, RULE_statement);
		try {
			State = 82;
			ErrorHandler.Sync(this);
			switch ( Interpreter.AdaptivePredict(TokenStream,4,Context) ) {
			case 1:
				EnterOuterAlt(_localctx, 1);
				{
				State = 78;
				eqValStatement();
				}
				break;
			case 2:
				EnterOuterAlt(_localctx, 2);
				{
				State = 79;
				notEqValStatement();
				}
				break;
			case 3:
				EnterOuterAlt(_localctx, 3);
				{
				State = 80;
				inListStatement();
				}
				break;
			case 4:
				EnterOuterAlt(_localctx, 4);
				{
				State = 81;
				notInListStatement();
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class EqValStatementContext : ParserRuleContext {
		public IToken key;
		public ValContext value;
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode STR() { return GetToken(ConstraintParser.STR, 0); }
		[System.Diagnostics.DebuggerNonUserCode] public ValContext val() {
			return GetRuleContext<ValContext>(0);
		}
		public EqValStatementContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_eqValStatement; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override void EnterRule(IParseTreeListener listener) {
			IConstraintListener typedListener = listener as IConstraintListener;
			if (typedListener != null) typedListener.EnterEqValStatement(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override void ExitRule(IParseTreeListener listener) {
			IConstraintListener typedListener = listener as IConstraintListener;
			if (typedListener != null) typedListener.ExitEqValStatement(this);
		}
	}

	[RuleVersion(0)]
	public EqValStatementContext eqValStatement() {
		EqValStatementContext _localctx = new EqValStatementContext(Context, State);
		EnterRule(_localctx, 20, RULE_eqValStatement);
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 84;
			_localctx.key = Match(STR);
			State = 85;
			Match(T__8);
			State = 86;
			_localctx.value = val();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class NotEqValStatementContext : ParserRuleContext {
		public IToken key;
		public ValContext value;
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode STR() { return GetToken(ConstraintParser.STR, 0); }
		[System.Diagnostics.DebuggerNonUserCode] public ValContext val() {
			return GetRuleContext<ValContext>(0);
		}
		public NotEqValStatementContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_notEqValStatement; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override void EnterRule(IParseTreeListener listener) {
			IConstraintListener typedListener = listener as IConstraintListener;
			if (typedListener != null) typedListener.EnterNotEqValStatement(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override void ExitRule(IParseTreeListener listener) {
			IConstraintListener typedListener = listener as IConstraintListener;
			if (typedListener != null) typedListener.ExitNotEqValStatement(this);
		}
	}

	[RuleVersion(0)]
	public NotEqValStatementContext notEqValStatement() {
		NotEqValStatementContext _localctx = new NotEqValStatementContext(Context, State);
		EnterRule(_localctx, 22, RULE_notEqValStatement);
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 88;
			_localctx.key = Match(STR);
			State = 89;
			Match(T__9);
			State = 90;
			_localctx.value = val();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class InListStatementContext : ParserRuleContext {
		public IToken key;
		public LstContext list;
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode STR() { return GetToken(ConstraintParser.STR, 0); }
		[System.Diagnostics.DebuggerNonUserCode] public LstContext lst() {
			return GetRuleContext<LstContext>(0);
		}
		public InListStatementContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_inListStatement; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override void EnterRule(IParseTreeListener listener) {
			IConstraintListener typedListener = listener as IConstraintListener;
			if (typedListener != null) typedListener.EnterInListStatement(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override void ExitRule(IParseTreeListener listener) {
			IConstraintListener typedListener = listener as IConstraintListener;
			if (typedListener != null) typedListener.ExitInListStatement(this);
		}
	}

	[RuleVersion(0)]
	public InListStatementContext inListStatement() {
		InListStatementContext _localctx = new InListStatementContext(Context, State);
		EnterRule(_localctx, 24, RULE_inListStatement);
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 92;
			_localctx.key = Match(STR);
			State = 93;
			Match(T__8);
			State = 94;
			_localctx.list = lst();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class NotInListStatementContext : ParserRuleContext {
		public IToken key;
		public LstContext list;
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode STR() { return GetToken(ConstraintParser.STR, 0); }
		[System.Diagnostics.DebuggerNonUserCode] public LstContext lst() {
			return GetRuleContext<LstContext>(0);
		}
		public NotInListStatementContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_notInListStatement; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override void EnterRule(IParseTreeListener listener) {
			IConstraintListener typedListener = listener as IConstraintListener;
			if (typedListener != null) typedListener.EnterNotInListStatement(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override void ExitRule(IParseTreeListener listener) {
			IConstraintListener typedListener = listener as IConstraintListener;
			if (typedListener != null) typedListener.ExitNotInListStatement(this);
		}
	}

	[RuleVersion(0)]
	public NotInListStatementContext notInListStatement() {
		NotInListStatementContext _localctx = new NotInListStatementContext(Context, State);
		EnterRule(_localctx, 26, RULE_notInListStatement);
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 96;
			_localctx.key = Match(STR);
			State = 97;
			Match(T__9);
			State = 98;
			_localctx.list = lst();
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class LstContext : ParserRuleContext {
		public ValContext _val;
		public IList<ValContext> _values = new List<ValContext>();
		[System.Diagnostics.DebuggerNonUserCode] public ValContext[] val() {
			return GetRuleContexts<ValContext>();
		}
		[System.Diagnostics.DebuggerNonUserCode] public ValContext val(int i) {
			return GetRuleContext<ValContext>(i);
		}
		public LstContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_lst; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override void EnterRule(IParseTreeListener listener) {
			IConstraintListener typedListener = listener as IConstraintListener;
			if (typedListener != null) typedListener.EnterLst(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override void ExitRule(IParseTreeListener listener) {
			IConstraintListener typedListener = listener as IConstraintListener;
			if (typedListener != null) typedListener.ExitLst(this);
		}
	}

	[RuleVersion(0)]
	public LstContext lst() {
		LstContext _localctx = new LstContext(Context, State);
		EnterRule(_localctx, 28, RULE_lst);
		int _la;
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 100;
			Match(T__10);
			State = 101;
			_localctx._val = val();
			_localctx._values.Add(_localctx._val);
			State = 106;
			ErrorHandler.Sync(this);
			_la = TokenStream.LA(1);
			while (_la==T__11) {
				{
				{
				State = 102;
				Match(T__11);
				State = 103;
				_localctx._val = val();
				_localctx._values.Add(_localctx._val);
				}
				}
				State = 108;
				ErrorHandler.Sync(this);
				_la = TokenStream.LA(1);
			}
			State = 109;
			Match(T__12);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class ValContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public StrValContext strVal() {
			return GetRuleContext<StrValContext>(0);
		}
		[System.Diagnostics.DebuggerNonUserCode] public NullValContext nullVal() {
			return GetRuleContext<NullValContext>(0);
		}
		public ValContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_val; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override void EnterRule(IParseTreeListener listener) {
			IConstraintListener typedListener = listener as IConstraintListener;
			if (typedListener != null) typedListener.EnterVal(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override void ExitRule(IParseTreeListener listener) {
			IConstraintListener typedListener = listener as IConstraintListener;
			if (typedListener != null) typedListener.ExitVal(this);
		}
	}

	[RuleVersion(0)]
	public ValContext val() {
		ValContext _localctx = new ValContext(Context, State);
		EnterRule(_localctx, 30, RULE_val);
		try {
			State = 113;
			ErrorHandler.Sync(this);
			switch (TokenStream.LA(1)) {
			case STR:
				EnterOuterAlt(_localctx, 1);
				{
				State = 111;
				strVal();
				}
				break;
			case NULL:
				EnterOuterAlt(_localctx, 2);
				{
				State = 112;
				nullVal();
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class StrValContext : ParserRuleContext {
		public IToken value;
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode STR() { return GetToken(ConstraintParser.STR, 0); }
		public StrValContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_strVal; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override void EnterRule(IParseTreeListener listener) {
			IConstraintListener typedListener = listener as IConstraintListener;
			if (typedListener != null) typedListener.EnterStrVal(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override void ExitRule(IParseTreeListener listener) {
			IConstraintListener typedListener = listener as IConstraintListener;
			if (typedListener != null) typedListener.ExitStrVal(this);
		}
	}

	[RuleVersion(0)]
	public StrValContext strVal() {
		StrValContext _localctx = new StrValContext(Context, State);
		EnterRule(_localctx, 32, RULE_strVal);
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 115;
			_localctx.value = Match(STR);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class NullValContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode NULL() { return GetToken(ConstraintParser.NULL, 0); }
		public NullValContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_nullVal; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override void EnterRule(IParseTreeListener listener) {
			IConstraintListener typedListener = listener as IConstraintListener;
			if (typedListener != null) typedListener.EnterNullVal(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override void ExitRule(IParseTreeListener listener) {
			IConstraintListener typedListener = listener as IConstraintListener;
			if (typedListener != null) typedListener.ExitNullVal(this);
		}
	}

	[RuleVersion(0)]
	public NullValContext nullVal() {
		NullValContext _localctx = new NullValContext(Context, State);
		EnterRule(_localctx, 34, RULE_nullVal);
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 117;
			Match(NULL);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	private static int[] _serializedATN = {
		4,1,16,120,2,0,7,0,2,1,7,1,2,2,7,2,2,3,7,3,2,4,7,4,2,5,7,5,2,6,7,6,2,7,
		7,7,2,8,7,8,2,9,7,9,2,10,7,10,2,11,7,11,2,12,7,12,2,13,7,13,2,14,7,14,
		2,15,7,15,2,16,7,16,2,17,7,17,1,0,1,0,1,1,1,1,1,1,1,1,1,1,3,1,44,8,1,1,
		2,1,2,1,2,1,2,1,2,3,2,51,8,2,1,3,1,3,1,3,1,3,1,3,3,3,58,8,3,1,4,1,4,1,
		4,1,4,1,4,3,4,65,8,4,1,5,1,5,1,6,1,6,1,7,1,7,1,7,1,7,1,8,1,8,1,8,1,8,1,
		9,1,9,1,9,1,9,3,9,83,8,9,1,10,1,10,1,10,1,10,1,11,1,11,1,11,1,11,1,12,
		1,12,1,12,1,12,1,13,1,13,1,13,1,13,1,14,1,14,1,14,1,14,5,14,105,8,14,10,
		14,12,14,108,9,14,1,14,1,14,1,15,1,15,3,15,114,8,15,1,16,1,16,1,17,1,17,
		1,17,0,0,18,0,2,4,6,8,10,12,14,16,18,20,22,24,26,28,30,32,34,0,0,113,0,
		36,1,0,0,0,2,43,1,0,0,0,4,50,1,0,0,0,6,57,1,0,0,0,8,64,1,0,0,0,10,66,1,
		0,0,0,12,68,1,0,0,0,14,70,1,0,0,0,16,74,1,0,0,0,18,82,1,0,0,0,20,84,1,
		0,0,0,22,88,1,0,0,0,24,92,1,0,0,0,26,96,1,0,0,0,28,100,1,0,0,0,30,113,
		1,0,0,0,32,115,1,0,0,0,34,117,1,0,0,0,36,37,3,2,1,0,37,1,1,0,0,0,38,44,
		3,4,2,0,39,40,3,4,2,0,40,41,5,1,0,0,41,42,3,4,2,0,42,44,1,0,0,0,43,38,
		1,0,0,0,43,39,1,0,0,0,44,3,1,0,0,0,45,51,3,6,3,0,46,47,3,6,3,0,47,48,5,
		2,0,0,48,49,3,6,3,0,49,51,1,0,0,0,50,45,1,0,0,0,50,46,1,0,0,0,51,5,1,0,
		0,0,52,58,3,8,4,0,53,54,3,8,4,0,54,55,5,3,0,0,55,56,3,8,4,0,56,58,1,0,
		0,0,57,52,1,0,0,0,57,53,1,0,0,0,58,7,1,0,0,0,59,65,3,10,5,0,60,65,3,12,
		6,0,61,65,3,14,7,0,62,65,3,16,8,0,63,65,3,18,9,0,64,59,1,0,0,0,64,60,1,
		0,0,0,64,61,1,0,0,0,64,62,1,0,0,0,64,63,1,0,0,0,65,9,1,0,0,0,66,67,5,4,
		0,0,67,11,1,0,0,0,68,69,5,5,0,0,69,13,1,0,0,0,70,71,5,6,0,0,71,72,3,0,
		0,0,72,73,5,7,0,0,73,15,1,0,0,0,74,75,5,8,0,0,75,76,3,0,0,0,76,77,5,7,
		0,0,77,17,1,0,0,0,78,83,3,20,10,0,79,83,3,22,11,0,80,83,3,24,12,0,81,83,
		3,26,13,0,82,78,1,0,0,0,82,79,1,0,0,0,82,80,1,0,0,0,82,81,1,0,0,0,83,19,
		1,0,0,0,84,85,5,15,0,0,85,86,5,9,0,0,86,87,3,30,15,0,87,21,1,0,0,0,88,
		89,5,15,0,0,89,90,5,10,0,0,90,91,3,30,15,0,91,23,1,0,0,0,92,93,5,15,0,
		0,93,94,5,9,0,0,94,95,3,28,14,0,95,25,1,0,0,0,96,97,5,15,0,0,97,98,5,10,
		0,0,98,99,3,28,14,0,99,27,1,0,0,0,100,101,5,11,0,0,101,106,3,30,15,0,102,
		103,5,12,0,0,103,105,3,30,15,0,104,102,1,0,0,0,105,108,1,0,0,0,106,104,
		1,0,0,0,106,107,1,0,0,0,107,109,1,0,0,0,108,106,1,0,0,0,109,110,5,13,0,
		0,110,29,1,0,0,0,111,114,3,32,16,0,112,114,3,34,17,0,113,111,1,0,0,0,113,
		112,1,0,0,0,114,31,1,0,0,0,115,116,5,15,0,0,116,33,1,0,0,0,117,118,5,14,
		0,0,118,35,1,0,0,0,7,43,50,57,64,82,106,113
	};

	public static readonly ATN _ATN =
		new ATNDeserializer().Deserialize(_serializedATN);


}