using System;
using System.Linq;
using System.Text;
using Irony.Ast;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace FreightLanguageCompiler.Nodes
{
    public class ExpressionNode : AstNode, IJavascriptGenerator
	{
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            TreeNode = treeNode;
        }

        private ParseTreeNode TreeNode { get; set; }

	    public void GenerateScript(StringBuilder builder)
		{
			// different expression types have different number of children
            foreach (ParseTreeNode child in TreeNode.ChildNodes)
			{

				var jsChild = child.AstNode as IJavascriptGenerator;
				if (jsChild != null)
				{
					jsChild.GenerateScript(builder);
				}
				else
				{
					Token token = child.Token;
					if (token != null)
					{
						// Just send the text that the user entered straight to javascript.
						// In most languages, it is not as simple as this (some sort of 
						// transformation from the source to the destination language is needed).
						string expressionAsJavaScript = token.Text;

						// A string (which is an StringLiteral in the grammar) is either a region
						//  or a customer type. To simplify comparisons, strings are converted to
						// lowercase here.
						if (token.Terminal is StringLiteral)
						{
							expressionAsJavaScript = expressionAsJavaScript.ToLower();
						}

						builder.Append(expressionAsJavaScript);
					}
					else
					{
						throw new InvalidOperationException("Was not expected a child of type " + child.GetType().FullName);
					}
				}
			}

		}

	}
}
