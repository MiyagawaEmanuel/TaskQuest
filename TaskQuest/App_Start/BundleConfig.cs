using System.Web.Optimization;
using System.Collections.Generic;

namespace TaskQuest
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.UseCdn = true;

            //Bundle JS Index
            var IndexJs = new ScriptBundle("~/bundles/index-js").Include(
                "~/Content/js/jquery-1.11.0.min.js",
                "~/Content/js/bootstrap.min.js",
                //"~/Content/js/modernizr-2.7.1.js",
                "~/Content/js/main.js",
                "~/Content/js/wow.min.js"
                //"~/Content/js/spectrum.js",
                //"~/Content/js/codespec.js"
                );
            
            IndexJs.Orderer = new AsIsBundleOrderer();
            bundles.Add(IndexJs);
            
            //Bundle JS Geral (Cru, para páginas que só usam Bootstrap e JQuery
            var GeralJs = new ScriptBundle("~/bundles/geral-js").Include(
                "~/Content/js/jquery-1.11.0.min.js",
                "~/Content/js/bootstrap.min.js",
                "~/Content/js/modernizr-2.7.1.js");
            
            GeralJs.Orderer = new AsIsBundleOrderer();
            bundles.Add(GeralJs);
            
            //Bundle JS Grupos 
            var GruposJs = new ScriptBundle("~/bundles/grupo-js").Include(
                "~/Content/js/jquery-1.11.0.min.js",
                "~/Content/js/bootstrap.min.js",
                "~/Content/jquery-ui/jquery-ui.js",
                "~/Content/js/modernizr-2.7.1.js",
                "~/Content/js/validate.js"
                //"~/Content/js/canvasjs.min.js",
                //"~/Content/js/spectrum.js",
                //"~/Content/js/graphics.js",
                //"~/Content/js/codespec.js"
                );
            
            GruposJs.Orderer = new AsIsBundleOrderer();
            bundles.Add(GruposJs);
            
            //Bundle JS CriarGrupo 
            var CriarGrupoJs = new ScriptBundle("~/bundles/criargrupo-js").Include(
                "~/Content/js/jquery-1.11.0.min.js",
                "~/Content/js/bootstrap.min.js",
                "~/Content/js/modernizr-2.7.1.js"
                //"~/Content/js/spectrum.js",
                //"~/Content/js/codespec.js"
                );
            
            CriarGrupoJs.Orderer = new AsIsBundleOrderer();
            bundles.Add(CriarGrupoJs);
            
            //Bundle JS Usuario 
            var UsuarioJs = new ScriptBundle("~/bundles/usuario-js").Include(
                "~/Content/js/jquery-1.11.0.min.js",
                "~/Content/js/bootstrap.min.js",
                "~/Content/js/modernizr-2.7.1.js",
                "~/Content/js/validate.js"
                //"~/Content/js/canvasjs.min.js",
                //"~/Content/js/spectrum.js",
                //"~/Content/js/graphics.js",
                //"~/Content/js/codespec.js"
                );
            
            UsuarioJs.Orderer = new AsIsBundleOrderer();
            bundles.Add(UsuarioJs);
            
            //Bundle CSS Index 
            bundles.Add( new StyleBundle("~/Content/index-css").Include(
                "~/Content/css/bootstrap.min.css",
                "~/Content/css/main.css",
                //"http://fonts.googleapis.com/css?family=Raleway:400,100,200,300,500,600,700,800,900|Montserrat:400,700",
                "~/Content/font-awesome/font-awesome.min.css",
                "~/Content/font-awesome/animate.css"
                //"~/Content/css/spectrum.css"
                ));
            
            
            //Bundle CSS Geral(Inicio, Grupos)
            var GeralCss = new StyleBundle("~/Content/geral-css").Include(
                "~/Content/css/style.css");
            
            GeralCss.Orderer = new AsIsBundleOrderer();
            bundles.Add(GeralCss);
                
            //Bundle CSS Grupos
            var GruposCss = new StyleBundle("~/Content/grupos-css").Include(
                "~/Content/css/bootstrap.min.css",
                "~/Content/css/style.css",
                //"http://fonts.googleapis.com/css?family=Raleway:400,100,200,300,500,600,700,800,900|Montserrat:400,700",
                "~/Content/font-awesome/font-awesome.min.css",
                //"~/Content/css/spectrum.css",
                "~/Content/jquery-ui/jquery-ui.css");
            
            GruposCss.Orderer = new AsIsBundleOrderer();
            bundles.Add(GruposCss);
            
            //Bundle CSS CriarGrupo
            var CriarGruposCss = new StyleBundle("~/Content/criargrupo-css").Include(
                "~/Content/css/bootstrap.min.css",
                "~/Content/css/style.css",
                //"http://fonts.googleapis.com/css?family=Raleway:400,100,200,300,500,600,700,800,900|Montserrat:400,700",
                "~/Content/font-awesome/font-awesome.min.css"
                //"~/Content/css/spectrum.css"
                );
            
            CriarGruposCss.Orderer = new AsIsBundleOrderer();
            bundles.Add(CriarGruposCss);
            
             //Bundle CSS Usuario
            var UsuarioCss = new StyleBundle("~/Content/usuario-css").Include(
                "~/Content/css/bootstrap.min.css",
                "~/Content/css/style.css",
                //"http://fonts.googleapis.com/css?family=Raleway:400,100,200,300,500,600,700,800,900|Montserrat:400,700",
                "~/Content/font-awesome/font-awesome.min.css",
                //"~/Content/css/spectrum.css",
                "~/Content/jquery-ui/jquery-ui.css");
            
            UsuarioCss.Orderer = new AsIsBundleOrderer();
            bundles.Add(UsuarioCss);
            
            //Bundle CSS Layout
            var Layout = new StyleBundle("~/Content/layout-css").Include(
                "~/Content/css/bootstrap.min.css",
                "~/Content/font-awesome/font-awesome.min.css",
                "~/Content/css/layout.css"
                //"http://fonts.googleapis.com/css?family=Raleway:400,100,200,300,500,600,700,800,900|Montserrat:400,700",
                );
            
            Layout.Orderer = new AsIsBundleOrderer();
            bundles.Add(Layout);

            
           
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
