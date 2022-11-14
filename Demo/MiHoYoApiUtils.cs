using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Web.WebView2.Core;
using MiHoYoWebBridge.Models;

namespace Demo; 

public static class AccountInfo {
    public const int Gender = 0;
    public const string Uid = "100000008";
    public const string Mid = "0abcdefg12_mhy";
    public const string Name = "HoloHat";
    public const string Introduce = "";
    public const string LToken = "58b34ea60a82ecbd88bb00778ab21c75f354c50b";
    public const string STokenV2 = "v2_e29e12c9fed0787790f16d7f52d958c3_o_22b38ecde29e12c9fed07_-87790f16d-7f52d958c322b38=";
    public const string AvatarUrl = "https://img-static.mihoyo.com/communityweb/upload/52de23f1b1a060e4ccaa8b24c1305dd9.png";
    public static string LoginTicket { get; set; } = string.Empty;
    public static string CookieToken { get; set; } = "e29e12c9fed0787790f16d7f52d958c322b38ecd";
}

internal static class MiHoYoApiUtils {

    private const string Takumi = "https://api-takumi.mihoyo.com/";
    private const string Passport = "https://passport-api.mihoyo.com/";
    
    private static readonly HttpClient HClient = new () {
        DefaultRequestHeaders = {
            {
                "Cookie", new Dictionary<string, object> {
                    { "mid", AccountInfo.Mid },
                    { "stuid", AccountInfo.Uid },
                    { "stoken", AccountInfo.STokenV2 }
                }.Select(pair => $"{pair.Key}={pair.Value}").JoinToString(';')
            }
        }
    };

    private static void AddMhyCookie(this CoreWebView2CookieManager cookieManager, string name, string value) {
        cookieManager.AddOrUpdateCookie(cookieManager.CreateCookie(name, value, ".mihoyo.com", "/"));
    }

    public static void InitCookies(this CoreWebView2 webView) {
        var cookieManager = webView.CookieManager;
        cookieManager.AddMhyCookie("account_id", AccountInfo.Uid);
        cookieManager.AddMhyCookie("cookie_token", AccountInfo.CookieToken);
        cookieManager.AddMhyCookie("ltuid", AccountInfo.Uid);
        cookieManager.AddMhyCookie("ltoken", AccountInfo.LToken);
        cookieManager.AddMhyCookie("login_ticket", AccountInfo.LoginTicket);
    }

    public static async Task<string> GetActionTicketAsync(string type) {
        return await HClient.GetStringAsync($"{Takumi}auth/api/getActionTicketBySToken?action_type={type}");
    }

    public static async Task<string> GetCookieTokenAsync() {
        var rsp = await HClient.GetFromJsonAsync<JsResult>($"{Passport}account/auth/api/getCookieAccountInfoBySToken");
        var token = rsp!.Data["cookie_token"].ToString();
        Debug.WriteLine(token);
        return token!;
    }

    private static string JoinToString(this IEnumerable<string> strings, char separator) {
        return string.Join(separator, strings);
    }
}
