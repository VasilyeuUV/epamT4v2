using AutoMapper;
using BLL.DTO;
using BLL.Infrastructure;
using BLL.Interfaces;
using plAspNetWebApp.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace plAspNetWebApp.Controllers
{
    public class HomeController : Controller
    {
        ISaleService<ManagerDTO> saleService;

        public HomeController(ISaleService<ManagerDTO> serv)
        {
            saleService = serv;
        }

        public ActionResult Index()
        {
            IEnumerable<ManagerDTO> phoneDtos = saleService.GetEntities();
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<ManagerDTO, ManagerViewModel>()).CreateMapper();
            var managers = mapper.Map<IEnumerable<ManagerDTO>, List<ManagerViewModel>>(phoneDtos);
            return View(managers);
        }

        public ActionResult MakeManager(int? id)
        {
            try
            {
                ManagerDTO manager = saleService.GetEntity(id);
                var order = new ManagerViewModel { Id = manager.Id };

                return View(order);
            }
            catch (ValidationException ex)
            {
                return Content(ex.Message);
            }
        }

        protected override void Dispose(bool disposing)
        {
            saleService.Dispose();
            base.Dispose(disposing);
        }








        //public ActionResult Index()
        //{
        //    return View();
        //}

        //public ActionResult About()
        //{
        //    ViewBag.Message = "Your application description page.";

        //    return View();
        //}

        //public ActionResult Contact()
        //{
        //    ViewBag.Message = "Your contact page.";

        //    return View();
        //}
    }
}