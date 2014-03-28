using System;
using System.Text;
using Irony.Parsing;

namespace FreightLanguageCompiler
{
	/// <summary>
	/// Translates from FL to JavaScript.
	/// Unlike the original article, our JS generator nodes are accessed through <see cref="ParseTreeNode.AstNode"/>.
	/// </summary>
	public static class FLCompiler
	{

		public static string Compile(string sourceCode)
		{
			var grammar = new FLGrammar();
            var parser = new Parser(grammar);

            var parseTree = parser.Parse(sourceCode);
            if (parseTree.HasErrors())
			{
                throw new CompilationException(string.Join("\n", parseTree.ParserMessages));
			}

            var generator = (IJavascriptGenerator)parseTree.Root.AstNode;
			var js = new StringBuilder();
			generator.GenerateScript(js);
			return js.ToString();
		}

	}

	public class CompilationException : Exception
	{
		public CompilationException(string message)
			: base(message)
		{

		}
	}


}
