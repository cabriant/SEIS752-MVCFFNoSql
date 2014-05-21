using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SEIS752MVCFriendFaceNoSql.Models
{
	public class ProfileModel
	{
		public FFUser ProfileUser { get; set; }
		public List<FFUserDistance> NearbyUsers { get; set; }
		public List<FFUserDistance> AntipodalUsers { get; set; }
	}
}