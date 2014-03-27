using System;
using System.Text;
using Irony.Compiler;

namespace FreightLanguageCompiler.Nodes
{
	internal class OrderLoopNode : AstNode, IJavascriptGenerator
	{

		private StatementListNode StatementList
		{
			get
			{
				return (StatementListNode)ChildNodes[3];
			}
		}

		public OrderLoopNode(AstNodeArgs args)
			: base(args)
		{
		}

		public void GenerateScript(StringBuilder builder)
		{
			// assume array called "items" exists
			builder.AppendLine("for (var __i = 0; __i < items.length; __i++) {");
			builder.AppendLine("var weight = items[__i].weight;");
			builder.AppendLine("var quantity = items[__i].quantity;");
			StatementList.GenerateScript(builder);
			builder.AppendLine("}");
		}

	}
}
