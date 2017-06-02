using System.Web.Optimization;
using System.Collections.Generic;

namespace TaskQuest
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            

            //Bundle JS Index
            var indexjs = new ScriptBundle("~/bundles/index").Include(
                "~/Scripts/jquery-1.11.0.min.js",
                "~/Scripts/bootstrap.min.js", 
                "~/Scripts/modernizr-2.7.1.js",
                "~/Scripts/main.js", 
                "~/Scripts/wow.min.js", 
                "~/Scripts/spectrum.js", 
                "~/Scripts/codespec.js",));
            
            indexjs.Orderer = new AsIsBundleOrderer();
            bundles.Add(indexjs);
            
            //Bundle JS Geral (Cru, para páginas que só usam Bootstrap e JQuery
            var geraljs = new ScriptBundle("~/bundles/geraljs").Include(
                "~/Scripts/jquery-1.11.0.min.js",
                "~/Scripts/bootstrap.min.js", 
                "~/Scripts/modernizr-2.7.1.js",));
            
            geraljs.Orderer = new AsIsBundleOrderer();
            bundles.Add(geraljs);
            
            //Bundle JS GruposInterna 
            var gruposinternajs = new ScriptBundle("~/bundles/gruposinterna").Include(
                "~/Scripts/jquery-1.11.0.min.js",
                "~/Scripts/bootstrap.min.js", 
                "~/Scripts/jquery-ui.js",
                "~/Scripts/modernizr-2.7.1.js",
                "~/Scripts/validate.js"
                "http://canvasjs.com/assets/script/canvasjs.min.js",
                "~/Scripts/spectrum.js",
                "~/Scripts/graphics.js",
                "~/Scripts/codespec.js",));
            
            gruposinternajs.Orderer = new AsIsBundleOrderer();
            bundles.Add(gruposinternajs);
            
            //Bundle JS CriarGrupo 
            var criargrupojs = new ScriptBundle("~/bundles/criargrupo").Include(
                "~/Scripts/jquery-1.11.0.min.js",
                "~/Scripts/bootstrap.min.js", 
                "~/Scripts/modernizr-2.7.1.js",
                "~/Scripts/spectrum.js",
                "~/Scripts/codespec.js",));
            
            criargrupojs.Orderer = new AsIsBundleOrderer();
            bundles.Add(criargrupojs);
            
            //Bundle JS Configurações 
            var configuracoesjs = new ScriptBundle("~/bundles/configuracoes").Include(
                "~/Scripts/jquery-1.11.0.min.js",
                "~/Scripts/bootstrap.min.js", 
                "~/Scripts/modernizr-2.7.1.js",
                "~/Scripts/validate.js"
                "http://canvasjs.com/assets/script/canvasjs.min.js",
                "~/Scripts/spectrum.js",
                "~/Scripts/graphics.js",
                "~/Scripts/codespec.js",));
            
            configuracoesjs.Orderer = new AsIsBundleOrderer();
            bundles.Add(configuracoesjs);
            
            
            
            
            
            
            //Bundle CSS Index 
            var indexcss = new StyleBundle("~/Content/index").Include(
                "~/Content/bootstrap.min.css"
                "~/Content/main.css"
                "http://fonts.googleapis.com/css?family=Raleway:400,100,200,300,500,600,700,800,900|Montserrat:400,700"
                "~/Content/font-awesome.min.css"
                "~/Content/animate.css"
                "~/Content/spectrum.css"));
            
            indexcss.Orderer = new AsIsBundleOrderer();
            bundles.Add(indexcss);
            
            //Bundle CSS Geral(Inicio, Grupos)
            var geralcss = new StyleBundle("~/Content/geral").Include(
                "~/Content/bootstrap.min.css"
                "~/Content/style.css"
                "http://fonts.googleapis.com/css?family=Raleway:400,100,200,300,500,600,700,800,900|Montserrat:400,700"
                "~/Content/font-awesome.min.css"));
            
            geralcss.Orderer = new AsIsBundleOrderer();
            bundles.Add(geralcss);
                
            //Bundle CSS GruposInterna
            var gruposinternacss = new StyleBundle("~/Content/gruposinterna").Include(
                "~/Content/bootstrap.min.css"
                "~/Content/style.css"
                "http://fonts.googleapis.com/css?family=Raleway:400,100,200,300,500,600,700,800,900|Montserrat:400,700"
                "~/Content/font-awesome.min.css"
                "~/Content/spectrum.css"
                "~/Content/jquery-ui.css"));
            
            gruposinternacss.Orderer = new AsIsBundleOrderer();
            bundles.Add(gruposinternacss);
            
            //Bundle CSS CriarGrupo
            var criargrupocss = new StyleBundle("~/Content/criargrupo").Include(
                "~/Content/bootstrap.min.css"
                "~/Content/style.css"
                "http://fonts.googleapis.com/css?family=Raleway:400,100,200,300,500,600,700,800,900|Montserrat:400,700"
                "~/Content/font-awesome.min.css"
                "~/Content/spectrum.css"));
            
            criargrupocss.Orderer = new AsIsBundleOrderer();
            bundles.Add(criargrupocss);
            
             //Bundle CSS Configurações
            var configuracoescss = new StyleBundle("~/Content/configuracoes").Include(
                "~/Content/bootstrap.min.css"
                "~/Content/style.css"
                "http://fonts.googleapis.com/css?family=Raleway:400,100,200,300,500,600,700,800,900|Montserrat:400,700"
                "~/Content/font-awesome.min.css"
                "~/Content/spectrum.css"
                "~/Content/jquery-ui.css"));
            
            configuracoescss.Orderer = new AsIsBundleOrderer();
            bundles.Add(configuracoescss);
            
            //Bundle CSS Layout
            var layout = new StyleBundle("~/Content/layout").Include(
                "~/Content/bootstrap.min.css"
                "~/Content/layout.css"
                "http://fonts.googleapis.com/css?family=Raleway:400,100,200,300,500,600,700,800,900|Montserrat:400,700"
                "~/Content/font-awesome.min.css"));
            
            layout.Orderer = new AsIsBundleOrderer();
            bundles.Add(layout);

            
            BundleTable.EnableOptimizations = true;
           
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
