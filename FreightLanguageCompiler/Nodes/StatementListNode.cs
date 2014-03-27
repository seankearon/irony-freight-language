using System;
using System.Text;
using Irony.Compiler;

namespace FreightLanguageCompiler.Nodes
{
	internal class StatementListNode : AstNode, IJavascriptGenerator
	{

		public StatementListNode(AstNodeArgs args)
			: base(args)
		{
		}

		public void GenerateScript(StringBuilder builder)
		{
			foreach (StatementNode statement in ChildNodes)
			{
				statement.GenerateScript(builder);
			}
		}

	}
}
