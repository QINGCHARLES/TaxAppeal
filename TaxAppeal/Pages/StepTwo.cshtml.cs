using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.NetworkInformation;
using System.Net;
using System.Web;
using TaxAppeal.Models;

namespace TaxAppeal.Pages;

public class StepTwoModel : PageModel
{
	[BindProperty(Name = "pin", SupportsGet = true)]
	public string? PropertyPin { get; set; }


	public async Task<IActionResult> OnGetAsync()
	{
		using HttpClient client = new()
		{
			BaseAddress = new Uri("https://gis.cookcountyil.gov")
		};
		//

		GisComparables? JsonComparables = await client.GetFromJsonAsync<GisComparables>($"/traditional/rest/services/cookVwrDynmc/MapServer/44/query?where=Town+%3D+%27Jefferson%27+AND+NBHD+%3D+%27074%27+AND+BLDGClass+%3D+%27203%27+AND+%28BldgSqft+%3E%3D+1190.7+AND+BldgSqft+%3C%3D+1455.3%29+AND+%28LandSqft+%3E%3D+7021.8+AND+LandSqft+%3C%3D+8582.2%29+AND+%28BldgAge+%3E%3D+87+AND+BldgAge+%3C%3D+117%29+AND+BldgConst+%3D+%27Masonry%27+AND+PIN14+%3C%3E+%2713154150070000%27&text=&objectIds=&time=&timeRelation=esriTimeRelationOverlaps&geometry=&geometryType=esriGeometryEnvelope&inSR=&spatialRel=esriSpatialRelIntersects&distance=&units=esriSRUnit_Foot&relationParam=&outFields=ParcelType%2CAddress%2CCity%2CTown%2CNBHD%2CTotalValue%2CBldgValue%2CBLDGClass%2CBldgSqft%2CLandvalue%2CLandSqft%2CBldgConst%2CBldgAge%2CMlt_IND%2CPer_Ass%2CPIN10%2CPIN14%2CSV_URL%2CTAXYR%2Cvalue_description&returnGeometry=true&returnTrueCurves=false&maxAllowableOffset=&geometryPrecision=&outSR=&havingClause=&returnIdsOnly=false&returnCountOnly=false&orderByFields=&groupByFieldsForStatistics=&outStatistics=&returnZ=false&returnM=false&gdbVersion=&historicMoment=&returnDistinctValues=false&resultOffset=&resultRecordCount=&returnExtentOnly=false&sqlFormat=none&datumTransformation=&parameterValues=&rangeValues=&quantizationParameters=&featureEncoding=esriDefault&f=pjson");

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

			return Content(ff);
		}

		return Page();
	}
}
