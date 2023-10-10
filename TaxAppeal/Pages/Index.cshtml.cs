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
using System.Text;

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

	public IActionResult OnPostConfirm()
	{
		//string Data = TempData["Data"].ToString();
		//string[] DataParts = Data.Split("||", StringSplitOptions.TrimEntries);

		//string[] parts = DataParts[0].Split(new string[] { "," }, StringSplitOptions.TrimEntries);

		//string Line1 = parts[0];
		//string City = parts[^3];
		//string State = parts[^2];
		//string Zip = parts[^1];
		//string Town = DataParts[1];
		//string Pin14 = DataParts[2];

		//Data = Line1 + "||" + City + "||" + State + "||" + Zip + "||" + (string.IsNullOrEmpty(Email) ? "john@example.com" : Email) + "||" + (string.IsNullOrEmpty(Name) ? "John Example Smith" : Name) + "||" + (string.IsNullOrEmpty(Phone) ? "(773) 555-5555" : Phone) + "||" + Town + "||" + Pin14;
		//string urlSafeEncoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(Data)).Replace('+', '-').Replace('/', '_').TrimEnd('=');

		//return Redirect($"/step-three?data={urlSafeEncoded}");
		return Redirect($"/find-property");
	}

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

				// https://gis.cookcountyil.gov/traditional/rest/services/AddressLocator/addressPtMuniZip/GeocodeServer/findAddressCandidates?Street=4127%2ctripp&f=json

				// this call converts an x/y coord to a property PIN
				GisPin? JsonGisPin = await client.GetFromJsonAsync<GisPin>($"traditional/rest/services/cookVwrDynmc/MapServer/44/query?where=&text=&objectIds=&time=&timeRelation=esriTimeRelationOverlaps&geometry={JsonAddress.candidates[0].location!.x.ToString()}%2C{JsonAddress.candidates[0].location!.y.ToString()}&geometryType=esriGeometryPoint&inSR=&spatialRel=esriSpatialRelIntersects&distance=&units=esriSRUnit_Foot&relationParam=&outFields=PIN14%2CTown&returnGeometry=true&returnTrueCurves=false&maxAllowableOffset=&geometryPrecision=&outSR=&havingClause=&returnIdsOnly=false&returnCountOnly=false&orderByFields=&groupByFieldsForStatistics=&outStatistics=&returnZ=false&returnM=false&gdbVersion=&historicMoment=&returnDistinctValues=false&resultOffset=&resultRecordCount=&returnExtentOnly=false&sqlFormat=none&datumTransformation=&parameterValues=&rangeValues=&quantizationParameters=&featureEncoding=esriDefault&f=pjson");

				string? Town = JsonGisPin!.features![0]!.attributes!.Town;

				GisComparables? JsonComparables = await client.GetFromJsonAsync<GisComparables>($"/traditional/rest/services/cookVwrDynmc/MapServer/44/query?where=Town+%3D+%27{HttpUtility.UrlEncode(Town)}%27+AND+NBHD+%3D+%27074%27+AND+BLDGClass+%3D+%27203%27+AND+%28BldgSqft+%3E%3D+1190.7+AND+BldgSqft+%3C%3D+1455.3%29+AND+%28LandSqft+%3E%3D+7021.8+AND+LandSqft+%3C%3D+8582.2%29+AND+%28BldgAge+%3E%3D+87+AND+BldgAge+%3C%3D+117%29+AND+BldgConst+%3D+%27Masonry%27+AND+PIN14+%3C%3E+%2713154150070000%27&text=&objectIds=&time=&timeRelation=esriTimeRelationOverlaps&geometry=&geometryType=esriGeometryEnvelope&inSR=&spatialRel=esriSpatialRelIntersects&distance=&units=esriSRUnit_Foot&relationParam=&outFields=ParcelType%2CAddress%2CCity%2CTown%2CNBHD%2CTotalValue%2CBldgValue%2CBLDGClass%2CBldgSqft%2CLandvalue%2CLandSqft%2CBldgConst%2CBldgAge%2CMlt_IND%2CPer_Ass%2CPIN10%2CPIN14%2CSV_URL%2CTAXYR%2Cvalue_description&returnGeometry=true&returnTrueCurves=false&maxAllowableOffset=&geometryPrecision=&outSR=&havingClause=&returnIdsOnly=false&returnCountOnly=false&orderByFields=&groupByFieldsForStatistics=&outStatistics=&returnZ=false&returnM=false&gdbVersion=&historicMoment=&returnDistinctValues=false&resultOffset=&resultRecordCount=&returnExtentOnly=false&sqlFormat=none&datumTransformation=&parameterValues=&rangeValues=&quantizationParameters=&featureEncoding=esriDefault&f=pjson");

				if (JsonComparables == null)
				{
					return Content("not found");
				}
				else
				{
					string ff = "";
					foreach (Feature feature in JsonComparables.features!)
					{

						ff += feature.attributes!.Address + "<br />";
						ff += feature.attributes!.PIN14 + "<br />";

					}

					//return Content(ff);
				}

				return Redirect($"/step-two?pin={JsonGisPin!.features![0].attributes!.PIN14!}");

				//fff = "Your property PIN is: " + JsonGisPin!.features![0].attributes!.PIN14!;
			}




		}





		return Content(fff);
	}
}