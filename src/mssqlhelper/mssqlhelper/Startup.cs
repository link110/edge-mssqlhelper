using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
namespace mssqlhelper
{
    public class Startup
    {
        
        /// <summary>
        /// 代理查看类
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<object> Invoke(dynamic parms)
        {
            //string connectionString = Environment.GetEnvironmentVariable("OWIN_SQL_CONNECTION_STRING");
            string connectionString = "packet size=4096;user id=sa;password=1qaz@WSX;data source=192.168.0.110;persist security info=false;initial catalog=RecoData2011";

            switch ((string)parms.et)
            {
                case "ExecuteType.ExecuteSql":
                    break;
                case "ExecuteType.ExecuteSqlTran":
                    break;
                case "ExecuteType.ExecuteSqlWithParms":
                    break;
                case "ExecuteType.ExecuteSqlInsertImg":
                    break;
                case "ExecuteType.GetSingle":
                    break;
                case "ExecuteType.SqlDataReader":
                    break;
                case "ExecuteType.Query":
                    return Task.Run(()=> 
                    {
                        DataSet ds = MSSqlOperate.Query((string)parms.command);
                        //转换ds为json
                        string ss = JsonHelper.DataTableToJson(ds.Tables[0]);
                        return ss;
                    });
                    break;
                case "ExecuteType.SqlGetResult":
                    break;
                case "ExecuteType.UpdateDataSet":
                    break;
                case "ExecuteType.UpdateDataTable":
                    break;
                case "ExecuteType.ExecuteSqlWithCmdParms":
                    break;
                case "ExecuteType.ExecuteSqlTrans":
                    break;
                case "ExecuteType.GetSingleWithCmdParms":
                    break;
                case "ExecuteType.ExecuteReaderWithCmdParms":
                    break;
                case "ExecuteType.QueryWithCmdParms":
                    break;
                default:
                    throw new InvalidOperationException("Unsupported type of SQL command. Only select, insert, update, and delete are supported.");
                    break;
            }
            throw new InvalidOperationException("Unsupported type of SQL command. Only select, insert, update, and delete are supported.");
            //if (((Parms)parms).et == ExecuteType.Query)
            //{

            //    //return await this.ExecuteQuery(connectionString, command);
            //}
            //else if (command.StartsWith("insert ", StringComparison.InvariantCultureIgnoreCase)
            //    || command.StartsWith("update ", StringComparison.InvariantCultureIgnoreCase)
            //    || command.StartsWith("delete ", StringComparison.InvariantCultureIgnoreCase))
            //{

            //    return await this.ExecuteNonQuery(connectionString, command);
            //}
            //else
            //{

            //}
        }
       
    }


}
