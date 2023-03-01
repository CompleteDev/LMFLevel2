using System;
using System.Data;

namespace Models
{
    public class HeatHistoryModel
    {
        ConnClass conn = new ConnClass();

        public DataTable GetHeats()
        {
            DataTable oDt = new DataTable();
            conn = new ConnClass();
            conn.SqlQuery("SELECT HeatID, HeatNumber FROM HTS_Heats ORDER BY HeatID DESC", 9);
            oDt = conn.ExecuteQuery();
            return oDt;
        }

        private int GetVisitID(int HeatID, int Visit)
        {
            int VisitID;
            DataTable oDt = new DataTable();
            conn = new ConnClass();
            conn.SqlQuery("SELECT LMFVisitID FROM LMF_Visits WHERE HeatID = @htid AND Visit = @vst", 9);
            conn.cmd.Parameters.AddWithValue("htid", HeatID);
            conn.cmd.Parameters.AddWithValue("vst", Visit);
            oDt = conn.ExecuteQuery();
            conn.cmd.Parameters.Clear();
            VisitID = Convert.ToInt32(oDt.Rows[0]["LMFVisitID"]);
            return VisitID;
        }

        private int GetLastChemMasterID(int HeatID)
        {
            int ChemMaster = 0;
            DataTable oDt = new DataTable();
            conn.SqlQuery("SELECT TOP(1) HeatMasterID FROM HTS_Chem_Master WHERE HeatID = @ht ORDER BY HeatMasterID DESC", 9);
            conn.cmd.Parameters.AddWithValue("ht", HeatID);
            oDt = conn.ExecuteQuery();
            conn.cmd.Parameters.Clear();
            if(oDt.Rows.Count > 0)
            {
                ChemMaster = Convert.ToInt32(oDt.Rows[0]["HeatMasterID"]);
            }
            return ChemMaster;
        }

        public DataTable GetGeneralInfo(int HeatID, int VisitNumber)
        {
            DataTable oDt = new DataTable();
            conn = new ConnClass();
            conn.SqlQuery("SELECT grd.GradeName,vst.Ladle,vst.VisitTimeDate,vst.CompletedDate " +
                          "FROM HTS_Heats hts " +
                          "INNER JOIN LMF_Visits vst ON vst.HeatID = hts.HeatID " +
                          "INNER JOIN GRD_Grade grd ON grd.GradeID = hts.GradeID " +
                          "WHERE hts.HeatID = @htid AND vst.Visit = @vst", 9);
            conn.cmd.Parameters.AddWithValue("htid", HeatID);
            conn.cmd.Parameters.AddWithValue("vst", VisitNumber);
            oDt = conn.ExecuteQuery();
            conn.cmd.Parameters.Clear();
            return oDt;
        }

        public string GetSteelWeight(int HeatID, int VisitNumber)
        {
            string LadleWeight = "Steel Weight -";
            int VisitID = GetVisitID(HeatID, VisitNumber);
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
                //Get from PLC ladle car scale
            }
            return LadleWeight;
        }

        public string GetLiquidous(int HeatID)
        {
            string Liquidous;
            int ChemMasterID = GetLastChemMasterID(HeatID);
            if(ChemMasterID != 0)
            {
                DataTable oDt = new DataTable();
                conn = new ConnClass();
                conn.SqlQuery("SELECT Liquidous FROM LMF_Liquidous WHERE ChemMasterID = @chm", 9);
                conn.cmd.Parameters.AddWithValue("chm", ChemMasterID);
                oDt = conn.ExecuteQuery();
                conn.cmd.Parameters.Clear();
                if(oDt.Rows.Count > 0)
                {
                    Liquidous = "Liquidous - " + Convert.ToString(oDt.Rows[0]["Liquidous"]);
                }
                else
                {
                    Liquidous = "Liquidous -";
                }               
            }
            else
            {
                Liquidous = "Liquidous -";
            }
            
            return Liquidous;
        }

        public string GetKWH(int HeatID, int VisitNumber)
        {
            string KWH = "";
            int VisitID = GetVisitID(HeatID, VisitNumber);
            DataTable oDt = new DataTable();
            conn = new ConnClass();
            conn.SqlQuery("SELECT TOP(1)KWH FROM LMF_PowerOn WHERE VisitID = @vst ORDER BY PowerOnID DESC", 9);
            conn.cmd.Parameters.AddWithValue("vst", VisitID);
            oDt = conn.ExecuteQuery();
            conn.cmd.Parameters.Clear();
            if(oDt.Rows.Count > 0)
            {
                KWH = Convert.ToString(oDt.Rows[0]["KWH"]);
            }
            return KWH;
        }

        public string GetPowerOnTime(int HeatID, int VisitNumber)
        {
            string PowerOnTime = "";
            int VisitID = GetVisitID(HeatID, 1);
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
            if (oDt.Rows.Count > 0)
            {
                foreach (DataRow dRow in oDt.Rows)
                {
                    PowerStart = Convert.ToDateTime(dRow["PowerOnStart"]);
                    if (Convert.ToString(dRow["PowerOff"]) != "")
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

        public string GetGasOnTime(int HeatID, int VisitNumber)
        {
            string GasOnTime = "";
            int VisitID = GetVisitID(HeatID, VisitNumber);
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
            if (oDt.Rows.Count > 0)
            {
                foreach (DataRow dRow in oDt.Rows)
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

        public DataTable GetHeatTemps(int HeatID)
        {
            DataTable oDt = new DataTable();
            conn = new ConnClass();
            conn.SqlQuery("SELECT tmp.Temp,tmp.TempDate,o.O2PPM,vst.Visit " +
                          "FROM LMF_Temps tmp " +
                          "INNER JOIN LMF_Visits vst ON vst.LMFVisitID = tmp.VistitID " +
                          "LEFT JOIN LMF_O2 o ON o.LMFTempID = tmp.LMFTempID " +
                          "WHERE tmp.VistitID IN (SELECT LMFVisitID FROM LMF_Visits WHERE HeatID = @ht) ORDER BY tmp.LMFTempID ASC", 9);
            conn.cmd.Parameters.AddWithValue("ht", HeatID);
            oDt = conn.ExecuteQuery();
            conn.cmd.Parameters.Clear();
            return oDt;
        }

        public DataTable GetHeatGasStir(int HeatID)
        {
            DataTable oDt = new DataTable();
            conn = new ConnClass();
            conn.SqlQuery("SELECT vst.Visit,StirStart,StirEnd,AVG(prs.SCFM) AS SCFM,AVG(prs.BackPressure) AS BackPres,prs.Plug,typ.GasType " +
                          "FROM LMF_Gas_Stir gas " +
                          "INNER JOIN LMF_Visits vst ON vst.LMFVisitID = gas.VisitID " +
                          "INNER JOIN LMF_Gas_Pressure prs ON prs.GasStirID = gas.GasStirID " +
                          "INNER JOIN LMF_Gas_Type typ ON typ.GasStirTypeID = gas.GasTypeID " +
                          "WHERE gas.VisitID IN (SELECT LMFVisitID FROM LMF_Visits WHERE HeatID = @ht) " +
                          "GROUP BY vst.Visit,StirStart,StirEnd,prs.Plug,typ.GasType,gas.GasStirID " +
                          "ORDER BY gas.GasStirID ASC", 9);
            conn.cmd.Parameters.AddWithValue("ht", HeatID);
            oDt = conn.ExecuteQuery();
            conn.cmd.Parameters.Clear();
            return oDt;
        }

        public DataTable GetHeatEMS(int HeatID)
        {
            DataTable oDt = new DataTable();
            conn = new ConnClass();
            conn.SqlQuery("SELECT ems.StirStart,ems.StirEnd,vst.Visit " +
                          "FROM LMF_EMS ems " +
                          "INNER JOIN LMF_Visits vst ON vst.LMFVisitID = ems.VisitID " +
                          "WHERE VisitID IN (SELECT LMFVisitID FROM LMF_Visits WHERE HeatID = @ht) ORDER BY ems.EMSStirID ASC", 9);
            conn.cmd.Parameters.AddWithValue("ht", HeatID);
            oDt = conn.ExecuteQuery();
            conn.cmd.Parameters.Clear();
            return oDt;
        }

        public DataTable GetHeatAdditions(int HeatID)
        {
            DataTable oDt = new DataTable();
            conn = new ConnClass();
            conn.SqlQuery("SELECT dmt.DropNumber, dmt.DropDateTime,vst.LMFVisitID, aldrp.DropAmount AS Al,cokedrp.DropAmount AS Coke,cudrp.DropAmount AS Cu,febdrp.DropAmount AS FeB, " +
                          "fenbdrp.DropAmount AS FeNb,femodrp.DropAmount AS FeMo,fepdrp.DropAmount AS FeP,fesidrp.DropAmount AS FeSi,fetidrp.DropAmount AS FeTi,fevdrp.DropAmount AS FeV, " +
                          "hcfecrdrp.DropAmount AS HCFeCr,hcfemndrp.DropAmount AS HCFeMn,lcfecrdrp.DropAmount AS LCFeCr,mcfemndrp.DropAmount AS MCFeMn,lowaldrp.DropAmount AS LowAlFeSi, " +
                          "lown2drp.DropAmount AS LowN2Coke,nidrp.DropAmount AS Ni,nifemndrp.DropAmount AS NiFeMn,nvdrp.DropAmount AS NitroVan,simndrp.DropAmount AS SiMn,suldrp.DropAmount AS Sulfur " +
                          "FROM LMF_Addition_Drop_Master dmt " +
                          "INNER JOIN LMF_Visits vst ON vst.LMFVisitID = dmt.VisitID " +
                          "LEFT JOIN LMF_Addition_Drops aldrp ON aldrp.DropMasterID = dmt.MasterDropID AND aldrp.AdditionID = @al " +
                          "LEFT JOIN LMF_Addition_Drops cokedrp ON cokedrp.DropMasterID = dmt.MasterDropID AND cokedrp.AdditionID = @coke " +
                          "LEFT JOIN LMF_Addition_Drops cudrp ON cudrp.DropMasterID = dmt.MasterDropID AND cudrp.AdditionID = @cu " +
                          "LEFT JOIN LMF_Addition_Drops febdrp ON febdrp.DropMasterID = dmt.MasterDropID AND febdrp.AdditionID = @feb " +
                          "LEFT JOIN LMF_Addition_Drops fenbdrp ON fenbdrp.DropMasterID = dmt.MasterDropID AND fenbdrp.AdditionID = @fenb " +
                          "LEFT JOIN LMF_Addition_Drops femodrp ON femodrp.DropMasterID = dmt.MasterDropID AND femodrp.AdditionID = @femo " +
                          "LEFT JOIN LMF_Addition_Drops fepdrp ON fepdrp.DropMasterID = dmt.MasterDropID AND fepdrp.AdditionID = @fep " +
                          "LEFT JOIN LMF_Addition_Drops fesidrp ON fesidrp.DropMasterID = dmt.MasterDropID AND fesidrp.AdditionID = @fesi " +
                          "LEFT JOIN LMF_Addition_Drops fetidrp ON fetidrp.DropMasterID = dmt.MasterDropID AND fetidrp.AdditionID = @feti " +
                          "LEFT JOIN LMF_Addition_Drops fevdrp ON fevdrp.DropMasterID = dmt.MasterDropID AND fevdrp.AdditionID = @fev " +
                          "LEFT JOIN LMF_Addition_Drops hcfecrdrp ON hcfecrdrp.DropMasterID = dmt.MasterDropID AND hcfecrdrp.AdditionID = @hcfecr " +
                          "LEFT JOIN LMF_Addition_Drops hcfemndrp ON hcfemndrp.DropMasterID = dmt.MasterDropID AND hcfemndrp.AdditionID = @hcfemn " +
                          "LEFT JOIN LMF_Addition_Drops lcfecrdrp ON lcfecrdrp.DropMasterID = dmt.MasterDropID AND lcfecrdrp.AdditionID = @lcfecr " +
                          "LEFT JOIN LMF_Addition_Drops mcfemndrp ON mcfemndrp.DropMasterID = dmt.MasterDropID AND mcfemndrp.AdditionID = @mcfemn " +
                          "LEFT JOIN LMF_Addition_Drops lowaldrp ON lowaldrp.DropMasterID = dmt.MasterDropID AND lowaldrp.AdditionID = @lowal " +
                          "LEFT JOIN LMF_Addition_Drops lown2drp ON lown2drp.DropMasterID = dmt.MasterDropID AND lown2drp.AdditionID = @lown2 " +
                          "LEFT JOIN LMF_Addition_Drops nidrp ON nidrp.DropMasterID = dmt.MasterDropID AND nidrp.AdditionID = @ni " +
                          "LEFT JOIN LMF_Addition_Drops nifemndrp ON nifemndrp.DropMasterID = dmt.MasterDropID AND nifemndrp.AdditionID = @nifemn " +
                          "LEFT JOIN LMF_Addition_Drops nvdrp ON nvdrp.DropMasterID = dmt.MasterDropID AND nvdrp.AdditionID = @nv " +
                          "LEFT JOIN LMF_Addition_Drops simndrp ON simndrp.DropMasterID = dmt.MasterDropID AND simndrp.AdditionID = @simn " +
                          "LEFT JOIN LMF_Addition_Drops suldrp ON suldrp.DropMasterID = dmt.MasterDropID AND suldrp.AdditionID = @sul " +
                          "WHERE dmt.MasterDropID IN (SELECT MasterDropID FROM LMF_Addition_Drop_Master WHERE VisitID IN(SELECT LMFVisitID FROM LMF_Visits WHERE HeatID = @htid)) " +
                          "ORDER BY dmt.DropNumber ASC", 9);
            conn.cmd.Parameters.AddWithValue("al", ConstClass.Aluminum);
            conn.cmd.Parameters.AddWithValue("coke", ConstClass.Coke);
            conn.cmd.Parameters.AddWithValue("cu", ConstClass.Copper);
            conn.cmd.Parameters.AddWithValue("feb", ConstClass.FeB);
            conn.cmd.Parameters.AddWithValue("fenb", ConstClass.FeNb);
            conn.cmd.Parameters.AddWithValue("femo", ConstClass.FeMo);
            conn.cmd.Parameters.AddWithValue("fep", ConstClass.FeP);
            conn.cmd.Parameters.AddWithValue("fesi", ConstClass.FeSi);
            conn.cmd.Parameters.AddWithValue("feti", ConstClass.FeTi);
            conn.cmd.Parameters.AddWithValue("fev", ConstClass.FeV);
            conn.cmd.Parameters.AddWithValue("hcfecr", ConstClass.HCFeCr);
            conn.cmd.Parameters.AddWithValue("hcfemn", ConstClass.HCFeMn);
            conn.cmd.Parameters.AddWithValue("lcfecr", ConstClass.LCFeCr);
            conn.cmd.Parameters.AddWithValue("mcfemn", ConstClass.MCFeMn);
            conn.cmd.Parameters.AddWithValue("lowal", ConstClass.LowAlFeSi);
            conn.cmd.Parameters.AddWithValue("lown2", ConstClass.LowN2Coke);
            conn.cmd.Parameters.AddWithValue("ni", ConstClass.Nickel);
            conn.cmd.Parameters.AddWithValue("nifemn", ConstClass.NitridedFeMn);
            conn.cmd.Parameters.AddWithValue("nv", ConstClass.NitroVan);
            conn.cmd.Parameters.AddWithValue("simn", ConstClass.SiMn);
            conn.cmd.Parameters.AddWithValue("sul", ConstClass.Sulfur);
            conn.cmd.Parameters.AddWithValue("htid", HeatID);
            oDt = conn.ExecuteQuery();
            conn.cmd.Parameters.Clear();
            return oDt;
        }

        public DataTable GetActualDrops(int HeatID)
        {
            DataTable oDt = new DataTable();
            //conn = new ConnClass();
            //conn.SqlQuery("SELECT dmt.DropNumber, dmt.DropDateTime,vst.LMFVisitID, aldrp.Weight AS Al,cokedrp.Weight AS Coke,cudrp.Weight AS Cu,febdrp.Weight AS FeB, " +
            //              "fenbdrp.Weight AS FeNb,femodrp.Weight AS FeMo,fepdrp.Weight AS FeP,fesidrp.Weight AS FeSi,fetidrp.Weight AS FeTi,fevdrp.Weight AS FeV, " +
            //              "hcfecrdrp.Weight AS HCFeCr,hcfemndrp.Weight AS HCFeMn,lcfecrdrp.Weight AS LCFeCr,mcfemndrp.Weight AS MCFeMn,lowaldrp.Weight AS LowAlFeSi, " +
            //              "lown2drp.Weight AS LowN2Coke,nidrp.Weight AS Ni,nifemndrp.Weight AS NiFeMn,nvdrp.Weight AS NitroVan,simndrp.Weight AS SiMn,suldrp.Weight AS Sulfur,caaldrp.Weight AS CaAl " +
            //              "FROM LMF_Addition_Drop_Master dmt " +
            //              "INNER JOIN LMF_Visits vst ON vst.LMFVisitID = dmt.VisitID " +
            //              "LEFT JOIN LMF_Actual_Drops aldrp ON aldrp.DropMasterID = dmt.MasterDropID AND aldrp.AdditonID = @al " +
            //              "LEFT JOIN LMF_Actual_Drops cokedrp ON cokedrp.DropMasterID = dmt.MasterDropID AND cokedrp.AdditonID = @coke " +
            //              "LEFT JOIN LMF_Actual_Drops cudrp ON cudrp.DropMasterID = dmt.MasterDropID AND cudrp.AdditonID = @cu " +
            //              "LEFT JOIN LMF_Actual_Drops febdrp ON febdrp.DropMasterID = dmt.MasterDropID AND febdrp.AdditonID = @feb " +
            //              "LEFT JOIN LMF_Actual_Drops fenbdrp ON fenbdrp.DropMasterID = dmt.MasterDropID AND fenbdrp.AdditonID = @fenb " +
            //              "LEFT JOIN LMF_Actual_Drops femodrp ON femodrp.DropMasterID = dmt.MasterDropID AND femodrp.AdditonID = @femo " +
            //              "LEFT JOIN LMF_Actual_Drops fepdrp ON fepdrp.DropMasterID = dmt.MasterDropID AND fepdrp.AdditonID = @fep " +
            //              "LEFT JOIN LMF_Actual_Drops fesidrp ON fesidrp.DropMasterID = dmt.MasterDropID AND fesidrp.AdditonID = @fesi " +
            //              "LEFT JOIN LMF_Actual_Drops fetidrp ON fetidrp.DropMasterID = dmt.MasterDropID AND fetidrp.AdditonID = @feti " +
            //              "LEFT JOIN LMF_Actual_Drops fevdrp ON fevdrp.DropMasterID = dmt.MasterDropID AND fevdrp.AdditonID = @fev " +
            //              "LEFT JOIN LMF_Actual_Drops hcfecrdrp ON hcfecrdrp.DropMasterID = dmt.MasterDropID AND hcfecrdrp.AdditonID = @hcfecr " +
            //              "LEFT JOIN LMF_Actual_Drops hcfemndrp ON hcfemndrp.DropMasterID = dmt.MasterDropID AND hcfemndrp.AdditonID = @hcfemn " +
            //              "LEFT JOIN LMF_Actual_Drops lcfecrdrp ON lcfecrdrp.DropMasterID = dmt.MasterDropID AND lcfecrdrp.AdditonID = @lcfecr " +
            //              "LEFT JOIN LMF_Actual_Drops mcfemndrp ON mcfemndrp.DropMasterID = dmt.MasterDropID AND mcfemndrp.AdditonID = @mcfemn " +
            //              "LEFT JOIN LMF_Actual_Drops lowaldrp ON lowaldrp.DropMasterID = dmt.MasterDropID AND lowaldrp.AdditonID = @lowal " +
            //              "LEFT JOIN LMF_Actual_Drops lown2drp ON lown2drp.DropMasterID = dmt.MasterDropID AND lown2drp.AdditonID = @lown2 " +
            //              "LEFT JOIN LMF_Actual_Drops nidrp ON nidrp.DropMasterID = dmt.MasterDropID AND nidrp.AdditonID = @ni " +
            //              "LEFT JOIN LMF_Actual_Drops nifemndrp ON nifemndrp.DropMasterID = dmt.MasterDropID AND nifemndrp.AdditonID = @nifemn " +
            //              "LEFT JOIN LMF_Actual_Drops nvdrp ON nvdrp.DropMasterID = dmt.MasterDropID AND nvdrp.AdditonID = @nv " +
            //              "LEFT JOIN LMF_Actual_Drops simndrp ON simndrp.DropMasterID = dmt.MasterDropID AND simndrp.AdditonID = @simn " +
            //              "LEFT JOIN LMF_Actual_Drops suldrp ON suldrp.DropMasterID = dmt.MasterDropID AND suldrp.AdditonID = @sul " +
            //              "LEFT JOIN LMF_Actual_Drops caaldrp ON caaldrp.DropMasterID = dmt.MasterDropID AND caaldrp.AdditonID = @caal " +
            //              "WHERE dmt.MasterDropID IN (SELECT MasterDropID FROM LMF_Actual_Drops WHERE VisitID IN(SELECT LMFVisitID FROM LMF_Visits WHERE HeatID = @htid)) " +
            //              "ORDER BY dmt.DropNumber ASC", 9);
            //conn.cmd.Parameters.AddWithValue("al", ConstClass.Aluminum);
            //conn.cmd.Parameters.AddWithValue("coke", ConstClass.Coke);
            //conn.cmd.Parameters.AddWithValue("cu", ConstClass.Copper);
            //conn.cmd.Parameters.AddWithValue("feb", ConstClass.FeB);
            //conn.cmd.Parameters.AddWithValue("fenb", ConstClass.FeNb);
            //conn.cmd.Parameters.AddWithValue("femo", ConstClass.FeMo);
            //conn.cmd.Parameters.AddWithValue("fep", ConstClass.FeP);
            //conn.cmd.Parameters.AddWithValue("fesi", ConstClass.FeSi);
            //conn.cmd.Parameters.AddWithValue("feti", ConstClass.FeTi);
            //conn.cmd.Parameters.AddWithValue("fev", ConstClass.FeV);
            //conn.cmd.Parameters.AddWithValue("hcfecr", ConstClass.HCFeCr);
            //conn.cmd.Parameters.AddWithValue("hcfemn", ConstClass.HCFeMn);
            //conn.cmd.Parameters.AddWithValue("lcfecr", ConstClass.LCFeCr);
            //conn.cmd.Parameters.AddWithValue("mcfemn", ConstClass.MCFeMn);
            //conn.cmd.Parameters.AddWithValue("lowal", ConstClass.LowAlFeSi);
            //conn.cmd.Parameters.AddWithValue("lown2", ConstClass.LowN2Coke);
            //conn.cmd.Parameters.AddWithValue("ni", ConstClass.Nickel);
            //conn.cmd.Parameters.AddWithValue("nifemn", ConstClass.NitridedFeMn);
            //conn.cmd.Parameters.AddWithValue("nv", ConstClass.NitroVan);
            //conn.cmd.Parameters.AddWithValue("simn", ConstClass.SiMn);
            //conn.cmd.Parameters.AddWithValue("sul", ConstClass.Sulfur);
            //conn.cmd.Parameters.AddWithValue("caal", ConstClass.CaAl);
            //conn.cmd.Parameters.AddWithValue("htid", HeatID);
            //oDt = conn.ExecuteQuery();
            //conn.cmd.Parameters.Clear();
            return oDt;
        }

        public DataTable GetAllChemMasters(int HeatID)
        {
            DataTable oDt = new DataTable();
            conn = new ConnClass();
            conn.SqlQuery("SELECT typ.Type,chm.HeatMasterID,chm.ChemDate " +
                          "FROM HTS_Chem_Master chm " +
                          "INNER JOIN CHM_SampleTypes typ ON typ.ChemSampTypeID = chm.SampleTypeID " +
                          "WHERE chm.HeatID = @ht ORDER BY chm.HeatMasterID DESC", 9);
            conn.cmd.Parameters.AddWithValue("ht", HeatID);
            oDt = conn.ExecuteQuery();
            conn.cmd.Parameters.Clear();
            return oDt;
        }

        public DataTable GetChmElem()
        {
            DataTable oDt = new DataTable();
            conn = new ConnClass();
            conn.SqlQuery("SELECT ElementID,Element,DecimialPlaces FROM CHM_Elements ORDER BY DisplayPosition ASC", 9);
            oDt = conn.ExecuteQuery();
            return oDt;
        }

        public decimal GetChemRes(int ChmMasterID, int ElmID)
        {
            decimal Results = 0;
            DataTable oDt = new DataTable();
            conn = new ConnClass();
            conn.SqlQuery("SELECT Results FROM HTS_Chem WHERE ChemMasterID = @chm AND ElemID = @eid", 9);
            conn.cmd.Parameters.AddWithValue("chm", ChmMasterID);
            conn.cmd.Parameters.AddWithValue("eid", ElmID);
            oDt = conn.ExecuteQuery();
            conn.cmd.Parameters.Clear();
            if(oDt.Rows.Count > 0)
            {
                Results = Convert.ToDecimal(oDt.Rows[0]["Results"]);
            }
            return Results;
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

        public string GetPractice(int HeatID)
        {
            string Practice = "";
            int GradeID = GetGradeID(HeatID);
            DataTable oDt = new DataTable();
            conn = new ConnClass();
            conn.SqlQuery("SELECT pr.Practice " +
                          "FROM LMF_WorkInstructions wi " +
                          "INNER JOIN LMF_Practice pr ON pr.PracticeID = wi.PracticeID " +
                          "WHERE wi.GradeID = @grd", 9);
            conn.cmd.Parameters.AddWithValue("grd", GradeID);
            oDt = conn.ExecuteQuery();
            conn.cmd.Parameters.Clear();
            if(oDt.Rows.Count > 0)
            {
                Practice = Convert.ToString(oDt.Rows[0]["Practice"]);
            }
            return Practice;
        }
    }
}
