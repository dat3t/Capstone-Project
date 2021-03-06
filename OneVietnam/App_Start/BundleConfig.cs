﻿using System.Web.Optimization;

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
                "~/Scripts/flickity.pkgd.min.js",
                "~/Scripts/global.js",
                "~/Scripts/infiniteScroll.js",
                "~/Scripts/imagesloaded.pkgd.min.js",
                "~/Scripts/masonry.pkgd.min.js",
                "~/Scripts/isotope.pkgd.min.js"));
            bundles.Add(new StyleBundle("~/semantic/css").Include(
                      "~/semantic/dist/semantic.css"));
            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/StyleSheet.css",
                "~/Content/chatbox.css",
                "~/Content/flickity.css",
                  "~/Content/Map/map.css",
                "~/Content/Map/searchbox.css",
                "~/Content/Map/mappagecustom.css",
                 "~/Content/Map/scrollbar.css"));
            bundles.Add(new StyleBundle("~/Scripts/Map").Include(
                "~/Scripts/Markerclusterer.js"
                ));
            bundles.Add(new StyleBundle("~/signalR.js").Include(
                "~/Scripts/jquery.signalR-2.2.0.min.js"));

        }
    }
}
