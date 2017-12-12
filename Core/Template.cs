using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Complier.Core
{
	public class Template
	{
		internal const string Key_ResourceValueXMLName = "ConfigResourceValue.xml";

		internal const string Key_CurrentEnvironment = "Key_CurrentEnvironment";

		internal const string Key_RootNode = "ConfigResourceValueRoot";

		internal const string Key_Environments = "Environments";

		internal const string Key_Environment = "Environment";

		internal const string Key_ConfigResourceValues = "ConfigResourceValues";

		private string projectPath;

		private string currentEnvironment;

		private string environmentsFile;

		private List<TemplateEnvironment> Environments;

		public string CurrentEnvironment
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

		public TemplateEnvironment GetCurrentEnvironment()
		{
			return (from currentEnv in this.Environments
			where currentEnv.EnvironmentName.Equals(this.CurrentEnvironment)
			select currentEnv).First<TemplateEnvironment>();
		}

		public Template(string _projectPath, string _currentEnvironment)
		{
			this.projectPath = _projectPath;
			this.currentEnvironment = _currentEnvironment;
			this.environmentsFile = Path.Combine(this.projectPath, "ConfigResourceValue.xml");
			Dictionary<string, string> defaultConfig = new Dictionary<string, string>
			{
				{
					"Key_RootNode",
					"ConfigResourceValueRoot"
				},
				{
					"Key_Environments",
					"Environments"
				},
				{
					"Key_Environment",
					"Environment"
				},
				{
					"Key_ConfigResourceValues",
					"ConfigResourceValues"
				},
				{
					"Key_CurrentEnvironment",
					this.currentEnvironment
				}
			};
			this.Environments = TemplateEnvironment.GetEnvironments(this.environmentsFile, defaultConfig);
		}

		public ComplierMessage IsSame()
		{
			ComplierMessage result = new ComplierMessage(true);
			TemplateEnvironment currentEnvironmentTemplate = (from template in this.Environments
			where template.EnvironmentName.Equals(this.currentEnvironment)
			select template).First<TemplateEnvironment>();
			foreach (TemplateEnvironment environmentTemplate in this.Environments)
			{
				if (!currentEnvironmentTemplate.Equals(environmentTemplate))
				{
					result.Success = false;
					result.ComplierMsg.AddRange(this.PrintDifference(currentEnvironmentTemplate, environmentTemplate));
				}
			}
			return result;
		}

		private List<string> PrintDifference(TemplateEnvironment source, TemplateEnvironment target)
		{
			List<string> differenceContents = new List<string>();
			foreach (KeyValuePair<string, string> sourceNodePair in source.NodePairs)
			{
				if (!target.NodePairs.ContainsKey(sourceNodePair.Key))
				{
					string keyDifference = string.Format("{0}-{1}:{2}", source.EnvironmentName, target.EnvironmentName, sourceNodePair.Key);
					differenceContents.Add(keyDifference);
				}
			}
			this.ResolveDifference(target, source, differenceContents);
			return differenceContents;
		}

		private void ResolveDifference(TemplateEnvironment target, TemplateEnvironment source, List<string> differenceContents)
		{
			foreach (KeyValuePair<string, string> targetNodePair in target.NodePairs)
			{
				if (!source.NodePairs.ContainsKey(targetNodePair.Key))
				{
					string keyDifference = string.Format("{0}-{1}:{2}", source.EnvironmentName, target.EnvironmentName, targetNodePair.Key);
					if ((from difference in differenceContents
					where difference.Equals(keyDifference)
					select difference).Count<string>() == 0)
					{
						differenceContents.Add(keyDifference);
					}
				}
			}
		}
	}
}
