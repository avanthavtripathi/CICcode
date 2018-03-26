﻿using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class PasswordRecovery : System.Web.UI.Page
{
    LoginHelper ObjLoginHelper = new LoginHelper();
  
    protected void Page_Load(object sender, EventArgs e)
    {
      
    }

    protected void LoginButton_Click(object sender, ImageClickEventArgs e)
    {
            ObjLoginHelper.LoginName = TxtUserName.Text.Trim();
            ObjLoginHelper.UnlockUserAccount();
            if (string.IsNullOrEmpty(ObjLoginHelper.Message))
            { 
                 tblAccount.Visible = false;
                 dvsuccess.Visible = true;
                 LoginButton.Enabled = false;
            }
            else
            {
                 FailureText.Text = ObjLoginHelper.Message;

            }
    }
}
