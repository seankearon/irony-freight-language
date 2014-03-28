using System.Linq;
using System.Text;
using Irony.Ast;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace FreightLanguageCompiler.Nodes
{
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
			get
			{
				return (StatementListNode) TreeNode.ChildNodes[0].AstNode;
			}
		}

		private FreightDeclarationNode FreightDeclaration
		{
			get
			{
                return (FreightDeclarationNode) TreeNode.ChildNodes[1].AstNode;
			}
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
}
