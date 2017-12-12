using System;
using System.Configuration;

namespace Complier.Core
{
	public class Project
	{
		private const string Key_CurrentEnvironment = "CurrentEnvironment";

		private string projectPath;

		private string currentEnvironment;

		private Configs projectConfigs;

		private Template projectTemplate;

		public Project(string _projectPath)
		{
			this.projectPath = _projectPath;
			this.currentEnvironment = ConfigurationManager.AppSettings["CurrentEnvironment"].Trim();
			this.projectTemplate = new Template(this.projectPath, this.currentEnvironment);
			this.projectConfigs = new Configs(this.projectPath, this.projectTemplate.GetCurrentEnvironment());
		}

		public bool Compile()
		{
			bool successCompile = true;
			ComplierMessage projectTemplateCheck = this.projectTemplate.IsSame();
			if (projectTemplateCheck.Success)
			{
				ComplierMessage configsCheck = this.projectConfigs.IsSuitClone();
				ComplierMessage compileCheck = projectTemplateCheck + configsCheck;
				if (compileCheck.Success)
				{
					this.projectConfigs.CreateConfigFiles();
				}
				else
				{
					successCompile = false;
					compileCheck.PrintMessage();
				}
			}
			else
			{
				successCompile = false;
				projectTemplateCheck.PrintMessage();
			}
			return successCompile;
		}
	}
}
