using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Models;
using ServiceContracts;

namespace StocksApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IFinnHubService _finnHubService;
        private readonly IConfiguration _configuration;

        public HomeController(IFinnHubService finnHubService, IConfiguration configuration)
        {
            _finnHubService = finnHubService;
            _configuration = configuration;
        }

        [Route("/")]
        public async Task<IActionResult> Index()
        {
            TradingOptionPattern? tradingOption =  _configuration.GetSection("TradingOption").Get<TradingOptionPattern>();
           
            Dictionary<string,object>? read = await _finnHubService.GetStockPriceQuote(tradingOption.DefualtStockSymbole);

            StockProperties stockProperties = new StockProperties()
            {
                StockSymbole = tradingOption.DefualtStockSymbole,
                CurrentPrice =Convert.ToDouble(read["c"].ToString()),
                HighPrice =Convert.ToDouble(read["h"].ToString()),
                LowtPrice =Convert.ToDouble(read["l"].ToString()),
                OpenPrice =Convert.ToDouble(read["o"].ToString())
            };
            return View(stockProperties);
        }
    }
}
