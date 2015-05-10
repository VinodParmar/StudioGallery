using System.Web;
using System.Web.Optimization;

namespace Majstic
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                            "~/Scripts/jquery.unobtrusive-ajax",
                            "~/Scripts/classie.js",
                            "~/Scripts/menu.js",
                        "~/Scripts/jquery.unobtrusive-ajax.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/modernizr.custom.25376.js"
                                            ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/component.css",
                      "~/Content/normalize.css",
                      "~/Content/demo.css",
                      "~/Content/index.css"));



            bundles.Add(new StyleBundle("~/XContent/css").Include(
                   "~/Content/bootstrap.css",
                   "~/Content/site.css",
                   "~/Content/index.css"));


            bundles.Add(new ScriptBundle("~/bundles/Xbootstrap").Include(
                    "~/Scripts/bootstrap.js"
                                          ));

            bundles.Add(new ScriptBundle("~/bundles/Xjquery").Include(
                       "~/Scripts/jquery-{version}.js",
                           "~/Scripts/jquery.unobtrusive-ajax",
                       "~/Scripts/jquery.unobtrusive-ajax.min.js"));



        }
    }
}

