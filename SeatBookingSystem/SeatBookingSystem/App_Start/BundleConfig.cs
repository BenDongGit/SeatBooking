using System.Web;
using System.Web.Optimization;

namespace SeatBookingSystem
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
              "~/Scripts/angular/angular.js",
              "~/Scripts/angular/angular-sanitize.js",
              "~/Scripts/angular/angular-animate.js",
              "~/Scripts/angular/angular-ui/ui-bootstrap.js",
              "~/Scripts/angular/angular-ui/ui-bootstrap-tpls.js"));

            bundles.Add(new ScriptBundle("~/bundles/bookingseat")
                   .Include("~/Scripts/bookingseat/main.js")
                   .IncludeDirectory("~/Scripts/bookingseat/controllers", "*.js"));
        }
    }
}
