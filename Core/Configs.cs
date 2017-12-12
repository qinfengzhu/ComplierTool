using System;
using System.Collections.Generic;

namespace Complier.Core
{
	public class Configs
	{
		internal const string Key_DefaultConfigsTplDictionary = "Configs";

		internal const string Key_TemplateFileExtension = "tpl";

		internal const string Key_SearchTplPattern = "*.tpl";

		private string projectPath;

		private TemplateEnvironment currentEnvironment;

		private List<TplFile> TplFiles;

		public TemplateEnvironment CurrentEnvironment
		{
			get
			{
				return this.currentEnvironment;
			}
			private set
			{
				this.currentEnvironment = value;
			}
		}

		public Configs(string _projectPath, TemplateEnvironment _currentEnvironment)
		{
			this.projectPath = _projectPath;
			this.currentEnvironment = _currentEnvironment;
			this.TplFiles = TplFile.GetTplFiles(this.projectPath);
		}

		public ComplierMessage IsSuitClone()
		{
			ComplierMessage result = new ComplierMessage(true);
			Dictionary<string, string> replaceSigns = new Dictionary<string, string>();
			foreach (TplFile tplfile in this.TplFiles)
			{
				foreach (KeyValuePair<string, string> sign in tplfile.ReplaceSign)
				{
					if (!replaceSigns.ContainsKey(sign.Key))
					{
						replaceSigns.Add(sign.Key, sign.Value);
					}
				}
			}
			foreach (KeyValuePair<string, string> replaceSign in replaceSigns)
			{
				if (!this.currentEnvironment.NodePairs.ContainsKey(replaceSign.Value))
				{
					result.Success = false;
					result.ComplierMsg.Add(string.Format("{0}:缺少节点{1},资源标识{2}", this.currentEnvironment.EnvironmentName, replaceSign.Value, replaceSign.Key));
				}
			}
			return result;
		}

		public void CreateConfigFiles()
		{
			foreach (TplFile tplFile in this.TplFiles)
			{
				tplFile.CloneFile(this.currentEnvironment);
			}
		}
	}
}
