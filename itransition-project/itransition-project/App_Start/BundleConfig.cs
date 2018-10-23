using System.Web;
using System.Web.Optimization;

namespace itransition_project
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
            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                "~/Scripts/angular.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/DragAndDrop").Include(
                "~/Scripts/DragAndDrop.js"));
            bundles.Add(new ScriptBundle("~/bundles/AdminDragDrop").Include(
                "~/Scripts/app/AdminDragDrop.js"));
            bundles.Add(new ScriptBundle("~/bundles/LikeAndDislike").Include(
                "~/Scripts/app/LikeAndDislike.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-1.11.4.min.js"));

            bundles.Add(new StyleBundle("~/Content/jqueryui").Include(
                      "~/Content/themes/base/all.css"));

            bundles.Add(new ScriptBundle("~/bundles/template").Include(
                        "~/Scripts/templates_script.js"));

            bundles.Add(new ScriptBundle("~/Content/DragAndDrop").Include(
                "~/Content/DragAndDrop.css"));

            bundles.Add(new ScriptBundle("~/bundles/AddComix").Include(
                        "~/Scripts/app/AddComix.js"));

            bundles.Add(new ScriptBundle("~/bundles/ViewComix").Include(
                        "~/Scripts/app/ViewComix.js"));

            bundles.Add(new ScriptBundle("~/bundles/UserInfo").Include(
                        "~/Scripts/app/UserInfo.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/ng-tags-input").Include(
                "~/Scripts/ng-tags-input.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/tagCloud").Include(
                "~/Scripts/app/TagCloud.js"
                ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/css_light").Include(
                      "~/Content/bootstrap-flatly.min.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/css_dark").Include(
                      "~/Content/bootstrap-darkly.min.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/balloons").Include(
                      "~/Content/app/balloons_styles.css"));

            bundles.Add(new StyleBundle("~/Content/ng-tags-input").Include(
                    "~/Content/ng-tags-input.css",
                    "~/Content/ng-tags-input.bootstrap.css"));
        }
    }
}
