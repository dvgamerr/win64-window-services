using System;
using System.Data.SqlClient;
using System.Data;
using System.Text.RegularExpressions;

public class MSSQL
{
    protected SqlConnection cConnDB = null;
    protected SqlTransaction cTranDB = null;
    protected bool IsTransection = false;
    string sConnectionString = "Data Source={0};Initial Catalog={1};User Id={2};Password={3};Timeout=6000;MultipleActiveResultSets=True;Connection Timeout=60;Min Pool Size=2;Max Pool Size=200;";

    public MSSQL(bool IsOpen = true)
    {
        var cINI = new IniFile(Program.ConfigINI);
        var sDataSource = cINI.Read("Source", "DataInfo");
        var sDBName = cINI.Read("Name", "DataInfo");
        var sDBUser = cINI.Read("Username", "DataInfo");
        var sDBPass = cINI.Read("Password", "DataInfo");
        cINI = null;
        sConnectionString = String.Format(sConnectionString, sDataSource, sDBName, sDBUser, sDBPass);
        if (IsOpen) this.OpenConnection();
    }

    protected void SetConnection(MSSQL cDB)
    {
        cConnDB = cDB.cConnDB;
        cTranDB = cDB.cTranDB;
        IsTransection = cDB.IsTransection;
    }

    public void OpenConnection()
    {
        try
        {
            if (cConnDB == null) cConnDB = new SqlConnection(sConnectionString);
            if (cConnDB.State == ConnectionState.Closed) cConnDB.Open();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public void OpenTransaction()
    {
        if (cConnDB == null) this.OpenConnection();
        cTranDB = cConnDB.BeginTransaction();
        IsTransection = true;
    }

    public void Commit()
    {
        cTranDB.Commit();
        IsTransection = false;
        this.CloseConnection();
    }

    public void Rollback()
    {
        cTranDB.Rollback();
        IsTransection = false;
        this.CloseConnection();
    }

    public void CloseConnection()
    {
        if (cConnDB != null && cConnDB.State != ConnectionState.Closed)
        {
            try
            {
                if (IsTransection) cTranDB.Rollback();
                cConnDB.Close();
                cConnDB.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                IsTransection = false;
                cConnDB = null;
                cTranDB = null;
            }
        }
	}

    public DataRow GetRow(string sCommand, params object[] aArgs)
    {
        DataRow dr = null;
        using (SqlDataAdapter cAdapter = new SqlDataAdapter(String.Format(sCommand, this.ReplaceString(aArgs)), cConnDB))
        {
            DataTable dt = new DataTable();
            cAdapter.Fill(dt);
            if (dt.Rows.Count > 0) dr = dt.Rows[0];
        }
        return dr;
    }

    public DataTable GetTable(string sCommand, params object[] aArgs)
    {
        DataTable dt = null;
        using (SqlDataAdapter cAdapter = new SqlDataAdapter(String.Format(sCommand, this.ReplaceString(aArgs)), cConnDB))
        {
            cAdapter.SelectCommand.CommandTimeout = 180;
            dt = new DataTable();
            cAdapter.Fill(dt);
        }
        return dt;
    }

    public object GetField(string sCommand, params object[] aArgs)
    {
        object sResult = null;
        using (SqlCommand cCmd = cConnDB.CreateCommand())
        {
            cCmd.CommandText = String.Format(sCommand, this.ReplaceString(aArgs));
            cCmd.CommandTimeout = 3000;
            sResult = cCmd.ExecuteScalar();
        }
        return sResult;
    }

    public bool Execute(string sCommand, params object[] aArgs)
    {
        int nEffect = 0;
        using (SqlCommand cCmd = cConnDB.CreateCommand())
        {
            cCmd.CommandText = String.Format(sCommand, this.ReplaceString(aArgs));
            cCmd.CommandTimeout = 3000;
            nEffect = cCmd.ExecuteNonQuery();
        }
        return nEffect > 0;
    }

    private object[] ReplaceString(object[] oArgs)
    {
        for (int i = 0; i < oArgs.Length; i++)
        {
            oArgs[i] = Regex.Replace(oArgs[i].ToString(), "'", "\\'", RegexOptions.IgnoreCase);
        }
        return oArgs;
    }
}



