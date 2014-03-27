using System;
using System.Text;
using Irony.Compiler;

namespace FreightLanguageCompiler.Nodes
{
	internal class FreightDeclarationNode : AstNode, IJavascriptGenerator
	{

		private ExpressionNode Expression
		{
			get
			{
				return (ExpressionNode)ChildNodes[3];
			}
		}

		public FreightDeclarationNode(AstNodeArgs args)
			: base(args)
		{
		}

		public void GenerateScript(StringBuilder builder)
		{
			builder.Append("return ");
			Expression.GenerateScript(builder);
			builder.AppendLine(";");
		}

	}
}
