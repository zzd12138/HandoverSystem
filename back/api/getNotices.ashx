<%@ WebHandler Language="C#" Class="getNotices" %>

using System;
using System.Web;
using System.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

public class getNotices : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/json";
        context.Response.AddHeader("Access-Control-Allow-Origin", "*");//解决跨域问题
        DbInfoInterface dbInfo = new DbInfoInterface();
        DataTable notices = dbInfo.GetUnexpiredNotice();

        List<Model.NameToDataTable> nameList = new List<Model.NameToDataTable>();
        nameList.Add(new Model.NameToDataTable("id", "ID", false));
        nameList.Add(new Model.NameToDataTable("title", "NOTICE_TITLE", true));
        nameList.Add(new Model.NameToDataTable("content", "NOTICE_CONTENT", true));
        nameList.Add(new Model.NameToDataTable("initiator", "NOTICE_INITIATOR", false));
        nameList.Add(new Model.NameToDataTable("creatime", "NOTICE_CREATETIME", false));
        nameList.Add(new Model.NameToDataTable("status", "NOTICE_STATUS", false));

        context.Response.Write(JsonHelper.detailJsonInfo(notices,nameList));
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}