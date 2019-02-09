using System;
using System.Collections.Generic;
using System.Text;

namespace mAgicTvPlugIn
{
	public class ReservationModel
	{
		public string   searchText;
		public int      areaID;
		public int      platformID;
		public int      categoryID;
		public DateTime startDateTime;
		public string   tvStation;

		public bool     outputReceiveHTML;

		public ReservationModel(string search_text, int areaID, int platformID, int categoryID, DateTime startTime, string tvStation, bool outputReceiveHTML)
		{
			this.searchText        = search_text;
			this.areaID            = areaID;
			this.platformID        = platformID;
			this.categoryID        = categoryID;
			this.startDateTime     = startTime;
			this.tvStation         = tvStation;
			this.outputReceiveHTML = outputReceiveHTML;
		}

		public override string ToString()
		{
			return string.Format("searchText='{0}', areaID={1}, platformID={2}, categoryID={3}, startDateTime='{4}', tvStation='{5}', outputRecvHtml={6}",
				searchText,
				areaID,
				platformID,
				categoryID,
				startDateTime.ToString("s"),
				tvStation,
				outputReceiveHTML);
		}
	}
}
