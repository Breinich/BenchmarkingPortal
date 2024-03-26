using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using BenchmarkingPortal.Dal.Dtos;

namespace BenchmarkingPortal.Bll.Services;

public class ConstraintEvaluator : ConstraintBaseListener
{
    /// <summary>
    /// Evaluates the input string and returns a delegate that can be used to filter a list of ConfigurationItemHeader.
    /// </summary>
    /// <param name="input"> the input string that should be evaluated</param>
    /// <returns> the delegate that can be used to filter a list of ConfigurationItemHeader</returns>
    public Predicate<List<ConfigurationItemHeader>>? Evaluate(string input)
    {
        _exprStack.Clear();
        _strStack.Clear();
        
        var stream = CharStreams.fromString(input);
        ITokenSource lexer = new ConstraintLexer(stream);
        ITokenStream tokens = new CommonTokenStream(lexer);
        ConstraintParser parser = new(tokens);
        IParseTree tree = parser.expr();
        ParseTreeWalker.Default.Walk(this, tree);
        return Result;
    }

    private Predicate<List<ConfigurationItemHeader>>? Result { get; set; }
    
    private readonly Stack<Predicate<List<ConfigurationItemHeader>>> _exprStack = new();
    private readonly Stack<string?> _strStack = new();
    
    /// <summary>
    /// Creates a delegate that evaluates an implication between two expressions.
    /// </summary>
    /// <param name="left"> the left expression</param>
    /// <param name="right"> the right expression</param>
    /// <returns> the delegate that evaluates an implication between two expressions</returns>
    private static Predicate<List<ConfigurationItemHeader>> ImplicationFactory(Predicate<List<ConfigurationItemHeader>> left, Predicate<List<ConfigurationItemHeader>> right)
    {
        return config => !left(config) || right(config);
    }
    
    /// <summary>
    /// Creates a delegate that checks if at least one of the expressions is true.
    /// </summary>
    /// <param name="left"> the left expression</param>
    /// <param name="right"> the right expression</param>
    /// <returns> the delegate that checks if at least one of the expressions is true</returns>
    private static Predicate<List<ConfigurationItemHeader>> OrFactory(Predicate<List<ConfigurationItemHeader>> left, Predicate<List<ConfigurationItemHeader>> right)
    {
        return config => left(config) || right(config);
    }
    
    /// <summary>
    /// Creates a delegate that checks if both expressions are true.
    /// </summary>
    /// <param name="left"> the left expression</param>
    /// <param name="right"> the right expression</param>
    /// <returns> the delegate that checks if both expressions are true</returns>
    private static Predicate<List<ConfigurationItemHeader>> AndFactory(Predicate<List<ConfigurationItemHeader>> left, Predicate<List<ConfigurationItemHeader>> right)
    {
        return config => left(config) && right(config);
    }
    
    /// <summary>
    /// Creates a delegate that negates the value of the expression.
    /// </summary>
    /// <param name="expr"> the expression that should be negated</param>
    /// <returns> the delegate that negates the value of the expression</returns>
    private static Predicate<List<ConfigurationItemHeader>> NotFactory(Predicate<List<ConfigurationItemHeader>> expr)
    {
        return config => !expr(config);
    }
    
    /// <summary>
    /// Creates a delegate that checks if the value of the key is equal to the value.
    /// </summary>
    /// <param name="key"> the key property of the ConfigurationItemHeader</param>
    /// <param name="value"> the value that the key's value should be equal to</param>
    /// <returns> the delegate that checks if the value of the key is equal to the value</returns>
    /// <exception cref="ArgumentException"> if the key is not found in the config list</exception>
    private static Predicate<List<ConfigurationItemHeader>> EqValFactory(string key, string? value)
    {
        return config =>
        {
            if (config.All(x => x.Key != key))
            {
                throw new ArgumentException(KeyNotFoundError(key));
            }
            return config.Find(x => x.Key == key)?.Value == value;
        };
    }
    
    /// <summary>
    /// Creates a delegate that checks if the value of the key is not equal to the value.
    /// </summary>
    /// <param name="key"> the key property of the ConfigurationItemHeader</param>
    /// <param name="value"> the value that the key's value should not be equal to</param>
    /// <returns> the delegate that checks if the value of the key is not equal to the value</returns>
    /// <exception cref="ArgumentException"> if the key is not found in the config list</exception>
    private static Predicate<List<ConfigurationItemHeader>> NotEqValFactory(string key, string? value)
    {
        return config =>
        {
            if (value == null)
            {
                return config.All(x => x.Key != key);
            }
            if (config.All(x => x.Key != key))
            {
                throw new ArgumentException(KeyNotFoundError(key));
            }
            return config.Find(x => x.Key == key)?.Value != value;
        };
    }
    
    /// <summary>
    /// Creates a delegate that checks if the value of the key is in the list of values.
    /// </summary>
    /// <param name="key"> the key property of the ConfigurationItemHeader</param>
    /// <param name="values"> the list of values that the key's value should be in</param>
    /// <returns> the delegate that checks if the value of the key is in the list of values</returns>
    /// <exception cref="ArgumentException"> if the key is not found in the config list</exception>
    private static Predicate<List<ConfigurationItemHeader>> InListFactory(string key, List<string?> values)
    {
        return config =>
        {
            if (config.All(x => x.Key != key))
            {
                throw new ArgumentException(KeyNotFoundError(key));
            }
            // if x.Value is null, it means that it is a flag and the values list should contain the null value too,
            // to allow the flag to be set (usually the flag shouldn't be allowed to have any other value, than null)
            return values.Contains(config.Find(x => x.Key == key)?.Value);
        };
    }
    
    /// <summary>
    /// Creates a delegate that checks if the value of the key is not in the list of values.
    /// </summary>
    /// <param name="key"> the key property of the ConfigurationItemHeader</param>
    /// <param name="values"> the list of values that the key's value should not be in</param>
    /// <returns> the delegate that checks if the value of the key is not in the list of values</returns>
    /// <exception cref="ArgumentException"> if the key is not found in the config list</exception>
    private static Predicate<List<ConfigurationItemHeader>> NotInListFactory(string key, List<string?> values)
    {
        return config =>
        {
            if (config.All(x => x.Key != key))
            {
                throw new ArgumentException(KeyNotFoundError(key));
            }
            return !values.Contains(config.Find(x => x.Key == key)?.Value);
        };
    }
    
    /// <summary>
    /// Creates a delegate that always returns false.
    /// </summary>
    /// <returns> the delegate that always returns false</returns>
    private static Predicate<List<ConfigurationItemHeader>> FalseFactory()
    {
        return _ => false;
    }
    /// <summary>
    /// Creates a delegate that always returns true.
    /// </summary>
    /// <returns> the delegate that always returns true</returns>
    private static Predicate<List<ConfigurationItemHeader>> TrueFactory()
    {
        return _ => true;
    }
    
    /// <summary>
    /// Creates an error message for the case when the stack is not empty after a method call.
    /// </summary>
    /// <param name="stackName"> the name of the stack that should be empty</param>
    /// <param name="methodName"> the name of the method after that the stack should be empty</param>
    /// <returns> the error message</returns>
    private static string StackNotEmptyError(string stackName, string methodName)
    {
        return $"{stackName} is not empty after {methodName}!";
    }
    
    /// <summary>
    /// Creates an error message for the case when the key is not found in the config list.
    /// </summary>
    /// <param name="key"> the key that is not found in the config list</param>
    /// <returns> the error message</returns>
    private static string KeyNotFoundError(string key)
    {
        return $"Key <{key}> not found in config!";
    }

    public override void EnterExpr(ConstraintParser.ExprContext context) { }

    /// <summary>
    /// Peeks the top expression from the stack and sets it as the result.
    /// </summary>
    /// <param name="context"> the context of the expression</param>
    public override void ExitExpr(ConstraintParser.ExprContext context)
    {
        Result = _exprStack.Peek();
    }

    public override void EnterImplyExpr(ConstraintParser.ImplyExprContext context) { }

    /// <summary>
    /// Pops the top two expressions from the stack, applies the implication operation on them and pushes the result back.
    /// </summary>
    /// <param name="context"> the context of the implication expression</param>
    /// <exception cref="ApplicationException"> if the stack is not empty after the method call</exception>
    public override void ExitImplyExpr(ConstraintParser.ImplyExprContext context)
    {
        if (context.leftOp == null || context.rightOp == null) return;
        var right = _exprStack.Pop();
        var left = _exprStack.Pop();
        if (_exprStack.Count > 0)
        {
            throw new ApplicationException(StackNotEmptyError(nameof(_exprStack), nameof(ExitImplyExpr)));
        }

        _exprStack.Push(ImplicationFactory(left, right));
    }

    public override void EnterOrExpr(ConstraintParser.OrExprContext context) { }

    /// <summary>
    /// Pops the top two expressions from the stack, applies the or operation on them and pushes the result back.
    /// </summary>
    /// <param name="context"> the context of the or expression</param>
    /// <exception cref="ApplicationException"> if the stack is not empty after the method call</exception>
    public override void ExitOrExpr(ConstraintParser.OrExprContext context)
    {
        if (context.leftOp == null || context.rightOp == null) return;
        var right = _exprStack.Pop();
        var left = _exprStack.Pop();
        if (_exprStack.Count > 0)
        {
            throw new ApplicationException(StackNotEmptyError(nameof(_exprStack), nameof(ExitOrExpr)));
        }
        _exprStack.Push(OrFactory(left, right));
    }

    public override void EnterAndExpr(ConstraintParser.AndExprContext context) { }

    /// <summary>
    /// Pops the top two expressions from the stack, applies the and operation on them and pushes the result back.
    /// </summary>
    /// <param name="context"> the context of the and expression</param>
    /// <exception cref="ApplicationException"> if the stack is not empty after the method call</exception>
    public override void ExitAndExpr(ConstraintParser.AndExprContext context)
    {
        if (context.leftOp == null || context.rightOp == null) return;
        var right = _exprStack.Pop();
        var left = _exprStack.Pop();
        if (_exprStack.Count > 0)
        {
            throw new ApplicationException(StackNotEmptyError(nameof(_exprStack), nameof(ExitAndExpr)));
        }
        _exprStack.Push(AndFactory(left, right));
    }

    public override void EnterPrimaryExpr(ConstraintParser.PrimaryExprContext context) { }

    public override void ExitPrimaryExpr(ConstraintParser.PrimaryExprContext context) { }

    public override void EnterTrueExpr(ConstraintParser.TrueExprContext context) { }

    /// <summary>
    /// Creates a delegate that always returns true.
    /// </summary>
    /// <param name="context"> the context of the true expression</param>
    public override void ExitTrueExpr(ConstraintParser.TrueExprContext context)
    {
        _exprStack.Push(TrueFactory());
    }

    public override void EnterFalseExpr(ConstraintParser.FalseExprContext context) { }

    /// <summary>
    /// Creates a delegate that always returns false.
    /// </summary>
    /// <param name="context"> the context of the false expression</param>
    public override void ExitFalseExpr(ConstraintParser.FalseExprContext context)
    {
        _exprStack.Push(FalseFactory());
    }

    public override void EnterParenExpr(ConstraintParser.ParenExprContext context) { }

    public override void ExitParenExpr(ConstraintParser.ParenExprContext context) { }

    public override void EnterNotParenExpr(ConstraintParser.NotParenExprContext context) { }

    /// <summary>
    /// Pops the top expression from the stack, negates it and pushes it back.
    /// </summary>
    /// <param name="context"> the context of the not expression</param>
    /// <exception cref="ApplicationException"> if the stack is not empty after the method call</exception>
    public override void ExitNotParenExpr(ConstraintParser.NotParenExprContext context)
    {
        var expr = _exprStack.Pop();
        if (_exprStack.Count > 0)
        {
            throw new ApplicationException(StackNotEmptyError(nameof(_exprStack), nameof(ExitNotParenExpr)));
        }
        _exprStack.Push(NotFactory(expr));
    }

    public override void EnterStatement(ConstraintParser.StatementContext context) { }

    public override void ExitStatement(ConstraintParser.StatementContext context) { }

    public override void EnterEqValStatement(ConstraintParser.EqValStatementContext context) { }

    /// <summary>
    /// Pops the value from the stack and pushes a delegate that checks if the key's value is equal to the value.
    /// </summary>
    /// <param name="context"> the context of the equal value statement</param>
    /// <exception cref="ApplicationException"> if the stack is not empty after the method call</exception>
    public override void ExitEqValStatement(ConstraintParser.EqValStatementContext context)
    {
        var key = context.key.Text;
        var value = _strStack.Pop();
        if (_strStack.Count > 0)
        {
            throw new ApplicationException(StackNotEmptyError(nameof(_strStack), nameof(ExitEqValStatement)));
        }
        _exprStack.Push(EqValFactory(key, value));
    }

    public override void EnterNotEqValStatement(ConstraintParser.NotEqValStatementContext context) { }

    /// <summary>
    /// Pops the value from the stack and pushes a delegate that checks if the key's value is not equal to the value.
    /// </summary>
    /// <param name="context"> the context of the not equal value statement</param>
    /// <exception cref="ApplicationException"> if the stack is not empty after the method call</exception>
    public override void ExitNotEqValStatement(ConstraintParser.NotEqValStatementContext context)
    {
        var key = context.key.Text;
        var value = _strStack.Pop();
        if (_strStack.Count > 0)
        {
            throw new ApplicationException(StackNotEmptyError(nameof(_strStack),nameof(ExitNotEqValStatement)));
        }
        _exprStack.Push(NotEqValFactory(key, value));
    }

    public override void EnterInListStatement(ConstraintParser.InListStatementContext context) { }

    /// <summary>
    /// Pops the values from the stack and pushes a delegate that checks if the key's value is in the list of values.
    /// </summary>
    /// <param name="context"> the context of the in list statement</param>
    public override void ExitInListStatement(ConstraintParser.InListStatementContext context)
    {
        var key = context.key.Text;
        var values = _strStack.ToList();
        _strStack.Clear();
        _exprStack.Push(InListFactory(key, values));
    }

    public override void EnterNotInListStatement(ConstraintParser.NotInListStatementContext context) { }

    /// <summary>
    /// Pops the values from the stack and pushes a delegate that checks if the key's value is not in the list of values.
    /// </summary>
    /// <param name="context"> the context of the not in list statement</param>
    public override void ExitNotInListStatement(ConstraintParser.NotInListStatementContext context)
    {
        var key = context.key.Text;
        var values = _strStack.ToList();
        _strStack.Clear();
        _exprStack.Push(NotInListFactory(key, values));
    }

    public override void EnterLst(ConstraintParser.LstContext context) { }

    public override void ExitLst(ConstraintParser.LstContext context) { }

    public override void EnterVal(ConstraintParser.ValContext context) { }

    public override void ExitVal(ConstraintParser.ValContext context) { }

    public override void EnterStrVal(ConstraintParser.StrValContext context) { }

    /// <summary>
    /// Pushes the value to the stack.
    /// </summary>
    /// <param name="context"> the context of the string value</param>
    public override void ExitStrVal(ConstraintParser.StrValContext context)
    {
        _strStack.Push(context.value.Text);
    }

    public override void EnterNullVal(ConstraintParser.NullValContext context) { }

    /// <summary>
    /// Pushes null to the stack.
    /// </summary>
    /// <param name="context"> the context of the null value</param>
    public override void ExitNullVal(ConstraintParser.NullValContext context)
    {
        _strStack.Push(null);
    }
}