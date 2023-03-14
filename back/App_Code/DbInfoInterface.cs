using System;
using System.Collections.Generic;
using System.Data;
using DB.IDAL;


/// <summary>
///DbInfoInterface 的摘要说明
/// </summary>
public class DbInfoInterface
{
    public DbInfoInterface()
    {
        //
        //TODO: 在此处添加构造函数逻辑
        //
    }




    //获取金点子分页信息
    public DataTable getGoldInfo(String page, String limit)
    {
        string sql = null;
        int ipage = Convert.ToInt32(page);
        int ilimit = Convert.ToInt32(limit);

        sql = "select * from"
        + "(select ROW_NUMBER() OVER(Order by GOLD_CREATETIME DESC) AS RowNumber,* "
        + "from ("
        + "select GOLD_ID,GOLD_THEME,GOLD_NAME,GOLD_CREATETIME from GoldPoint"
        + ")as t1"
        + ")as t2 "
        + "where t2.RowNumber BETWEEN " + ((ipage - 1) * ilimit + 1) + " and " + (ipage * ilimit);

        DB_IDAL_SQL db_sql = new DB_IDAL_SQL();
        DataTable res = db_sql.QueryDataTable(sql);
        return res;
    }

    //获取所有用户姓名
    public DataTable getAllUserName()
    {
        string sql = "select USER_NAME,USER_POLICE_NUMBER from user_info";

        DB_IDAL_SQL db_sql = new DB_IDAL_SQL();

        DataTable res = db_sql.QueryDataTable(sql);

        return res;
    }




    //登陆
    public DataTable Login(Model.User user)
    {
        string sql = null;
        sql = String.Format(("select * from user_info where USER_POLICE_NUMBER='{0}' and USER_PASSWORD='{1}'"), user.policeNumber, user.password);
        DB_IDAL_SQL db_sql = new DB_IDAL_SQL();
        DataTable res = db_sql.QueryDataTable(sql);
        return res;
    }

    //注册
    public bool Reg(Model.User user)
    {
        string sql = String.Format("insert into user_info(USER_NAME,USER_PASSWORD,USER_POLICE_NUMBER) values('{0}','{1}','{2}')", user.userName, user.password,user.policeNumber);
        DB_IDAL_SQL db_sql = new DB_IDAL_SQL();
        bool res = db_sql.QueryExecuteNonQuery(sql);
        return res;
    }

    //修改密码
    public bool ModifyPwd(Model.User user)
    {
        string sql = string.Format("update user_info set USER_PASSWORD='{0}' where USER_POLICE_NUMBER='{1}'", user.password, user.policeNumber);
        DB_IDAL_SQL db_sql = new DB_IDAL_SQL();
        bool res = db_sql.QueryExecuteNonQuery(sql);
        return res;
    }

    //判断用户是否存在
    public bool IsExist(String policeNumber)
    {
        string sql = String.Format(("select count(*) from user_info where USER_POLICE_NUMBER='{0}'"), policeNumber);
        DB_IDAL_SQL db_sql = new DB_IDAL_SQL();
        int count = Convert.ToInt32(db_sql.QueryExecuteScalar(sql));
        return count >= 1;
    }

    //删除用户
    public bool DeleteUser(string policeNumber) {
        string sql = string.Format("delete  from user_info where USER_POLICE_NUMBER='{0}'", policeNumber);
        DB_IDAL_SQL db_sql = new DB_IDAL_SQL();
        bool res = db_sql.QueryExecuteNonQuery(sql);
        return res;
    }

    //增加通告
    public bool AddNotice(Model.Notice notice)
    {
        string sql = string.Format("insert into notices(NOTICE_TITLE,NOTICE_CONTENT,NOTICE_INITIATOR,NOTICE_CREATETIME) " +
        "values('{0}','{1}','{2}','{3}')", notice.title, notice.content, notice.initiator, notice.createTime);
        DB_IDAL_SQL db_sql = new DB_IDAL_SQL();
        bool res = db_sql.QueryExecuteNonQuery(sql);
        return res;
    }

    //获取未删除通告，按发起时间倒序排序
    public DataTable GetUnexpiredNotice()
    {
        string sql = "select * from notices where NOTICE_STATUS='0' order by NOTICE_CREATETIME desc";
        DB_IDAL_SQL db_sql = new DB_IDAL_SQL();
        DataTable res = db_sql.QueryDataTable(sql);
        return res;
    }

    //删除公告
    public bool deleteNotice(int id)
    {
        string sql = String.Format("update notices set NOTICE_STATUS='1' where ID={0}", id);
        DB_IDAL_SQL db_sql = new DB_IDAL_SQL();
        bool res = db_sql.QueryExecuteNonQuery(sql);
        return res;
    }

    //修改公告
    public bool ModifyNotice(Model.Notice notice)
    {
        string sql = string.Format("update notices set NOTICE_TITLE='{0}',NOTICE_CONTENT='{1}' where ID={2}",
        notice.title, notice.content, notice.id);
        DB_IDAL_SQL db_sql = new DB_IDAL_SQL();
        bool res = db_sql.QueryExecuteNonQuery(sql);
        return res;
    }

    //添加事项
    public bool AddItems(List<Model.Item> items)
    {
        if (items.Count != 0)
        {
            string sql = "insert into items(HANDOVER_TYPE,HANDOVER_CONTENT,HANDOVER_INITIATOR,HANDOVER_PRIORITY,HANDOVER_RECIPIENT) ";
            foreach (var i in items)
            {
                if (items.IndexOf(i) == 0)
                {
                    sql += string.Format("values('{0}','{1}','{2}','{3}','{4}')", i.type, i.content, i.initiator, i.priority, i.recipient);
                    continue;
                }
                sql += string.Format(",('{0}','{1}','{2}','{3}','{4}')", i.type, i.content, i.initiator, i.priority, i.recipient);
            }
            DB_IDAL_SQL db_sql = new DB_IDAL_SQL();
            bool res = db_sql.QueryExecuteNonQuery(sql);
            return res;
        }
        return true;
    }

    //未完成事项转交
    public bool TransmittedUnDoItems(String initiator, String recipient)
    {
        string sql = string.Format("update items set HANDOVER_RECIPIENT='{0}' where HANDOVER_RECIPIENT='{1}' and HANDOVER_STATUS='False' ", recipient, initiator);
        DB_IDAL_SQL db_sql = new DB_IDAL_SQL();
        bool res = db_sql.QueryExecuteNonQuery(sql);
        return res;
    }

    //获取待办事项（三天内已完成事项+所有未完成事项）
    public DataTable getToDoItems(String name)
    {
        string sql = String.Format("select * from items where (HANDOVER_RECIPIENT='{0}'"
+ "and datediff(day, HANDOVER_CREATETIME, getdate())<= 2"
+ "and datediff(day, HANDOVER_CREATETIME, getdate())>= 0"
+ "and HANDOVER_STATUS = '1') or(HANDOVER_STATUS = '0' and HANDOVER_RECIPIENT = '{0}')  ORDER BY HANDOVER_CREATETIME DESC", name);
        DB_IDAL_SQL db_sql = new DB_IDAL_SQL();
        DataTable res = db_sql.QueryDataTable(sql);
        return res;
    }

    //更新事项完成状态
    public bool updateItemStatus(int id, int status)
    {
        string sql = String.Format("update items set HANDOVER_STATUS={0} where ID={1}", status, id);
        DB_IDAL_SQL db_sql = new DB_IDAL_SQL();
        bool res = db_sql.QueryExecuteNonQuery(sql);
        return res;
    }

    //获取发起事项分页信息
    public DataTable getInitiatedItems(string initiator, string page, string limit, string orderByItem, string collation)
    {
        string sql = null;
        if (string.IsNullOrEmpty(page)) page = "1";
        if (string.IsNullOrEmpty(limit)) limit = "10";
        if (string.IsNullOrEmpty(orderByItem)) orderByItem = "ID";
        if (string.IsNullOrEmpty(collation)) collation = "desc";
        int ipage = Convert.ToInt32(page);
        int ilimit = Convert.ToInt32(limit);

        sql = string.Format("select * from(select ROW_NUMBER() OVER(Order by {0} {4}) AS RowNumber,* from items where HANDOVER_INITIATOR='{1}')as t1"
                            + " where t1.RowNumber BETWEEN {2} and {3}", orderByItem, initiator, ((ipage - 1) * ilimit + 1), (ipage * ilimit), collation);

        DB_IDAL_SQL db_sql = new DB_IDAL_SQL();
        DataTable res = db_sql.QueryDataTable(sql);

        return res;
    }

    //获取发起事项数量
    public string getInitiatedItemsCount(string initiator)
    {
        string sql = string.Format("select count(*) from items where HANDOVER_INITIATOR='{0}'", initiator);
        DB_IDAL_SQL db_sql = new DB_IDAL_SQL();
        return db_sql.QueryExecuteScalar(sql);


    }
}