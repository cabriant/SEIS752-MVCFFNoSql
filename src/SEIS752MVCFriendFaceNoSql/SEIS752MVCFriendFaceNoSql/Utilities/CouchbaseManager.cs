using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Couchbase;
using Couchbase.Extensions;
using Enyim.Caching.Memcached;
using WebGrease.Css.Extensions;

namespace SEIS752MVCFriendFaceNoSql.Utilities
{
	public class CouchbaseManager
	{
		private readonly CouchbaseClient _client;
		private CouchbaseManager()
		{
			_client = new CouchbaseClient();
		}

		private static CouchbaseManager _instance;
		public static CouchbaseManager Instance
		{
			get { return _instance ?? (_instance = new CouchbaseManager()); }
		}

		public bool Store(string key, object model)
		{
			return _client.StoreJson(StoreMode.Set, key, model);
		}

		public T Get<T>(string key) where T : class
		{
			var result = _client.GetJson<T>(key);
			return result;
		}

		public void Flush()
		{
			_client.FlushAll();
		}
	}
}