using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WEB_253502_Chertok.Helpers;
using WEB_253502_Chertok.Models;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewData["Message"] = "Лабораторная работа 2";

            var demoList = new List<ListDemo>
            {
                new ListDemo {Id = 1, Name = "Item 1"},
                new ListDemo {Id = 2, Name = "Item 2"},
                new ListDemo {Id = 3, Name = "Item 3"}
            };

            var viewModel = new IndexViewModel
            {
                ListItems = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(demoList, "Id", "Name")
            };

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
