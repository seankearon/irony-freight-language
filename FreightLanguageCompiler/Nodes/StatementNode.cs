using System;
using System.Text;
using Irony.Compiler;

namespace FreightLanguageCompiler.Nodes
{
	internal class StatementNode : AstNode, IJavascriptGenerator
	{

		/// <summary>
		/// A statement has a single child: either SetVariable, IfStatement,  OrderLoop, or Expression.
		/// </summary>
		private AstNode Child
		{
			get
			{
				return (AstNode)ChildNodes[0];
			}
		}


		public StatementNode(AstNodeArgs args)
			: base(args)
		{
		}

		public void GenerateScript(StringBuilder builder)
		{
			IJavascriptGenerator child = (IJavascriptGenerator)Child;
			child.GenerateScript(builder);

			// if the statement is just an expression, then there will be no semicolon at the end
			if (child is ExpressionNode)
			{
				builder.AppendLine(";");
			}

		}

	}
}
