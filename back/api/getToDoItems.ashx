<%@ WebHandler Language="C#" Class="getToDoItems" %>

using System;
using System.Web;
using System.Data;
using System.Collections.Generic;

public class getToDoItems : IHttpHandler {

    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/json";
        String name = context.Request["userName"];
        DbInfoInterface db = new DbInfoInterface();
        DataTable data = db.getToDoItems(name);
         
        //优先级字段去除空格
        for (int i = 0; i < data.Rows.Count; i++)
        {
            data.Rows[i]["HANDOVER_PRIORITY"] = data.Rows[i]["HANDOVER_PRIORITY"].ToString().Trim();
        }

        List<Model.NameToDataTable> itemList = new List<Model.NameToDataTable>();
        itemList.Add(new Model.NameToDataTable("id", "ID",false));
        itemList.Add(new Model.NameToDataTable("type", "HANDOVER_TYPE",false));
        itemList.Add(new Model.NameToDataTable("content", "HANDOVER_CONTENT",true));
        itemList.Add(new Model.NameToDataTable("initiator", "HANDOVER_INITIATOR",false));
        itemList.Add(new Model.NameToDataTable("recipient", "HANDOVER_RECIPIENT",false));
        itemList.Add(new Model.NameToDataTable("priority", "HANDOVER_PRIORITY",false));
        itemList.Add(new Model.NameToDataTable("status", "HANDOVER_STATUS",false));
        itemList.Add(new Model.NameToDataTable("initiationTime", "HANDOVER_CREATETIME",false));

        string res = JsonHelper.detailJsonInfo(data, itemList);
        context.Response.Write(res);

    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}