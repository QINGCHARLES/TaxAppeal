using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TaxAppeal.Pages;

public class PropertyRelationshipModel : PageModel
{
    public void OnGet()
    {
    }

    public IActionResult OnPost()
    {
        return Redirect("/select-years?");
    }
}
