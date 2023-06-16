using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using HtmlAgilityPack;
using Dapper;
using System.Web;
using Microsoft.Data.SqlClient;
using System.Security.Policy;
using System.Net;

namespace TaxAppeal.Pages
{
	public class IndexModel : PageModel
	{
		private readonly ILogger<IndexModel> _logger;

		public IndexModel(ILogger<IndexModel> logger)
		{
			_logger = logger;
		}

//		https://gis.cookcountyil.gov/traditional/rest/services/cookVwrDynmc/MapServer/44/query?f=json&where=&returnGeometry=true&spatialRel=esriSpatialRelIntersects&geometry=%7B%22points%22%3A%5B%5B1147343.1412663457%2C1927226.7877129088%5D%5D%2C%22spatialReference%22%3A%7B%22wkid%22%3A3435%7D%7D&geometryType=esriGeometryMultipoint&inSR=3435&outFields=PIN14&outSR=3435

//https://gis.cookcountyil.gov/traditional/rest/services/cookVwrDynmc/MapServer/44/query?f=json&where=&returnGeometry=true&spatialRel=esriSpatialRelIntersects&geometry=%7B%22points%22%3A%5B%5B1147343.1412663457%2C1927226.7877129088%5D%5D%2C%22spatialReference%22%3A%7B%22wkid%22%3A3435%7D%7D&geometryType=esriGeometryMultipoint&inSR=3435&outFields=PIN14&outSR=3435


//https://gis.cookcountyil.gov/traditional/rest/services/cookVwrDynmc/MapServer/44/query?f=json&where=&returnGeometry=true&spatialRel=esriSpatialRelIntersects&geometry={"points":[[1147343.1412663457,1927226.7877129088]],"spatialReference":{"wkid":3435}}&geometryType=esriGeometryMultipoint&inSR=3435&outFields=ParcelType,Address,City,Town,NBHD,TotalValue,BldgValue,BLDGClass,BldgSqft,Landvalue,LandSqft,BldgConst,BldgAge,Mlt_IND,Per_Ass,PIN10,PIN14,SV_URL,TAXYR,value_description&outSR=3435


		public IActionResult OnGet()
		{
			string fff = "";
			using (var client = new WebClient())
			{
				string Url = "https://gis.cookcountyil.gov/traditional/rest/services/cookVwrDynmc/MapServer/44/query?where=&text=&objectIds=&time=&timeRelation=esriTimeRelationOverlaps&geometry=1147343.1412663457%2C1927226.7877129088&geometryType=esriGeometryPoint&inSR=&spatialRel=esriSpatialRelIntersects&distance=&units=esriSRUnit_Foot&relationParam=&outFields=PIN14&returnGeometry=true&returnTrueCurves=false&maxAllowableOffset=&geometryPrecision=&outSR=&havingClause=&returnIdsOnly=false&returnCountOnly=false&orderByFields=&groupByFieldsForStatistics=&outStatistics=&returnZ=false&returnM=false&gdbVersion=&historicMoment=&returnDistinctValues=false&resultOffset=&resultRecordCount=&returnExtentOnly=false&sqlFormat=none&datumTransformation=&parameterValues=&rangeValues=&quantizationParameters=&featureEncoding=esriDefault&f=pjson";

				fff = client.DownloadString(Url);
			}

			return Content(fff);
		}
	}
}