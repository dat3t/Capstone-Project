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
       
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/semantic/dist/semantic.min.js",
                        "~/Scripts/global.js",
                        "~/Scripts/map.js",
                        "~/Scripts/dropzone.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/semantic/dist/semantic.min.css",
                "~/Content/Site.css",
                      "~/Content/dropzone.css",
                      "~/Content/stylesheet.css",                                      
                  "~/Content/Map/map.css",
                "~/Content/Map/infoWindow.css",
                "~/Content/Map/searchbox.css",
                "~/Content/Map/mappagecustom.css",
                 "~/Content/Map/scrollbar.css"
                 ));
            bundles.Add(new StyleBundle("~/Scripts/Map").Include(
                "~/Scripts/map.js",
                "~/Scripts/markerclusterer.js",
                "~/Scripts/searchboxCreatePost"
                ));
        }
    }
}
