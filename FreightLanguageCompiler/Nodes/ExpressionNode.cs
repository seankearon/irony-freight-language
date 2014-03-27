using System;
using System.Text;
using Irony.Compiler;

namespace FreightLanguageCompiler.Nodes
{
	internal class ExpressionNode : AstNode, IJavascriptGenerator
	{

		public ExpressionNode(AstNodeArgs args)
			: base(args)
		{
		}

		public void GenerateScript(StringBuilder builder)
		{
			// different expression types have different number of children
			foreach (var child in ChildNodes)
			{

				IJavascriptGenerator jsChild = child as IJavascriptGenerator;
				if (jsChild != null)
				{
					jsChild.GenerateScript(builder);
				}
				else
				{
					Token token = child as Token;
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
