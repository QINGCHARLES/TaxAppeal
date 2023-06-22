namespace TaxAppeal.Models
{


	public class Candidate
	{
		public string? address { get; set; }
		public Location? location { get; set; }
		public float score { get; set; }
		public Attributes? attributes { get; set; }
	}

	public class Location
	{
		public double x { get; set; }
		public double y { get; set; }
	}

	public class GisAddressPin
	{
		public SpatialReference? spatialReference { get; set; }
		public List<Candidate>? candidates { get; set; }
	}

	public class SpatialReference
	{
		public int wkid { get; set; }
		public int latestWkid { get; set; }
	}


	public class Feature
	{
		public Attributes? attributes { get; set; }
		public Geometry? geometry { get; set; }
	}


	public class Field
	{
		public string? name { get; set; }
		public string? type { get; set; }
		public string? alias { get; set; }
		public int? length { get; set; }
	}

	public class Geometry
	{
		public List<List<List<double>>>? rings { get; set; }
	}

	public class GisPin
	{
		public string? displayFieldName { get; set; }
		public FieldAliases? fieldAliases { get; set; }
		public string? geometryType { get; set; }
		public SpatialReference? spatialReference { get; set; }
		public List<Field>? fields { get; set; }
		public List<Feature>? features { get; set; }
	}

	// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
	public class Attributes
	{
		public object? PARCELTYPE { get; set; }
		public string? Address { get; set; }
		public string? City { get; set; }
		public string? Zip_Code { get; set; }
		public string? Town { get; set; }
		public string? NBHD { get; set; }
		public string? TotalValue { get; set; }
		public string? BldgValue { get; set; }
		public string? BLDGClass { get; set; }
		public int? BldgSqft { get; set; }
		public string? LandValue { get; set; }
		public double? LandSqft { get; set; }
		public string? BldgConst { get; set; }
		public int? BldgAge { get; set; }
		public object? MLT_IND { get; set; }
		public object? Per_Ass { get; set; }
		public string? Pin10 { get; set; }
		public string? PIN14 { get; set; }
		public string? SV_URL { get; set; }
		public int? TAXYR { get; set; }
		public string? value_description { get; set; }
	}





	public class FieldAliases
	{
		public string? PARCELTYPE { get; set; }
		public string? Address { get; set; }
		public string? City { get; set; }
		public string? Town { get; set; }
		public string? NBHD { get; set; }
		public string? TotalValue { get; set; }
		public string? BldgValue { get; set; }
		public string? BLDGClass { get; set; }
		public string? BldgSqft { get; set; }
		public string? LandValue { get; set; }
		public string? LandSqft { get; set; }
		public string? BldgConst { get; set; }
		public string? BldgAge { get; set; }
		public string? MLT_IND { get; set; }
		public string? Per_Ass { get; set; }
		public string? Pin10 { get; set; }
		public string? PIN14 { get; set; }
		public string? SV_URL { get; set; }
		public string? TAXYR { get; set; }
		public string? value_description { get; set; }
	}



	public class GisComparables
	{
		public string? displayFieldName { get; set; }
		public FieldAliases? fieldAliases { get; set; }
		public string? geometryType { get; set; }
		public SpatialReference? spatialReference { get; set; }
		public List<Field>? fields { get; set; }
		public List<Feature>? features { get; set; }
	}


	
	public class AddressConfirmation
	{
		public string? displayFieldName { get; set; }
		public FieldAliases? fieldAliases { get; set; }
		public string? geometryType { get; set; }
		public SpatialReference? spatialReference { get; set; }
		public List<Field>? fields { get; set; }
		public List<Feature>? features { get; set; }
	}




}
