Irony Freight Language
======================

An update of Daniel Flower's Freight Language example more the more recent version of Irony.

[Daniel Flower's](http://www.codeproject.com/script/Membership/View.aspx?mid=3290305) original [article ](http://www.codeproject.com/Articles/29058/Writing-your-first-Domain-Specific-Language-Part) on Code Project shows how to use the [Irony .Net Language Implementation Kit](http://irony.codeplex.com/) to generate Javascript from an external DSL.
   
Overview
=========
Firstly, note that I am new to Irony so I do not have experience of the previous version.  Please feel free to contribute any corrections or updates!

The most significant change seems to be the way Irony let you access your nodes in the parse tree.  From reading Daniel's code it seems like Irony would previously give you back instances of your node classes through `ChildNodes`: 


    internal class ProgramNode : AstNode, IJavascriptGenerator
	{
		private StatementListNode StatementList
		{
			get { return (StatementListNode)ChildNodes[0]; }
		}

		private FreightDeclarationNode FreightDeclaration
		{
			get { return (FreightDeclarationNode)ChildNodes[1]; }
		}

		public ProgramNode(AstNodeArgs args) : base(args) { }

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


This has now changed and it seems you need to access these through the related `ParseTreeNode` which can be accessed using `Init()`.  To keep the code looking similar to Daniel's original I'm just holding the `ParseTreeNode` and using that in the remaining code to access Daniel's custom node instances:

    public class ProgramNode : AstNode, IJavascriptGenerator
	{
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            TreeNode = treeNode;
        }

        private ParseTreeNode TreeNode { get; set; }

    	private StatementListNode StatementList
		{
			get { return (StatementListNode) TreeNode.ChildNodes[0].AstNode; } }

		private FreightDeclarationNode FreightDeclaration
		{
			get { return (FreightDeclarationNode) TreeNode.ChildNodes[1].AstNode; }
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
 
The other area that's changed is how you use the `Parser`.  Daniel's original `FLCompiler` looked like this:

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

This has now become:

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

Lastly, I was getting an NRE when parsing the tree.  This was addressed with some very rapid assistance from the creator of Irony, [rivanstov](https://www.codeplex.com/site/users/view/rivantsov), who suggested deriving the `FLGrammar` from `InterpretedLanguageGrammar`.  Read more [here](https://irony.codeplex.com/discussions/361018).