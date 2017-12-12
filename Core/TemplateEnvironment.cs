using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Complier.Core
{
	public class TemplateEnvironment
	{
		private string environmentName;

		private StringBuilder keyNodesStack;

		private Dictionary<string, string> nodePairs;

		private static XmlDocument templateXmlDocument;

		private static Dictionary<string, string> defaultConfig;

		public string EnvironmentName
		{
			get
			{
				return this.environmentName;
			}
			private set
			{
				this.environmentName = value;
			}
		}

		public Dictionary<string, string> NodePairs
		{
			get
			{
				return this.nodePairs;
			}
		}

		public override int GetHashCode()
		{
			string keyNodes = this.keyNodesStack.ToString();
			return keyNodes.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			return this.GetHashCode() == obj.GetHashCode();
		}

		public TemplateEnvironment(string _environmentName)
		{
			this.environmentName = _environmentName;
			this.keyNodesStack = new StringBuilder();
			this.nodePairs = new Dictionary<string, string>();
		}

		static TemplateEnvironment()
		{
			TemplateEnvironment.templateXmlDocument = new XmlDocument();
		}

		public static List<TemplateEnvironment> GetEnvironments(string _templateFilePath, Dictionary<string, string> _defaultConfig)
		{
			TemplateEnvironment.templateXmlDocument.Load(_templateFilePath);
			TemplateEnvironment.defaultConfig = _defaultConfig;
			return TemplateEnvironment.GetTemplateInfoFromFile();
		}

		internal static List<TemplateEnvironment> GetTemplateInfoFromFile()
		{
			List<TemplateEnvironment> templateEnvironments = new List<TemplateEnvironment>();
			string environmentNodeSelect = string.Format("/{0}/{1}/{2}", "ConfigResourceValueRoot", "Environments", "Environment");
			string resourceNodeSelect = string.Format("/{0}/{1}/{2}", "ConfigResourceValueRoot", "ConfigResourceValues", "{0}");
			XmlNodeList environmentNodes = TemplateEnvironment.templateXmlDocument.SelectNodes(environmentNodeSelect);
			foreach (XmlNode environmentNodeItem in environmentNodes)
			{
				TemplateEnvironment templateEnvironment = new TemplateEnvironment(environmentNodeItem.Attributes["Name"].Value.Trim());
				string resourceEnvironmentSelect = string.Format(resourceNodeSelect, templateEnvironment.environmentName);
				XmlNode resourceEnvironmentNode = TemplateEnvironment.templateXmlDocument.SelectSingleNode(resourceEnvironmentSelect);
				XmlNodeList resourceNodes = resourceEnvironmentNode.ChildNodes;
				foreach (XmlNode resourceNode in resourceNodes)
				{
					templateEnvironment.keyNodesStack.Append(resourceNode.Name);
					templateEnvironment.nodePairs.Add(resourceNode.Name, resourceNode.InnerText.Trim());
				}
				templateEnvironments.Add(templateEnvironment);
			}
			return templateEnvironments;
		}
	}
}
