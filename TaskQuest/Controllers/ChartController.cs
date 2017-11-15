using Microsoft.AspNet.Identity;
using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TaskQuest.Models;
using TaskQuest.ViewModels;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using Newtonsoft.Json;
using TaskQuest.Data;

namespace TaskQuest.Controllers
{
    [Authorize]
    public class ChartController : Controller
    {

        private DbContext db = new DbContext();

        [HttpPost]
        public ActionResult Index(string Id)
        {
            if (!Util.IsPremium(Id))
                return Json(null);

            int id;
            if (int.TryParse(Util.Decrypt(Id), out id))
            {
                var grupo = db.Grupo.Find(id);

                if (!grupo.Users.Where(e => e.Id == User.Identity.GetUserId<int>()).Any())
                    return Json(new ChartViewModel() { Response = "Error" });

                ChartViewModel model = new ChartViewModel() { Response = "Ok" };
                model.Title = string.Format("Gráfico de barras dos pontos adquiridos pelos colaboradores da equipe {0}", grupo.Nome);

                foreach (var user in grupo.Users)
                {
                    int pontos = 0;
                    foreach (var exp in user.PontosUsuario)
                        if (exp.Task.Quest.GrupoCriador != null)
                            if (exp.Task.Quest.GrupoCriadorId == grupo.Id)
                                pontos += exp.Valor;

                    if (pontos != 0)
                    {
                        model.Labels.Add(user.Nome);
                        model.Data.Add(pontos);
                    }

                }
                return Json(new
                {
                    model = model,
                    Integrantes = grupo.Users.Select(e => new { Nome = e.Nome, Id = Util.Encrypt(e.Id.ToString()), IsCurrentuser = (e.Id == User.Identity.GetUserId<int>()) }),
                    Quests = grupo.Quests.Select(e => new { Nome = e.Nome, Id = Util.Encrypt(e.Id.ToString()) })
                });
            }

            return Json(new ChartViewModel() { Response = "Error" });
        }

        [HttpPost]
        public ActionResult BarChartIntegrante(string UserId, string GrupoId)
        {
            if (!Util.IsPremium(GrupoId))
                return Json(null);

            int Id;
            if (int.TryParse(Util.Decrypt(GrupoId), out Id))
            {
                var grupo = db.Grupo.Find(Id);

                if (!User.Identity.IsAdm(grupo.Id))
                    return Json(new ChartViewModel() { Response = "Error" });

                if (int.TryParse(Util.Decrypt(UserId), out Id))
                {
                    var user = db.Users.Find(Id);

                    if (!grupo.Users.Where(e => e.Id == User.Identity.GetUserId<int>()).Any())
                        return Json(new ChartViewModel() { Response = "Error" });

                    ChartViewModel model = new ChartViewModel() { Response = "Ok" };
                    model.Title = string.Format("Gráfico das de barras Quests realizadas por {0} na equipe {1}", user.Nome + " " + user.Sobrenome, db.Grupo.Find(grupo.Id).Nome);
                    int quantidade = 0;

                    foreach (var exp in user.PontosUsuario.OrderByDescending(e => e.Data))
                    {
                        if (quantidade >= 10)
                            break;

                        if (exp.Task.Quest.GrupoCriador != null)
                        {
                            if (exp.Task.Quest.GrupoCriador.Id == grupo.Id)
                            {
                                model.Labels.Add(exp.Task.Nome);
                                model.Data.Add(exp.Valor);
                                quantidade++;
                            }
                        }
                    }
                    return Json(model);
                }
            }
            return Json(new ChartViewModel() { Response = "Error" });
        }

        [HttpPost]
        public ActionResult BarChartGrupo(string Id)
        {
            if (!Util.IsPremium(Id))
                return Json(null);

            int grupoId;
            if (int.TryParse(Util.Decrypt(Id), out grupoId))
            {
                var grupo = db.Grupo.Find(grupoId);

                if (!User.Identity.IsAdm(grupo.Id))
                    return Json(new ChartViewModel() { Response = "Error" });

                if (!grupo.Users.Where(e => e.Id == User.Identity.GetUserId<int>()).Any())
                    return Json(new ChartViewModel() { Response = "Error" });

                ChartViewModel model = new ChartViewModel() { Response = "Ok" };
                model.Title = string.Format("Gráfico de barras dos pontos adquiridos pelos colaboradores da equipe {0}", grupo.Nome);

                foreach (var user in grupo.Users)
                {
                    int pontos = 0;
                    foreach (var exp in user.PontosUsuario)
                        if (exp.Task.Quest.GrupoCriador != null)
                            if (exp.Task.Quest.GrupoCriadorId == grupo.Id)
                                pontos += exp.Valor;

                    if (pontos != 0)
                    {
                        model.Labels.Add(user.Nome);
                        model.Data.Add(pontos);
                    }

                }
                return Json(model);
            }

            return Json(new ChartViewModel() { Response = "Error" });
        }

        [HttpPost]
        public ActionResult PieChart(string Id)
        {
            if (!Util.IsPremium(Id))
                return Json(null);

            if (!User.Identity.HasQuest(Id))
                return Json(new ChartViewModel() { Response = "Error" });

            int questId;
            if (int.TryParse(Util.Decrypt(Id), out questId))
            {
                var quest = db.Quest.Find(questId);

                if (!User.Identity.IsAdm(quest.GrupoCriadorId.Value))
                    return Json(new ChartViewModel() { Response = "Error" });

                ChartViewModel model = new ChartViewModel() { Response = "Ok" };
                model.Title = string.Format("Gráfico de pizza da duração das Task da Quest {0}", quest.Nome);

                foreach (var task in quest.Tasks)
                {
                    model.Labels.Add(task.Nome);
                    model.Data.Add(Convert.ToInt32((task.DataConclusao - task.DataInicio).TotalMinutes));
                }

                return Json(model);
            }

            return Json(new ChartViewModel() { Response = "Error" });
        }

        [HttpPost]
        public ActionResult GanttChart(string Id)
        {
            if (!Util.IsPremium(Id))
                return Json(null);

            if (!User.Identity.HasQuest(Id))
                return Json(new ChartViewModel() { Response = "Error" });

            int questId;
            if (int.TryParse(Util.Decrypt(Id), out questId))
            {
                var quest = db.Quest.Find(questId);

                if (!User.Identity.IsAdm(quest.GrupoCriadorId.Value))
                    return Json(new ChartViewModel() { Response = "Error" });

                ChartViewModel model = new ChartViewModel() { Response = "Ok" };
                model.Month = DateTime.Now.ToString("MMMM");
                model.DaysInMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
                model.Title = string.Format("Gráfico de Gantt da duração das Tasks da Quest {0}", quest.Nome);

                foreach (var task in quest.Tasks)
                {
                    if (task.DataConclusao.Month == DateTime.Now.Month)
                    {
                        model.Labels.Add(task.Nome);
                        model.Data.Add(task.DataConclusao.Day - task.DataInicio.Day);
                        model.Offset.Add(task.DataInicio.Day);
                    }
                }

                return Json(model);
            }
            return Json(new ChartViewModel() { Response = "Error" });
        }

        public ActionResult ConfiguracaoChart()
        {
            var user = db.Users.Find(User.Identity.GetUserId<int>());

            ChartViewModel model = new ChartViewModel() { Response = "Ok" };
            model.Title = string.Format("Gráfico das de barras Quests realizadas por {0}", user.Nome + " " + user.Sobrenome);
            int quantidade = 0;

            foreach (var exp in user.PontosUsuario.OrderByDescending(e => e.Data))
            {
                if (quantidade >= 10)
                    break;

                if (exp.Task.Quest.GrupoCriador != null)
                {
                    model.Labels.Add(exp.Task.Nome);
                    model.Data.Add(exp.Valor);
                    quantidade++;
                }
            }
            return Json(model);
        }

    }
}