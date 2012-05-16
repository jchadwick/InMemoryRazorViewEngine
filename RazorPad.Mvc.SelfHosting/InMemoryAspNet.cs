using System;
using System.Web;
using System.Web.Hosting;
using System.IO;
using System.Web.Mvc;
using System.Web.Routing;

[assembly: PreApplicationStartMethod(typeof(RouteInitializer), "Initialize")]


public class InMemoryAspNet
{
    public string PhysicalDirectory { get; set; }

    public InMemoryAspNet()
    {
        PhysicalDirectory = Environment.CurrentDirectory;
        ProcessRequest("default.aspx");
    }

    public string ProcessRequest(string path, string query = null)
    {
        using (var stream = new StringWriter())
        {
            ProcessRequest(stream, path, query);
            stream.Flush();
            return stream.GetStringBuilder().ToString();
        }
    }

    public void ProcessRequest(TextWriter stream, string path, string query = null)
    {
        var host = (AspNetHost)ApplicationHost.CreateApplicationHost(typeof(AspNetHost), "/", PhysicalDirectory);
        host.ProcessRequest(stream, path, query);
    }


    class AspNetHost : MarshalByRefObject
    {
        public void ProcessRequest(TextWriter stream, string path, string query = null)
        {
            var request = new SimpleWorkerRequest(path, query, stream);
            HttpRuntime.ProcessRequest(request);
        }

        public override object InitializeLifetimeService()
        {
            // This tells the CLR not to surreptitiously 
            // destroy this object -- it's a singleton
            // and will live for the life of the appdomain
            return null;
        }
    }

}

public class RouteInitializer
{
    public static void Initialize()
    {
        RouteTable.Routes.MapRoute("Razor", "", new { controller = "Razor", action = "Index" });
    }
}

public class RazorController : Controller
{
    public ActionResult Index(string view)
    {
        return View(view);
    }
}