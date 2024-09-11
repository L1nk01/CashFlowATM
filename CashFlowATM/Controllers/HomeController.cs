using CashFlowATM.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;

namespace CashFlowATM.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SelectDispensationMode(string dispensationMode)
        {
            ViewData["Mode"] = dispensationMode;
            ViewData["ModeText"] = GetModeText(dispensationMode);

            return View("CashWithdrawal");
        }

        [HttpPost]
        public IActionResult WithdrawCash(string dispensationMode, double cash)
        {
            string result = CalculateDispensation(dispensationMode, cash);

            ViewData["DispensationResult"] = result;
            ViewData["ModeText"] = GetModeText(dispensationMode);
            ViewData["Total"] = cash;

            return View("CashWithdrawalResult");
        }

        private string GetModeText(string dispensationMode)
        {
            switch (dispensationMode)
            {
                case "1":
                    return "Billetes de 200 y 1000";
                case "2":
                    return "Billetes de 100 y 500";
                case "3":
                    return "Modo eficiente (Todas las denominaciones)";
                default:
                    return "Modo desconocido";
            }
        }

        private string CalculateDispensation(string dispensationMode, double amount)
        {
            if (amount % 100 != 0) return "La cantidad ingresada no puede ser dispensada, asegúrate de ingresar un múltiplo de 100.";

            List<int> denominations = new List<int> { 100, 200, 500, 1000 };

            switch (dispensationMode)
            {
                case "1":
                    denominations.Remove(100);
                    denominations.Remove(500);
                    break;
                case "2":
                    denominations.Remove(200);
                    denominations.Remove(1000);
                    break;
                default:
                    break;
            }

            denominations.Sort((a, b) => b.CompareTo(a));

            Dictionary<int, int> result = new Dictionary<int, int>();
            foreach (int denomination in denominations)
            {
                if (amount == 0) break;

                int count = (int)(amount / denomination);

                if (count > 0)
                {
                    result[denomination] = count;
                    amount -= count * denomination;
                }
            }

            if (amount > 0) return "No es posible dispensar la cantidad exacta con las denominaciones disponibles";

            string output = "Dispensación: ";

            foreach (var entry in result)
            {
                output += $"{entry.Value} billete(s) de {entry.Key}, ";
            }

            output = output.TrimEnd(' ', ',');

            return output;
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
