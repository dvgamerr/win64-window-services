using System;

public class TimeElapsed
{
    private long nElapsed = 0;
    private long nTotal = 0;

    public TimeElapsed()
    {
        this.Reset();
    }

    public void NewCheckIn()
    {
        nElapsed = TicksPerMillisecond();
    }

    public void Reset()
    {
        nElapsed = TicksPerMillisecond();
        nTotal = 0;
    }

    private long TicksPerMillisecond()
    {
        return DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
    }
    private string toMessage(string format, string msg = null)
    {
        var nLap = TicksPerMillisecond();
        nTotal += nLap - nElapsed;
        var sResult = String.Format(format, (nLap - nElapsed).ToString("#,##0"), msg);
        nElapsed = nLap;
        return sResult;
    }

    public string toTotalElapsed(string format)
    {
        var nLap = TicksPerMillisecond();
        nTotal += nLap - nElapsed;
        var summary = nTotal + "ms.";
        if (nTotal > 86400000) summary = (nTotal / 86400000) + "day.";
        else if (nTotal > 3600000) summary = (nTotal / 3600000).ToString("#,##0.00") + "h.";
        else if(nTotal > 60000) summary = (nTotal / 60000).ToString("#,##0.00") + "m.";
        else if(nTotal > 1000) summary = (nTotal / 1000).ToString("#,##0.00") + "s.";
        return String.Format(format, summary);
    }

    //public void toLog(WriteLog clog, string msg)
    //{
    //    clog.WriteLogMassage("TIME", this.toMessage("{0} ms | {1}", msg));
    //}

    public string toStringMillisecond()
    {
        return this.toMessage(" ({0} ms)");
    }
    public string toMillisecond()
    {
        return this.toMessage("{0} ms");
    }
}
