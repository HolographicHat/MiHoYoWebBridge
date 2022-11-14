using System.Text.Json;
using System.Text.Json.Serialization;

namespace MiHoYoWebBridge.Models; 

[Serializable]
public class JsResult {

    [JsonPropertyName("retcode")]
    public int Code { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;

    [JsonPropertyName("data")]
    public Dictionary<string, object> Data { get; set; } = new ();

    public string ToJson() => JsonSerializer.Serialize(this);

    public static JsResult FromJson(string str) => JsonSerializer.Deserialize<JsResult>(str)!;
    
}
