# MiHoYoWebBridge

![Nuget](https://img.shields.io/nuget/v/MiHoYoWebBridge?style=flat-square)

Getting started
---

1. Install the standard Nuget package into your application.
    
    ```
    Package Manager : Install-Package MiHoYoWebBridge -Version 1.0.0
    CLI : dotnet add package --version 1.0.0 MiHoYoWebBridge
    ```
    
2. Between `WebView.EnsureCoreWebView2Async()` and `WebView.LoadFirstUrl`, register the message handler, set user-agent and cookies.

   ```csharp
   using MiHoYoWebBridge;
   using MiHoYoWebBridge.Models;
   ```
   ```csharp
   WebView.CoreWebView2.SetUserAgent();
   WebView.CoreWebView2.InitCookies();
   WebView.CoreWebView2.InitBridge(true).Register<EvtClosePage>(_ => {
       Environment.Exit(0);
   })
   ```

_Now you can restart your application and check out the effect by `window.MiHoYoJSInterface.closePage()`_   

For more information and a full example about [米游社原神每日签到](https://webstatic.mihoyo.com/bbs/event/signin-ys/index.html?act_id=e202009291139501): [MiHoYoWebBridge/Demo](https://github.com/HolographicHat/MiHoYoWebBridge/tree/main/Demo)

Questions, bug reports or feature requests?
---
Please post them on the [issue list](https://github.com/HolographicHat/MiHoYoWebBridge/issues).

License
---
MiHoYoWebBridge is open source software, see [LICENSE](LICENSE) for details.
