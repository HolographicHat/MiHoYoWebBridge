namespace MiHoYoWebBridge.Models; 

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
public class WebInvokeAttribute : Attribute {
    
    public readonly string Name;
    
    public WebInvokeAttribute(string name) {
        Name = name;
    }
}
