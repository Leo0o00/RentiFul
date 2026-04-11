using System.Text.Json.Serialization;

namespace Properties.Contracts;

public class OpenStreetMapGetLocationResponse
{
    [JsonPropertyName("place_id")]
    public long PlaceId { get; set; }

    [JsonPropertyName("licence")]
    public string Licence { get; set; } = default!;

    [JsonPropertyName("osm_type")]
    public string OsmType { get; set; } = default!;

    [JsonPropertyName("osm_id")]
    public long OsmId { get; set; }

    // NOTE: lat/lon come as STRINGS, not numbers
    [JsonPropertyName("lat")]
    public string Lat { get; set; } = default!;

    [JsonPropertyName("lon")]
    public string Lon { get; set; } = default!;

    [JsonPropertyName("class")]
    public string Class { get; set; } = default!;

    [JsonPropertyName("type")]
    public string Type { get; set; } = default!;

    [JsonPropertyName("place_rank")]
    public int PlaceRank { get; set; }

    [JsonPropertyName("importance")]
    public double Importance { get; set; }

    [JsonPropertyName("addresstype")]
    public string AddressType { get; set; } = default!;

    [JsonPropertyName("name")]
    public string Name { get; set; } = default!;

    [JsonPropertyName("display_name")]
    public string DisplayName { get; set; } = default!;

    // Array of strings in the JSON
    [JsonPropertyName("boundingbox")]
    public string[] BoundingBox { get; set; } = Array.Empty<string>();

    // Optional convenience properties
    [JsonIgnore]
    public double Latitude => double.Parse(Lat, System.Globalization.CultureInfo.InvariantCulture);

    [JsonIgnore]
    public double Longitude => double.Parse(Lon, System.Globalization.CultureInfo.InvariantCulture);
}