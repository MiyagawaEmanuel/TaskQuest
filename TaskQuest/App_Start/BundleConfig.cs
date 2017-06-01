using System.Web.Optimization;
using System.Collections.Generic;

namespace TaskQuest
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            var valBundle = new

            //Bundles de JavaScript
            bundles.Add(new ScriptBundle("~/bundles/bootstrap-jquery").Include("~/Scripts/jquery-1.11.0.min.js", "~/Scripts/bootstrap.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include("~/Scripts/modernizr-2.7.1.js"));
            bundles.Add(new ScriptBundle("~/bundles/colors").Include("~/Scripts/spectrum.js", "~/Scripts/codespec.js"));
            bundles.Add(new ScriptBundle("~/bundles/canvas").Include("http://canvasjs.com/assets/script/canvasjs.min.js", "~/Scripts/graphics.js"));
            bundles.Add(new ScriptBundle("~/bundles/uitabs").Include("~/Scripts/jquery-ui/jquery-ui.js", "~/Scripts/tabs.js"));
            bundles.Add(new ScriptBundle("~/bundles/validate").Include("~/Scripts/validate.js"));
            bundles.Add(new ScriptBundle("~/bundles/wow").Include("~/Scripts//wow.min.js"));

            //Bundles de CSS
            bundles.Add(new StyleBundle("~/Content/bootstrap").Include("~/Content/bootstrap.min.css"));
            bundles.Add(new StyleBundle("~/Content/fonts").Include("http://fonts.googleapis.com/css?family=Raleway:400,100,200,300,500,600,700,800,900|Montserrat:400,700","~/Content/font-awesome.min.css"));
            bundles.Add(new StyleBundle("~/Content/layout").Include("~/Content/layout.css"));
            bundles.Add(new StyleBundle("~/Content/landing-page").Include("~/Content/main.css", "~/Content/animate.css"));
            bundles.Add(new StyleBundle("~/Content/style").Include("~/Content/style.css"));
            bundles.Add(new StyleBundle("~/Content/spectrum").Include("~/Content/spectrum.css"));
            bundles.Add(new StyleBundle("~/Content/jquery-ui").Include("~/Content/jquery-ui.css"));

            valBundle.Orderer = new AsIsBundleOrderer();

            bundles.Add(valBundle);

        }

        public class AsIsBundleOrderer : IBundleOrderer
        {
            public IEnumerable<BundleFile> OrderFiles(BundleContext context, IEnumerable<BundleFile> files)
            {
                return files;
            }
        }

    }
}