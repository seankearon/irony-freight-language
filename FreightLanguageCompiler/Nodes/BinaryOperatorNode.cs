using System.Text;
using Irony.Compiler;

namespace FreightLanguageCompiler.Nodes
{
	internal class BinaryOperatorNode : AstNode, IJavascriptGenerator
	{

		private Token Token
		{
			get
			{ // there is only one child, which is a binary operator
				return (Token)ChildNodes[0];
			}
		}
		
		public BinaryOperatorNode(AstNodeArgs args)
			: base(args)
		{
		}


		public void GenerateScript(StringBuilder builder)
		{
			// The binary operators for FL and Javascript are the same,
			// except for the equality operator.
			string op = Token.Text;
			if (op == "is")
			{
				builder.Append("==");
			}
			else
			{
				builder.Append(op);
			}
		}

	}
}
