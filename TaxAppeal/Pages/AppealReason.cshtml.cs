using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TaxAppeal.Pages
{
    public class AppealReasonModel : PageModel
    {
        public void OnGet()
        {
        }

        
		public IActionResult OnPostConfirm()
		{
            return Page();
			return Redirect("/appeal-reason");

		}
    }
}
