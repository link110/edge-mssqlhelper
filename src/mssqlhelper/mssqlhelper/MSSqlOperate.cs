using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;


namespace mssqlhelper
{
    public class MSSqlOperate
    {
        public MSSqlOperate()
        { }

        #region  执行简单SQL语句
        #region 原来的函数

        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSql(string SQLString)
        {
            dbsqlHelper.MSSqlConnect Connect = dbsqlHelper.MSSqlConnect.getInstance();
            SqlConnection connection = Connect.getSystemConnect();

            using (SqlCommand cmd = new SqlCommand(SQLString))
            {
                try
                {
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }

                    int rows = cmd.ExecuteNonQuery();

                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                    return rows;
                }
                catch (System.Data.SqlClient.SqlException E)
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }

                    throw new Exception(E.Message);
                }
            }

        }

        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">多条SQL语句</param>		
        public static void ExecuteSqlTran(ArrayList SQLStringList)
        {
            dbsqlHelper.MSSqlConnect Connect = dbsqlHelper.MSSqlConnect.getInstance();
            SqlConnection connection = Connect.getSystemConnect();

            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            SqlTransaction tx = connection.BeginTransaction();
            cmd.Transaction = tx;
            try
            {
                for (int n = 0; n < SQLStringList.Count; n++)
                {
                    string strsql = SQLStringList[n].ToString();
                    if (strsql.Trim().Length > 1)
                    {
                        cmd.CommandText = strsql;
                        cmd.ExecuteNonQuery();
                    }
                }
                tx.Commit();
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            catch (System.Data.SqlClient.SqlException E)
            {
                tx.Rollback();
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                throw new Exception(E.Message);
            }

        }
        /// <summary>
        /// 执行带一个存储过程参数的的SQL语句。
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <param name="content">参数内容,比如一个字段是格式复杂的文章，有特殊符号，可以通过这个方式添加</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSql(string SQLString, string content)
        {
            dbsqlHelper.MSSqlConnect Connect = dbsqlHelper.MSSqlConnect.getInstance();
            SqlConnection connection = Connect.getSystemConnect();

            SqlCommand cmd = new SqlCommand(SQLString, connection);
            System.Data.SqlClient.SqlParameter myParameter = new System.Data.SqlClient.SqlParameter("@content", SqlDbType.NText);
            myParameter.Value = content;
            cmd.Parameters.Add(myParameter);
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                int rows = cmd.ExecuteNonQuery();
                return rows;
            }
            catch (System.Data.SqlClient.SqlException E)
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                throw new Exception(E.Message);
            }
            finally
            {
                cmd.Dispose();
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }

        }
        /// <summary>
        /// 向数据库里插入图像格式的字段(和上面情况类似的另一种实例)
        /// </summary>
        /// <param name="strSQL">SQL语句</param>
        /// <param name="fs">图像字节,数据库的字段类型为image的情况</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSqlInsertImg(string strSQL, byte[] fs)
        {
            dbsqlHelper.MSSqlConnect Connect = dbsqlHelper.MSSqlConnect.getInstance();
            SqlConnection connection = Connect.getSystemConnect();

            SqlCommand cmd = new SqlCommand(strSQL, connection);
            System.Data.SqlClient.SqlParameter myParameter = new System.Data.SqlClient.SqlParameter("@fs", SqlDbType.Image);
            myParameter.Value = fs;
            cmd.Parameters.Add(myParameter);
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                int rows = cmd.ExecuteNonQuery();
                return rows;
            }
            catch (System.Data.SqlClient.SqlException E)
            {

                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                throw new Exception(E.Message);
            }
            finally
            {
                cmd.Dispose();
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }

        }

        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="SQLString">计算查询结果语句</param>
        /// <returns>查询结果（object）</returns>
        public static object GetSingle(string SQLString)
        {
            dbsqlHelper.MSSqlConnect Connect = dbsqlHelper.MSSqlConnect.getInstance();
            SqlConnection connection = Connect.getSystemConnect();

            using (SqlCommand cmd = new SqlCommand(SQLString, connection))
            {
                try
                {
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    object obj = cmd.ExecuteScalar();
                    if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                    {
                        return null;
                    }
                    else
                    {
                        return obj;
                    }
                }
                catch (System.Data.SqlClient.SqlException e)
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                    throw new Exception(e.Message);
                }
            }


        }
        /// <summary>
        /// 执行查询语句，返回SqlDataReader
        /// </summary>
        /// <param name="strSQL">查询语句</param>
        /// <returns>SqlDataReader</returns>
        public static SqlDataReader ExecuteReader(string strSQL)
        {
            dbsqlHelper.MSSqlConnect Connect = dbsqlHelper.MSSqlConnect.getInstance();
            SqlConnection connection = Connect.getSystemConnect();

            SqlCommand cmd = new SqlCommand(strSQL, connection);
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                SqlDataReader myReader = cmd.ExecuteReader();
                cmd.Dispose();
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                return myReader;
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                throw new Exception(e.Message);
            }

        }
        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataSet</returns>
        public static DataSet Query(string SQLString)
        {
            dbsqlHelper.MSSqlConnect Connect = dbsqlHelper.MSSqlConnect.getInstance();
            SqlConnection connection = Connect.getSystemConnect();

            DataSet ds = new DataSet();
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                SqlDataAdapter command = new SqlDataAdapter(SQLString, connection);
                command.Fill(ds, "ds");
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                throw new Exception(ex.Message);
            }
            return ds;

        }
        #endregion


        #region 额外增加的类型
        /// <summary>
        /// DataReader查询
        /// </summary>
        /// <param name="sql"></param>
        /// <returns> 查找的字符型数据 </returns>
        public static string SqlGetResult(string sql)
        {
            string strTemp = "-1";
            System.Data.SqlClient.SqlDataReader reader = null;

            dbsqlHelper.MSSqlConnect Connect = dbsqlHelper.MSSqlConnect.getInstance();
            SqlConnection connection = Connect.getSystemConnect();


            if (sql != null && sql.Trim() != "")
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                System.Data.SqlClient.SqlCommand cmd = new SqlCommand();//(sql, sqlConnection);
                cmd.Connection = connection;
                cmd.CommandText = sql;
                string strname = connection.Database;
                try
                {
                    strTemp = "";
                    reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        strTemp = reader.GetValue(0).ToString();
                    }
                    reader.Close();
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
                catch
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                    //MessageBox.Show(ex.Message);
                }
            }
            return strTemp;
        }
        /// <summary>
        /// 更新数据集到数据库
        /// </summary>
        /// <param name="strSql">sql语句</param>
        /// <param name="changeDataSet">需要更新的dataset</param>
        /// <param name="strTableName">需要更新的表名</param>
        /// <returns>更新数据提示</returns>
        public static string UpdateDataSet(string strSql, DataSet changeDataSet, string strTableName)
        {
            dbsqlHelper.MSSqlConnect Connect = dbsqlHelper.MSSqlConnect.getInstance();
            SqlConnection connection = Connect.getSystemConnect();

            string errStr = "";
            if (strSql != null && strSql.Trim() != "")
            {
                try
                {
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    System.Data.SqlClient.SqlDataAdapter ap = new SqlDataAdapter(strSql, connection);
                    SqlCommandBuilder cmdbuilder = new SqlCommandBuilder(ap);
                    ap.Update(changeDataSet, strTableName);
                    changeDataSet.AcceptChanges();
                    errStr = "数据更新成功!";
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
                catch (Exception ee)
                {
                    // MessageBox.Show(ee.ToString());
                    errStr = ee.ToString();
                }
            }
            return errStr;
        }
        /// <summary>
        /// 更新表
        /// </summary>
        /// <param name="strSql">sql语句</param>
        /// <param name="changeDatatable">要更新的表</param>
        /// <returns>更新成功提示</returns>
        public static string UpdateDataTable(string strSql, DataTable changeDatatable)
        {
            dbsqlHelper.MSSqlConnect Connect = dbsqlHelper.MSSqlConnect.getInstance();
            SqlConnection connection = Connect.getSystemConnect();

            string errStr = "";
            if (strSql != null && strSql.Trim() != "")
            {
                try
                {
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    System.Data.SqlClient.SqlDataAdapter ap = new SqlDataAdapter(strSql, connection);
                    SqlCommandBuilder cmdbuilder = new SqlCommandBuilder(ap);
                    ap.Update(changeDatatable);
                    changeDatatable.AcceptChanges();
                    errStr = "数据更新成功!";
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
                catch (Exception ee)
                {
                    //MessageBox.Show(ee.Message);
                    errStr = ee.ToString();
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }
            return errStr;
        }
        #endregion

        #endregion

        #region 执行带参数的SQL语句

        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSql(string SQLString, params SqlParameter[] cmdParms)
        {
            dbsqlHelper.MSSqlConnect Connect = dbsqlHelper.MSSqlConnect.getInstance();
            SqlConnection connection = Connect.getSystemConnect();

            using (SqlCommand cmd = new SqlCommand())
            {
                try
                {
                    PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                    int rows = cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    return rows;
                }
                catch (System.Data.SqlClient.SqlException E)
                {
                    throw new Exception(E.Message);
                }
            }

        }


        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">SQL语句的哈希表（key为sql语句，value是该语句的SqlParameter[]）</param>
        public static void ExecuteSqlTran(Hashtable SQLStringList)
        {
            dbsqlHelper.MSSqlConnect Connect = dbsqlHelper.MSSqlConnect.getInstance();
            SqlConnection connection = Connect.getSystemConnect();
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            using (SqlTransaction trans = connection.BeginTransaction())
            {
                SqlCommand cmd = new SqlCommand();
                try
                {
                    //循环
                    foreach (DictionaryEntry myDE in SQLStringList)
                    {
                        string cmdText = myDE.Key.ToString();
                        SqlParameter[] cmdParms = (SqlParameter[])myDE.Value;
                        PrepareCommand(cmd, connection, trans, cmdText, cmdParms);
                        int val = cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();

                        trans.Commit();
                    }
                }
                catch
                {
                    trans.Rollback();
                    throw;
                }
            }

        }


        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="SQLString">计算查询结果语句</param>
        /// <returns>查询结果（object）</returns>
        public static object GetSingle(string SQLString, params SqlParameter[] cmdParms)
        {

            dbsqlHelper.MSSqlConnect Connect = dbsqlHelper.MSSqlConnect.getInstance();
            SqlConnection connection = Connect.getSystemConnect();
            using (SqlCommand cmd = new SqlCommand())
            {
                try
                {
                    PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                    object obj = cmd.ExecuteScalar();
                    cmd.Parameters.Clear();
                    if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                    {
                        return null;
                    }
                    else
                    {
                        return obj;
                    }
                }
                catch (System.Data.SqlClient.SqlException e)
                {
                    throw new Exception(e.Message);
                }
            }

        }

        /// <summary>
        /// 执行查询语句，返回SqlDataReader
        /// </summary>
        /// <param name="strSQL">查询语句</param>
        /// <returns>SqlDataReader</returns>
        public static SqlDataReader ExecuteReader(string SQLString, params SqlParameter[] cmdParms)
        {
            dbsqlHelper.MSSqlConnect Connect = dbsqlHelper.MSSqlConnect.getInstance();
            SqlConnection connection = Connect.getSystemConnect();
            SqlCommand cmd = new SqlCommand();
            try
            {
                PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                SqlDataReader myReader = cmd.ExecuteReader();
                cmd.Parameters.Clear();
                return myReader;
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                throw new Exception(e.Message);
            }

        }

        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataSet</returns>
        public static DataSet Query(string SQLString, params SqlParameter[] cmdParms)
        {
            dbsqlHelper.MSSqlConnect Connect = dbsqlHelper.MSSqlConnect.getInstance();
            SqlConnection connection = Connect.getSystemConnect();
            SqlCommand cmd = new SqlCommand();
            PrepareCommand(cmd, connection, null, SQLString, cmdParms);
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                DataSet ds = new DataSet();
                try
                {
                    da.Fill(ds, "ds");
                    cmd.Parameters.Clear();
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                return ds;
            }

        }


        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, string cmdText, SqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;//cmdType;
            if (cmdParms != null)
            {
                foreach (SqlParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }

        #endregion

        #region 存储过程操作

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>SqlDataReader</returns>
        public static SqlDataReader RunProcedure(string storedProcName, IDataParameter[] parameters)
        {
            dbsqlHelper.MSSqlConnect Connect = dbsqlHelper.MSSqlConnect.getInstance();
            SqlConnection connection = Connect.getSystemConnect();

            SqlDataReader returnReader;
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            SqlCommand command = BuildQueryCommand(connection, storedProcName, parameters);
            command.CommandType = CommandType.StoredProcedure;
            returnReader = command.ExecuteReader();
            return returnReader;
        }


        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <param name="tableName">DataSet结果中的表名</param>
        /// <returns>DataSet</returns>
        public static DataSet RunProcedure(string storedProcName, IDataParameter[] parameters, string tableName)
        {
            dbsqlHelper.MSSqlConnect Connect = dbsqlHelper.MSSqlConnect.getInstance();
            SqlConnection connection = Connect.getSystemConnect();
            DataSet dataSet = new DataSet();
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            SqlDataAdapter sqlDA = new SqlDataAdapter();
            sqlDA.SelectCommand = BuildQueryCommand(connection, storedProcName, parameters);
            sqlDA.Fill(dataSet, tableName);
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
            return dataSet;

        }


        /// <summary>
        /// 构建 SqlCommand 对象(用来返回一个结果集，而不是一个整数值)
        /// </summary>
        /// <param name="connection">数据库连接</param>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>SqlCommand</returns>
        private static SqlCommand BuildQueryCommand(SqlConnection connection, string storedProcName, IDataParameter[] parameters)
        {
            SqlCommand command = new SqlCommand(storedProcName, connection);
            command.CommandType = CommandType.StoredProcedure;
            foreach (SqlParameter parameter in parameters)
            {
                command.Parameters.Add(parameter);
            }
            return command;
        }

        /// <summary>
        /// 执行存储过程，返回影响的行数		
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <param name="rowsAffected">影响的行数</param>
        /// <returns></returns>
        public static int RunProcedure(string storedProcName, IDataParameter[] parameters, out int rowsAffected)
        {
            dbsqlHelper.MSSqlConnect Connect = dbsqlHelper.MSSqlConnect.getInstance();
            SqlConnection connection = Connect.getSystemConnect();

            int result;
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            SqlCommand command = BuildIntCommand(connection, storedProcName, parameters);
            rowsAffected = command.ExecuteNonQuery();
            result = (int)command.Parameters["ReturnValue"].Value;
            //Connection.Close();
            return result;

        }

        /// <summary>
        /// 创建 SqlCommand 对象实例(用来返回一个整数值)	
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>SqlCommand 对象实例</returns>
        private static SqlCommand BuildIntCommand(SqlConnection connection, string storedProcName, IDataParameter[] parameters)
        {
            SqlCommand command = BuildQueryCommand(connection, storedProcName, parameters);
            command.Parameters.Add(new SqlParameter("ReturnValue",
                SqlDbType.Int, 4, ParameterDirection.ReturnValue,
                false, 0, 0, string.Empty, DataRowVersion.Default, null));
            return command;
        }
        #endregion
    }
}
