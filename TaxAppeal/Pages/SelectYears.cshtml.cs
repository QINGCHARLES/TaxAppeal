using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TaxAppeal.Pages
{
	public class SelectYearsModel : PageModel
	{
		public void OnGet()
		{
		}

		public IActionResult OnPostConfirm()
		{
			return Redirect("/appeal-reason");
		}
	}
}
