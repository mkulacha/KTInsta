using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using KTInsta.Entities;
using KTInsta.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace KTInsta.Controllers
{
    [Route("api/KTInstaController")]
    public class KTInstaController : Controller
    {
        private static string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        [HttpGet("[action]")]
        public IEnumerable<WeatherForecast> WeatherForecasts()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                DateFormatted = DateTime.Now.AddDays(index).ToString("d"),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            });
        }

        public class WeatherForecast
        {
            public string DateFormatted { get; set; }
            public int TemperatureC { get; set; }
            public string Summary { get; set; }

            public int TemperatureF
            {
                get
                {
                    return 32 + (int)(TemperatureC / 0.5556);
                }
            }
        }
        public class KTInstaCardRS
        {
            public KTInstaCardRS()
            {
                KTInstaCards = new List<KTInstaCards>();
            }
            public List<KTInstaCards> KTInstaCards { get; set; }
        }
        [HttpGet]
        public List<KTInstaCards> GetKTInstaCards()
        {
            //Temporary disabled to keep api running
            //IKTInstaCradsRepositories<KTInstaCards> _KTInstaCardsRepo =
            //    KTInstaCardsRepositoriesFactory.Get<KTInstaCards>();
            //var cardsList =_KTInstaCardsRepo.GetAll().ToList();
            //KTInstaCardRS response = new KTInstaCardRS {KTInstaCards = cardsList};
            //return response;
            KTInstaCardRS response = new KTInstaCardRS();
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(
                "Data Source=sql-srv;Initial Catalog=Hack2020;user=gttmt;password=chu1C!imB;MultipleActiveResultSets=True"))
            {
                SqlCommand command = new SqlCommand("Select * from KTInstaCards", connection);
                connection.Open();
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(dataTable);
                
            }

            List<KTInstaCards> KTInstaCards = new List<KTInstaCards>();
            KTInstaCards = (from DataRow dr in dataTable.Rows
                select new KTInstaCards()
                {
                    CardId = Convert.ToInt32(dr["CardId"]),
                    CardType = Convert.ToInt32(dr["CardType"]),
                    CardHeadLine = dr["CardHeadLine"].ToString(),
                    CardText = dr["CardText"].ToString()
                }).ToList();
            response.KTInstaCards = KTInstaCards;
            return KTInstaCards;
        }
        public enum CardType
        {
            FAMCard = 1,
            ProductUpdateCard = 2,
            MassDistruption = 3,
            TopLeader = 4
        }
    }
}
