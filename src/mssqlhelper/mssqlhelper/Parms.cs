using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mssqlhelper
{
    public class Parms
    {
        public string command;
        public string et;

        public Parms(string cm, string et)
        {
            this.command = cm;
            this.et = et;
        }
    }
    public enum ExecuteType
    {
        ExecuteSql,
        ExecuteSqlTran,
        ExecuteSqlWithParms,
        ExecuteSqlInsertImg,
        GetSingle,
        SqlDataReader,
        Query,
        SqlGetResult,
        UpdateDataSet,
        UpdateDataTable,
        ExecuteSqlWithCmdParms,
        ExecuteSqlTrans,
        GetSingleWithCmdParms,
        ExecuteReaderWithCmdParms,
        QueryWithCmdParms
    }
}
