using Complier.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComplierTool
{
    /// <summary>
    /// 入口程序
    /// </summary>
    public class Program
    {
        static void Main(string[] args)
        {
            bool success = true;
            try
            {
                Project project = new Project(args[0]);
                success = project.Compile();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (success)
                {
                    Environment.Exit(0);
                }
                else
                {
                    Environment.Exit(1);
                }
            }
        }
    }
}
