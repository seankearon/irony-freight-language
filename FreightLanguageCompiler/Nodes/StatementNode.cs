using System.Text;
using Irony.Ast;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace FreightLanguageCompiler.Nodes
{
    public class StatementNode : AstNode, IJavascriptGenerator
	{
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            TreeNode = treeNode;
        }

        private ParseTreeNode TreeNode { get; set; }
        
        /// <summary>
		/// A statement has a single child: either SetVariable, IfStatement,  OrderLoop, or Expression.
		/// </summary>
		private AstNode Child
		{
			get
			{
				return (AstNode)TreeNode.ChildNodes[0].AstNode;
			}
		}

		public void GenerateScript(StringBuilder builder)
		{
			var child = (IJavascriptGenerator)Child;
			child.GenerateScript(builder);

			// if the statement is just an expression, then there will be no semicolon at the end
			if (child is ExpressionNode)
			{
				builder.AppendLine(";");
			}

		}

	}
}
