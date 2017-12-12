using System;
using System.Collections.Generic;

namespace Complier.Core
{
	public class ComplierMessage
	{
		private bool success;

		private List<string> complierMsg;

		public bool Success
		{
			get
			{
				return this.success;
			}
			set
			{
				this.success = value;
			}
		}

		public List<string> ComplierMsg
		{
			get
			{
				return this.complierMsg;
			}
		}

		public ComplierMessage()
		{
			this.complierMsg = new List<string>();
		}

		public ComplierMessage(bool _success)
		{
			this.complierMsg = new List<string>();
			this.success = _success;
		}

		public static ComplierMessage operator +(ComplierMessage a, ComplierMessage b)
		{
			ComplierMessage newComplierMessage = new ComplierMessage(a.success && b.success);
			newComplierMessage.complierMsg.AddRange(a.complierMsg);
			newComplierMessage.complierMsg.AddRange(b.complierMsg);
			return newComplierMessage;
		}

		public void PrintMessage()
		{
			foreach (string msg in this.complierMsg)
			{
				Console.WriteLine(msg);
			}
		}
	}
}
