using System.Data.SqlClient;

namespace dbsqlHelper
{
    public class MSSqlConnect
    {


        private static MSSqlConnect _instance;
        private static SqlConnection SystemConnect;
        private static SqlConnection ProjConnect;
        private string _ServerName;
        private string _ProjDBName;



        #region class member
        public string ServerName
        {
            get
            {
                return _ServerName;
            }
            set
            {
                this._ServerName = value;
            }
        }

        public string ProjDBName
        {
            get
            {
                return _ProjDBName;
            }
            set
            {
                this._ProjDBName = value;
            }
        }
        #endregion

        #region class method




        public SqlConnection getSystemConnect()
        {
            if (SystemConnect == null)
            {
                SystemConnect = new SqlConnection(getSystemString());
            }
            return SystemConnect;
        }

        public SqlConnection getProjConnect()
        {
            if (ProjConnect == null)
            {
                ProjConnect = new SqlConnection(getProjString());
            }
            return ProjConnect;
        }

        public SqlConnection refreshSysconnect(string SysID)
        {
            this._ServerName = SysID;
            if (SystemConnect != null)
            {
                SystemConnect.Dispose();
            }
            return SystemConnect = new SqlConnection(getSystemString());
        }


        public SqlConnection refreshProjconnect(string ProjID)
        {
            this._ProjDBName = ProjID;
            if (ProjConnect != null)
            {
                ProjConnect.Dispose();
            }
            return ProjConnect = new SqlConnection(getProjString());
        }


        private string getSystemString()
        {
            return "packet size=4096;user id=reco;password=Des_Reco_2006;data source=" + _ServerName + ";persist security info=false;initial catalog=RecoData2011";
        }
        private string getProjString()
        {
            return "packet size=4096;user id=reco;password=Des_Reco_2006;data source=" + _ServerName + ";persist security info=false;initial catalog=" + _ProjDBName;
        }

        #endregion

        #region Single method

        public static MSSqlConnect getInstance()
        {
            if (_instance == null)
            {
                _instance = new MSSqlConnect();
            }
            return _instance;
        }

        #endregion





    }
}
