using System;
using System.Text;
using Irony.Compiler;

namespace FreightLanguageCompiler.Nodes
{
	internal class SetVariableNode : AstNode, IJavascriptGenerator
	{

		public Token Variable
		{
			get
			{
				return (Token)ChildNodes[1];
			}
		}

		private ExpressionNode Expression
		{
			get
			{
				return (ExpressionNode)ChildNodes[3];
			}
		}

		public SetVariableNode(AstNodeArgs args)
			: base(args)
		{
		}

		public void GenerateScript(StringBuilder builder)
		{
			builder.Append(Variable.Text);
			builder.Append(" = ");
			Expression.GenerateScript(builder);
			builder.AppendLine(";");
		}

	}
}
