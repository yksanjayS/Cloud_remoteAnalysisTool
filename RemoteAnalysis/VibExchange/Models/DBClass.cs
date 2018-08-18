using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace VibExchange.Models
{
    public class DBClass : IDisposable
    {
        #region[Private Variable]
        private string _ConnectionString;
        private SqlCommand objSqlCommand;
        private SqlConnection objSqlConnection;
        private DataTable dt = new DataTable();
        private SqlDataAdapter da = new SqlDataAdapter();
        #endregion

        #region[Public Properties]
        private String ConnectionString
        {
            get { return _ConnectionString; }
            set { _ConnectionString = value; }

        }
        #endregion

        public DBClass()
            //: this("Data Source=10.50.250.96;Initial Catalog=Iadept_Cloud;user id=sa;password=abc@123")
            : this(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString())
        {
            SqlConnection.ClearAllPools();
            objSqlConnection = new SqlConnection();
            objSqlConnection.ConnectionString = ConnectionString;
            objSqlCommand = new SqlCommand();
            objSqlCommand.Connection = objSqlConnection;
            objSqlConnection.Open();
        }

        DBClass(string strConnectionString)
        {
            ConnectionString = strConnectionString;
        }

        public void AddParameter(string Name, object Value)
        {
            SqlParameter objSqlParameter = objSqlCommand.CreateParameter();
            objSqlParameter.ParameterName = Name;
            objSqlParameter.Value = Value;
            objSqlCommand.Parameters.Add(objSqlParameter);
        }

        public void AddParameter(string Name, object Value, ParameterDirection SqlParameterDirection)
        {

            SqlParameter objSqlParameter = objSqlCommand.CreateParameter();
            objSqlParameter.ParameterName = Name;
            objSqlParameter.Value = Value;
            objSqlParameter.Direction = SqlParameterDirection;
            objSqlCommand.Parameters.Add(objSqlParameter);
        }

        public void AddParameter(string Name, object Value, SqlDbType dbType)
        {
            SqlParameter objSqlParameter = objSqlCommand.CreateParameter();
            objSqlParameter.ParameterName = Name;
            objSqlParameter.Value = Value;
            objSqlParameter.SqlDbType = dbType;
            objSqlCommand.Parameters.Add(objSqlParameter);
        }

        public void AddParameter(string Name, object Value, SqlDbType dbType, ParameterDirection SqlParameterDirection)
        {

            SqlParameter objSqlParameter = objSqlCommand.CreateParameter();
            objSqlParameter.ParameterName = Name;
            objSqlParameter.Value = Value;
            objSqlParameter.SqlDbType = dbType;
            objSqlParameter.Direction = SqlParameterDirection;
            objSqlCommand.Parameters.Add(objSqlParameter);
        }

        public void AddParameter(string Name, object Value, SqlDbType dbType, ParameterDirection SqlParameterDirection, bool IsNull)
        {

            SqlParameter objSqlParameter = objSqlCommand.CreateParameter();
            objSqlParameter.ParameterName = Name;
            objSqlParameter.Value = Value;
            objSqlParameter.SqlDbType = dbType;
            objSqlParameter.IsNullable = IsNull;
            objSqlParameter.Direction = SqlParameterDirection;
            objSqlCommand.Parameters.Add(objSqlParameter);
        }

        public int ExecuteNonQuery(string Query, CommandType Commandtyp)
        {
            int i = 0;
            try
            {
                objSqlCommand.CommandText = Query;
                objSqlCommand.CommandType = Commandtyp;
                if (objSqlConnection.State == ConnectionState.Closed)
                    objSqlConnection.Open();
                i = objSqlCommand.ExecuteNonQuery();
                
            }
            catch (Exception ex)
            {
                if (objSqlConnection.State == ConnectionState.Open) objSqlConnection.Close();
                throw ex;
            }
            finally
            {
               // objSqlCommand.Parameters.Clear();
                objSqlConnection.Close();
            }

            return i;

        }

        public void ExecuteNonQuery(string Query, CommandType Commandtyp, ParameterDirection parameterdirection)
        {
            try
            {
                objSqlCommand.CommandText = Query;
                objSqlCommand.CommandType = Commandtyp;
                if (objSqlConnection.State == ConnectionState.Closed) objSqlConnection.Open();
                objSqlCommand.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                if (objSqlConnection.State == ConnectionState.Open) objSqlConnection.Close();
                throw ex;
            }
            finally
            {
                objSqlCommand.Parameters.Clear();
                objSqlConnection.Close();
            }
        }
        public string ExecuteNonQuery(string Query, CommandType Commandtyp,string Parameter)
        {
             string ID = string.Empty;  
            try
            {
                objSqlCommand.CommandText = Query;
                objSqlCommand.CommandType = Commandtyp;
                if (objSqlConnection.State == ConnectionState.Closed) objSqlConnection.Open();
                objSqlCommand.ExecuteNonQuery();
                ID = (string)objSqlCommand.Parameters[Parameter].Value;  
            }
            catch (Exception ex)
            {
                if (objSqlConnection.State == ConnectionState.Open) objSqlConnection.Close();
                throw ex;
            }
            finally
            {
                objSqlCommand.Parameters.Clear();
                objSqlConnection.Close();
            }
            return ID;
        }

        public long ExecuteNonQuery(string Query, CommandType Commandtyp, ParameterDirection parameterdirection, string OutPutParameter)
        {
            long ReturnValue;
            try
            {

                objSqlCommand.CommandText = Query;
                objSqlCommand.CommandType = Commandtyp;
                if (objSqlConnection.State == ConnectionState.Closed) objSqlConnection.Open();
                int i = objSqlCommand.ExecuteNonQuery();
                ReturnValue = (!string.IsNullOrEmpty(OutPutParameter)) ? Convert.ToInt64(objSqlCommand.Parameters[OutPutParameter].Value) : 0;
            }
            catch (Exception ex)
            {
                if (objSqlConnection.State == ConnectionState.Open) objSqlConnection.Close();
                throw ex;
            }
            finally
            {
                objSqlCommand.Parameters.Clear();
                objSqlConnection.Close();
            }


            return ReturnValue;
        }

        public DataSet ExecuteDataSet(string Query, CommandType Commandtyp)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlDataAdapter adpt = new SqlDataAdapter();
                objSqlCommand.CommandText = Query;
                objSqlCommand.CommandType = Commandtyp;
                adpt.SelectCommand = objSqlCommand;
                if (objSqlConnection.State == ConnectionState.Closed) objSqlConnection.Open();
                adpt.Fill(ds);

            }
            catch (Exception ex)
            {
                if (objSqlConnection.State == ConnectionState.Open) objSqlConnection.Close();
                throw ex;
            }
            finally
            {
                objSqlCommand.Parameters.Clear();
                objSqlConnection.Close();
            }

            return ds;

        }

        public SqlDataReader ExecuteDataReader(string Query, CommandType Commandtyp)
        {
            SqlDataReader rdr = null;
            try
            {
                objSqlCommand.CommandText = Query;
                objSqlCommand.CommandType = Commandtyp;
                rdr = objSqlCommand.ExecuteReader();
            }
            catch (Exception ex)
            {
                objSqlConnection.Close();
                throw ex;
            }
            finally
            {
                //objSqlCommand.Parameters.Clear();
                //objSqlConnection.Close();
            }
            return rdr;
        }

        public object ExecuteScalar(string Query, CommandType Commandtyp)
        {
            object objValue;
            try
            {
                objSqlCommand.CommandText = Query;
                objSqlCommand.CommandType = Commandtyp;
                if (objSqlConnection.State == ConnectionState.Closed) objSqlConnection.Open();
                objValue = objSqlCommand.ExecuteScalar();

            }
            catch (Exception ex)
            {
                if (objSqlConnection.State == ConnectionState.Open) objSqlConnection.Close();
                throw ex;
            }
            finally
            {
                objSqlCommand.Parameters.Clear();
                objSqlConnection.Close();
            }

            return objValue;

        }

        public DataTable getData(string Query, CommandType Commandtyp)
        {
            try
            {
                dt.Clear();
                objSqlCommand.CommandText = Query;
                objSqlCommand.CommandType = Commandtyp;
                if (objSqlConnection.State == ConnectionState.Closed) objSqlConnection.Open();
                da.SelectCommand = objSqlCommand;
                da.Fill(dt);
                objSqlConnection.Close();

            }
            catch (Exception ex)
            {
                if (objSqlConnection.State == ConnectionState.Open) objSqlConnection.Close();
                throw ex;
            }
            finally
            {
                objSqlCommand.Parameters.Clear();
                objSqlConnection.Close();
            }
            return dt;
        }

        public Boolean getStatus(string Query, CommandType Commandtyp)
        {
            bool status = false;
            try
            {
                objSqlCommand.CommandText = Query;
                objSqlCommand.CommandType = Commandtyp;
                if (objSqlConnection.State == ConnectionState.Closed) objSqlConnection.Open();
                da.SelectCommand = objSqlCommand;
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                { status = true; }
                objSqlConnection.Close();

            }
            catch (Exception ex)
            {
                if (objSqlConnection.State == ConnectionState.Open) objSqlConnection.Close();
                throw ex;
            }
            finally
            {
                objSqlCommand.Parameters.Clear();
                objSqlConnection.Close();
            }
            return status;
        }

        public string getUserRole(string UserName)
        {
            AddParameter("@username", UserName);
            string userrole = null;
            DataTable dt = getData("getUserRole", CommandType.StoredProcedure);
            if (dt.Rows.Count > 0)
            {
                userrole = Convert.ToString(dt.Rows[0]["UserRole"]);
            }
            return userrole;

        }
        public string getPointID(int Fileid,string Userid)
        {
            AddParameter("@Userid", Userid);
            AddParameter("@Fileid", Fileid);
            string userrole = null;
            DataTable dt = getData("Select Pointid from tblFileMaster where Fileid = " + Fileid + " and Userid = '" + Userid + "'", CommandType.Text);
            if (dt.Rows.Count > 0)
            {
                userrole = Convert.ToString(dt.Rows[0]["Pointid"]);
            }
            return userrole;

        }

        public string GetDateTime()
        {
            string ss = "";
            ss = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
            return ss;
        }

        public void Dispose()
        {
            objSqlConnection.Dispose();
            objSqlConnection = null;
            objSqlCommand.Dispose();
        }
    }
}