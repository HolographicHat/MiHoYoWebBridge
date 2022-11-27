using MiHoYoWebBridge;
using MiHoYoWebBridge.Models;
using System;
using System.Diagnostics;
using System.Windows;

namespace Demo; 

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow {
        
    public MainWindow() {
        InitializeComponent();
    }

    private void Window_SizeChanged(object sender, SizeChangedEventArgs e) {
        Title = "MainWindow - " + e.NewSize;
    }

    private void BtnShowDev_Click(object sender, RoutedEventArgs e) {
        CWebView.CoreWebView2?.OpenDevToolsWindow();
    }

    private async void BtnInitWebview_Click(object sender, RoutedEventArgs evt) {
        await CWebView.EnsureCoreWebView2Async();
        CWebView.CoreWebView2.SetUserAgent();
        CWebView.CoreWebView2.InitCookies();
        CWebView.CoreWebView2.InitBridge(false).Register<EvtShowAlertDialog>((s, e) => {
            var mResult = MessageBox.Show(this, e.Message, e.Title, MessageBoxButton.OKCancel, MessageBoxImage.Information);
            s.Callback(result => { result.Data["buttonIndex"] = (int)mResult - 1; });
        }).Register<EvtClosePage>(_ => {
            Environment.Exit(0);
        }).Register<EvtRealPersonValidation>(_ => {
            MessageBox.Show(this, "请前往米游社进行实名认证后重试", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        }).Register<EvtGetDynamicSecretV1>(s => s.Callback(result => {
            var t = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var h = $"salt=jEpJb9rRARU2rXDA9qYbZ3selxkuct9a&t={t}&r=048pm3".Hash();
            result.Data["DS"] = $"{t},048pm3,{h}";
        })).Register<EvtGetDynamicSecretV2>(_ => {
        }).Register<EvtGetStatusBarHeight>(s => s.Callback(result => {
            result.Data["statusBarHeight"] = 0;
        })).Register<EvtGetUserInfo>(s => s.Callback(result => {
            result.Data["id"] = AccountInfo.Uid;
            result.Data["gender"] = AccountInfo.Gender;
            result.Data["nickname"] = AccountInfo.Name;
            result.Data["introduce"] = AccountInfo.Introduce;
            result.Data["avatar_url"] = AccountInfo.AvatarUrl;
        })).Register<EvtGetActionTicket>((s, e) => {
            Dispatcher.InvokeAsync(async () => {
                var rsp = await MiHoYoApiUtils.GetActionTicketAsync(e.ActionType);
                s.Callback(rsp);
            });
        }).Register<EvtGetCookieToken>((s, e) => {
            if (e.ForceRefresh) {
                Dispatcher.InvokeAsync(async () => {
                    var cookieToken = await MiHoYoApiUtils.GetCookieTokenAsync();
                    AccountInfo.CookieToken = cookieToken;
                    s.Callback(result => result.Data["cookie_token"] = cookieToken);
                });
            } else {
                s.Callback(result => result.Data["cookie_token"] = AccountInfo.CookieToken);
            }
        }).Register<EvtGetRequestHeader>(s => s.Callback(result => {
            result.Data["x-rpc-client_type"] = "2";
            result.Data["x-rpc-app_version"] = "2.40.1";
            result.Data["x-rpc-sys_version"] = "12";
            result.Data["x-rpc-channel"] = "miyousheluodi";
            result.Data["x-rpc-device_id"] = "00000000-0000-0000-0000-000000000000";
            result.Data["x-rpc-device_name"] = "Xiaomi Device";
            result.Data["x-rpc-device_model"] = "Device";
            result.Data["Referer"] = "https://app.mihoyo.com";
        })).Register<EvtPushPage>((_, e) => {
            e.PageUrl.Open();
        }).Register<EvtOpenSystemBrowser>((_, e) => {
            e.PageUrl.Open();
        }).Register<EvtGetWebLoginInfo>(s => s.Callback(result => {
            result.Data["ltuid"] = AccountInfo.Uid;
            result.Data["ltoken"] = AccountInfo.LToken;
            result.Data["login_ticket"] = AccountInfo.LoginTicket;
        })).Register<EvtSaveLoginTicket>((_, e) => {
            AccountInfo.LoginTicket = e.LoginTicket;
        }).Register<EvtGetNotificationSettings>(s => s.Callback(result => {
            result.Data["authorized"] = true;
        }));
        CWebView.Source = new Uri($"file:///{AppDomain.CurrentDomain.BaseDirectory}/JSBridgeUnitTest.html");
    }

    private bool _landscape = true;
    
    private void BtnOrientation_OnClick(object sender, RoutedEventArgs e) {
        CWebView.Width = _landscape ? 390 : 844;
        CWebView.Height = _landscape ? 844 : 390;
        _landscape = !_landscape;
    }

    private void BtnDemoPage_OnClick(object sender, RoutedEventArgs e) {
        CWebView.Source = new Uri("https://webstatic.mihoyo.com/bbs/event/signin-ys/index.html?act_id=e202009291139501");
    }
}
