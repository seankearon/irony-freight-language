using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Irony.Compiler;

namespace FreightLanguageCompiler
{
	/// <summary>
	/// Translates from FL to JavaScript.
	/// </summary>
	public static class FLCompiler
	{

		public static string Compile(string sourceCode)
		{
			// create a compiler from the grammar
			FLGrammar grammar = new FLGrammar();
			LanguageCompiler compiler = new LanguageCompiler(grammar);

			// Attempt to compile into an Abstract Syntax Tree. Because FLGrammar
			// defines the root node as ProgramNode, that is what will be returned.
			// This happens to implement IJavaScriptGenerator, which is what we need.
			IJavascriptGenerator program = (IJavascriptGenerator)compiler.Parse(sourceCode);
			if (program == null || compiler.Context.Errors.Count > 0)
			{
				// Didn't compile.  Generate an error message.
				SyntaxError error = compiler.Context.Errors[0];
				string location = string.Empty;
				if (error.Location.Line > 0 && error.Location.Column > 0)
				{
					location = "Line " + (error.Location.Line + 1) + ", column " + (error.Location.Column + 1);
				}
				string message = location + ": " + error.Message + ":" + Environment.NewLine;
				message += sourceCode.Split('\n')[error.Location.Line];

				throw new CompilationException(message);
			}

			// now just instruct the compilation of to javascript
			StringBuilder js = new StringBuilder();
			program.GenerateScript(js);
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
