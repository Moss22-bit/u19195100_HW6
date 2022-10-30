using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using u19195100_HW6.Models;
using Newtonsoft.Json;
using u19195100_HW6.Models.ViewModels;

namespace u19195100_HW6.Controllers
{
    public class ChartController : Controller
    {

        private BikeStoresEntities db = new BikeStoresEntities();

        // GET: Chart
        public ActionResult GetChartData()
        {
			
			
				List<Charts> dataPoints = new List<Charts>();

				dataPoints.Add(new Charts("USA", 121));
				dataPoints.Add(new Charts("Great Britain", 67));
				dataPoints.Add(new Charts("China", 70));
				dataPoints.Add(new Charts("Russia", 56));
				dataPoints.Add(new Charts("Germany", 42));
				dataPoints.Add(new Charts("Japan", 41));
				dataPoints.Add(new Charts("France", 42));
				dataPoints.Add(new Charts("South Korea", 21));

				ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);

				return View();
			
		}
	}
}
