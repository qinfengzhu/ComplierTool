using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Complier.Core
{
	public class TplFile
	{
		internal const string regexPattern = "\\$\\S+\\$";

		private string fileDirectory;

		private string fileName;

		private string fileContent;

		private Dictionary<string, string> replaceSign;

		public string FileDirectory
		{
			get
			{
				return this.fileDirectory;
			}
		}

		public string FileName
		{
			get
			{
				return this.fileName;
			}
		}

		public string FileContent
		{
			get
			{
				return this.fileContent;
			}
		}

		public Dictionary<string, string> ReplaceSign
		{
			get
			{
				return this.replaceSign;
			}
		}

		public TplFile(string _projectPath, string _fileName)
		{
			this.fileDirectory = Path.Combine(_projectPath, "Configs");
			this.fileName = _fileName;
			string filePath = Path.Combine(this.fileDirectory, this.fileName);
			this.fileContent = File.ReadAllText(filePath);
			this.replaceSign = TplFile.TakeReplaceSignAndNodeName(this.fileContent);
		}

		private static Dictionary<string, string> TakeReplaceSignAndNodeName(string _fileContent)
		{
			Dictionary<string, string> replcaceSign = new Dictionary<string, string>();
			Regex regex = new Regex("\\$\\S+\\$");
			MatchCollection matchCollection = regex.Matches(_fileContent);
			foreach (Match matchItem in matchCollection)
			{
				string mvalue = matchItem.Value.Trim(new char[]
				{
					'$'
				});
				string mkey = matchItem.Value;
				if (!replcaceSign.ContainsKey(mkey))
				{
					replcaceSign.Add(mkey, mvalue);
				}
			}
			return replcaceSign;
		}

		internal static List<TplFile> GetTplFiles(string _projectPath)
		{
			List<TplFile> tplFiles = new List<TplFile>();
			string tplPath = Path.Combine(_projectPath, "Configs");
			string[] tplFilesPath = Directory.GetFiles(tplPath, "*.tpl");
			string[] array = tplFilesPath;
			for (int i = 0; i < array.Length; i++)
			{
				string tplFilePath = array[i];
				string[] tplFileSplits = tplFilePath.Split(new char[]
				{
					'\\'
				});
				string fileName = tplFileSplits[tplFileSplits.Length - 1];
				TplFile tplFile = new TplFile(_projectPath, fileName);
				tplFiles.Add(tplFile);
			}
			return tplFiles;
		}

		public void CloneFile(TemplateEnvironment _currentEnvironment)
		{
			foreach (KeyValuePair<string, string> replaceSignItem in this.replaceSign)
			{
				this.fileContent = this.fileContent.Replace(replaceSignItem.Key, _currentEnvironment.NodePairs[replaceSignItem.Value]);
			}
			string fileExtension = string.Format(".{0}", "tpl");
			string newFileName = this.fileName.Replace(fileExtension, "");
			string newFilePath = Path.Combine(this.fileDirectory, newFileName);
			File.WriteAllText(newFilePath, this.fileContent);
		}
	}
}
