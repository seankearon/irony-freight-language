using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Irony.Compiler;
using FreightLanguageCompiler.Nodes;

namespace FreightLanguageCompiler
{
	public class FLGrammar : Grammar
	{

		public FLGrammar()
		{

			#region Initial setup of the grammar

			// turn off case sensitivity
			this.CaseSensitive = false;

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
			variable.AddKeywords("set", "to", "if", "freight", "cost", "is", "loop", "through", "order");
			var number = new NumberLiteral("number");
			var stringLiteral = new StringLiteral("string", "\"", ScanFlags.None);

			// remove uninteresting nodes from the AST (note: in current version of Irony,
			// keywords added to the variable cannot be registered as punctuation).
			this.RegisterPunctuation(";", "[", "]", "(", ")");

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
			setVariable.Rule = Symbol("set") + variable + "to" + expression;

			//<IfStatement> ::= "if" <Expression> "[" <StatementList> "]"
			ifStatement.Rule = Symbol("if") + expression + "[" + statementList + "]";

			//<OrderLoop> ::= "loop" "through" "order" "[" <StatementList> "]"
			orderLoop.Rule = Symbol("loop") + "through" + "order" + "[" + statementList + "]";

			// <FreightDeclaration> ::= "freight" "cost" "is" <Expression> ";"
			freightDeclaration.Rule = Symbol("freight") + "cost" + "is" + expression + ";";

			//<Expression> ::= <number> | <variable> | <string>
			//  | <Expression> <BinaryOperator> <Expression>
			//  | "(" <Expression> ")"
			expression.Rule = number | variable | stringLiteral
				| expression + binaryOperator + expression
				| "(" + expression + ")";

			//<BinaryOperator> ::= "+" | "-" | "*" | "/" | "<" | ">" | "<=" | ">=" | "is"
			binaryOperator.Rule = Symbol("+") | "-" | "*" | "/" | "<" | ">" | "<=" | ">=" | "is";

			#endregion

		}

	}
}
