<%@ WebHandler Language="C#" Class="getInitiatedItems" %>

using System;
using System.Web;
using System.Data;
using System.Collections.Generic;

public class getInitiatedItems : IHttpHandler {

    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/json";
        string page = context.Request["page"];
        string limit = context.Request["limit"];
        string initiator=context.Request["initiator"];
        string orderByItem=context.Request["orderByItem"];
        string collation=context.Request["collation"];


        if (string.IsNullOrEmpty(initiator))
        {
            context.Response.Write(JsonHelper.submitErrorJson("发起人不能为空"));
            return;
        }

        if (string.IsNullOrEmpty(page)) page = "1";

        DbInfoInterface db = new DbInfoInterface();

        string count = db.getInitiatedItemsCount(initiator);
        DataTable data = db.getInitiatedItems(initiator, page, limit, orderByItem,collation);
        List<Model.NameToDataTable> itemList = new List<Model.NameToDataTable>();
        itemList.Add(new Model.NameToDataTable("id", "ID", false));
        itemList.Add(new Model.NameToDataTable("type", "HANDOVER_TYPE", false));
        itemList.Add(new Model.NameToDataTable("content", "HANDOVER_CONTENT", true));
        itemList.Add(new Model.NameToDataTable("initiator", "HANDOVER_INITIATOR", false));
        itemList.Add(new Model.NameToDataTable("recipient", "HANDOVER_RECIPIENT", false));
        itemList.Add(new Model.NameToDataTable("priority", "HANDOVER_PRIORITY", false));
        itemList.Add(new Model.NameToDataTable("status", "HANDOVER_STATUS", false));
        itemList.Add(new Model.NameToDataTable("initiationTime", "HANDOVER_CREATETIME", false));

        context.Response.Write(JsonHelper.pageJsonInfo(page, count, data, itemList));

    }

    public bool IsReusable {
        get {
            return false;
        }
    }


}