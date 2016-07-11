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
            bundles.Add(new ScriptBundle("~/bundles/semantic").Include(
                      "~/semantic/dist/semantic.js"));
            bundles.Add(new ScriptBundle("~/bundles/Scripts").Include(
                "~/Scripts/global.js"));
            bundles.Add(new StyleBundle("~/semantic/css").Include(
                      "~/semantic/dist/semantic.css"));
            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/StyleSheet.css",
                  "~/Content/Map/map.css",
                "~/Content/Map/infoWindow.css",
                "~/Content/Map/searchbox.css"));
            bundles.Add(new StyleBundle("~/Scripts/Map").Include(
                        "~/Scripts/map.js",
                       
                        "~/semantic/dist/semantic.js",
                        "~/Scripts/minimal-slide.js",
                       
                "~/Scripts/markerclusterer.js"
                        ));
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/site.css",
                      "~/Content/icon.css",
                      "~/Content/idangerous.swiper.css",
                      "~/Content/idangerous.swiper_backup.css",
                      "~/Content/ie-9.css",
                      "~/Content/loader.css",
                      "~/semantic/dist/semantic.min.css",
                      "~/Content/icon.min.css",
                      "~/Content/minimal-slide.css"));
        }
    }
}
