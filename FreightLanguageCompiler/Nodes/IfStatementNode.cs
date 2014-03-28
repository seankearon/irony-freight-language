using System.Text;
using Irony.Ast;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace FreightLanguageCompiler.Nodes
{
    public class IfStatementNode : AstNode, IJavascriptGenerator
	{
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            TreeNode = treeNode;
        }

        private ParseTreeNode TreeNode { get; set; }


		private ExpressionNode Condition
		{
			get
			{
				return (ExpressionNode)TreeNode.ChildNodes[1].AstNode;
			}
		}

		private StatementListNode StatementList
		{
			get
			{
				return (StatementListNode)TreeNode.ChildNodes[2].AstNode;
			}
		}

		public void GenerateScript(StringBuilder builder)
		{
			builder.Append("if (");
			Condition.GenerateScript(builder);
			builder.AppendLine(") {");
			StatementList.GenerateScript(builder);
			builder.AppendLine("}");

		}

	}
}
