using System.Text;
using Irony.Ast;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace FreightLanguageCompiler.Nodes
{
    public class OrderLoopNode : AstNode, IJavascriptGenerator
	{
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            TreeNode = treeNode;
        }

        private ParseTreeNode TreeNode { get; set; }
        
        private StatementListNode StatementList
		{
			get
			{
				return (StatementListNode)TreeNode.ChildNodes[3].AstNode;
			}
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
