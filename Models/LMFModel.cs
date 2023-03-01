using System;
using System.Data;

namespace Models
{
    public class LMFModel
    {
        ConnClass conn = new ConnClass();
        Level1Model L1MDL = new Level1Model();

        HeatInfoModel heatinfo = new HeatInfoModel();
        public string LMFVisitStart(int VisitID)
        {
            string VisitStart = "";
            DateTime dtVisitStart;
            DataTable oDt = new DataTable();
            conn = new ConnClass();
            conn.SqlQuery("SELECT VisitTimeDate FROM LMF_Visits WHERE LMFVisitID = @vst", 9);
            conn.cmd.Parameters.AddWithValue("vst", VisitID);
            oDt = conn.ExecuteQuery();
            conn.cmd.Parameters.Clear();
            if(oDt.Rows.Count > 0)
            {
                dtVisitStart = Convert.ToDateTime(oDt.Rows[0]["VisitTimeDate"]);
                VisitStart = dtVisitStart.ToString("yyyy-MM-dd HH:mm:ss");
            }
            return VisitStart;
        }

        public DataTable GetHeatInfo(int LocID)
        {
            if (LocID == 0)
            {
                LocID = 4;
            }
            DataTable oDt = new DataTable();
            conn = new ConnClass();
            conn.SqlQuery("SELECT grd.GradeName,vst.Ladle,hts.HeatNumber,vst.LMFVisitID,hts.HeatID,vst.VisitTimeDate,hts.GradeID " +
                          "FROM LMF_Visits vst " +
                          "INNER JOIN HTS_Heats hts ON hts.HeatID = vst.HeatID " +
                          "INNER JOIN GRD_Grade grd ON grd.GradeID = hts.GradeID " +
                          "WHERE vst.LocID = @locid AND vst.CompletedDate IS NULL", 9);
            conn.cmd.Parameters.AddWithValue("locid", LocID);
            oDt = conn.ExecuteQuery();
            conn.cmd.Parameters.Clear();
            return oDt;
        }

        public string HeatNumber(int LocID)
        {
            string HeatNumber;
            if (LocID == 0)
            {
                LocID = 4;
            }
            DataTable oDt = new DataTable();
            conn = new ConnClass();
            conn.SqlQuery("SELECT grd.GradeName,vst.Ladle,hts.HeatNumber " +
                          "FROM LMF_Visits vst " +
                          "INNER JOIN HTS_Heats hts ON hts.HeatID = vst.HeatID " +
                          "INNER JOIN GRD_Grade grd ON grd.GradeID = hts.GradeID " +
                          "WHERE vst.LocID = @locid AND vst.CompletedDate IS NULL", 9);
            conn.cmd.Parameters.AddWithValue("locid", LocID);
            oDt = conn.ExecuteQuery();
            conn.cmd.Parameters.Clear();
            if (oDt.Rows.Count > 0)
            {
                HeatNumber = "Heat - " + Convert.ToString(oDt.Rows[0]["HeatNumber"]);
            }
            else
            {
                HeatNumber = "Heat - ";
            }

            return HeatNumber;
        }

        public string LastTemp(int VisitID)
        {
            string LastTemp = "";
            DataTable oDt = new DataTable();
            conn = new ConnClass();
            conn.SqlQuery("SELECT TOP(1) Temp,TempDate FROM LMF_Temps WHERE VistitID = @vst ORDER BY LMFTempID DESC", 9);
            conn.cmd.Parameters.AddWithValue("vst", VisitID);
            oDt = conn.ExecuteQuery();
            conn.cmd.Parameters.Clear();
            if (oDt.Rows.Count > 0)
            {
                DateTime TempDate = Convert.ToDateTime(oDt.Rows[0]["TempDate"]);
                LastTemp = "Last Temp - " + Convert.ToString(oDt.Rows[0]["Temp"]) + " @ " + TempDate.ToString("yyyy-MM-dd HH:mm");
            }
            else
            {
                LastTemp = "Temp - ";
            }
            return LastTemp;
        }

        public DataTable GetTemps(int VisitID)
        {
            DataTable oDt = new DataTable();
            conn = new ConnClass();
            conn.SqlQuery("SELECT Temp FROM LMF_Temps WHERE VistitID = @vst ORDER BY LMFTempID ASC", 9);
            conn.cmd.Parameters.AddWithValue("vst", VisitID);
            oDt = conn.ExecuteQuery();
            conn.cmd.Parameters.Clear();
            return oDt;
        }

        public DataTable GetTempTime(int VisitID)
        {
            DataTable oDt = new DataTable();
            conn = new ConnClass();
            conn.SqlQuery("SELECT TempDate,Temp FROM LMF_Temps WHERE VistitID = @vst ORDER BY LMFTempID ASC", 9);
            conn.cmd.Parameters.AddWithValue("vst", VisitID);
            oDt = conn.ExecuteQuery();
            conn.cmd.Parameters.Clear();
            return oDt;
        }

        public string GetLiquidous(int HeatID)
        {
            int ChemMaster = heatinfo.GetChemMasterID(HeatID);
            string Liquidous;
            DataTable oDt = new DataTable();
            conn = new ConnClass();
            conn.SqlQuery("SELECT Liquidous FROM LMF_Liquidous WHERE ChemMasterID = @chm", 9);
            conn.cmd.Parameters.AddWithValue("chm", ChemMaster);
            oDt = conn.ExecuteQuery();
            conn.cmd.Parameters.Clear();
            if(oDt.Rows.Count > 0)
            {
                Liquidous = "Liquidous - " + Convert.ToString(oDt.Rows[0]["Liquidous"]) + " F ";
            }
            else
            {
                Liquidous = "Liquidous - ";
            }
            return Liquidous;
        }

        public int intGetLiquidous(int HeatID)
        {
            int Liquid = 0;
            int ChemMaster = heatinfo.GetChemMasterID(HeatID);
            DataTable oDt = new DataTable();
            conn = new ConnClass();
            conn.SqlQuery("SELECT Liquidous FROM LMF_Liquidous WHERE ChemMasterID = @chm", 9);
            conn.cmd.Parameters.AddWithValue("chm", ChemMaster);
            oDt = conn.ExecuteQuery();
            conn.cmd.Parameters.Clear();
            if(oDt.Rows.Count > 0)
            {
                Liquid = Convert.ToInt32(oDt.Rows[0]["Liquidous"]);
            }
            return Liquid;
        }
        public int GetShipTemp(int HeatID)
        {
            int ShipTemp = 0;
            int Liquidous = Convert.ToInt32(intGetLiquidous(HeatID));
            int GradeID = GetGradeID(HeatID);
            DataTable oDt = new DataTable();
            conn = new ConnClass();
            conn.SqlQuery("SELECT ShipTemp FROM LMF_ShipTemp WHERE GradeID = @grd", 9);
            conn.cmd.Parameters.AddWithValue("grd", GradeID);
            oDt = conn.ExecuteQuery();
            conn.cmd.Parameters.Clear();
            ShipTemp = Liquidous + Convert.ToInt32(oDt.Rows[0]["ShipTemp"]);
            return ShipTemp;
        }
        public string GetLadleWeight(int VisitID)
        {
            string LadleWeight = "0";
            DataTable oDt = new DataTable();
            conn = new ConnClass();
            conn.SqlQuery("SELECT TOP(1) LadleWeight FROM LMF_Addition_Drop_Master WHERE VisitID = @vst ORDER BY MasterDropID DESC", 9);
            conn.cmd.Parameters.AddWithValue("vst", VisitID);
            oDt = conn.ExecuteQuery();
            conn.cmd.Parameters.Clear();
            if(oDt.Rows.Count > 0)
            {
                LadleWeight = "Steel Weight - " + Convert.ToString(oDt.Rows[0]["LadleWeight"]);
            }
            else
            {
                LadleWeight = "Steel Weight -";
            }
            return LadleWeight;
        }

        public string GetPowerOnTime(int VisitID)
        {
            string PowerOnTime = "";
            DateTime PowerStart;
            DateTime PowerOff;
            int PowerInSec = 0;
            TimeSpan powerSpan;
            DataTable oDt = new DataTable();
            conn = new ConnClass();
            conn.SqlQuery("SELECT PowerOnStart, PowerOff FROM LMF_PowerOn WHERE VisitID = @vst ORDER BY PowerOnID ASC", 9);
            conn.cmd.Parameters.AddWithValue("vst", VisitID);
            oDt = conn.ExecuteQuery();
            conn.cmd.Parameters.Clear();
            if(oDt.Rows.Count > 0)
            {
                foreach(DataRow dRow in oDt.Rows)
                {
                    PowerStart = Convert.ToDateTime(dRow["PowerOnStart"]);
                    if(Convert.ToString(dRow["PowerOff"]) != "")
                    {
                        PowerOff = Convert.ToDateTime(dRow["PowerOff"]);
                    }
                    else
                    {
                        PowerOff = DateTime.Now;
                    }

                    powerSpan = PowerOff - PowerStart;

                    PowerInSec = Convert.ToInt32(PowerInSec + powerSpan.TotalSeconds);
                }
                TimeSpan TotalSeconds = TimeSpan.FromSeconds(PowerInSec);

                PowerOnTime = "Power On - " + TotalSeconds.ToString(@"hh\:mm\:ss");
            }
            else
            {
                PowerOnTime = "Power On - ";
            }
            return PowerOnTime;
        }

        public string GetGasOnTime(int VisitID)
        {
            string GasOnTime = "";
            DataTable oDt = new DataTable();
            DateTime GasStart;
            DateTime GasOff;
            int GasInSec = 0;
            TimeSpan gasSpan;
            conn = new ConnClass();
            conn.SqlQuery("SELECT StirStart,StirEnd FROM LMF_Gas_Stir WHERE VisitID = @vst ORDER BY GasStirID ASC", 9);
            conn.cmd.Parameters.AddWithValue("vst", VisitID);
            oDt = conn.ExecuteQuery();
            conn.cmd.Parameters.Clear();
            if(oDt.Rows.Count > 0)
            {
                foreach(DataRow dRow in oDt.Rows)
                {
                    GasStart = Convert.ToDateTime(dRow["StirStart"]);
                    if (Convert.ToString(dRow["StirEnd"]) != "")
                    {
                        GasOff = Convert.ToDateTime(dRow["StirEnd"]);
                    }
                    else
                    {
                        GasOff = DateTime.Now;
                    }
                    gasSpan = GasOff - GasStart;

                    GasInSec = Convert.ToInt32(GasInSec + gasSpan.TotalSeconds);
                }

                TimeSpan totalSec = TimeSpan.FromSeconds(GasInSec);

                GasOnTime = "Gas Stir On - " + totalSec.ToString(@"hh\:mm\:ss");
            }
            else
            {
                GasOnTime = "Gas Stir On -";
            }
            return GasOnTime;
        }
        private int GetLoc(int VisitID)
        {
            int LocID;
            DataTable oDt = new DataTable();
            conn = new ConnClass();
            conn.SqlQuery("SELECT LocID FROM LMF_Visits WHERE LMFVisitID = @vst", 9);
            conn.cmd.Parameters.AddWithValue("vst", VisitID);
            oDt = conn.ExecuteQuery();
            conn.cmd.Parameters.Clear();
            LocID = Convert.ToInt32(oDt.Rows[0]["LocID"]);
            return LocID;
        }

        public DataTable GetLMFVisitID(int LocID)
        {
            DataTable oDt = new DataTable();
            conn = new ConnClass();
            conn.SqlQuery("SELECT LMFVisitID,HeatID FROM LMF_Visits WHERE LocID = @loc AND CompletedDate IS NULL", 9);
            conn.cmd.Parameters.AddWithValue("loc", LocID);
            oDt = conn.ExecuteQuery();
            conn.cmd.Parameters.Clear();
            return oDt;
        }

        public bool HasJominy(int HeatID)
        {
            bool HasJominy = false;
            int GradeID = GetGradeID(HeatID);
            DataTable oDt = new DataTable();
            conn = new ConnClass();
            conn.SqlQuery("SELECT JominyPointID FROM GRD_JominyPoint WHERE GradeID = @grd", 9);
            conn.cmd.Parameters.AddWithValue("grd", GradeID);
            oDt = conn.ExecuteQuery();
            conn.cmd.Parameters.Clear();
            if(oDt.Rows.Count > 0)
            {
                HasJominy = true;
            }
            return HasJominy;
        }

        public int GetGradeID(int HeatID)
        {
            int GradeID;
            DataTable oDt = new DataTable();
            conn = new ConnClass();
            conn.SqlQuery("SELECT GradeID FROM HTS_Heats WHERE HeatID = @ht", 9);
            conn.cmd.Parameters.AddWithValue("ht", HeatID);
            oDt = conn.ExecuteQuery();
            conn.cmd.Parameters.Clear();
            GradeID = Convert.ToInt32(oDt.Rows[0]["GradeID"]);
            return GradeID;
        }

        public bool IsJominyOut(int HeatID)
        {
            bool IsOut = false;
            int ChemMaster = ChemMasterId(HeatID);
            if(ChemMaster > 0)
            {
                DataTable oDt = new DataTable();
                conn = new ConnClass();
                conn.SqlQuery("SELECT JominyOutID FROM HTS_JominyOut WHERE ChemMasterID = @chm", 9);
                conn.cmd.Parameters.AddWithValue("chm", ChemMaster);
                oDt = conn.ExecuteQuery();
                conn.cmd.Parameters.Clear();
                if(oDt.Rows.Count > 0)
                {
                    IsOut = true;
                }
            }
            return IsOut;
        }

        private int ChemMasterId(int HeatID)
        {
            int ChemMaster = 0;
            DataTable oDt = new DataTable();
            conn = new ConnClass();
            conn.SqlQuery("SELECT TOP(1)HeatMasterID FROM HTS_Chem_Master WHERE HeatID = @ht ORDER BY HeatMasterID DESC", 9);
            conn.cmd.Parameters.AddWithValue("ht", HeatID);
            oDt = conn.ExecuteQuery();
            conn.cmd.Parameters.Clear();
            if(oDt.Rows.Count > 0)
            {
                ChemMaster = Convert.ToInt32(oDt.Rows[0]["HeatMasterID"]);
            }
            return ChemMaster;
        }

        public string GetKWH(int VisitID)
        {
            string KWH = "KWH -";
            DataTable oDt = new DataTable();
            conn = new ConnClass();
            conn.SqlQuery("SELECT TOP(1)KWH FROM LMF_PowerOn WHERE VisitID = @vst ORDER BY PowerOnID DESC", 9);
            conn.cmd.Parameters.AddWithValue("vst", VisitID);
            oDt = conn.ExecuteQuery();
            conn.cmd.Parameters.Clear();
            if(oDt.Rows.Count > 0)
            {
                KWH = "KWH - " + Convert.ToString(oDt.Rows[0]["KWH"]);
            }
            return KWH;
        }
    }
}
