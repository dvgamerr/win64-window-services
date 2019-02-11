using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

class Program
{
    public static bool IsDebug = true;
    public static bool IsWorker = false;

    public static IniFile aConfig;
    public static string ConfigINI = null;

    private static LogFile aLog;
    static void Main(string[] args)
    {
        Program.ConfigINI = typeof(Program).Assembly.GetName().Name + ".ini";
        var elapsed = new TimeElapsed();
        if (args.Length == 0) {
            HelpCommand();
            return;
        }
        if (!VerifyArgSetting(args)) return;

        while (IsWorker)
        {

        }
    }

    public static void Debug(LogType eType, string sMessage, params object[] aArgs)
    {
        if (IsDebug)
        {
            Console.WriteLine(String.Format(" {2} - {0} {1}", eType.ToString(), sMessage, DateTime.Now.ToString("HH:mm:ss.fff")), aArgs);
        }
        else
        {
            aLog.WriteLogMassage(eType, String.Format(sMessage, aArgs));
        }

    }
    public static void Debug(LogType eType, Exception ex)
    {
        if (IsDebug)
        {
            Console.WriteLine(String.Format(" {2} - {0} {1}", eType.ToString(), ex.Message, DateTime.Now.ToString("HH:mm:ss.fff")));
            Console.WriteLine("----------------------------------------------------------------------------------------");
            Console.WriteLine(ex.Source);
            Console.WriteLine("----------------------------------------------------------------------------------------");
        }
        else
        {
            aLog.WriteLogMassage(eType, ex.Message);
            aLog.WriteLogMassage(eType, ex.Source.Replace(Environment.NewLine, " -> "));
        }
    }
    private static bool VerifyArgSetting(string[] args)
    {
        string[] aCommand = { "--directory", "--worker", "--debug", "--ftp", "--plant", "--sloc", "--date" };
        string IsError = null;
        for (var i = 0; i < args.Length; i++)
        {
            var n = i + 1;
            switch (args[i].ToLower())
            {
                //case "--directory": IsCurrentDirectory = true; break;
                case "--debug": IsDebug = true; break;
                case "--worker": IsWorker = true; break;
                //case "--ftp": IsFTPSend = true; break;
                //case "--plant":
                //    if (n >= args.Length || aCommand.Contains(args[n].ToLower()))
                //    {
                //        IsError = "sPlant is not found.";
                //    }
                //    else
                //    {
                //        sPlant = args[n];
                //    }
                //    break;
                //case "--sloc":
                //    if (n >= args.Length || aCommand.Contains(args[n].ToLower()))
                //    {
                //        IsError = "sSLOC is not found.";
                //    }
                //    else
                //    {
                //        sSloc = args[n];
                //    }
                //    break;
                //case "--date":
                //    if (n >= args.Length || aCommand.Contains(args[n].ToLower()))
                //    {
                //        IsError = "nTransactionDate is not found.";
                //    }
                //    else
                //    {
                //        sDate = args[n];
                //        if (!DateTime.TryParse(args[n], CultureInfo.CreateSpecificCulture("en-US"), DateTimeStyles.None, out dDate)) {
                //            IsError = String.Format("nTransactionDate try format '{0}'.", sDateFormat);
                //        }
                //    }
                //    break;
            }
            if (!String.IsNullOrEmpty(IsError)) break;
        }
        if (!String.IsNullOrEmpty(IsError)) Console.WriteLine(IsError);
        return String.IsNullOrEmpty(IsError);
    }
    private static void HelpCommand()
    {
        Console.WriteLine(@"
sale-fccr appication for generate sale to fie type fccr.

--worker Current Process all data to ini.
--plant 0000 set plant only worker.
--sloc 0000 set sloc only worker.
--date dd-mm-yyyy set date only worker.

Example
sale-fccr --worker
sale-fccr --plant 1106 --sloc 1000 --date 31-12-2018

");          
    }
}
