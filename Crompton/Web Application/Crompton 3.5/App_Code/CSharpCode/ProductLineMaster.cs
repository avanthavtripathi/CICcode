﻿using System.Data;
using System.Web.Security;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

/// <summary>
/// Summary description for ProductLineMaster
/// </summary>
public class ProductLineMaster
{
    SqlDataAccessLayer objSql = new SqlDataAccessLayer();
    string strMsg;
    public ProductLineMaster()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    #region Properties and Variables
    public int ProductLineSNo
    { get; set; }

    // Added by Gaurav Garg
    public int BusinessLine_Sno
    { get; set; }

    public string ProductLineCode
    { get; set; }
    public int UnitSno
    { get; set; }

    public string ProductLineDesc
    { get; set; }
    public string PPR_Code
    { get; set; }
    public string EmpCode
    { get; set; }
    public string ActiveFlag
    { get; set; }
    public int ReturnValue
    { get; set; }

    public string DisplayName // 19 Feb 13 : By Bhawesh
    { get; set; }


    #endregion Properties and Variables
  
    #region Functions For save data
    public string SaveData(string strType)
    {
        SqlParameter[] sqlParamI =
        {
            new SqlParameter("@MessageOut",SqlDbType.VarChar,200),
            new SqlParameter("@Return_Value",SqlDbType.Int),
            new SqlParameter("@Type",strType),
            new SqlParameter("@EmpCode",this.EmpCode),
            new SqlParameter("@ProductLine_Code",this.ProductLineCode),
            new SqlParameter("@ProductLine_Desc",this.ProductLineDesc),
            new SqlParameter("@DisplayName",this.DisplayName), // Bhawesh 19 feb 13
            new SqlParameter("@PPR_Code",this.PPR_Code),
            new SqlParameter("@Unit_Sno",this.UnitSno),           
            new SqlParameter("@Active_Flag",int.Parse(this.ActiveFlag)),
            new SqlParameter("@ProductLine_SNo",this.ProductLineSNo),
            new SqlParameter("@BusinessLine_SNo",this.BusinessLine_Sno)
        };
        sqlParamI[0].Direction = ParameterDirection.Output;
        sqlParamI[1].Direction = ParameterDirection.ReturnValue;
        objSql.ExecuteNonQuery(CommandType.StoredProcedure, "uspProductLineMaster", sqlParamI);
        if (int.Parse(sqlParamI[1].Value.ToString()) == -1)
        {
            this.ReturnValue = int.Parse(sqlParamI[1].Value.ToString());
        }
        strMsg = sqlParamI[0].Value.ToString();
        sqlParamI = null;
        return strMsg;
    }
    #endregion Functions For save data
    #region Get ProductLine Master Data
    public void BindProductLineOnSNo(int intProductLineSNo, string strType)
    {
        DataSet dsProductLine = new DataSet();
        SqlParameter[] sqlParamG =
        {
            new SqlParameter("@Type",strType),
            new SqlParameter("@ProductLine_SNo",intProductLineSNo)
        };

        dsProductLine = objSql.ExecuteDataset(CommandType.StoredProcedure, "uspProductLineMaster", sqlParamG);
        if (dsProductLine.Tables[0].Rows.Count > 0)
        {
            ProductLineSNo = int.Parse(dsProductLine.Tables[0].Rows[0]["ProductLine_SNo"].ToString());
            ProductLineCode = dsProductLine.Tables[0].Rows[0]["ProductLine_Code"].ToString();
            ProductLineDesc = dsProductLine.Tables[0].Rows[0]["ProductLine_Desc"].ToString();
            DisplayName = dsProductLine.Tables[0].Rows[0]["DisplayName"].ToString(); // Bhawesh 19 feb 13
            PPR_Code = dsProductLine.Tables[0].Rows[0]["PPR_Code"].ToString();
            UnitSno = int.Parse(dsProductLine.Tables[0].Rows[0]["Unit_Sno"].ToString());
            // Added by Gaurav Garg 20 OCT 09 for MTO
            BusinessLine_Sno = int.Parse(dsProductLine.Tables[0].Rows[0]["BusinessLine_SNo"].ToString());
            // END
            ActiveFlag = dsProductLine.Tables[0].Rows[0]["Active_Flag"].ToString();
        }
        dsProductLine = null;
    }
    #endregion Get ProductLine Master Data
    public void BindUnitSno(DropDownList ddlUnitSno, int search, string EmpName)
    {
        DataSet dsUnitSno = new DataSet();
        SqlParameter[] sqlParam = 
        {
            new SqlParameter("@Type", "SELECT_ALL_UNITCODE_UNITSNO"),
             // Added by Gaurav Garg 20 OCT 09 for MTO
            new SqlParameter("@BusinessLine_SNo", search),
        // END
            new SqlParameter("@EmpCode",Membership.GetUser().UserName.ToString())
        };
        dsUnitSno = objSql.ExecuteDataset(CommandType.StoredProcedure, "uspProductLineMaster", sqlParam);
        ddlUnitSno.DataSource = dsUnitSno;
        ddlUnitSno.DataTextField = "Unit_Code";
        ddlUnitSno.DataValueField = "Unit_Sno";
        ddlUnitSno.DataBind();
        ddlUnitSno.Items.Insert(0, new ListItem("Select", "Select"));
        dsUnitSno = null;
        sqlParam = null;
    }


    // Code Added by Naveen on 06/11/2009 for MTO

    public void BindDdl(DropDownList ddl, int searchParam, string strType, string EmpCode)
    {
        DataSet ds = new DataSet();
        SqlParameter[] sqlParam = {
                                    new SqlParameter("@Type", strType),
                                    new SqlParameter("@Unit_Sno", searchParam),
                                    new SqlParameter("@ProductLine_SNo", searchParam),
                                    new SqlParameter("@BusinessLine_SNo", searchParam),
                                    new SqlParameter("@EmpCode",EmpCode) 
                                };

        //Getting values of ddls to bind department drop downlist using SQL Data Access Layer 
        ds = objSql.ExecuteDataset(CommandType.StoredProcedure, "uspProductLineMaster", sqlParam);
        ddl.DataSource = ds;
        if (strType == "SELECT_ALL_UNITCODE_UNITSNO")
        {
            ddl.DataTextField = "Unit_Code";
            ddl.DataValueField = "Unit_Sno";

        }
        if (strType == "FILLPRODUCTLINE")
        {
            ddl.DataTextField = "ProductLine_Desc";
            ddl.DataValueField = "ProductLine_SNo";
        }
        if (strType == "FILLPRODUCTGROUP")
        {
            ddl.DataTextField = "ProductGroup_Desc";
            ddl.DataValueField = "ProductGroup_SNo";
        }

        ddl.DataBind();
        ddl.Items.Insert(0, new ListItem("Select", "Select"));
        ds = null;
        sqlParam = null;
    }

}
