using System.Web;
using System.Web.Optimization;

namespace OneVietnam
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));
            bundles.Add(new ScriptBundle("~/bundles/theme").Include(
                        "~/Scripts/global.js",
                        "~/Scripts/idangerous.swiper.min.js",
                        "~/Scripts/isotope.pkgd.min.js",
                        "~/Scripts/jquery.countTo.js",
                        "~/Scripts/jquery.viewportchecker.min.js",
                        "~/Scripts/map.js",
                        "~/Scripts/sorttable.js",
                        "~/Scripts/wow.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.min.css",
                      "~/Content/icon.css",
                      "~/Content/idangerous.swiper.css",
                      "~/Content/idangerous.swiper_backup.css",
                      "~/Content/ie-9.css",
                      "~/Content/loader.css",
                      "~/Content/stylesheet.css"));
        }
    }
}
