using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Web.WebView2.Core;

namespace MiHoYoWebBridge.Models; 

public class JsParams {
    
    [JsonPropertyName("method")]
    public string Method { get; set; } = string.Empty;

    [JsonPropertyName("payload")]
    public JsonElement Data { get; set; }

    [JsonPropertyName("callback")]
    public string CallbackName { get; set; } = string.Empty;

    internal T GetData<T>() where T : notnull => Data.Deserialize<T>()!;

    public void Callback(Action<JsResult>? resultInitializer = null) {
        JsResult? result = null;
        if (resultInitializer != null) {
            result = new JsResult();
            resultInitializer.Invoke(result);
        }
        Callback(result?.ToJson());
    }

    public void Callback(string? result = null) => _webView.InvokeJsCallback(CallbackName, result);
    
    private CoreWebView2 _webView = null!;
    
    public static JsParams FromJson(CoreWebView2 webView, string str) {
        var instance = JsonSerializer.Deserialize<JsParams>(str)!;
        instance._webView = webView;
        return instance;
    }
}
