using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProdutoFidelidadeWeb.Models;
using ProdutoFidelidadeWeb.Model;
using ProdutoFidelidadeWeb.Repositories;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;

namespace ProdutoFidelidadeWeb.Controllers
{
    public class HomeController : Controller
    {
        ProdutoFidelidadeContext _context;
        HttpClient _client;
        ConfigRepository _configRepository;
        LogRepository _logRepository;

        public HomeController(ProdutoFidelidadeContext context)
        {
            _context = context;
            _configRepository = new ConfigRepository(_context);
            _logRepository = new LogRepository(_context);

            _client = new HttpClient();
            _client.BaseAddress = new Uri("https://hfllinxintegracaogiftwebapi-hom.azurewebsites.net/");
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json")
                );
        }

        public IActionResult Index()
        {
            ViewBag.Searched = false;
            return View();
        }

        [HttpPost]        
        public async Task<IActionResult> Index([FromForm]string Number)
        {
            ViewBag.Searched = false;
            try
            {
                var configs = _configRepository.List();
                string resquestId = Guid.NewGuid().ToString();
                object data = new
                {
                    chaveIntegracao = configs.FirstOrDefault(x => x.Name == "IntegrationKey")?.Value ?? "4B335B6F-9C4D-47F7-B798-C46FFBC4881A",
                    codigoLoja = configs.FirstOrDefault(x => x.Name == "MallCode")?.Value ?? "1",
                    numeroCartao = Number,
                    nsuCliente = resquestId,
                    codigoSeguranca = "123"
                };

                HttpResponseMessage response = await _client.PostAsync("/LinxServiceApi/FidelidadeService/ConsultaFidelizacao", data.AsJson());

                string result = await response.Content.ReadAsStringAsync();
                _logRepository.Save(new Model.Log
                {
                    Datetime = DateTime.Now,
                    RequestId = resquestId,
                    Message = result
                });

                JToken jsonresult = Newtonsoft.Json.JsonConvert.DeserializeObject<JToken>(result);

                ViewBag.Result = jsonresult;
                ViewBag.Searched = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return View();
        }

        [NonAction]
        private void LoadConfigsViewBag()
        {
            var configs = _configRepository.List();

            ViewBag.MallNumber = configs.FirstOrDefault(x => x.Name == "IntegrationKey")?.Value ?? "4B335B6F-9C4D-47F7-B798-C46FFBC4881A";
            ViewBag.IntegrationKey = configs.FirstOrDefault(x => x.Name == "MallCode")?.Value ?? "1";
        }
        public IActionResult About()
        {
            LoadConfigsViewBag();
            ViewBag.IsUpdated = false;
            return View();
        }

        [HttpPost]
        public IActionResult About([FromForm] string key,[FromForm] string number)
        {
            ViewBag.IsUpdated = false;

            var configs = _configRepository.List();
            var keyConfig = configs.FirstOrDefault(x => x.Name == "IntegrationKey") ?? new Config { Name = "IntegrationKey", Value = "4B335B6F-9C4D-47F7-B798-C46FFBC4881A" };
            keyConfig.Value = number;

            _configRepository.Save(keyConfig);
            var numberConfig = configs.FirstOrDefault(x => x.Name == "MallCode") ?? new Config { Name = "MallCode", Value = "1" };
            numberConfig.Value = key;

            _configRepository.Save(numberConfig);

            LoadConfigsViewBag();
            ViewBag.IsUpdated = true;

            return View();
        }


        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
