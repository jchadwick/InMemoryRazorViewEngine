using System;
using System.IO;

namespace RazorPad.Mvc.SelfHosting.Tests
{
    public class RazorViewTest
    {
        public void Test()
        {
            var aspNet = new InMemoryAspNet();
            Console.WriteLine(aspNet.ProcessRequest("", "view=test"));
        }
    }
}
