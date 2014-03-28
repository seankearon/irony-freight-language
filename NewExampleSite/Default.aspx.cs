using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FreightLanguageCompiler;

namespace NewExampleSite
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                sourceCodeTextBox.Text = @"Set pricePerKG to 2.5;
If region is ""asia"" [
set pricePerKG to 2.2;
]
Set totalWeight to 0;
loop through order [
Set totalWeight to totalWeight + (quantity * weight);
]
Set freightCost to totalWeight * pricePerKG;
if customer is ""VIP"" [
set freightCost to freightCost * 0.7;
]
Freight cost is freightCost;
";
            }
        }

        protected void runButton_Click(object sender, EventArgs e)
        {

            try
            {
                string js = FLCompiler.Compile(sourceCodeTextBox.Text);
                js = MakeIndented(js);
                javascriptTextBox.Text = js;
                ClientScript.RegisterClientScriptBlock(GetType(), "gend", js, true);
            }
            catch (CompilationException ex)
            {
                errorMessageParagraph.Visible = true;
                errorMessageParagraph.InnerText = ex.Message;
            }


        }

        /// <summary>
        /// Indent functions, if statments, and loops, just so the javascript is easier to read.
        /// </summary>
        private string MakeIndented(string js)
        {
            int indentation = 0;
            StringReader sr = new StringReader(js);
            StringBuilder pretty = new StringBuilder();
            string line = sr.ReadLine();
            while (line != null)
            {
                if (line.Contains("}"))
                {
                    indentation--;
                }
                pretty.Append(new string('\t', indentation));
                pretty.AppendLine(line);


                if (line.Contains("{"))
                {
                    indentation++;
                }


                line = sr.ReadLine();
            }
            return pretty.ToString();
        }
    }
}