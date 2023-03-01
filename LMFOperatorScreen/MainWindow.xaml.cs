using C1.WPF.C1Chart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using System.Windows.Forms;
using System.Threading;



namespace LMFOperatorScreen
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ViewModel.HeatHistoryVM HeatHistory = new ViewModel.HeatHistoryVM();
        ViewModel.SelectedLMFVM sltLMF = new ViewModel.SelectedLMFVM();
        ViewModel.PowerKWHVM pwrkwhVM = new ViewModel.PowerKWHVM();
        private bool FirstLoad = true;
        bool DoWork = false;
        public MainWindow()
        {
            InitializeComponent();
            LoadLMFLocs();
            DoWork = true;
            Thread lmfStart = new Thread(StartLMFLevel2);
            lmfStart.Start();
        }

        private void StartLMFLevel2()
        {
            while(DoWork == true)
            { 
                this.Dispatcher.BeginInvoke(new System.Action(() =>
                {
                    LoadCurrentLMF();
                    LoadLevel2();
                    LoadWorkInst();
                    LoadHeats();
                }));
                
                Thread.Sleep(30000);
            }
        }


        #region MSOverview
        private void LoadLevel2()
        {
            ViewModel.GeneralInfoVM genVM = new ViewModel.GeneralInfoVM();
            genVM.GetLevel2Data();

            EAFAHeatLabel.Content = genVM.EAFAHeatNumber;
            EAFALadleLabel.Content = genVM.EAFALadle;
            EAFAGradeLabel.Content = genVM.EAFAGradeName;
            EAFATempLabel.Content = genVM.EAFATemp;
            EAFAKWHLabel.Content = pwrkwhVM.GetAShellKWH;

            EAFBHeatLabel.Content = genVM.EAFBHeatNumber;
            EAFBLadleLabel.Content = genVM.EAFBLadle;
            EAFBGradeLabel.Content = genVM.EAFBGradeName;
            EAFBTempLabel.Content = genVM.EAFBTemp;
            EAFBKWHLabel.Content = pwrkwhVM.GetBShellKWH;

            LMF2HeatLabel.Content = genVM.LMF2HeatNumber;
            LMF2GradeLabel.Content = genVM.LMF2GradeName;
            LMF2LadleLabel.Content = genVM.LMF2Ladle;
            if(Convert.ToString(LMF2HeatLabel.Content) != "Heat - ")
            {
                LMF2PowerLabel.Content = pwrkwhVM.LMF2Power;
            }
            else
            {
                LMF2PowerLabel.Content = "Power -";
            }
            LMF2StartLabel.Content = genVM.LMF2Start;
            LMF2LiquidusLabel.Content = sltLMF.GetLMF2Liquidous;
            if(sltLMF.LMF2HasJominy == true)
            {
                if(sltLMF.LMF2JominyOut == true)
                {
                    LMF2JomineyLabel.Content = "JOMINEY";
                    LMF2JomineyLabel.Foreground = new SolidColorBrush(Colors.Red);
                }
                else
                {
                    LMF2JomineyLabel.Content = "JOMINEY";
                    LMF2JomineyLabel.Foreground = new SolidColorBrush(Colors.Green);
                }
            }
            else
            {
                LMF2JomineyLabel.Content = "JOMINEY";
                LMF2JomineyLabel.Foreground = new SolidColorBrush(Colors.Black);
            }
            
            LMF2SteelWeightLabel.Content = sltLMF.GetLMF2LadleWeight;

            LMF1HeatLabel.Content = genVM.LMF1HeatNumber;
            LMF1GradeLabel.Content = genVM.LMF1GradeName;
            LMF1LadleLabel.Content = genVM.LMF1Ladle;
            if(Convert.ToString(LMF1HeatLabel.Content) != "Heat - ")
            {
                LMF1PowerLabel.Content = pwrkwhVM.LMF1Power;
            }
            else
            {
                LMF1PowerLabel.Content = "Power -";
            }
            LMF1StartLabel.Content = genVM.LMF1Start;
            LMF1LiquidusLabel.Content = sltLMF.GetLMF1Liquidous;
            if (sltLMF.LMF1HasJominy == true)
            {
                if (sltLMF.LMF1JominyOut == true)
                {
                    LMF1JomineyLabel.Content = "JOMINEY";
                    LMF1JomineyLabel.Foreground = new SolidColorBrush(Colors.Red);
                }
                else
                {
                    LMF1JomineyLabel.Content = "JOMINEY";
                    LMF1JomineyLabel.Foreground = new SolidColorBrush(Colors.Green);
                }
            }
            else
            {
                LMF1JomineyLabel.Content = "JOMINEY";
                LMF1JomineyLabel.Foreground = new SolidColorBrush(Colors.Black);
            }
            LMF1SteelWeightLabel.Content = sltLMF.GetLMF1LadleWeight;

            VTDHeatLabel.Content = genVM.VTDHeatNumber;
            VTDGradeLabel.Content = genVM.VTDGrade;
            VTDLadleLabel.Content = genVM.VTDLadle;
            VTDStartLabel.Content = genVM.VTDStart;

            CasterCastingHeatLabel.Content = genVM.CasterHeat;
            CasterLadleLabel.Content = genVM.CasterLadle;
            CasterGradeLabel.Content = genVM.CasterGrade;
            CasterLdlWghtLabel.Content = genVM.CasterWeight;
            CasterModeLabel.Content = genVM.CasterMode;
            OpenHeatLabel.Content = genVM.OpenHeat;
            CastingHeatLabel.Content = genVM.CasterHeat;
            Strand1Label.Content = genVM.Strand1Status;
            Strand2Label.Content = genVM.Strand2Status;
            Strand3Label.Content = genVM.Strand3Status;
            Strand4Label.Content = genVM.Strand4Status;
            Strand5Label.Content = genVM.Strand5Status;
            MinTillEmptyLabel.Content = genVM.MinTilEmpty;
            CastLadleWeightLabel.Content = genVM.CastLadleWgt;
            TundTempLabel.Content = genVM.TundTemp;
            TungWgtLabel.Content = genVM.TundWgt;
            TonsPerHourLabel.Content = genVM.TonsPerHour;
            CastingHeatLabel.Content = genVM.CasterHeat;

            
            

            FirstLoad = false;
        }
        #endregion

        #region LMFInfo
        private void LoadCurrentLMF()
        {            
            sltLMF.SelectedLMF = Convert.ToInt32(LMFComboBox.SelectedValue);
            sltLMF.StartCurrentLMF();
            if(Convert.ToInt32(LMFComboBox.SelectedValue) == 3)
            {
                LMF2GroupBox.Background = new SolidColorBrush(Colors.LightGreen);
                LMF1GroupBox.Background = new SolidColorBrush(Colors.LightGray);
            }
            else
            {
                LMF2GroupBox.Background = new SolidColorBrush(Colors.LightGray);
                LMF1GroupBox.Background = new SolidColorBrush(Colors.LightGreen);
            }

            LMF1KWHLabel.Content = sltLMF.GetLMF1KWH;
            LMF2KWH.Content = sltLMF.GetLMF2KWH;


            LoadChemGrid();       

            WIDoloLabel.Content = sltLMF.GetDoloWI;
            WILimeLabel.Content = sltLMF.GetLimeWI;
            WISparLabel.Content = sltLMF.GetSparWI;
            WICaLabel.Content = sltLMF.GetCaAlWI;
            WISilicaLabel.Content = sltLMF.GetSilicaWI;

            WorkFlowGrid.ItemsSource = sltLMF.GetWorkFlow.DefaultView;
            SpecialInstrGrid.ItemsSource = sltLMF.GetSpecialInstr.DefaultView;

            CurrentHeatHistory(sltLMF.GetHeatID);


            TempChart.Data.Children.Clear();

            TempChart.View.AxisY.Title = "Temps";
            TempChart.View.AxisX.Title = "Time/Temp";

            TempChart.View.AxisY.Min = 2750;
            TempChart.View.AxisY.Max = 3050;
            TempChart.View.AxisY.MajorUnit = 50;

            //TempChart.Data.ItemNames = "12:30 13:35 13:40 13:45 13:50 13:55 14:00 14:05 14:10 14:15";
            List<int> shiptemp = new List<int>();
            int ShipTemp = sltLMF.GetShipTemp;
            DataTable tmeDt = new DataTable();
            tmeDt = sltLMF.GetTempTimes;
            List<string> TempTimeList = new List<string>();
            foreach (DataRow dRow in tmeDt.Rows)
            {
                DateTime tmpDate = Convert.ToDateTime(dRow["TempDate"]);
                string temp = Convert.ToString(dRow["Temp"]);
                string dttmp = tmpDate.ToString("HH:mm") + " - " + temp;
                TempTimeList.Add(dttmp);
                shiptemp.Add(ShipTemp);
            }
            string[] strDates = TempTimeList.ToArray();
            TempChart.Data.ItemNames = strDates;

            DataTable tmpDt = sltLMF.GetTemps;
            List<int> TempList = new List<int>();
            foreach (DataRow tDt in tmpDt.Rows)
            {
                TempList.Add(Convert.ToInt32(tDt["Temp"]));
            }
            int[] Temps = TempList.ToArray();

            DataSeries ds1 = new DataSeries();
            ds1.ValuesSource = Temps;
            ds1.Label = "Temp";
            TempChart.Data.Children.Add(ds1);


            int[] sTemps = shiptemp.ToArray();

            DataSeries ds2 = new DataSeries();
            ds2.ValuesSource = sTemps;
            ds2.Label = "Ship";
            TempChart.Data.Children.Add(ds2);

        }

        private void LoadChemGrid()
        {
            LastChemLabel.Content = sltLMF.GetLastChemTime;
            ChemGrid.Items.Clear();
            ChemGrid.Items.Refresh();
            int HeatID = sltLMF.GetHeatID;
            if(HeatID != 0)
            {
                ViewModel.GradeInfoVM GradeInfo = new ViewModel.GradeInfoVM();
                DataTable oDt = sltLMF.GetChemDisplay;

                foreach (DataRow dRow in oDt.Rows)
                {

                    string Aim = "";
                    string Max = "";
                    string Min = "";
                    string Element = Convert.ToString(dRow["Element"]);
                    string Results = "";
                    GradeInfo.ElmID = Convert.ToInt32(dRow["ElmID"]);
                    sltLMF.ElmID = Convert.ToInt32(dRow["ElmID"]);
                    GradeInfo.GradeID = sltLMF.GetGradeID;
                    int DecPlaces = GradeInfo.GradeDecm;
                    DataTable mmaDt = GradeInfo.GradeMMA;
                    if (mmaDt.Rows.Count > 0)
                    {
                        Min = Convert.ToString(mmaDt.Rows[0]["Min"]);
                        Max = Convert.ToString(mmaDt.Rows[0]["Max"]);
                        Aim = Convert.ToString(mmaDt.Rows[0]["Aim"]);

                        if (Min != "")
                        {
                            Min = Convert.ToString(Math.Round(Convert.ToDecimal(mmaDt.Rows[0]["Min"]), DecPlaces));
                        }
                        if (Max != "")
                        {
                            Max = Convert.ToString(Math.Round(Convert.ToDecimal(mmaDt.Rows[0]["Max"]), DecPlaces));
                        }
                        if (Aim != "")
                        {
                            Aim = Convert.ToString(Math.Round(Convert.ToDecimal(mmaDt.Rows[0]["Aim"]), DecPlaces));
                        }
                    }
                    object Spec = "";
                    DataTable elmChmRes = sltLMF.GetChemResElm;
                    // elm.Element,chm.Results
                    if (elmChmRes.Rows.Count > 0)
                    {
                        Results = Convert.ToString(Math.Round(Convert.ToDecimal(elmChmRes.Rows[0]["Results"]), DecPlaces));
                    }

                    if (Convert.ToString(Min) != "")
                    {
                        if (Convert.ToDecimal(Results) < Convert.ToDecimal(Min))
                        {
                            Spec = "/images/BlueDownArrow.png";
                        }
                    }
                    if(Convert.ToString(Results) != "")
                    {
                        if (Convert.ToString(Max) != "")
                        {
                            if (Convert.ToDecimal(Results) > Convert.ToDecimal(Max))
                            {
                                Spec = "/images/RedUpArrowNoBG.png";
                            }
                        }
                    }
                    
                    
                    ChemGrid.Items.Add(new { Element, Aim, Min, Max, Results, Spec });
                }

            }
        }

        private void LoadLMFLocs()
        {
            ViewModel.SelectedLMFVM sltLMF = new ViewModel.SelectedLMFVM();
            LMFComboBox.ItemsSource = sltLMF.LMFCombo.DefaultView;
            LMFComboBox.DisplayMemberPath = "Location";
            LMFComboBox.SelectedValuePath = "LocationID";
            LMFComboBox.SelectedIndex = 0;
        }

        private void LMF_Selection_Change(object sender, SelectionChangedEventArgs e)
        {
            if (FirstLoad == false)
            {
                LoadCurrentLMF();
            }
        }
        #endregion

        #region Load Work Inst
        private void LoadWorkInst()
        {
            ViewModel.WorkInstVM GradeInfo = new ViewModel.WorkInstVM();

            GradeSearchCombo.ItemsSource = GradeInfo.GetGrades.DefaultView;
            GradeSearchCombo.DisplayMemberPath = "GradeName";
            GradeSearchCombo.SelectedValuePath = "GradeID";
        }
        #endregion

        #region LoadHeats
        private void LoadHeats()
        {
            if(SearchHeatCombo.Text == "")
            {
                ViewModel.HeatHistoryVM HistoryVM = new ViewModel.HeatHistoryVM();
                SearchHeatCombo.ItemsSource = HistoryVM.GetHeats.DefaultView;
                SearchHeatCombo.DisplayMemberPath = "HeatNumber";
                SearchHeatCombo.SelectedValuePath = "HeatID";
            }           
        }
        #endregion


        #region Button Clicks
        private void GradeSearchButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.WorkInstVM GradeInfo = new ViewModel.WorkInstVM();
            if (GradeSearchCombo.Text != "")
            {
                GradeInfo.GradeID = Convert.ToInt32(GradeSearchCombo.SelectedValue);
                GradeChemGrid.ItemsSource = GradeInfo.GetGradeChem.DefaultView;

                GradeFluxGrid.ItemsSource = GradeInfo.GetGradeFlux.DefaultView;

                GradeSpecialInstructionGrid.ItemsSource = GradeInfo.GetSpecialInstr.DefaultView;

                GradeWorkFlowGrid.ItemsSource = GradeInfo.GetGradeWorkFlow.DefaultView;
            }
            else
            {
                System.Windows.MessageBox.Show("Please Select a Grade");
            }
        }

        private void HeatSearchButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.HeatHistoryVM HistoryVM = new ViewModel.HeatHistoryVM();
            if (SearchHeatCombo.Text != "")
            {
                StartHeatHistory(Convert.ToInt32(SearchHeatCombo.SelectedValue));
            }
            else
            {
                System.Windows.MessageBox.Show("Please Select a Heat");
            }
        }

        private void SchdButton_Click(object sender, RoutedEventArgs e)
        {
            SchdApplication.MainWindow Sch = new SchdApplication.MainWindow();
            Sch.Show();
        }

        private void ClearSearchButton_Click(object sender, RoutedEventArgs e)
        {
            SearchHeatCombo.SelectedIndex = -1;
            LoadHeats();
            CurrentHeatHistory(sltLMF.GetHeatID);
        }
        #endregion

        #region HeatHistory

        private void CurrentHeatHistory(int HeatID)
        {
            if(SearchHeatCombo.Text == "")
            {
                if (HeatID != 0)
                {
                    LoadVisit1Info(HeatID);
                    LoadVisit2Info(HeatID);
                    LoadGrids(HeatID);
                }
            }
        }
        private void StartHeatHistory(int HeatID)
        {
            LoadVisit1Info(HeatID);
            LoadVisit2Info(HeatID);
            LoadGrids(HeatID);
        }

        private void LoadVisit1Info(int HeatID)
        {            
            HeatHistory.HeatID = HeatID;
            HeatHistory.Visit1GenInfo(HeatID);

            Visit1GradeLabel.Content = HeatHistory.Visit1GradeName;
            Visit1LadleLabel.Content = HeatHistory.Visit1Ladle;
            Visit1StartLabel.Content = HeatHistory.Visit1StartTime;
            Visit1EndLabel.Content = HeatHistory.Visit1EndTime;
            Visit1KWHLabel.Content = HeatHistory.Visit1KWH;
            Visit1Liquidous.Content = HeatHistory.Visit1GetLiquidous;
            Visit1WeightLabel.Content = HeatHistory.Visit1GetSteelWeight;
            Visit1PowerOnLabel.Content = HeatHistory.Visit1PowerOnTime;
            Visit1GasStirLabel.Content = HeatHistory.Visit1GasOnTime;
            Practice.Content = HeatHistory.Visit1Practice;


        }

        private void LoadVisit2Info(int HeatID)
        {
            HeatHistory.Visit2GenInfo(HeatID);

            Visit2StartLabel.Content = HeatHistory.Visit2StartTime;
            Visit2EndLabel.Content = HeatHistory.Visit2EndTime;
            Visit2KWHLabel.Content = HeatHistory.Visit2KWH;
            Visit2Liquidous.Content = HeatHistory.Visit2GetLiquidous;
            Visit2WeightLabel.Content = HeatHistory.Visit2GetSteelWeight;
            Visit2PowerOnLabel.Content = HeatHistory.Visit2PowerOnTime;
            Visit2GasStirLabel.Content = HeatHistory.Visit2GasOnTime;
        }

        private void LoadVisit3INfo(int HeatID)
        {
            HeatHistory.Visit2GenInfo(HeatID);

            Visit3StartLabel.Content = HeatHistory.Visit3StartTime;
            Visit3EndLabel.Content = HeatHistory.Visit3EndTime;
            Visit3KWHLabel.Content = HeatHistory.Visit3KWH;
            Visit3Liquidous.Content = HeatHistory.Visit3GetLiquidous;
            Visit3WeightLabel.Content = HeatHistory.Visit3GetSteelWeight;
            Visit3PowerOnLabel.Content = HeatHistory.Visit3PowerOnTime;
            Visit3GasStirLabel.Content = HeatHistory.Visit3GasOnTime;
        }

        private void LoadGrids(int HeatID)
        {
            HistoryTempGrid.ItemsSource = HeatHistory.TempDataTable.DefaultView;
            HistoryGasStirTimeGrid.ItemsSource = HeatHistory.GasStirDataTable.DefaultView;
            HistoryEMSGrid.ItemsSource = HeatHistory.EMSStirtDatatable.DefaultView;
            AdditionsGrid.ItemsSource = HeatHistory.HeatAdditonsDatatable.DefaultView;
            ActualDropGrid.ItemsSource = HeatHistory.GetActualDrops.DefaultView;
            LoadJominyChart();
            LoadChemHistory();
        }

        #region LoadChemInfo
        private void LoadChemHistory()
        {
            ChemHistoryGrid.Items.Clear();
            ChemHistoryGrid.Items.Refresh();
            DataTable eDt = HeatHistory.GetChemElm;
            int GradeID = HeatHistory.GetGradeID;
            LoadChemMin(eDt, GradeID);
            LoadChemMax(eDt, GradeID);

            DataTable oDt = HeatHistory.GetAllChemMasters;
            string Type = "";
            string ChemDate = "";
            int ElmID = 0;
            string Elm = "";
            int ChemMasterID = 0;
            int DecmPlaces = 0;
            string C = "";
            string Mn = "";
            string V = "";
            string Si = "";
            string S = "";
            string P = "";
            string Cu = "";
            string Cr = "";
            string Ni = "";
            string Mo = "";
            string Al = "";
            string Sa = "";
            string Cb = "";
            string Pb = "";
            string Sn = "";
            string Ca = "";
            string B = "";
            string Ti = "";
            string N = "";
            string LECOC = "";
            string LECOS = "";
            string LECON2 = "";
            string Di = "";
            string USCuEQ = "";
            string Tensile = "";
            string Yield = "";
            string CARBEQ = "";
            string CE1 = "";
            string KFactor = "";
            string As = "";
            string Bi = "";
            string Ce = "";
            string Co = "";
            string Sb = "";
            string Se = "";
            string Te = "";
            string Zn = "";
            string GSTTensile = "";
            string Zr = "";
            string FEPercent = "";
            string O = "";
            string H = "";
            string LECOTOX = "";
            string DIBVAL = "";           

            foreach (DataRow dRow in oDt.Rows)
            {
                Type = Convert.ToString(dRow["Type"]);
                DateTime ChemDateTime = Convert.ToDateTime(dRow["ChemDate"]);
                ChemMasterID = Convert.ToInt32(dRow["HeatMasterID"]);
                ChemDate = ChemDateTime.ToString("HH:mm:ss yyyy-MM-dd");
                
                
                foreach (DataRow eRow in eDt.Rows)
                {
                    ElmID = Convert.ToInt32(eRow["ElementID"]);
                    Elm = Convert.ToString(eRow["Element"]);
                    DecmPlaces = Convert.ToInt32(eRow["DecimialPlaces"]);
                    HeatHistory.ChemMasterID = ChemMasterID;
                    HeatHistory.ElmID = ElmID;
                    decimal Results = HeatHistory.GetChemRes;

                    switch(ElmID)
                    {
                        case 5:
                            if(Results != 0)
                            {
                                C = Convert.ToString(Math.Round(Results, DecmPlaces));
                            }
                            else
                            {
                                C = "";
                            }
                            break;
                        case 23:
                            if (Results != 0)
                            {
                                Mn = Convert.ToString(Math.Round(Results, DecmPlaces));
                            }
                            else
                            {
                                Mn = "";
                            }
                            break;
                        case 41:
                            if (Results != 0)
                            {
                                V = Convert.ToString(Math.Round(Results, DecmPlaces));
                            }
                            else
                            {
                                V = "";
                            }
                            break;
                        case 35:
                            if (Results != 0)
                            {
                                Si = Convert.ToString(Math.Round(Results, DecmPlaces));
                            }
                            else
                            {
                                Si = "";
                            }
                            break;
                        case 31:
                            if (Results != 0)
                            {
                                S = Convert.ToString(Math.Round(Results, DecmPlaces));
                            }
                            else
                            {
                                S = "";
                            }
                            break;
                        case 29:
                            if (Results != 0)
                            {
                                P = Convert.ToString(Math.Round(Results, DecmPlaces));
                            }
                            else
                            {
                                P = "";
                            }
                            break;
                        case 12:
                            if (Results != 0)
                            {
                                Cu = Convert.ToString(Math.Round(Results, DecmPlaces));
                            }
                            else
                            {
                                Cu = "";
                            }
                            break;
                        case 11:
                            if (Results != 0)
                            {
                                Cr = Convert.ToString(Math.Round(Results, DecmPlaces));
                            }
                            else
                            {
                                Cr = "";
                            }
                            break;
                        case 27:
                            if (Results != 0)
                            {
                                Ni = Convert.ToString(Math.Round(Results, DecmPlaces));
                            }
                            else
                            {
                                Ni = "";
                            }
                            break;
                        case 24:
                            if (Results != 0)
                            {
                                Mo = Convert.ToString(Math.Round(Results, DecmPlaces));
                            }
                            else
                            {
                                Mo = "";
                            }
                            break;
                        case 1:
                            if (Results != 0)
                            {
                                Al = Convert.ToString(Math.Round(Results, DecmPlaces));
                            }
                            else
                            {
                                Al = "";
                            }
                            break;
                        case 32:
                            if (Results != 0)
                            {
                                Sa = Convert.ToString(Math.Round(Results, DecmPlaces));
                            }
                            else
                            {
                                Sa = "";
                            }
                            break;
                        case 26:
                            if (Results != 0)
                            {
                                Cb = Convert.ToString(Math.Round(Results, DecmPlaces));
                            }
                            else
                            {
                                Cb = "";
                            }
                            break;
                        case 30:
                            if (Results != 0)
                            {
                                Pb = Convert.ToString(Math.Round(Results, DecmPlaces));
                            }
                            else
                            {
                                Pb = "";
                            }
                            break;
                        case 36:
                            if (Results != 0)
                            {
                                Sn = Convert.ToString(Math.Round(Results, DecmPlaces)); 
                            }
                            else
                            {
                                Sn = "";
                            }
                            break;
                        case 6:
                            if (Results != 0)
                            {
                                Ca = Convert.ToString(Math.Round(Results, DecmPlaces));
                            }
                            else
                            {
                                Ca = "";
                            }
                            break;
                        case 3:
                            if (Results != 0)
                            {
                                B = Convert.ToString(Math.Round(Results, DecmPlaces));
                            }
                            else
                            {
                                B = "";
                            }
                            break;
                        case 39:
                            if (Results != 0)
                            {
                                Ti = Convert.ToString(Math.Round(Results, DecmPlaces));
                            }
                            else
                            {
                                Ti = "";
                            }
                            break;
                        case 25:
                            if (Results != 0)
                            {
                                N = Convert.ToString(Math.Round(Results, DecmPlaces));
                            }
                            else
                            {
                                N = "";
                            }
                            break;
                        case 19:
                            if (Results != 0)
                            {
                                LECOC = Convert.ToString(Math.Round(Results, DecmPlaces));
                            }
                            else
                            {
                                LECOC = "";
                            }
                            break;
                        case 21:
                            if (Results != 0)
                            {
                                LECOS = Convert.ToString(Math.Round(Results, DecmPlaces));
                            }
                            else
                            {
                                LECOS = "";
                            }
                            break;
                        case 20:
                            if (Results != 0)
                            {
                                LECON2 = Convert.ToString(Math.Round(Results, DecmPlaces));
                            }
                            else
                            {
                                LECON2 = "";
                            }
                            break;
                        case 13:
                            if (Results != 0)
                            {
                                Di = Convert.ToString(Math.Round(Results, DecmPlaces));
                            }
                            else
                            {
                                Di = "";
                            }
                            break;
                        case 40:
                            if (Results != 0)
                            {
                                USCuEQ = Convert.ToString(Math.Round(Results, DecmPlaces));
                            }
                            else
                            {
                                USCuEQ = "";
                            }
                            break;
                        case 38:
                            if (Results != 0)
                            {
                                Tensile = Convert.ToString(Math.Round(Results, DecmPlaces));
                            }
                            else
                            {
                                Tensile = "";
                            }
                            break;
                        case 42:
                            if (Results != 0)
                            {
                                Yield = Convert.ToString(Math.Round(Results, DecmPlaces));
                            }
                            else
                            {
                                Yield = "";
                            }
                            break;
                        case 7:
                            if (Results != 0)
                            {
                                CARBEQ = Convert.ToString(Math.Round(Results, DecmPlaces));
                            }
                            else
                            {
                                CARBEQ = "";
                            }
                            break;
                        case 9:
                            if (Results != 0)
                            {
                                CE1 = Convert.ToString(Math.Round(Results, DecmPlaces));
                            }
                            else
                            {
                                CE1 = "";
                            }
                            break;
                        case 18:
                            if (Results != 0)
                            {
                                KFactor = Convert.ToString(Math.Round(Results, DecmPlaces));
                            }
                            else
                            {
                                As = Convert.ToString(Math.Round(Results, DecmPlaces));
                            }
                            break;
                        case 2:
                            if (Results != 0)
                            {
                                As = Convert.ToString(Math.Round(Results, DecmPlaces));
                            }
                            else
                            {
                                As = "";
                            }
                            break;
                        case 4:
                            if (Results != 0)
                            {
                                Bi = Convert.ToString(Math.Round(Results, DecmPlaces));
                            }
                            else
                            {
                                Bi = "";
                            }
                            break;
                        case 8:
                            if (Results != 0)
                            {
                                Ce = Convert.ToString(Math.Round(Results, DecmPlaces));
                            }
                            else
                            {
                                Ce = "";
                            }
                            break;
                        case 10:
                            if (Results != 0)
                            {
                                Co = Convert.ToString(Math.Round(Results, DecmPlaces));
                            }
                            else
                            {
                                Co = "";
                            }
                            break;
                        case 33:
                            if (Results != 0)
                            {
                                Sb = Convert.ToString(Math.Round(Results, DecmPlaces));
                            }
                            else
                            {
                                Sb = "";
                            }
                            break;
                        case 34:
                            if (Results != 0)
                            {
                                Se = Convert.ToString(Math.Round(Results, DecmPlaces));
                            }
                            else
                            {
                                Se = "";
                            }
                            break;
                        case 37:
                            if (Results != 0)
                            {
                                Te = Convert.ToString(Math.Round(Results, DecmPlaces));
                            }
                            else
                            {
                                Te = "";
                            }
                            break;
                        case 43:
                            if (Results != 0)
                            {
                                Zn = Convert.ToString(Math.Round(Results, DecmPlaces));
                            }
                            else
                            {
                                Zn = "";
                            }
                            break;
                        case 16:
                            if (Results != 0)
                            {
                                GSTTensile = Convert.ToString(Math.Round(Results, DecmPlaces));
                            }
                            else
                            {
                                GSTTensile = "";
                            }
                            break;
                        case 44:
                            if (Results != 0)
                            {
                                Zr = Convert.ToString(Math.Round(Results, DecmPlaces));
                            }
                            else
                            {
                                Zr = "";
                            }
                            break;
                        case 15:
                            if (Results != 0)
                            {
                                FEPercent = Convert.ToString(Math.Round(Results, DecmPlaces));
                            }
                            else
                            {
                                FEPercent = "";
                            }
                            break;
                        case 28:
                            if (Results != 0)
                            {
                                O = Convert.ToString(Math.Round(Results, DecmPlaces));
                            }
                            else
                            {
                                O = "";
                            }
                            break;
                        case 17:
                            if (Results != 0)
                            {
                                H = Convert.ToString(Math.Round(Results, DecmPlaces));
                            }
                            else
                            {
                                H = "";
                            }
                            break;
                        case 22:
                            if (Results != 0)
                            {
                                LECOTOX = Convert.ToString(Math.Round(Results, DecmPlaces));
                            }
                            else
                            {
                                LECOTOX = "";
                            }
                            break;
                        case 14:
                            if (Results != 0)
                            {
                                DIBVAL = Convert.ToString(Math.Round(Results, DecmPlaces));
                            }
                            else
                            {
                                DIBVAL = "";
                            }
                            break;
                    }
                    
                }
                ChemHistoryGrid.Items.Add(new { Type, ChemDate, C, Mn, V, Si, S, P, Cu, Cr, Ni, Mo, Al, Sa, Cb, Pb, Sn, Ca, B, Ti, N, LECOC, LECOS, LECON2, Di, USCuEQ, Tensile, Yield, CARBEQ, CE1, KFactor, As, Bi, Ce, Co, Sb, Se, Te, Zn, GSTTensile, Zr, FEPercent, O, H, LECOTOX, DIBVAL });
            }
        }

        private void LoadChemMin(DataTable eDt, int GradeID)
        {
            string ChemDate = "";
            ViewModel.GradeInfoVM GradeVM = new ViewModel.GradeInfoVM();
            GradeVM.GradeID = GradeID;
            string Type = "Min";

            string C = "";
            string Mn = "";
            string V = "";
            string Si = "";
            string S = "";
            string P = "";
            string Cu = "";
            string Cr = "";
            string Ni = "";
            string Mo = "";
            string Al = "";
            string Sa = "";
            string Cb = "";
            string Pb = "";
            string Sn = "";
            string Ca = "";
            string B = "";
            string Ti = "";
            string N = "";
            string LECOC = "";
            string LECOS = "";
            string LECON2 = "";
            string Di = "";
            string USCuEQ = "";
            string Tensile = "";
            string Yield = "";
            string CARBEQ = "";
            string CE1 = "";
            string KFactor = "";
            string As = "";
            string Bi = "";
            string Ce = "";
            string Co = "";
            string Sb = "";
            string Se = "";
            string Te = "";
            string Zn = "";
            string GSTTensile = "";
            string Zr = "";
            string FEPercent = "";
            string O = "";
            string H = "";
            string LECOTOX = "";
            string DIBVAL = "";
            foreach (DataRow dRow in eDt.Rows)
            {
                int ElmID = Convert.ToInt32(dRow["ElementID"]);
                int DecmPlaces = Convert.ToInt32(dRow["DecimialPlaces"]);
                GradeVM.ElmID = ElmID;
                decimal Results = GradeVM.GetGradeMin;
                switch (ElmID)
                {
                    case 5:
                        if (Results != 0)
                        {
                            C = Convert.ToString(Math.Round(Results, DecmPlaces));
                        }
                        else
                        {
                            C = "";
                        }
                        break;
                    case 23:
                        if (Results != 0)
                        {
                            Mn = Convert.ToString(Math.Round(Results, DecmPlaces));
                        }
                        else
                        {
                            Mn = "";
                        }
                        break;
                    case 41:
                        if (Results != 0)
                        {
                            V = Convert.ToString(Math.Round(Results, DecmPlaces));
                        }
                        else
                        {
                            V = "";
                        }
                        break;
                    case 35:
                        if (Results != 0)
                        {
                            Si = Convert.ToString(Math.Round(Results, DecmPlaces));
                        }
                        else
                        {
                            Si = "";
                        }
                        break;
                    case 31:
                        if (Results != 0)
                        {
                            S = Convert.ToString(Math.Round(Results, DecmPlaces));
                        }
                        else
                        {
                            S = "";
                        }
                        break;
                    case 29:
                        if (Results != 0)
                        {
                            P = Convert.ToString(Math.Round(Results, DecmPlaces));
                        }
                        else
                        {
                            P = "";
                        }
                        break;
                    case 12:
                        if (Results != 0)
                        {
                            Cu = Convert.ToString(Math.Round(Results, DecmPlaces));
                        }
                        else
                        {
                            Cu = "";
                        }
                        break;
                    case 11:
                        if (Results != 0)
                        {
                            Cr = Convert.ToString(Math.Round(Results, DecmPlaces));
                        }
                        else
                        {
                            Cr = "";
                        }
                        break;
                    case 27:
                        if (Results != 0)
                        {
                            Ni = Convert.ToString(Math.Round(Results, DecmPlaces));
                        }
                        else
                        {
                            Ni = "";
                        }
                        break;
                    case 24:
                        if (Results != 0)
                        {
                            Mo = Convert.ToString(Math.Round(Results, DecmPlaces));
                        }
                        else
                        {
                            Mo = "";
                        }
                        break;
                    case 1:
                        if (Results != 0)
                        {
                            Al = Convert.ToString(Math.Round(Results, DecmPlaces));
                        }
                        else
                        {
                            Al = "";
                        }
                        break;
                    case 32:
                        if (Results != 0)
                        {
                            Sa = Convert.ToString(Math.Round(Results, DecmPlaces));
                        }
                        else
                        {
                            Sa = "";
                        }
                        break;
                    case 26:
                        if (Results != 0)
                        {
                            Cb = Convert.ToString(Math.Round(Results, DecmPlaces));
                        }
                        else
                        {
                            Cb = "";
                        }
                        break;
                    case 30:
                        if (Results != 0)
                        {
                            Pb = Convert.ToString(Math.Round(Results, DecmPlaces));
                        }
                        else
                        {
                            Pb = "";
                        }
                        break;
                    case 36:
                        if (Results != 0)
                        {
                            Sn = Convert.ToString(Math.Round(Results, DecmPlaces));
                        }
                        else
                        {
                            Sn = "";
                        }
                        break;
                    case 6:
                        if (Results != 0)
                        {
                            Ca = Convert.ToString(Math.Round(Results, DecmPlaces));
                        }
                        else
                        {
                            Ca = "";
                        }
                        break;
                    case 3:
                        if (Results != 0)
                        {
                            B = Convert.ToString(Math.Round(Results, DecmPlaces));
                        }
                        else
                        {
                            B = "";
                        }
                        break;
                    case 39:
                        if (Results != 0)
                        {
                            Ti = Convert.ToString(Math.Round(Results, DecmPlaces));
                        }
                        else
                        {
                            Ti = "";
                        }
                        break;
                    case 25:
                        if (Results != 0)
                        {
                            N = Convert.ToString(Math.Round(Results, DecmPlaces));
                        }
                        else
                        {
                            N = "";
                        }
                        break;
                    case 19:
                        if (Results != 0)
                        {
                            LECOC = Convert.ToString(Math.Round(Results, DecmPlaces));
                        }
                        else
                        {
                            LECOC = "";
                        }
                        break;
                    case 21:
                        if (Results != 0)
                        {
                            LECOS = Convert.ToString(Math.Round(Results, DecmPlaces));
                        }
                        else
                        {
                            LECOS = "";
                        }
                        break;
                    case 20:
                        if (Results != 0)
                        {
                            LECON2 = Convert.ToString(Math.Round(Results, DecmPlaces));
                        }
                        else
                        {
                            LECON2 = "";
                        }
                        break;
                    case 13:
                        if (Results != 0)
                        {
                            Di = Convert.ToString(Math.Round(Results, DecmPlaces));
                        }
                        else
                        {
                            Di = "";
                        }
                        break;
                    case 40:
                        if (Results != 0)
                        {
                            USCuEQ = Convert.ToString(Math.Round(Results, DecmPlaces));
                        }
                        else
                        {
                            USCuEQ = "";
                        }
                        break;
                    case 38:
                        if (Results != 0)
                        {
                            Tensile = Convert.ToString(Math.Round(Results, DecmPlaces));
                        }
                        else
                        {
                            Tensile = "";
                        }
                        break;
                    case 42:
                        if (Results != 0)
                        {
                            Yield = Convert.ToString(Math.Round(Results, DecmPlaces));
                        }
                        else
                        {
                            Yield = "";
                        }
                        break;
                    case 7:
                        if (Results != 0)
                        {
                            CARBEQ = Convert.ToString(Math.Round(Results, DecmPlaces));
                        }
                        else
                        {
                            CARBEQ = "";
                        }
                        break;
                    case 9:
                        if (Results != 0)
                        {
                            CE1 = Convert.ToString(Math.Round(Results, DecmPlaces));
                        }
                        else
                        {
                            CE1 = "";
                        }
                        break;
                    case 18:
                        if (Results != 0)
                        {
                            KFactor = Convert.ToString(Math.Round(Results, DecmPlaces));
                        }
                        else
                        {
                            KFactor = Convert.ToString(Math.Round(Results, DecmPlaces));
                        }
                        break;
                    case 2:
                        if (Results != 0)
                        {
                            As = Convert.ToString(Math.Round(Results, DecmPlaces));
                        }
                        else
                        {
                            As = "";
                        }
                        break;
                    case 4:
                        if (Results != 0)
                        {
                            Bi = Convert.ToString(Math.Round(Results, DecmPlaces));
                        }
                        else
                        {
                            Bi = "";
                        }
                        break;
                    case 8:
                        if (Results != 0)
                        {
                            Ce = Convert.ToString(Math.Round(Results, DecmPlaces));
                        }
                        else
                        {
                            Ce = "";
                        }
                        break;
                    case 10:
                        if (Results != 0)
                        {
                            Co = Convert.ToString(Math.Round(Results, DecmPlaces));
                        }
                        else
                        {
                            Co = "";
                        }
                        break;
                    case 33:
                        if (Results != 0)
                        {
                            Sb = Convert.ToString(Math.Round(Results, DecmPlaces));
                        }
                        else
                        {
                            Sb = "";
                        }
                        break;
                    case 34:
                        if (Results != 0)
                        {
                            Se = Convert.ToString(Math.Round(Results, DecmPlaces));
                        }
                        else
                        {
                            Se = "";
                        }
                        break;
                    case 37:
                        if (Results != 0)
                        {
                            Te = Convert.ToString(Math.Round(Results, DecmPlaces));
                        }
                        else
                        {
                            Te = "";
                        }
                        break;
                    case 43:
                        if (Results != 0)
                        {
                            Zn = Convert.ToString(Math.Round(Results, DecmPlaces));
                        }
                        else
                        {
                            Zn = "";
                        }
                        break;
                    case 16:
                        if (Results != 0)
                        {
                            GSTTensile = Convert.ToString(Math.Round(Results, DecmPlaces));
                        }
                        else
                        {
                            GSTTensile = "";
                        }
                        break;
                    case 44:
                        if (Results != 0)
                        {
                            Zr = Convert.ToString(Math.Round(Results, DecmPlaces));
                        }
                        else
                        {
                            Zr = "";
                        }
                        break;
                    case 15:
                        if (Results != 0)
                        {
                            FEPercent = Convert.ToString(Math.Round(Results, DecmPlaces));
                        }
                        else
                        {
                            FEPercent = "";
                        }
                        break;
                    case 28:
                        if (Results != 0)
                        {
                            O = Convert.ToString(Math.Round(Results, DecmPlaces));
                        }
                        else
                        {
                            O = "";
                        }
                        break;
                    case 17:
                        if (Results != 0)
                        {
                            H = Convert.ToString(Math.Round(Results, DecmPlaces));
                        }
                        else
                        {
                            H = "";
                        }
                        break;
                    case 22:
                        if (Results != 0)
                        {
                            LECOTOX = Convert.ToString(Math.Round(Results, DecmPlaces));
                        }
                        else
                        {
                            LECOTOX = "";
                        }
                        break;
                    case 14:
                        if (Results != 0)
                        {
                            DIBVAL = Convert.ToString(Math.Round(Results, DecmPlaces));
                        }
                        else
                        {
                            DIBVAL = "";
                        }
                        break;
                }
            }

            ChemHistoryGrid.Items.Add(new { Type, ChemDate, C, Mn, V, Si, S, P, Cu, Cr, Ni, Mo, Al, Sa, Cb, Pb, Sn, Ca, B, Ti, N, LECOC, LECOS, LECON2, Di, USCuEQ, Tensile, Yield, CARBEQ, CE1, KFactor, As, Bi, Ce, Co, Sb, Se, Te, Zn, GSTTensile, Zr, FEPercent, O, H, LECOTOX, DIBVAL });

        }

        private void LoadChemMax(DataTable eDt, int GradeID)
        {
            string ChemDate = "";
            ViewModel.GradeInfoVM GradeVM = new ViewModel.GradeInfoVM();
            GradeVM.GradeID = GradeID;
            string Type = "Max";

            string C = "";
            string Mn = "";
            string V = "";
            string Si = "";
            string S = "";
            string P = "";
            string Cu = "";
            string Cr = "";
            string Ni = "";
            string Mo = "";
            string Al = "";
            string Sa = "";
            string Cb = "";
            string Pb = "";
            string Sn = "";
            string Ca = "";
            string B = "";
            string Ti = "";
            string N = "";
            string LECOC = "";
            string LECOS = "";
            string LECON2 = "";
            string Di = "";
            string USCuEQ = "";
            string Tensile = "";
            string Yield = "";
            string CARBEQ = "";
            string CE1 = "";
            string KFactor = "";
            string As = "";
            string Bi = "";
            string Ce = "";
            string Co = "";
            string Sb = "";
            string Se = "";
            string Te = "";
            string Zn = "";
            string GSTTensile = "";
            string Zr = "";
            string FEPercent = "";
            string O = "";
            string H = "";
            string LECOTOX = "";
            string DIBVAL = "";
            foreach (DataRow dRow in eDt.Rows)
            {
                int ElmID = Convert.ToInt32(dRow["ElementID"]);
                int DecmPlaces = Convert.ToInt32(dRow["DecimialPlaces"]);
                GradeVM.ElmID = ElmID;
                decimal Results = GradeVM.GetGradeMax;
                switch (ElmID)
                {
                    case 5:
                        if (Results != 0)
                        {
                            C = Convert.ToString(Math.Round(Results, DecmPlaces));
                        }
                        else
                        {
                            C = "";
                        }
                        break;
                    case 23:
                        if (Results != 0)
                        {
                            Mn = Convert.ToString(Math.Round(Results, DecmPlaces));
                        }
                        else
                        {
                            Mn = "";
                        }
                        break;
                    case 41:
                        if (Results != 0)
                        {
                            V = Convert.ToString(Math.Round(Results, DecmPlaces));
                        }
                        else
                        {
                            V = "";
                        }
                        break;
                    case 35:
                        if (Results != 0)
                        {
                            Si = Convert.ToString(Math.Round(Results, DecmPlaces));
                        }
                        else
                        {
                            Si = "";
                        }
                        break;
                    case 31:
                        if (Results != 0)
                        {
                            S = Convert.ToString(Math.Round(Results, DecmPlaces));
                        }
                        else
                        {
                            S = "";
                        }
                        break;
                    case 29:
                        if (Results != 0)
                        {
                            P = Convert.ToString(Math.Round(Results, DecmPlaces));
                        }
                        else
                        {
                            P = "";
                        }
                        break;
                    case 12:
                        if (Results != 0)
                        {
                            Cu = Convert.ToString(Math.Round(Results, DecmPlaces));
                        }
                        else
                        {
                            Cu = "";
                        }
                        break;
                    case 11:
                        if (Results != 0)
                        {
                            Cr = Convert.ToString(Math.Round(Results, DecmPlaces));
                        }
                        else
                        {
                            Cr = "";
                        }
                        break;
                    case 27:
                        if (Results != 0)
                        {
                            Ni = Convert.ToString(Math.Round(Results, DecmPlaces));
                        }
                        else
                        {
                            Ni = "";
                        }
                        break;
                    case 24:
                        if (Results != 0)
                        {
                            Mo = Convert.ToString(Math.Round(Results, DecmPlaces));
                        }
                        else
                        {
                            Mo = "";
                        }
                        break;
                    case 1:
                        if (Results != 0)
                        {
                            Al = Convert.ToString(Math.Round(Results, DecmPlaces));
                        }
                        else
                        {
                            Al = "";
                        }
                        break;
                    case 32:
                        if (Results != 0)
                        {
                            Sa = Convert.ToString(Math.Round(Results, DecmPlaces));
                        }
                        else
                        {
                            Sa = "";
                        }
                        break;
                    case 26:
                        if (Results != 0)
                        {
                            Cb = Convert.ToString(Math.Round(Results, DecmPlaces));
                        }
                        else
                        {
                            Cb = "";
                        }
                        break;
                    case 30:
                        if (Results != 0)
                        {
                            Pb = Convert.ToString(Math.Round(Results, DecmPlaces));
                        }
                        else
                        {
                            Pb = "";
                        }
                        break;
                    case 36:
                        if (Results != 0)
                        {
                            Sn = Convert.ToString(Math.Round(Results, DecmPlaces));
                        }
                        else
                        {
                            Sn = "";
                        }
                        break;
                    case 6:
                        if (Results != 0)
                        {
                            Ca = Convert.ToString(Math.Round(Results, DecmPlaces));
                        }
                        else
                        {
                            Ca = "";
                        }
                        break;
                    case 3:
                        if (Results != 0)
                        {
                            B = Convert.ToString(Math.Round(Results, DecmPlaces));
                        }
                        else
                        {
                            B = "";
                        }
                        break;
                    case 39:
                        if (Results != 0)
                        {
                            Ti = Convert.ToString(Math.Round(Results, DecmPlaces));
                        }
                        else
                        {
                            Ti = "";
                        }
                        break;
                    case 25:
                        if (Results != 0)
                        {
                            N = Convert.ToString(Math.Round(Results, DecmPlaces));
                        }
                        else
                        {
                            N = "";
                        }
                        break;
                    case 19:
                        if (Results != 0)
                        {
                            LECOC = Convert.ToString(Math.Round(Results, DecmPlaces));
                        }
                        else
                        {
                            LECOC = "";
                        }
                        break;
                    case 21:
                        if (Results != 0)
                        {
                            LECOS = Convert.ToString(Math.Round(Results, DecmPlaces));
                        }
                        else
                        {
                            LECOS = "";
                        }
                        break;
                    case 20:
                        if (Results != 0)
                        {
                            LECON2 = Convert.ToString(Math.Round(Results, DecmPlaces));
                        }
                        else
                        {
                            LECON2 = "";
                        }
                        break;
                    case 13:
                        if (Results != 0)
                        {
                            Di = Convert.ToString(Math.Round(Results, DecmPlaces));
                        }
                        else
                        {
                            Di = "";
                        }
                        break;
                    case 40:
                        if (Results != 0)
                        {
                            USCuEQ = Convert.ToString(Math.Round(Results, DecmPlaces));
                        }
                        else
                        {
                            USCuEQ = "";
                        }
                        break;
                    case 38:
                        if (Results != 0)
                        {
                            Tensile = Convert.ToString(Math.Round(Results, DecmPlaces));
                        }
                        else
                        {
                            Tensile = "";
                        }
                        break;
                    case 42:
                        if (Results != 0)
                        {
                            Yield = Convert.ToString(Math.Round(Results, DecmPlaces));
                        }
                        else
                        {
                            Yield = "";
                        }
                        break;
                    case 7:
                        if (Results != 0)
                        {
                            CARBEQ = Convert.ToString(Math.Round(Results, DecmPlaces));
                        }
                        else
                        {
                            CARBEQ = "";
                        }
                        break;
                    case 9:
                        if (Results != 0)
                        {
                            CE1 = Convert.ToString(Math.Round(Results, DecmPlaces));
                        }
                        else
                        {
                            CE1 = "";
                        }
                        break;
                    case 18:
                        if (Results != 0)
                        {
                            KFactor = Convert.ToString(Math.Round(Results, DecmPlaces));
                        }
                        else
                        {
                            KFactor = Convert.ToString(Math.Round(Results, DecmPlaces));
                        }
                        break;
                    case 2:
                        if (Results != 0)
                        {
                            As = Convert.ToString(Math.Round(Results, DecmPlaces));
                        }
                        else
                        {
                            As = "";
                        }
                        break;
                    case 4:
                        if (Results != 0)
                        {
                            Bi = Convert.ToString(Math.Round(Results, DecmPlaces));
                        }
                        else
                        {
                            Bi = "";
                        }
                        break;
                    case 8:
                        if (Results != 0)
                        {
                            Ce = Convert.ToString(Math.Round(Results, DecmPlaces));
                        }
                        else
                        {
                            Ce = "";
                        }
                        break;
                    case 10:
                        if (Results != 0)
                        {
                            Co = Convert.ToString(Math.Round(Results, DecmPlaces));
                        }
                        else
                        {
                            Co = "";
                        }
                        break;
                    case 33:
                        if (Results != 0)
                        {
                            Sb = Convert.ToString(Math.Round(Results, DecmPlaces));
                        }
                        else
                        {
                            Sb = "";
                        }
                        break;
                    case 34:
                        if (Results != 0)
                        {
                            Se = Convert.ToString(Math.Round(Results, DecmPlaces));
                        }
                        else
                        {
                            Se = "";
                        }
                        break;
                    case 37:
                        if (Results != 0)
                        {
                            Te = Convert.ToString(Math.Round(Results, DecmPlaces));
                        }
                        else
                        {
                            Te = "";
                        }
                        break;
                    case 43:
                        if (Results != 0)
                        {
                            Zn = Convert.ToString(Math.Round(Results, DecmPlaces));
                        }
                        else
                        {
                            Zn = "";
                        }
                        break;
                    case 16:
                        if (Results != 0)
                        {
                            GSTTensile = Convert.ToString(Math.Round(Results, DecmPlaces));
                        }
                        else
                        {
                            GSTTensile = "";
                        }
                        break;
                    case 44:
                        if (Results != 0)
                        {
                            Zr = Convert.ToString(Math.Round(Results, DecmPlaces));
                        }
                        else
                        {
                            Zr = "";
                        }
                        break;
                    case 15:
                        if (Results != 0)
                        {
                            FEPercent = Convert.ToString(Math.Round(Results, DecmPlaces));
                        }
                        else
                        {
                            FEPercent = "";
                        }
                        break;
                    case 28:
                        if (Results != 0)
                        {
                            O = Convert.ToString(Math.Round(Results, DecmPlaces));
                        }
                        else
                        {
                            O = "";
                        }
                        break;
                    case 17:
                        if (Results != 0)
                        {
                            H = Convert.ToString(Math.Round(Results, DecmPlaces));
                        }
                        else
                        {
                            H = "";
                        }
                        break;
                    case 22:
                        if (Results != 0)
                        {
                            LECOTOX = Convert.ToString(Math.Round(Results, DecmPlaces));
                        }
                        else
                        {
                            LECOTOX = "";
                        }
                        break;
                    case 14:
                        if (Results != 0)
                        {
                            DIBVAL = Convert.ToString(Math.Round(Results, DecmPlaces));
                        }
                        else
                        {
                            DIBVAL = "";
                        }
                        break;
                }
            }

            ChemHistoryGrid.Items.Add(new { Type, ChemDate, C, Mn, V, Si, S, P, Cu, Cr, Ni, Mo, Al, Sa, Cb, Pb, Sn, Ca, B, Ti, N, LECOC, LECOS, LECON2, Di, USCuEQ, Tensile, Yield, CARBEQ, CE1, KFactor, As, Bi, Ce, Co, Sb, Se, Te, Zn, GSTTensile, Zr, FEPercent, O, H, LECOTOX, DIBVAL });
        }
        #endregion

        private void LoadJominyChart()
        {
            JomineyChart.Data.Children.Clear();

            JomineyChart.View.AxisY.Title = "Hardness";
            JomineyChart.View.AxisX.Title = "J Points";

            JomineyChart.View.AxisY.Min = 0;
            JomineyChart.View.AxisY.Max = 65;
            JomineyChart.View.AxisY.MajorUnit = 5;

            //JomineyChart.Data.ItemNames = "J1 J2 J3 J4 J5 J6 J7 J8 J9 J10 J11 J12 J13 J14 J15 J16 J17 J18 J19 J20 J21 J22 J23 J24 J25 J26 J27 J28 J29 J30 J31 J32";

            //DataTable tmeDt = new DataTable();
            //tmeDt = sltLMF.GetTempTimes;
            //List<string> TempTimeList = new List<string>();
            //foreach (DataRow dRow in tmeDt.Rows)
            //{
            //    DateTime tmpDate = Convert.ToDateTime(dRow["TempDate"]);
            //    TempTimeList.Add(tmpDate.ToString("HH:mm"));
            //}
            //string[] strDates = TempTimeList.ToArray();

            //DataTable tmpDt = sltLMF.GetTemps;
            //List<int> TempList = new List<int>();
            //foreach (DataRow tDt in tmpDt.Rows)
            //{
            //    TempList.Add(Convert.ToInt32(tDt["Temp"]));
            //}
            //int[] Temps = TempList.ToArray();

            //DataSeries ds1 = new DataSeries();
            //ds1.ValuesSource = Temps;
            //JomineyChart.Data.Children.Add(ds1);

            //RecordedTemps.ValuesSource = Temps;
        }
        #endregion

        private void End_Thread(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DoWork = false;
        }

        
    }
}
