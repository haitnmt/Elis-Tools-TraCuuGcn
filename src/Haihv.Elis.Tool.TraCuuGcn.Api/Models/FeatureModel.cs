using System.Text.Json;
using System.Text.Json.Serialization;
using OSGeo.OGR;

namespace Haihv.Elis.Tool.TraCuuGcn.Api.Models;

public class FeatureCollectionModel
{
    public string Type { get; } = "FeatureCollection";
    public List<FeatureModel> Features { get; set; }
    public Dictionary<string, object> Properties { get; set; }

    public FeatureCollectionModel()
    {
        Features = [];
        Properties = [];
    }
    public FeatureCollectionModel(List<Geometry> geometries,
                             Dictionary<string, object>? properties = null)
    {
        Features = [.. geometries.Select(g => new FeatureModel(g, properties))];
        Properties = properties ?? [];
    }
    public FeatureCollectionModel(List<FeatureModel> features,
                         Dictionary<string, object>? properties = null)
    {
        Features = features;
        Properties = properties ?? [];
    }
    public void AddFeature(FeatureModel feature) => Features.Add(feature);
    public void AddFeature(Geometry geometries, Dictionary<string, object>? properties = default) => Features.Add(new FeatureModel(geometries, properties));
    public void AddRangeFeature(List<FeatureModel> features) => Features.AddRange(features);
    public void AddRangeFeature(List<Geometry> geometries, Dictionary<string, object>? properties = default) => Features.AddRange(geometries.Select(g => new FeatureModel(g, properties)).ToList());

}
public class FeatureModel
{
    [JsonPropertyName("type")]
    public string Type { get; } = "Feature";
    [JsonPropertyName("geometry")]
    public object? Geometry { get; set; }
    [JsonPropertyName("properties")]
    public Dictionary<string, object> Properties { get; set; }
    public FeatureModel()
    {
        Geometry = null;
        Properties = [];
    }
    public FeatureModel(string geoJson, Dictionary<string, object>? properties = default)
    {
        // Giải mã GeoJSON geometry
        Geometry = JsonSerializer.Deserialize<object>(geoJson);
        Properties = properties ?? [];
    }
    public FeatureModel(Geometry geometry, Dictionary<string, object>? properties = default)
    {
        // Xuất polygon thành GeoJSON
        var geoJsonGeom = geometry.ExportToJson([]);
        // Giải mã GeoJSON geometry
        Geometry = JsonSerializer.Deserialize<object>(geoJsonGeom);
        Properties = properties ?? [];
        var area = geometry.GetArea();
        var length = geometry.Length();
        if (Properties.Count != 0) return;
        if (area > 0)
            Properties.Add("Diện tích", $"{area:00.0}m²");
        if (length > 0)
            Properties.Add("Chiều dài", $"{length:00.00}m");
    }
}