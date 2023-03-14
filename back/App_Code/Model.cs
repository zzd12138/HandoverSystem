using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// 实体类
/// </summary>
public class Model
{
    public Model()
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //
    }

    //用户实体类
    public class User
    {
        public String policeNumber { get; set; }
        public String userName { get; set; }
        public String password { get; set; }
    }


    //通告实体类

    public class Notice
    {

        public int id { get; set; }
        //通告标题
        public string title { get; set; }
        //通告内容
        public String content { get; set; }

        //发起人
        public String initiator { get; set; }

        //开始时间
        public DateTime createTime { get; set; }

        //通告状态
        public bool status { get; set; }
    }

    //事项实体类
    public class Item
    {
        public String id { get; set; }
        public String type { get; set; }

        public String content { get; set; }
        //发起人
        public String initiator { get; set; }
        //优先级
        public String priority { get; set; }
        //接收人
        public String recipient { get; set; }

        //完成状态
        public bool status { get; set; }

        //发起时间
        public DateTime initiationTime { get; set; }
    }


    //Json名称对应DataTable中的表头
    public class NameToDataTable
    {

        public NameToDataTable(string jsonName, string dataTableName, bool isTransition)
        {
            this.jsonName = jsonName;
            this.dataTableName = dataTableName;
            this.isTransition = isTransition;
        }
        public NameToDataTable(string jsonName, string dataTableName)
        {
            this.jsonName = jsonName;
            this.dataTableName = dataTableName;
        }
        public readonly String jsonName;
        public readonly String dataTableName;

        public readonly Boolean isTransition;//英文引号是否要转换为中文引号
    }
}