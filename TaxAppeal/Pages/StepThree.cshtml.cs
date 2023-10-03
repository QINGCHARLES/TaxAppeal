using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;
using System.Reflection.PortableExecutable;

using Microsoft.AspNetCore.Mvc;
using System.Text;

using System.Linq;
using PdfSharpCore;
using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.IO;
using PdfSharpCore.Pdf.AcroForms;
using PdfSharpCore.Pdf.Advanced;


namespace TaxAppeal.Pages
{
    public class StepThreeModel : PageModel
	{
		[BindProperty(Name = "data", SupportsGet = true)]
		public string? Address64 { get; set; }
		private string Address = "";

		public string stuff = "";

		private readonly IWebHostEnvironment _webHostEnvironment;

		public StepThreeModel(IWebHostEnvironment webHostEnvironment)
		{
			_webHostEnvironment = webHostEnvironment;
		}

		public IActionResult OnGet()
        {
			Address = Encoding.UTF8.GetString(Convert.FromBase64String(Address64.Replace('-', '+').Replace('_', '/') + new string('=', (4 - Address64.Length % 4) % 4)));

			stuff = Address;

			string[] stuffparts = Address.Split("||");

			//string[] AddressParts = stuffparts[0].Split(",");
			string Line1 = stuffparts[0];
			string City = stuffparts[1];
			string Zip = stuffparts[3];
			string Email = stuffparts[4];
			string TaxpayerName = stuffparts[5];
			string Phone = stuffparts[6];
			string Town = stuffparts[7];
			string Pin14 = stuffparts[8];


			string pdfPath = Path.Combine(_webHostEnvironment.WebRootPath, "resform.pdf");
			string pdfPathb = Path.Combine(_webHostEnvironment.WebRootPath, "resformb.pdf");

			// Open an existing document. Providing an unopened PdfDocument is important.
			PdfDocument pdf = PdfReader.Open(pdfPath, PdfDocumentOpenMode.Modify);

			//// Without this, your value won't render; won't work with PDF 2
			//// https://stackoverflow.com/questions/66227256/when-filling-a-pdf-document-with-pdfsharp-the-filled-form-doesnt-show-the-valu
			if (pdf.AcroForm.Elements.ContainsKey("/NeedAppearances"))
				pdf.AcroForm.Elements["/NeedAppearances"] = new PdfBoolean(true);
			else
				pdf.AcroForm.Elements.Add("/NeedAppearances", new PdfBoolean(true));

			PdfAcroForm form = pdf.AcroForm;
			PdfAcroField.PdfAcroFieldCollection fields = form.Fields;

			foreach (string fieldName in fields.DescendantNames)
			{
				PdfAcroField field = fields[fieldName];

				if (field.Elements.ContainsKey("/FT") && field.Elements["/FT"].ToString() == "/Btn") // Check if it's a button
				{
					int flags = field.Elements.GetInteger("/Ff"); // Get Flags
					if ((flags & 32768) != 0) // Check if it's a radio button (32768 is usually the flag for radio buttons)
					{
						// This is a radio button
						//stuff += $"Field {fieldName} is a radio button,";
					}
				}
			}

//			string stuff = "Debugging: ";
			PdfAcroField section1Field = fields["Section 1"];

			if (section1Field != null)
			{
				stuff += "Section 1 field is not null. ";

				// Try to find "/Opt" directly under "Section 1" field
				if (section1Field.Elements.ContainsKey("/Opt"))
				{
					PdfItem optItem = section1Field.Elements["/Opt"];
					stuff += $"Found /Opt, Option: {optItem}. ";
				}
				else
				{
					stuff += "Did not find /Opt under 'Section 1'. ";
				}

				// Continue with your existing logic for "/Kids"...
				// ...
			}

			//string stuff = "Debugging: ";

			//PdfAcroField section1Field = fields["Section 1"];

			//PdfAcroField section1Field = fields["Section 1"];

			if (section1Field != null)
			{
				// Set the value of the parent radio button group (Section 1)
				section1Field.Elements["/V"] = new PdfName("/1");

				// Traverse the Kids array for Section 1
				PdfArray kidsArray = section1Field.Elements["/Kids"] as PdfArray;
				if (kidsArray != null)
				{
					// Assuming you want to activate the second radio button (at index 1)
					PdfReference secondKidRef = kidsArray.Elements[2] as PdfReference;
					if (secondKidRef != null)
					{
						PdfDictionary secondKidDict = secondKidRef.Value as PdfDictionary;
						if (secondKidDict != null)
						{
							// Set the activation state of the second radio button
							secondKidDict.Elements["/AS"] = new PdfName("/1");
						}
					}
				}

			}


			// Output 'stuff' to see where the code might be failing
			//Console.WriteLine(stuff);







			string[] names = fields.DescendantNames;

			// For your reference; ask your PDF creation people to give these nice human friendly names
			foreach (string name in names)
			{
				var field = fields[name];

				//stuff += $"{field.Name},";

				//Console.WriteLine($"{field.Name}");
			}

			// Field: Name of Taxpayer
			fields["Name of Taxpayer"].Value = new PdfString(TaxpayerName);

			// Field: Taxpayer Address
			fields["Taxpayer Address"].Value = new PdfString(Line1);

			// Field: Email
			fields["Email"].Value = new PdfString(Email);

			// Field: City
			fields["City"].Value = new PdfString(City);

			// Field: State
			fields["State"].Value = new PdfString("IL");

			// Field: Taxpayer Zip Code
			fields["Taxpayer Zip Code"].Value = new PdfString(Zip);

			//fields["Current Year Appeal Only"].Value = new PdfName("Yes"); // Checked
			//fields["Current Year & C of E"].Value = new PdfName("Off");  // Unchecked

			// Field: Phone
			fields["Phone"].Value = new PdfString(Phone);

			// Field: Section 1 Other
			fields["Section 1 Other"].Value = new PdfString("N/A");

			// Field: Street Address
			fields["Street Address"].Value = new PdfString(Line1);

			// Field: City_2
			fields["City_2"].Value = new PdfString(City);

			// Field: Township
			fields["Township"].Value = new PdfString(Town);

			fields["1"].Value = new PdfString(Pin14);

			// Save the modified PDF to a new file
			pdf.Save(pdfPathb);


			// ... and so on ...

			pdf.Save(pdfPathb);
			//Process.Start("d:\\resout.pdf");

			return Redirect("/resformb.pdf");
		}
	}
}
