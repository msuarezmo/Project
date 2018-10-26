using System.Web;
using System.Web.Optimization;

namespace CapaPresentacion
{
    public class BundleConfig
    {
        // Para obtener más información sobre las uniones, visite https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/users").Include(
                       "~/scripts/Users.js"));
            //CURSOS
            bundles.Add(new ScriptBundle("~/bundles/courses").Include(
                       "~/scripts/Courses.js"));
            //Estudiantes
            bundles.Add(new ScriptBundle("~/bundles/students").Include(
                       "~/scripts/Students.js"));
            //MATERIAS
            bundles.Add(new ScriptBundle("~/bundles/subjects").Include(
           "~/scripts/Subjects.js"));
            //Notificaciones
            bundles.Add(new ScriptBundle("~/bundles/toastr").Include(
                       "~/Scripts/toastr.js*",
                       "~/Scripts/toastrImp.js"));

            bundles.Add(new StyleBundle("~/Content/login").Include(
          "~/Resources/css/login.css"));

            // Utilice la versión de desarrollo de Modernizr para desarrollar y obtener información. De este modo, estará
            // para la producción, use la herramienta de compilación disponible en https://modernizr.com para seleccionar solo las pruebas que necesite.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                       "~/Content/toastr.css"));
        }
    }
}
