using System;
using System.Data;
using System.Data.SqlClient;

namespace Models
{
    public class ConnClass
    {
        private SqlConnection con;
        public SqlCommand cmd;
        private SqlDataAdapter adpt;
        private DataTable dt;

        //public ConnClass(int UseThisDB)
        //{

        //}

        public void ConnectSQL(int UseThisDB)
        {
#if DEBUG
            switch (UseThisDB)
            {

            }
#else

            switch (UseThisDB)
            {
                case 0: //My local DB
                    con = new SqlConnection("");
                    con.Open();
                    break;
                case 1: //2LT
                    con = new SqlConnection("");
                    con.Open();
                    break;
                case 2: //2ELVIS
                    con = new SqlConnection("");
                    con.Open();
                    break;
                case 3:
                    con = new SqlConnection("");
                    con.Open();
                    break;
                case 4:
                    con = new SqlConnection("");
                    con.Open();
                    break;
                case 5:
                    con = new SqlConnection("");
                    con.Open();
                    break;
                case 6: //2Caster
                    con = new SqlConnection("");
                    con.Open();
                    break;
                case 7:
                    con = new SqlConnection("");
                    con.Open();
                    break;
                case 8:
                    con = new SqlConnection("");
                    con.Open();
                    break;
                case 9:
                    con = new SqlConnection("");
                    con.Open();
                    break;
            }

#endif
        }

        public void SqlQuery(string stringQuery, int UseThisDB)
        {
            ConnectSQL(UseThisDB);
            cmd = new SqlCommand(stringQuery, con);
        }

        public DataTable ExecuteQuery()
        {
            adpt = new SqlDataAdapter(cmd);
            dt = new DataTable();
            adpt.Fill(dt);
            ConDispose();
            return dt;
        }

        public void ExecuteNonQuery()
        {
            cmd.ExecuteNonQuery();
            ConDispose();
        }

        public string ExecuteScalar()
        {
            string ReturnId = cmd.ExecuteScalar().ToString();
            ConDispose();
            return ReturnId;
        }

        public long GetRowCount()
        {
            adpt = new SqlDataAdapter(cmd);
            dt = new DataTable();
            adpt.Fill(dt);
            int RowCount = dt.Rows.Count;
            ConDispose();
            return RowCount;
        }

        public decimal ReturnDecimalValue()
        {
            decimal ReturnValue;
            adpt = new SqlDataAdapter(cmd);
            dt = new DataTable();
            adpt.Fill(dt);
            ReturnValue = Convert.ToDecimal(ExecuteQuery());
            return ReturnValue;
        }

        public void ConDispose()
        {
            con.Close();
            con.Dispose();
        }

    }
}
