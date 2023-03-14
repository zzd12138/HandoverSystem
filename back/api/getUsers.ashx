<%@ WebHandler Language="C#" Class="getUsers" %>

using System;
using System.Web;
using System.Data;
using System.Collections.Generic;

public class getUsers : IHttpHandler {

    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/json";
        DbInfoInterface db = new DbInfoInterface();
        DataTable users = db.getAllUserName();
        List<Model.NameToDataTable> list=new List<Model.NameToDataTable>();
        list.Add(new Model.NameToDataTable("policeNumber", "USER_POLICE_NUMBER",false));
        list.Add(new Model.NameToDataTable("userName", "USER_NAME",false));
       context.Response.Write(JsonHelper.detailJsonInfo(users,list));
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}