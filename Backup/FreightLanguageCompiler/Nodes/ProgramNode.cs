using System;
using System.Linq;
using System.Text;
using Irony.Compiler;
using System.Collections.Generic;

namespace FreightLanguageCompiler.Nodes
{
	internal class ProgramNode : AstNode, IJavascriptGenerator
	{

		private StatementListNode StatementList
		{
			get
			{
				return (StatementListNode)ChildNodes[0];
			}
		}

		private FreightDeclarationNode FreightDeclaration
		{
			get
			{
				return (FreightDeclarationNode)ChildNodes[1];
			}
		}

		public ProgramNode(AstNodeArgs args)
			: base(args)
		{
		}

		public void GenerateScript(StringBuilder builder)
		{

			builder.AppendLine("function getFreightCost(customer, region, items) {");

			// declare all the variables used
			var variables = GetAll().OfType<SetVariableNode>().Select(node => node.Variable.Text).Distinct();
			foreach (string variable in variables)
			{
				builder.AppendLine("var " + variable + ";");
			}

			StatementList.GenerateScript(builder);
			FreightDeclaration.GenerateScript(builder);

			builder.AppendLine("}");

		}

		

	}
}
