using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Complier.Core;
using System.IO;

namespace Core.Test
{
    /// <summary>
    /// 编译项目的测试
    /// </summary>
    [TestFixture]
    public class ComplierProjectTest
    {
        /// <summary>
        /// 测试编译项目
        /// </summary>
        [Test]
        public void ComplateProject()
        {
            string projectDir =Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"Project");
            Project project = new Project(projectDir);
            var success = project.Compile();
            Assert.AreEqual(success, true);
        }
    }
}
