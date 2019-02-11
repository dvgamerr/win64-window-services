using System;
using System.IO;

public enum LogType { INFO, DEBUG, TIME, ERROR }
public class LogFile
{
    public static string PathLog = "";
    string sLogsPath = Path.GetTempPath();
    string sFilename = "FCCR.txt";
    string sFolderName = "SaleFCCR";
    string sLevel = "2";
    public LogFile(string sPMCSLogLevel, string sPMCSPath)
    {
        //nLogWrite = DateTime.Now.Ticks / TimeSpan.TicksPerSecond;
        //cLogFile = new List<String>();
        if (!String.IsNullOrEmpty(sPMCSPath)) sLogsPath = sPMCSPath;
        sLevel = sPMCSLogLevel.Trim();
    }

    public void WriteLogMassage(LogType LogType, string massage)
    {
        if (String.IsNullOrEmpty(massage) || sLevel == "0") return;

        if ((sLevel == "1" && LogType != LogType.INFO && LogType != LogType.DEBUG && LogType != LogType.TIME) || (sLevel == "2" && LogType != LogType.TIME) || sLevel == "3")
        {
            try
            {
                var file = Path.Combine(Path.Combine(sLogsPath, "Log"), Path.Combine(sFolderName, DateTime.Now.ToString("yyyyMMdd_") + sFilename));
                LogFile.PathLog = file;
                if (!Directory.Exists(Path.GetDirectoryName(file))) Directory.CreateDirectory(Path.GetDirectoryName(file));
                File.AppendAllText(file, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " " + LogType.ToString() + " " + massage + Environment.NewLine);
            }
            catch { }
        }
    }
}
