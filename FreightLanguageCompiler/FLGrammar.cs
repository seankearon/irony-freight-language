using FreightLanguageCompiler.Nodes;
using Irony.Ast;
using Irony.Interpreter;
using Irony.Parsing;
using StatementListNode = FreightLanguageCompiler.Nodes.StatementListNode;

namespace FreightLanguageCompiler
{
    /// <summary>
    /// Reasons for using <see cref="InterpretedLanguageGrammar"/> as the base here: 
    /// https://irony.codeplex.com/discussions/361018
    /// Sean Kearon, March 2014
    /// </summary>
	public class FLGrammar : InterpretedLanguageGrammar
	{

        public FLGrammar()
            : base(false) // turn off case sensitivity
		{

			#region Initial setup of the grammar

			// define all the non-terminals
			var program = new NonTerminal("program", typeof(ProgramNode));
			var statementList = new NonTerminal("statementList", typeof(StatementListNode));
			var freightDeclaration = new NonTerminal("freightDeclaration", typeof(FreightDeclarationNode));
			var statement = new NonTerminal("statement", typeof(StatementNode));
			var setVariable = new NonTerminal("setVariable", typeof(SetVariableNode));
			var ifStatement = new NonTerminal("ifStatement", typeof(IfStatementNode));
			var orderLoop = new NonTerminal("orderLoop", typeof(OrderLoopNode));
			var expression = new NonTerminal("expression", typeof(ExpressionNode));
			var binaryOperator = new NonTerminal("binaryOperator", typeof(BinaryOperatorNode));

			// define all the terminals
			var variable = new IdentifierTerminal("variable");
            //var variable = TerminalFactory.CreateCSharpIdentifier("variable");
            //variable.AstConfig.NodeType = typeof(MyVariable);


            // Not sure about how this is handled in 0.9 yet - I think it's handled by ToTerm().  Sean Kearon, March 2014.   variable.AddKeywords("set", "to", "if", "freight", "cost", "is", "loop", "through", "order");
            // See https://irony.codeplex.com/discussions/361018 var number = new NumberLiteral("number", NumberOptions.Default, typeof());
			var number = TerminalFactory.CreateCSharpNumber("number");
            number.AstConfig.NodeType = typeof (MyNumber);
            
            var stringLiteral = new StringLiteral("string", "\"", StringOptions.None);

			// remove uninteresting nodes from the AST (note: in current version of Irony,
			// keywords added to the variable cannot be registered as punctuation).
			this.MarkPunctuation(";", "[", "]", "(", ")");

			// specify the non-terminal which is the root of the AST
			this.Root = program;

			#endregion

			#region Define the grammar

			//<Program> ::= <StatementList> <FreightDeclaration>
			program.Rule = statementList + freightDeclaration;

			//<StatementList> ::= <Statement>*
			statementList.Rule = MakeStarRule(statementList, null, statement);

			//<Statement> ::= <SetVariable> ";" | <IfStatement> | <OrderLoop> | <Expression> ";"
			statement.Rule = setVariable + ";" | ifStatement | orderLoop | expression + ";";

			//<SetVariable> ::= "set" <Variable> "to" <Expression>
			setVariable.Rule = ToTerm("set") + variable + "to" + expression;

			//<IfStatement> ::= "if" <Expression> "[" <StatementList> "]"
			ifStatement.Rule = ToTerm("if") + expression + "[" + statementList + "]";

			//<OrderLoop> ::= "loop" "through" "order" "[" <StatementList> "]"
			orderLoop.Rule = ToTerm("loop") + "through" + "order" + "[" + statementList + "]";

			// <FreightDeclaration> ::= "freight" "cost" "is" <Expression> ";"
			freightDeclaration.Rule = ToTerm("freight") + "cost" + "is" + expression + ";";

			//<Expression> ::= <number> | <variable> | <string>
			//  | <Expression> <BinaryOperator> <Expression>
			//  | "(" <Expression> ")"
			expression.Rule = number | variable | stringLiteral
				| expression + binaryOperator + expression
				| "(" + expression + ")";

			//<BinaryOperator> ::= "+" | "-" | "*" | "/" | "<" | ">" | "<=" | ">=" | "is"
			binaryOperator.Rule = ToTerm("+") | "-" | "*" | "/" | "<" | ">" | "<=" | ">=" | "is";

            // Fix up the shift-reduce conflicts by defining precedence rules.  Sean Kearon, March 2014.
            RegisterOperators(10, "is", "+", "-");
            RegisterOperators(20, "*", "/");
            RegisterOperators(30, Associativity.Right, "**");
            RegisterOperators(40, "<", ">", "<=", ">=");

            LanguageFlags = LanguageFlags.NewLineBeforeEOF | LanguageFlags.CreateAst | LanguageFlags.SupportsBigInt;
            

			#endregion

		}

	}

    public class MyVariable : IAstNodeInit
    {
        public void Init(AstContext context, ParseTreeNode parseNode)
        {

        }
    }

    public class MyNumber : IAstNodeInit
    {
        public void Init(AstContext context, ParseTreeNode parseNode)
        {
            
        }
    }
}
