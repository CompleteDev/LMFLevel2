using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Models
{
    public class HeatInfoModel
    {
        ConnClass conn = new ConnClass();

        

        public DataTable GetLastChem(int HeatID)
        {
            DataTable oDt = new DataTable();
            int ChemMasterID =  GetChemMasterID(HeatID);
            conn = new ConnClass();
            conn.SqlQuery("SELECT elm.Element,chm.Results,gchm.Min, gchm.Aim,gchm.Max,hts.GradeID " +
                          "FROM HTS_Chem_Master mst " +
                          "INNER JOIN HTS_Chem chm ON chm.ChemMasterID = mst.HeatMasterID " +
                          "INNER JOIN CHM_Elements elm ON elm.ElementID = chm.ElemID " +
                          "INNER JOIN HTS_Heats hts ON hts.HeatID = mst.HeatID " +
                          "INNER JOIN GRD_Chem gchm ON gchm.GradeID = hts.GradeID AND gchm.ElementID = chm.ElemID " +
                          "WHERE mst.HeatMasterID = @mstr ORDER BY elm.DisplayPosition ASC", 9);
            conn.cmd.Parameters.AddWithValue("mstr", ChemMasterID);
            oDt = conn.ExecuteQuery();
            conn.cmd.Parameters.Clear();
            return oDt;
        }

        public DataTable GetChemByElm(int HeatID, int ElmID)
        {
            DataTable oDt = new DataTable();
            int ChemMasterID = GetChemMasterID(HeatID);
            conn = new ConnClass();
            conn.SqlQuery("SELECT elm.Element,chm.Results " +
                          "FROM HTS_Chem chm " +
                          "INNER JOIN CHM_Elements elm ON elm.ElementID = chm.ElemID " +
                          "WHERE chm.ChemMasterID = @chm AND chm.ElemID = @elm", 9);
            conn.cmd.Parameters.AddWithValue("chm", ChemMasterID);
            conn.cmd.Parameters.AddWithValue("elm", ElmID);
            oDt = conn.ExecuteQuery();
            conn.cmd.Parameters.Clear();
            return oDt;
        }
        public DataTable ChemDisplay()
        {
            DataTable oDt = new DataTable();
            conn = new ConnClass();
            conn.SqlQuery("SELECT dsp.ElmID, elm.Element " +
                          "FROM LMF_ChemDisplay dsp " +
                          "INNER JOIN CHM_Elements elm ON elm.ElementID = dsp.ElmID " +
                          "ORDER BY OrderByThis ASC", 9);
            oDt = conn.ExecuteQuery();
            return oDt;
        }

        public int GetChemMasterID(int HeatID)
        {
            int ChemMasterID = 0;
            DataTable oDt = new DataTable();
            conn = new ConnClass();
            conn.SqlQuery("SELECT TOP(1)HeatMasterID FROM HTS_Chem_Master WHERE HeatID = @ht ORDER BY HeatMasterID DESC", 9);
            conn.cmd.Parameters.AddWithValue("ht", HeatID);
            oDt = conn.ExecuteQuery();
            conn.cmd.Parameters.Clear();
            if(oDt.Rows.Count > 0)
            {
                ChemMasterID = Convert.ToInt32(oDt.Rows[0]["HeatMasterID"]);
            }
            return ChemMasterID;
        }

        public string GetLastChemTime(int HeatID)
        {
            string LastChem = "";
            DataTable oDt = new DataTable();
            conn = new ConnClass();
            conn.SqlQuery("SELECT ChemDate FROM HTS_Chem_Master WHERE HeatID = @ht ORDER BY HeatMasterID DESC", 9);
            conn.cmd.Parameters.AddWithValue("ht", HeatID);
            oDt = conn.ExecuteQuery();
            if(oDt.Rows.Count > 0)
            {
                DateTime chmDt = Convert.ToDateTime(oDt.Rows[0]["ChemDate"]);
                LastChem = chmDt.ToString("HH:mm:ss yyyy-MM-dd");
            }
            return LastChem;
        }
        
        public DataTable GetAdditions(int VisitID)
        {
            DataTable oDt = new DataTable();

            return oDt;
        }
    }
}
