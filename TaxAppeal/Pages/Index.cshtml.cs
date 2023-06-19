using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using HtmlAgilityPack;
using Dapper;
using System.Web;
using System.Text.Json;
using TaxAppeal.Models;
using System.Globalization;
using System.Text.Encodings.Web;

namespace TaxAppeal.Pages;

public class IndexModel : PageModel
{

	[BindProperty]
	public string? StreetAddress { get; set; }

	private readonly ILogger<IndexModel> _logger;

	public IndexModel(ILogger<IndexModel> logger)
	{
		_logger = logger;
	}

	//		https://gis.cookcountyil.gov/traditional/rest/services/cookVwrDynmc/MapServer/44/query?f=json&where=&returnGeometry=true&spatialRel=esriSpatialRelIntersects&geometry=%7B%22points%22%3A%5B%5B1147343.1412663457%2C1927226.7877129088%5D%5D%2C%22spatialReference%22%3A%7B%22wkid%22%3A3435%7D%7D&geometryType=esriGeometryMultipoint&inSR=3435&outFields=PIN14&outSR=3435

	//https://gis.cookcountyil.gov/traditional/rest/services/cookVwrDynmc/MapServer/44/query?f=json&where=&returnGeometry=true&spatialRel=esriSpatialRelIntersects&geometry=%7B%22points%22%3A%5B%5B1147343.1412663457%2C1927226.7877129088%5D%5D%2C%22spatialReference%22%3A%7B%22wkid%22%3A3435%7D%7D&geometryType=esriGeometryMultipoint&inSR=3435&outFields=PIN14&outSR=3435


	//https://gis.cookcountyil.gov/traditional/rest/services/cookVwrDynmc/MapServer/44/query?f=json&where=&returnGeometry=true&spatialRel=esriSpatialRelIntersects&geometry={"points":[[1147343.1412663457,1927226.7877129088]],"spatialReference":{"wkid":3435}}&geometryType=esriGeometryMultipoint&inSR=3435&outFields=ParcelType,Address,City,Town,NBHD,TotalValue,BldgValue,BLDGClass,BldgSqft,Landvalue,LandSqft,BldgConst,BldgAge,Mlt_IND,Per_Ass,PIN10,PIN14,SV_URL,TAXYR,value_description&outSR=3435


	public async Task<IActionResult> OnPost()
	{
		if (!ModelState.IsValid)
		{
			return Page();
		}


		string fff = "address not found";

		// this url is for a lookahead address validation if needed
		// https://gis.cookcountyil.gov/traditional/rest/services/AddressLocator/addressPtMuniZip/GeocodeServer/findAddressCandidates?Street=4127%20n%20tripp%20ave&f=json

		// Make sure there is some text entered in the textbox
		if (!string.IsNullOrWhiteSpace(StreetAddress))
		{

			using HttpClient client = new()
			{
				BaseAddress = new Uri("https://gis.cookcountyil.gov")
			};

			//fff = await client.GetStringAsync()

			// this call converts a street address text to x/y coords for the other functions
			GisAddressPin? JsonAddress = await client.GetFromJsonAsync<GisAddressPin>($"/traditional/rest/services/AddressLocator/addressPtMuniZip/GeocodeServer/findAddressCandidates?Street={HttpUtility.UrlEncode(StreetAddress)}&f=json");

			// check to see if the street address entered on the form validated to a street address in the county database
			if (JsonAddress != null && JsonAddress.candidates != null && JsonAddress.candidates.Count > 0)
			{
				//fff = JsonAddress.candidates[0].location!.x.ToString();
				//using (var clientb = new WebClient())
				//{
				//	// 
				//	string Url = $"https://gis.cookcountyil.gov/traditional/rest/services/cookVwrDynmc/MapServer/44/query?where=&text=&objectIds=&time=&timeRelation=esriTimeRelationOverlaps&geometry={JsonAddress.candidates[0].location!.x.ToString()}%2C{JsonAddress.candidates[0].location!.y.ToString()}&geometryType=esriGeometryPoint&inSR=&spatialRel=esriSpatialRelIntersects&distance=&units=esriSRUnit_Foot&relationParam=&outFields=PIN14&returnGeometry=true&returnTrueCurves=false&maxAllowableOffset=&geometryPrecision=&outSR=&havingClause=&returnIdsOnly=false&returnCountOnly=false&orderByFields=&groupByFieldsForStatistics=&outStatistics=&returnZ=false&returnM=false&gdbVersion=&historicMoment=&returnDistinctValues=false&resultOffset=&resultRecordCount=&returnExtentOnly=false&sqlFormat=none&datumTransformation=&parameterValues=&rangeValues=&quantizationParameters=&featureEncoding=esriDefault&f=pjson";

				//	fff = clientb.DownloadString(Url);
				//}

				// this call converts an x/y coord to a property PIN
				GisPin? JsonGisPin = await client.GetFromJsonAsync<GisPin>($"traditional/rest/services/cookVwrDynmc/MapServer/44/query?where=&text=&objectIds=&time=&timeRelation=esriTimeRelationOverlaps&geometry={JsonAddress.candidates[0].location!.x.ToString()}%2C{JsonAddress.candidates[0].location!.y.ToString()}&geometryType=esriGeometryPoint&inSR=&spatialRel=esriSpatialRelIntersects&distance=&units=esriSRUnit_Foot&relationParam=&outFields=PIN14&returnGeometry=true&returnTrueCurves=false&maxAllowableOffset=&geometryPrecision=&outSR=&havingClause=&returnIdsOnly=false&returnCountOnly=false&orderByFields=&groupByFieldsForStatistics=&outStatistics=&returnZ=false&returnM=false&gdbVersion=&historicMoment=&returnDistinctValues=false&resultOffset=&resultRecordCount=&returnExtentOnly=false&sqlFormat=none&datumTransformation=&parameterValues=&rangeValues=&quantizationParameters=&featureEncoding=esriDefault&f=pjson");

				return Redirect($"/step-two?pin={JsonGisPin!.features![0].attributes!.PIN14!}");

				//fff = "Your property PIN is: " + JsonGisPin!.features![0].attributes!.PIN14!;
			}




		}





		return Content(fff);
	}
}