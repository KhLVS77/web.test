using Microsoft.AspNetCore.Mvc;
using System.Linq;
using WebSite.DB;
using WebSite.Models;

namespace WebSite.Controllers
{
    [Route("api/[controller]")]
    public class HistoryController : Controller
    {
        [HttpGet]
        public IActionResult Get(string login)
        {
            var headers = new[] { "Amount", "%", "Term", "Year", "From", "To", "Interest", "Income" };
            var history = History.Get(login);
            history.Reverse();

            var result = history
                .Take(9)
                .Select(x =>
                new[]
                {
                    x.Currency + x.Amount.ToDecimal().FormatNumber(x.Login),
                    x.Percent,
                    x.Days.ToString(),
                    x.Year,
                    x.StartDate.FormatDate(login),
                    x.EndDate.FormatDate(login),
                    x.Currency + x.Interest.ToDecimal().FormatNumber(x.Login),
                    x.Currency + x.Income.ToDecimal().FormatNumber(x.Login)
                }).ToList();

            result.Insert(0, headers);

            return Json(result);
        }

        [HttpPost("clear")]
        public IActionResult Clear(string login)
        {
            History.Clear(login);
            return Ok();
        }

        [HttpPost("save")]
        public ActionResult Save([FromBody] SaveHistoryDto dto)
        {
            dto.Currency = Constants.Get("currency").ElementAt(Settings.Get(dto.Login).Currency).Split(' ').Last();

            History.Add(dto);
            return Ok();
        }
    }
}