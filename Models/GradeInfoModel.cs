using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Models
{
    public class GradeInfoModel
    {
        ConnClass conn = new ConnClass();

        public int GetGradeDecimal(int ElmID)
        {
            int DecimalPlaces;
            DataTable oDt = new DataTable();
            conn = new ConnClass();
            conn.SqlQuery("SELECT DecimialPlaces FROM CHM_Elements WHERE ElementID = @elm", 9);
            conn.cmd.Parameters.AddWithValue("elm", ElmID);
            oDt = conn.ExecuteQuery();
            conn.cmd.Parameters.Clear();
            DecimalPlaces = Convert.ToInt32(oDt.Rows[0]["DecimialPlaces"]);
            return DecimalPlaces;
        }

        public DataTable GetGradeMMA(int GradeID, int ElmID)
        {
            DataTable oDt = new DataTable();
            conn = new ConnClass();
            conn.SqlQuery("SELECT Min,Max,Aim FROM GRD_Chem WHERE GradeID = @grd AND ElementID = @elm", 9);
            conn.cmd.Parameters.AddWithValue("grd", GradeID);
            conn.cmd.Parameters.AddWithValue("elm", ElmID);
            oDt = conn.ExecuteQuery();
            conn.cmd.Parameters.Clear();
            return oDt;
        }

        public decimal GetGradeMin(int GradeID, int ElmID)
        {
            decimal GradeMin = 0;
            DataTable oDt = new DataTable();
            conn = new ConnClass();
            conn.SqlQuery("SELECT Min FROM GRD_Chem WHERE GradeID = @grd AND ElementID = @elm", 9);
            conn.cmd.Parameters.AddWithValue("grd", GradeID);
            conn.cmd.Parameters.AddWithValue("elm", ElmID);
            oDt = conn.ExecuteQuery();
            conn.cmd.Parameters.Clear();
            if(oDt.Rows.Count > 0)
            {
                if(Convert.ToString(oDt.Rows[0]["Min"]) != "")
                {
                    GradeMin = Convert.ToDecimal(oDt.Rows[0]["Min"]);
                }               
            }
            return GradeMin;
        }

        public decimal GetGradeMax(int GradeID, int ElmID)
        {
            decimal GradeMax = 0;
            DataTable oDt = new DataTable();
            conn = new ConnClass();
            conn.SqlQuery("SELECT Max FROM GRD_Chem WHERE GradeID = @grd AND ElementID = @elm", 9);
            conn.cmd.Parameters.AddWithValue("grd", GradeID);
            conn.cmd.Parameters.AddWithValue("elm", ElmID);
            oDt = conn.ExecuteQuery();
            conn.cmd.Parameters.Clear();
            if (oDt.Rows.Count > 0)
            {
                if(Convert.ToString(oDt.Rows[0]["Max"]) != "")
                {
                    GradeMax = Convert.ToDecimal(oDt.Rows[0]["Max"]);
                }                
            }
            return GradeMax;
        }
    }
}
