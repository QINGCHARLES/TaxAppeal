namespace TaxAppeal.Models
{
	public class Candidate
	{
		public string? address { get; set; }
		public Location? location { get; set; }
		public int score { get; set; }
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

	// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
	public class Attributes
	{
		public string? PIN14 { get; set; }
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

	public class FieldAliases
	{
		public string? PIN14 { get; set; }
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
}
