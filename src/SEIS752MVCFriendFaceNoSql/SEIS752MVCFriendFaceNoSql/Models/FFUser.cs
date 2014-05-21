using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SEIS752MVCFriendFaceNoSql.Models
{
	public class FFUser
	{
		public int UserId { get; set; }
		public string Name { get; set; }
		public string Username { get; set; }
		public double Lat { get; set; }
		public double Lon { get; set; }

		public double AntipodalLat
		{
			get { return (-1 * Lat); }
		}

		public double AntipodalLon
		{
			get { return ((Lon + 180.0 < 180.0) ? (Lon + 180.0) : (Lon - 180)); }
		}

		[Url]
		public string ImageUrl { get; set; }
	}
}