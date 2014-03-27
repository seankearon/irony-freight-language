using System;
using System.Text;
using Irony.Compiler;

namespace FreightLanguageCompiler.Nodes
{
	internal class IfStatementNode : AstNode, IJavascriptGenerator
	{

		private ExpressionNode Condition
		{
			get
			{
				return (ExpressionNode)ChildNodes[1];
			}
		}

		private StatementListNode StatementList
		{
			get
			{
				return (StatementListNode)ChildNodes[2];
			}
		}

		public IfStatementNode(AstNodeArgs args)
			: base(args)
		{
		}

		public void GenerateScript(StringBuilder builder)
		{
			builder.Append("if (");
			Condition.GenerateScript(builder);
			builder.AppendLine(") {");
			StatementList.GenerateScript(builder);
			builder.AppendLine("}");

		}

	}
}
