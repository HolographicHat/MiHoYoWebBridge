using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace Demo; 

public static class Miscs {
    
    public static Process? Start(this ProcessStartInfo startInfo) => Process.Start(startInfo);

    public static void Open(this string url) => new ProcessStartInfo {
        FileName = url,
        UseShellExecute = true
    }.Start();

    public static string Hash(this string text) {
        return Convert.ToHexString(MD5.HashData(Encoding.UTF8.GetBytes(text))).ToLowerInvariant();
    }
    
}
