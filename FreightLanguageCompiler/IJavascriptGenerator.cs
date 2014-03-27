using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FreightLanguageCompiler
{
	internal interface IJavascriptGenerator
	{
		void GenerateScript(StringBuilder builder);
	}
}
