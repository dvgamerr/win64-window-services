using System;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

class IniFile
{
    string sPath = "";

    [DllImport("kernel32", CharSet = CharSet.Unicode)]
    static extern long WritePrivateProfileString(string Section, string Key, string Value, string FilePath);

    [DllImport("kernel32", CharSet = CharSet.Unicode)]
    static extern int GetPrivateProfileString(string Section, string Key, string Default, StringBuilder RetVal, int Size, string FilePath);

    public IniFile(string sFileName = null)
    {
        // if (sFileName == null) sFileName = Application.Name // sale-fccr
        string sCMGPOSINI = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, sFileName);
        if (!File.Exists(sCMGPOSINI)) throw new Exception(String.Format("{0} not found '" + sCMGPOSINI + "'", sFileName));
        sPath = new FileInfo(sCMGPOSINI).FullName.ToString();
    }

    public string Read(string Key, string Section = null)
    {
        var RetVal = new StringBuilder(255);
        GetPrivateProfileString(Section, Key, "", RetVal, 255, sPath);
        var v = RetVal.ToString();
        return v.Length > 0 && v.IndexOf("#") > -1 ? v.Remove(v.IndexOf("#"), v.Length - v.IndexOf("#")).Trim() : v;
    }

    public bool KeyExists(string Key, string Section = null)
    {
        return Read(Key, Section).Length > 0;
    }
}
