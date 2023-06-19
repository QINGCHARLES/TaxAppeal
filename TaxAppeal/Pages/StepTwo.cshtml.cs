using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TaxAppeal.Pages;

public class StepTwoModel : PageModel
{
	[BindProperty(Name = "pin", SupportsGet = true)]
	public string? PropertyPin { get; set; }


	public void OnGet()
    {
    }
}
