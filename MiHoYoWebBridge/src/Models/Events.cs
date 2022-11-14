using System.Text.Json.Serialization;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace MiHoYoWebBridge.Models; 

[Serializable, WebInvoke("closePage")]
public struct EvtClosePage {}

[Serializable, WebInvoke("getUserInfo")]
public struct EvtGetUserInfo {}

[Serializable, WebInvoke("getCookieInfo")]
public struct EvtGetWebLoginInfo {}

[Serializable, WebInvoke("startRealPersonValidation")]
public struct EvtRealPersonValidation {}

[Serializable, WebInvoke("startRealnameAuth")]
public struct EvtGetRealNameStatus {} // guess

[Serializable, WebInvoke("getHTTPRequestHeaders")]
public struct EvtGetRequestHeader {}

[Serializable, WebInvoke("getNotificationSettings")]
public struct EvtGetNotificationSettings {}

// just zero
[Serializable, WebInvoke("getStatusBarHeight")]
public struct EvtGetStatusBarHeight {}

[Serializable, WebInvoke("getDS")]
public struct EvtGetDynamicSecretV1 {}

[Serializable, WebInvoke("getDS2")]
public class EvtGetDynamicSecretV2 {
    
    [JsonPropertyName("query")]
    public Dictionary<string, string> Query { get; set; } = new ();

    [JsonPropertyName("body")]
    public string Body { get; set; } = string.Empty;

}

[Serializable, WebInvoke("getCookieToken")]
public class EvtGetCookieToken {
    
    [JsonPropertyName("forceRefresh")]
    public bool ForceRefresh { get; set; }
    
}

[Serializable, WebInvoke("getActionTicket")]
public class EvtGetActionTicket {
    
    [JsonPropertyName("action_type")]
    public string ActionType { get; set; } = string.Empty;
    
}

[Serializable, WebInvoke("saveLoginTicket")]
public class EvtSaveLoginTicket {
    
    [JsonPropertyName("login_ticket")]
    public string LoginTicket { get; set; } = string.Empty;
    
}

[Serializable, WebInvoke("showToast")]
public class EvtShowToast {
    
    [JsonPropertyName("toast")]
    public string Text { get; set; } = string.Empty;
    
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

}

[Serializable, WebInvoke("openSystemBrowser")]
public class EvtOpenSystemBrowser {
    
    [JsonPropertyName("open_url")]
    public string PageUrl { get; set; } = string.Empty;
    
}

[Serializable, WebInvoke("pushPage")]
public class EvtPushPage {

    private string _pageUrl = string.Empty;
    
    [JsonPropertyName("page")]
    public string PageUrl {
        get => _pageUrl;
        set => _pageUrl = !value.StartsWith("mihoyobbs") 
            ? value 
            : value.Replace("mihoyobbs:/", "https://bbs.mihoyo.com/dby")
                .Replace("topic", "topicDetail");
    }
}

[Serializable, WebInvoke("configure_share")]
public class EvtConfigureShare {
    
    [JsonPropertyName("enable")]
    public bool Enable { get; set; }

}

[Serializable, WebInvoke("showAlertDialog")]
public class EvtShowAlertDialog {

    public class ButtonParams {
        
        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;
        
        [JsonPropertyName("style")]
        public string Style { get; set; } = string.Empty;
        
    }
    
    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;

    [JsonPropertyName("buttons")]
    public ButtonParams[] Buttons { get; set; } = Array.Empty<ButtonParams>();

}

public class GenAuthKeyBase {
    
    [JsonPropertyName("game_biz")]
    public string Biz { get; set; } = null!;

    [JsonPropertyName("auth_appid")]
    public string AppId { get; set; } = null!;

    [JsonPropertyName("game_uid")]
    public uint Uid { get; set; } = 0;

    [JsonPropertyName("region")]
    public string Region { get; set; } = string.Empty;

}

[Serializable, WebInvoke("genAuthKey")]
public class EvtGenAuthKey : GenAuthKeyBase {}

[Serializable, WebInvoke("genAppAuthKey")]
public class EvtGenAppAuthKey : GenAuthKeyBase {}
