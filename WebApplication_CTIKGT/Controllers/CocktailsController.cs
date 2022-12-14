using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication_CTIKGT.Models;

namespace WebApplication_CTIKGT.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    public class CocktailsController : ControllerBase
    {
        [HttpGet]
        [Route("hozzávalók/all")]

        public IActionResult m1()
        {
            SeCocktailsContext context = new SeCocktailsContext();
            return Ok(context.Materials.ToList());
        }

        [HttpPost]
        [Route("hozzávalók/id")]

        public IActionResult m2(int id)
        {
            SeCocktailsContext context = new SeCocktailsContext();
            var keresetthozzávaló = (from x in context.Materials
                                 where x.MaterialId == id
                                 select x).FirstOrDefault();
            return Ok(keresetthozzávaló);
        }

        [HttpPost]
        [Route("koktélok/id")]

        public IActionResult m3(int id)
        {
            SeCocktailsContext context = new SeCocktailsContext();
            var keresettkoktél = (from x in context.Cocktails
                                  where x.CocktailSk== id
                                  select x).FirstOrDefault();
            return Ok(keresettkoktél);
        }

        [HttpPost]
        [Route("add")]

        public void Add(string név)
        {
            SeCocktailsContext context = new SeCocktailsContext();
            Material ÚjHozzávaló = new Material();
            ÚjHozzávaló.Name = név;
            context.Materials.Add(ÚjHozzávaló);
            context.SaveChanges();
        }

        [HttpDelete]
        [Route("del")]

        public void Delete(int id)
        {
            SeCocktailsContext context = new SeCocktailsContext();
            var törlendŐ = (from x in context.Materials
                            where x.MaterialId == id
                            select x).FirstOrDefault();
            context.Materials.Remove(törlendŐ);
            context.SaveChanges();
        }
    }
}
