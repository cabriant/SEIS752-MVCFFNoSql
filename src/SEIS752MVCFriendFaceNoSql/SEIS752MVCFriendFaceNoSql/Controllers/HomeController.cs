using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SEIS752MVCFriendFaceNoSql.Models;
using SEIS752MVCFriendFaceNoSql.Utilities;

namespace SEIS752MVCFriendFaceNoSql.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
	        return View();
        }

	    public ActionResult Profile(string username)
	    {
		    var user = GetAllUsers().SingleOrDefault(u => u.Username == username);
		    var allUsers = GetAllUsers();
		    var nearbyUsers = (from u in allUsers
			    let distance = GetDistance(user, u)
			    where u.Username != username && distance <= 1.0
			    select new FFUserDistance() {Distance = distance, User = u}).OrderBy(ffu => ffu.Distance).ToList();
		    var antipodalUsers = (from u in allUsers
			    let antipodalDistance = GetDistanceFromAntipodal(user, u)
			    where u.Username != username && antipodalDistance <= 1.0
			    select new FFUserDistance() {Distance = antipodalDistance, User = u}).OrderBy(ffu => ffu.Distance).ToList();

		    var model = new ProfileModel {ProfileUser = user, NearbyUsers = nearbyUsers, AntipodalUsers = antipodalUsers};

		    return View(model);
	    }

	    public JsonResult Search(string searchName)
	    {
		    var allUsers = GetAllUsers().Where(u => u.Username == searchName || u.Username.Contains(searchName));

		    return Json(allUsers, JsonRequestBehavior.AllowGet);
	    }
		
	    public ActionResult Seed()
	    {
			var reader = new StreamReader(System.IO.File.OpenRead(@"C:\users.csv"));
			
			while (!reader.EndOfStream)
			{
				var line = reader.ReadLine();
				if (line != null)
				{
					var values = line.Split(',');

					var idStr = values[0];
					var id = int.Parse(idStr);
					var name = values[1];
					var username = values[2];
					var lat = double.Parse(values[3]);
					var lon = double.Parse(values[4]);
					var imageUrl = values[5];

					var user = new FFUser()
					{
						UserId = id,
						Name = name,
						Username = username,
						Lat = lat,
						Lon = lon,
						ImageUrl = imageUrl
					};

					CouchbaseManager.Instance.Store(idStr, user);
				}
			}

		    return RedirectToAction("Index");
	    }

	    private List<FFUser> GetAllUsers()
	    {
		    var finalList = new List<FFUser>();

		    for (var i = 1; i <= 2000; i++)
		    {
			    finalList.Add(CouchbaseManager.Instance.Get<FFUser>(i.ToString()));
		    }

		    return finalList;
	    }

	    private double GetDistance(FFUser thisUser, FFUser otherUser)
	    {
		    var latDiff = (69.16*(otherUser.Lat - thisUser.Lat));
		    var lonDiff = (48.91*(otherUser.Lon - thisUser.Lon));

		    var distance = Math.Sqrt((latDiff*latDiff) + (lonDiff*lonDiff));

		    return distance;
	    }

	    private double GetDistanceFromAntipodal(FFUser thisUser, FFUser otherUser)
	    {
			var latDiff = (69.16 * (otherUser.Lat - thisUser.AntipodalLat));
			var lonDiff = (48.91 * (otherUser.Lon - thisUser.AntipodalLon));

			var distance = Math.Sqrt((latDiff * latDiff) + (lonDiff * lonDiff));

			return distance;
	    }
    }
}
