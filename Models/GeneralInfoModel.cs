using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Models
{
    public class GeneralInfoModel
    {
        ConnClass conn = new ConnClass();

        public DataTable FACInfo(int FACCode)
        {
            DataTable oDt = new DataTable();
            conn = new ConnClass();
            conn.SqlQuery("SELECT FACCODE,HEATID,LADLENUMBER,HEATNUMBER,VISITNUMBER,PROCESSSTEP,GRADECODE,GRADENAME,STARTTIME,HEATSPEC FROM MELTSHOPSTATUS WHERE FACCODE = @fac", 2);
            conn.cmd.Parameters.AddWithValue("fac", FACCode);
            oDt = conn.ExecuteQuery();
            conn.cmd.Parameters.Clear();
            return oDt;
        }

        public int CurrentHeatTemp(int Code, int HeatID)
        {
            int Temp;
            DataTable oDt = new DataTable();
            conn = new ConnClass();
            conn.SqlQuery("SELECT TOP 1 TEMPERATURE FROM HEATTEMPERATURES WHERE HEATID = @htid AND FACCODE = @code ORDER BY RID DESC", 2);
            conn.cmd.Parameters.AddWithValue("code", Code);
            conn.cmd.Parameters.AddWithValue("htid", HeatID);
            oDt = conn.ExecuteQuery();
            conn.cmd.Parameters.Clear();
            if(oDt.Rows.Count > 0)
            {
                Temp = Convert.ToInt32(oDt.Rows[0]["TEMPERATURE"]);
            }
            else
            {
                Temp = 0;
            }
            return Temp;
        }

        public DataTable CasterInfo()
        {
            DataTable oDt = new DataTable();
            conn = new ConnClass();
            conn.SqlQuery("SELECT cs.CastMode,CAST(cs.MinutesUntilClose AS Decimal(18,2)) AS MinTilClose,cs.CastingHeatName,cs.OpenHeatName,cs.CalculatedLadleWeight, " +
                          "cs.TundishWeight,cs.TundishTemperature,cs.TurretLoadHeat,cs.CastLadleWeight,oh.CasterGradeName,oh.LadleName,th.CasterGradeName AS TurretGrade,th.LadleName AS TurretLadle " +
                          "FROM CasterStatus cs " +
                          "INNER JOIN Heat oh ON oh.HeatName = cs.OpenHeatName " +
                          "INNER JOIN Heat th ON th.HeatName = cs.TurretLoadHeat " +
                          "", 6);
            oDt = conn.ExecuteQuery();
            return oDt;
        }

        public DataTable StrandStatus(int Strand)
        {
            DataTable oDt = new DataTable();
            conn = new ConnClass();
            conn.SqlQuery("SELECT StrandMode,CastSpeed FROM StrandStatus WHERE StrandID = @strand", 6);
            conn.cmd.Parameters.AddWithValue("strand", Strand);
            oDt = conn.ExecuteQuery();
            conn.cmd.Parameters.Clear();

            return oDt;
        }

        public int TonsPerHour()
        {
            int CastSpeed = 0;
            DataTable oDt = new DataTable();
            conn = new ConnClass();
            conn.SqlQuery("SELECT CastSpeed FROM StrandStatus", 6);
            oDt = conn.ExecuteQuery();
            int rCount = oDt.Rows.Count;
            if(rCount > 0)
            {
                foreach (DataRow dRow in oDt.Rows)
                {
                    CastSpeed = CastSpeed + Convert.ToInt16(dRow["CastSpeed"]);
                }

                CastSpeed = (((CastSpeed * 147) * 60) / 12) / 2000;
            }
            else
            {
                CastSpeed = 0;
            }
            return CastSpeed;
        }

        public DataTable LMFLocs()
        {
            DataTable oDt = new DataTable();
            conn = new ConnClass();
            conn.SqlQuery("SELECT LocationID,Location FROM MS_Locations WHERE LocationID IN(3,4)", 9);
            oDt = conn.ExecuteQuery();
            return oDt;
        }
    }
}
