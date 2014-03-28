using System.Linq;
using System.Text;
using Irony.Ast;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace FreightLanguageCompiler.Nodes
{
    public class StatementListNode : AstNode, IJavascriptGenerator
	{
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            TreeNode = treeNode;
        }

        private ParseTreeNode TreeNode { get; set; }
        
        public void GenerateScript(StringBuilder builder)
		{
			foreach (var statement in TreeNode.ChildNodes.Select(x => x.AstNode).Cast<StatementNode>())
			{
				statement.GenerateScript(builder);
			}
		}

	}
}
