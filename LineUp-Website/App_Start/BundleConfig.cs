using System.Web;
using System.Web.Optimization;

namespace LineUp_Website
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
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new Bundle("~/bundles/angular").Include(
                      "~/Scripts/smart-table.min.js",
                      "~/Scripts/angular -input-stars.js",
                      "~/Angular/app.js",
                      "~/Angular/Factories/DefaultFactory.js",
                      "~/Angular/Controllers/League/LeagueDetailsCtrl.js",
                      "~/Angular/Controllers/League/LeagueCreateCtrl.js",
                      "~/Angular/Controllers/League/LeagueJoinCtrl.js",
                      "~/Angular/Controllers/League/LeagueSelectCtrl.js",
                      "~/Angular/Controllers/League_Team/League_TeamListCtrl.js",
                      "~/Angular/Controllers/League_Team/League_TeamDetailsCtrl.js",
                      "~/Angular/Controllers/Round/RoundDetailsCtrl.js",
                      "~/Angular/Controllers/Round/RoundListCtrl.js",
                      "~/Angular/Controllers/Round/RoundManagerCtrl.js",
                      "~/Angular/Controllers/SideBar/Make_PicksCtrl.js",
                      "~/Angular/Controllers/Game/GameDetailsCtrl.js",
                      "~/Angular/Controllers/Game/GameDashboardCtrl.js",
                      "~/Angular/Controllers/Game/GameListCtrl.js",
                      "~/Angular/Controllers/Team/TeamDetailsCtrl.js",
                      "~/Angular/Controllers/Account/AccountDetailsCtrl.js"
                      )); 

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/angular-input-stars.css",
                      "~/Content/site.css"));

        }
    }
}
