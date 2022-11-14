using Microsoft.Web.WebView2.Core;

namespace MiHoYoWebBridge;

public static class WebviewExtensions {

    private const string InitJsInterfaceScript =
        "let c={};c.postMessage=str=>chrome.webview.hostObjects.MiHoYoWebBridge.OnMessage(str);" +
        "c.closePage=()=>c.postMessage('{\"method\":\"closePage\"}');window.MiHoYoJSInterface=c";

    private const string HideScrollBarScript = 
        "let st=document.createElement('style');st.innerHTML='::-webkit-scro" +
        "llbar{display:none}';document.querySelector('body').appendChild(st)";
    
    public static void SetUserAgent(this CoreWebView2 webView) {
        webView.Settings.UserAgent = "Mozilla/5.0 (Linux; Android 12) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/106.0.5249.126 Mobile Safari/537.36 miHoYoBBS/2.40.1";
    }

    public static MiHoYoJsBridge InitBridge(this CoreWebView2 webView, bool checkDomain = true) {
        var bridge = new MiHoYoJsBridge(webView);
        webView.AddHostObjectToScript("MiHoYoWebBridge", bridge);
        webView.NavigationStarting += (_, args) => {
            if (!checkDomain || new Uri(args.Uri).Host.EndsWith("mihoyo.com")) {
                webView.ExecuteScriptAsync(InitJsInterfaceScript);
            }
        };
        webView.DOMContentLoaded += (_, _) => webView.ExecuteScriptAsync(HideScrollBarScript);
        return bridge;
    }

    internal static void InvokeJsCallback(this CoreWebView2 webView, string cb, string? payload = null) {
        if (cb == string.Empty) return;
        var dataStr = payload == null ? string.Empty : $", {payload}";
        var js = $"javascript:mhyWebBridge(\"{cb}\"{dataStr})";
        System.Diagnostics.Debug.WriteLine($"InvokeJsCallback: {js}");
        webView.ExecuteScriptAsync(js);
    }
}
