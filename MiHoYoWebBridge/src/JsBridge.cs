using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.Web.WebView2.Core;
using MiHoYoWebBridge.Models;

namespace MiHoYoWebBridge; 

[ComVisible(true)]
[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
public interface IBridgeInterface {
    void OnMessage(string str);
    
}

[ComVisible(true)]
[ClassInterface(ClassInterfaceType.None)]
public class MiHoYoJsBridge : IBridgeInterface {

    private CoreWebView2 _webView;
    private readonly Dictionary<string, Type> _typeMap = new ();
    private readonly Dictionary<string, Action<JsParams>> _callbackMap = new ();

    internal MiHoYoJsBridge(CoreWebView2 webView) {
        _webView = webView;
    }
    
    public void OnMessage(string str) {
        System.Diagnostics.Debug.WriteLine($"MiHoYoJsBridge.OnMessage -> {str}");
        var p = JsParams.FromJson(_webView, str);
        _callbackMap.GetValueOrDefault(p.Method)?.Invoke(p);
    }

    private string GetInvokeEntryName<T>() {
        var type = typeof(T);
        var invokeName = type.GetCustomAttribute<WebInvokeAttribute>()?.Name;
        if (invokeName == null) throw new ArgumentException("Invalid type.");
        _typeMap[invokeName] = type;
        return invokeName;
    }

    public MiHoYoJsBridge Register<T>(Action<JsParams> callback) where T : notnull {
        _callbackMap[GetInvokeEntryName<T>()] = callback;
        return this;
    }

    public MiHoYoJsBridge Register<T>(Action<JsParams, T> callback) where T : notnull {
        _callbackMap[GetInvokeEntryName<T>()] = p => callback(p, p.GetData<T>());
        return this;
    }
}
