using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using VibExchange.Models;


namespace VibExchange.Areas.RemoteAnalysis.Models
{
    public class RemoteAnalysisKohtect
    {
        public int FileID { get; set; }
        public string FilePath { get; set; }
        public string Channel1Input { get; set; }
        public string Channel2Input { get; set; }

        public int Channel1AmplifierMode { get; set; }
        public int Channel2AmplifierMode { get; set; }

        public int EnvelopingFrequency { get; set; }
        public int WindowType { get; set; }
        public int SpectralLines { get; set; }
        public float Fmin { get; set; }
        public float Fmax { get; set; }
        public int TriggerMode { get; set; }
        public int AverageMode { get; set; }
        public int Number { get; set; }
        public string Comments { get; set; }

        public int DataID { get; set; }
        public int PlantID { get; set; }
        public int AreaID { get; set; }
        public int TrainID { get; set; }
        public int MachineID { get; set; }
        public int PointID { get; set; }
        public int SetupID { get; set; }
        public int AlarmID { get; set; }
        public float Sensitivity1 { get; set; }
        public float Sensitivity2 { get; set; }
        public float RPM { get; set; }
        public DateTime MeasureTime { get; set; }

        public float OverallValueChnl1 { get; set; }
        public float OverallValueChnl2 { get; set; }
        public string OverallUnitChnl1 { get; set; }
        public string OverallUnitChnl2 { get; set; }

        public string Chnl1Time_X { get; set; }
        public string Chnl1Time_Y { get; set; }

        public string Chnl2Time_X { get; set; }
        public string Chnl2Time_Y { get; set; }

        public string Chnl1FFT_X { get; set; }
        public string Chnl1FFT_Y { get; set; }

        public string Chnl2FFT_X { get; set; }
        public string Chnl2FFT_Y { get; set; }

        public static string cPlantID, cAreaID, cTrainID, cMachineID, cPointID, cDataID, cSetupID;

        ///////////////////////////////////////////////////////////////////// Parameters ////////////////////////////////////////////

        public static string connval = "";
        public string sPathtosave = null;
        double[] XData = null;
        //double[] YData = null;
        public string[] sSplittedValue = null;
        public string filedate = null;

        //bool Is2Channel = false;
        public double[] YData { get; set; }
        public double[] YData2 { get; set; }
        public bool Is2Channel { get; set; }
        //{
        //    get
        //    {
        //        return Is2Channel;
        //    }
        //}

        float RMS = 0;
        public float _RMS
        {
            get
            {
                return RMS;
            }
            set
            {
                RMS = value;
            }
        }

        float RMS2 = 0;
        public float _RMS2
        {
            get
            {
                return RMS2;
            }
            set
            {
                RMS2 = value;
            }
        }

        float P_P = 0;
        public float _P_P
        {
            get
            {
                return P_P;
            }
            set
            {
                P_P = value;
            }
        }

        float P_P2 = 0;
        public float _P_P2
        {
            get
            {
                return P_P2;
            }
            set
            {
                P_P2 = value;
            }
        }
        double dF = 0;
        public double _dF
        {
            get
            {
                return dF;
            }
            set
            {
                dF = value;
            }
        }

        int Window = 0;
        public int _Window
        {
            get
            {
                return Window;
            }
            set
            {
                Window = value;
            }
        }

        int Window2 = 0;
        public int _Window2
        {
            get
            {
                return Window2;
            }
            set
            {
                Window2 = value;
            }
        }

        int pwr2 = 0;
        public int _pwr2
        {
            get
            {
                return pwr2;
            }
            set
            {
                pwr2 = value;
            }
        }

        int Measurement = 0;
        public int _Measurement
        {
            get
            {
                return Measurement;
            }
            set
            {
                Measurement = value;
            }
        }

        int Measurement2 = 0;
        public int _Measurement2
        {
            get
            {
                return Measurement2;
            }
            set
            {
                Measurement2 = value;
            }
        }

        int ChnlA = 0;
        public int _ChnlA
        {
            get
            {
                return ChnlA;
            }
            set
            {
                ChnlA = value;
            }
        }

        int ChnlB = 0;
        public int _ChnlB
        {
            get
            {
                return ChnlB;
            }
            set
            {
                ChnlB = value;
            }
        }

        int Trig = 0;
        public int _Trig
        {
            get
            {
                return Trig;
            }
            set
            {
                Trig = value;
            }
        }

        int Avgm = 0;
        public int _Avgm
        {
            get
            {
                return Avgm;
            }
            set
            {
                Avgm = value;
            }
        }


        int Navg = 0;
        public int _Navg
        {
            get
            {
                return Navg;
            }
            set
            {
                Navg = value;
            }
        }

        int EPC = 0;
        public int _EPC
        {
            get
            {
                return EPC;
            }
            set
            {
                EPC = value;
            }
        }

        enum ampmode { Mode_A, Mode_V, Mode_S, Mode_E };
        int Ampmode = 0;
        public int _Ampmode
        {
            get
            {
                return Ampmode;
            }
            set
            {
                Ampmode = value;
            }
        }

        int Ampmode2 = 0;
        public int _Ampmode2
        {
            get
            {
                return Ampmode2;
            }
            set
            {
                Ampmode2 = value;
            }
        }

        int Amphpf = 0;
        public int _Amphpf
        {
            get
            {
                return Amphpf;
            }
            set
            {
                Amphpf = value;
            }
        }

        int Amphpf2 = 0;
        public int _Amphpf2
        {
            get
            {
                return Amphpf2;
            }
            set
            {
                Amphpf2 = value;
            }
        }

        enum ampenvcr { KTu_2, KTu_4, KTu_8, KTu_16, KTu_32 };
        int Ampenvcr = 0;
        public int _Ampenvcr
        {
            get
            {
                return Ampenvcr;
            }
            set
            {
                Ampenvcr = value;
            }
        }

        int Ampenvcr2 = 0;
        public int _Ampenvcr2
        {
            get
            {
                return Ampenvcr2;
            }
            set
            {
                Ampenvcr2 = value;
            }
        }

        float Sens = 0;
        public float _Sens
        {
            get
            {
                return Sens;
            }
            set
            {
                Sens = value;
            }
        }

        float Sens2 = 0;
        public float _Sens2
        {
            get
            {
                return Sens2;
            }
            set
            {
                Sens2 = value;
            }
        }

        ulong SerialN = 0;
        public ulong _SerialN
        {
            get
            {
                return SerialN;
            }
            set
            {
                SerialN = value;
            }
        }

        int Revision = 0;
        public int _Revision
        {
            get
            {
                return Revision;
            }
            set
            {
                Revision = value;
            }
        }

        float HighestFrequency = 0;
        public float _highestFreq
        {
            get
            {
                return HighestFrequency;
            }
            set
            {
                HighestFrequency = value;
            }
        }

        int Number_Of_Spectrum = 0;
        public int _Number_Of_Spectrum
        {
            get
            {
                return Number_Of_Spectrum;
            }
            set
            {
                Number_Of_Spectrum = value;
            }
        }

        public bool FileDataSave(int fileID, string UserID, string filepath, string filetype)
        {
            try
            {
                bool status = false;
                string plantName, areaName, trainName, machineName, pointName = null;
                FileID = fileID;
                if (filetype == "Route")
                {
                    /////////////////////////////........Plant Level Starts ................//////////////////

                    string[] plantdirs = Directory.GetDirectories(filepath);
                    for (int i = 0; i < plantdirs.Length; i++)
                    {
                        string plant = plantdirs[i];
                        if (plant != null)
                        {
                            string[] pSplit = plant.Split(new char[] { '\\' });
                            plantName = pSplit[pSplit.Length - 1];
                            string plantID = FillPlantDetail(plantName, fileID);
                            cPlantID = plantID;
                            /////////////////////////////........Area Level Begin ................//////////////////

                            string[] areadirs = Directory.GetDirectories(plant);
                            for (int a = 0; a < areadirs.Length; a++)
                            {
                                string area = areadirs[a];
                                if (area != null)
                                {
                                    string[] aSplit = area.Split(new char[] { '\\' });
                                    areaName = Convert.ToString(aSplit[aSplit.Length - 1]);
                                    string areaeID = FillAreaDetail(areaName, fileID, plantID);
                                    cAreaID = areaeID;
                                    /////////////////////////////........Train Level Begin ................//////////////////

                                    string[] trainDirs = Directory.GetDirectories(area);
                                    for (int t = 0; t < trainDirs.Length; t++)
                                    {
                                        string train = trainDirs[t];
                                        if (train != null)
                                        {
                                            string[] tSplit = train.Split(new char[] { '\\' });
                                            trainName = tSplit[tSplit.Length - 1];
                                            string trainID = FillTrainDetail(trainName, fileID, areaeID, (Directory.GetDirectories(train).Length));
                                            cTrainID = trainID;
                                            /////////////////////////////........Machine Level Begin ................//////////////////

                                            string[] machineDirs = Directory.GetDirectories(train);
                                            for (int m = 0; m < machineDirs.Length; m++)
                                            {
                                                string machine = machineDirs[m];
                                                if (machine != null)
                                                {
                                                    string[] mSplit = machine.Split(new char[] { '\\' });
                                                    machineName = mSplit[mSplit.Length - 1];
                                                    string machineID = FillMachineDetail(machineName, fileID, trainID);
                                                    cMachineID = machineID;
                                                    /////////////////////////////........Point Level Begin ................//////////////////

                                                    string[] pointDirs = Directory.GetFiles(machine);

                                                    for (int p = 0; p < pointDirs.Length; p++)
                                                    {
                                                        string point = pointDirs[p];
                                                        if (point != null)
                                                        {
                                                            string[] pointSplit = point.Split(new string[] { "\\", ".fft" }, StringSplitOptions.RemoveEmptyEntries);
                                                            pointName = pointSplit[pointSplit.Length - 1];
                                                            using (FileStream objInput = new FileStream(point, FileMode.Open, FileAccess.Read))
                                                            {
                                                                byte[] MainArr = new byte[(int)objInput.Length];
                                                                int contents = objInput.Read(MainArr, 0, (int)objInput.Length);
                                                                ExtractData(MainArr);
                                                                string[] arrFilePath = point.ToString().Split(new string[] { "\\", ".fft" }, StringSplitOptions.RemoveEmptyEntries);
                                                                MeasureTime = File.GetCreationTime(filepath);
                                                                string pointID = FillPointDetail(pointName, fileID, cMachineID, UserID);
                                                                if (pointID != null)
                                                                {
                                                                    status = FillRecordedData(fileID, UserID, pointID);
                                                                }
                                                            }
                                                        }
                                                    }
                                                    /////////////////////////////........Point Level End ................//////////////////
                                                }
                                            }
                                            /////////////////////////////........Machine Level End ................//////////////////
                                        }
                                    }
                                    /////////////////////////////........Train Level End ................//////////////////
                                }
                            }
                            /////////////////////////////........Area Level End ................//////////////////
                        }
                    }
                    /////////////////////////////........Plant Level End ................//////////////////
                }
                else
                {
                    using (FileStream objInput = new FileStream(filepath, FileMode.Open, FileAccess.Read))
                    {
                        byte[] MainArr = new byte[(int)objInput.Length];
                        int contents = objInput.Read(MainArr, 0, (int)objInput.Length);
                        ExtractData(MainArr);
                        string[] arrFilePath = filepath.ToString().Split(new string[] { "\\", ".fft" }, StringSplitOptions.RemoveEmptyEntries);
                        string fileName = arrFilePath[arrFilePath.Length - 1];
                        MeasureTime = File.GetCreationTime(filepath);
                        string PointID = FillRouteDetail(fileName);
                        status = FillRecordedData(fileID, UserID, PointID);
                    }
                }
                return status;
            }
            catch (Exception e) { throw e; }
        }

        private string FillPlantDetail(string plantName, int fileID)
        {
            string NewPlantID = null;
            int i;
            try
            {
                using (DBClass context = new DBClass())
                {
                    context.AddParameter("@FileID", fileID);
                    context.AddParameter("@PlantName", plantName);
                    i = context.ExecuteNonQuery("AddPlantInRoute", CommandType.StoredProcedure);
                }
                if (i > 0)
                {
                    using (DBClass context = new DBClass())
                    {
                        context.AddParameter("@FileID", fileID);
                        DataTable dtPlant = context.getData("GetPlantIDForCurrentRoute", CommandType.StoredProcedure);
                        if (dtPlant.Rows.Count > 0)
                        {
                            NewPlantID = Convert.ToString(dtPlant.Rows[0]["NodeID"]);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return NewPlantID;
        }

        private string FillAreaDetail(string areaName, int fileID, string PID)
        {
            string NewAreaID = null;
            int a;
            try
            {
                using (DBClass context = new DBClass())
                {
                    context.AddParameter("@FileID", fileID);
                    context.AddParameter("@PID", PID);
                    context.AddParameter("@AreaName", areaName);
                    a = context.ExecuteNonQuery("AddAreaInRoute", CommandType.StoredProcedure);
                }
                if (a > 0)
                {
                    using (DBClass context = new DBClass())
                    {
                        context.AddParameter("@FileID", fileID);
                        DataTable dtArea = context.getData("GetAreaIDForCurrentRoute", CommandType.StoredProcedure);
                        if (dtArea.Rows.Count > 0)
                        {
                            NewAreaID = Convert.ToString(dtArea.Rows[0]["NodeID"]);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw new NotImplementedException();
            }
            return NewAreaID;
        }

        private string FillTrainDetail(string trainName, int fileID, string AID, int NoofMachine)
        {
            string NewTrainID = null;
            int t;
            try
            {
                using (DBClass context = new DBClass())
                {
                    context.AddParameter("@FileID", fileID);
                    context.AddParameter("@AID", AID);
                    context.AddParameter("@TrainName", trainName);
                    context.AddParameter("@NoofMachine", NoofMachine);
                    t = context.ExecuteNonQuery("AddTrainInRoute", CommandType.StoredProcedure);
                }
                if (t > 0)
                {
                    using (DBClass context = new DBClass())
                    {
                        context.AddParameter("@FileID", fileID);
                        DataTable dtTrain = context.getData("GetTrainIDForCurrentRoute", CommandType.StoredProcedure);
                        if (dtTrain.Rows.Count > 0)
                        {
                            NewTrainID = Convert.ToString(dtTrain.Rows[0]["NodeID"]);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw new NotImplementedException();
            }
            return NewTrainID;
        }

        private string FillMachineDetail(string machineName, int fileID, string TID)
        {
            string NewMachineID = null;
            int m;
            try
            {
                using (DBClass context = new DBClass())
                {
                    context.AddParameter("@FileID", fileID);
                    context.AddParameter("@TID", TID);
                    context.AddParameter("@MachineName", machineName);
                    m = context.ExecuteNonQuery("AddMachineInRoute", CommandType.StoredProcedure);
                }
                if (m > 0)
                {
                    using (DBClass context = new DBClass())
                    {
                        context.AddParameter("@FileID", fileID);
                        DataTable dtMachine = context.getData("GetMachineIDForCurrentRoute", CommandType.StoredProcedure);
                        if (dtMachine.Rows.Count > 0)
                        {
                            NewMachineID = Convert.ToString(dtMachine.Rows[0]["NodeID"]);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return NewMachineID;
        }

        private string FillPointDetail(string pointName, int fileID, string MID, string userid)
        {
            string NewPointID = null;
            int p;
            try
            {
                using (DBClass context = new DBClass())
                {
                    context.AddParameter("@UserID", userid);
                    context.AddParameter("@FileID", fileID);
                    context.AddParameter("@PlantID", cPlantID);
                    context.AddParameter("@AreaID", cAreaID);
                    context.AddParameter("@TrainID", cTrainID);
                    context.AddParameter("@MachineID", MID);
                    context.AddParameter("@PoinName", pointName);
                    p = context.ExecuteNonQuery("AddPointInRoute", CommandType.StoredProcedure);
                }
                if (p > 0)
                {
                    using (DBClass context = new DBClass())
                    {
                        context.AddParameter("@FileID", fileID);
                        DataTable dtPoint = context.getData("GetPointIDForCurrentRoute", CommandType.StoredProcedure);
                        if (dtPoint.Rows.Count > 0)
                        {
                            NewPointID = Convert.ToString(dtPoint.Rows[0]["NodeID"]);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw new NotImplementedException();
            }
            return NewPointID;
        }

        public void ExtractData(byte[] MainArr)
        {
            int CtrToStart = 0;
            byte[] fs = new byte[2];
            Is2Channel = false;
            try
            {
                //Reading Buffer  cnt
                fs[0] = MainArr[CtrToStart];
                fs[1] = MainArr[CtrToStart + 1];
                string byteval = fs[0].ToString() + fs[1].ToString();
                int ival = CommonConversions.HexadecimaltoDecimal(byteval);
                int BufferCNT = ival;

                //Reading buf1  --- 1 buffer size (currently 238 bytes)
                CtrToStart = 2;
                fs = new byte[2];
                fs[0] = MainArr[CtrToStart];
                fs[1] = MainArr[CtrToStart + 1];
                byteval = CommonConversions.DeciamlToHexadeciaml1(Convert.ToInt32(fs[0].ToString())) + CommonConversions.DeciamlToHexadeciaml1(Convert.ToInt32(fs[1].ToString()));
                ival = CommonConversions.HexadecimaltoDecimal(byteval);
                int Buf1 = ival;
                //int Buf1 = 238;

                //Reading buf2  --- 2 buffer size depends on the count of the spectral lines or sample length * (t)
                CtrToStart = 4;
                fs = new byte[2];
                fs[0] = MainArr[CtrToStart];
                fs[1] = MainArr[CtrToStart + 1];
                byteval = CommonConversions.DeciamlToHexadeciaml1(Convert.ToInt32(fs[0].ToString())) + CommonConversions.DeciamlToHexadeciaml1(Convert.ToInt32(fs[1].ToString()));
                ival = CommonConversions.HexadecimaltoDecimal(byteval);
                int Buf2 = ival;

                //Reading buf3  ---=0  if one channel---- 3 buffer size depends on the count of the spectral lines or sample length * (t)
                CtrToStart = 6;
                fs = new byte[2];
                fs[0] = MainArr[CtrToStart];
                fs[1] = MainArr[CtrToStart + 1];
                byteval = CommonConversions.DeciamlToHexadeciaml1(Convert.ToInt32(fs[0].ToString())) + CommonConversions.DeciamlToHexadeciaml1(Convert.ToInt32(fs[1].ToString()));
                ival = CommonConversions.HexadecimaltoDecimal(byteval);
                int Buf3 = ival;
                if (Buf3 > 0)
                {
                    this.Is2Channel = true;
                }

                //Reading LinesFFT  ---100, 200, 400, 800, 1600, 3200, 6400 ---- The number of spectral lines () - throwback - can take the value of the structure device [ ]
                CtrToStart = 8;
                fs = new byte[2];
                fs[0] = MainArr[CtrToStart];
                fs[1] = MainArr[CtrToStart + 1];
                byteval = CommonConversions.DeciamlToHexadeciaml1(Convert.ToInt32(fs[0].ToString())) + CommonConversions.DeciamlToHexadeciaml1(Convert.ToInt32(fs[1].ToString()));
                ival = CommonConversions.HexadecimaltoDecimal(byteval);
                SpectralLines = ival;

                //Reading device[238 byte]  ---             //Reading device[238 byte]  --- 100, 200, 400, 800, 1600, 3200, 6400 ---- The number of spectral lines () - throwback - can take the value of the structure device [ ]
                CtrToStart = 10;
                fs = new byte[238];
                byteval = null;
                //int[] devicedata = new int[238];
                byte[] devicedata = new byte[238];
                for (int i = 0; i < 238; i++, CtrToStart++)
                {
                    devicedata[i] = MainArr[CtrToStart];
                }

                GetDevicestructure(devicedata);

                //Reading ch1 float FFT[size] or int   F(t)[size] ---- CH1 or range of functions. time
                CtrToStart = 248;
                fs = new byte[Buf2];
                byteval = null;
                YData = new double[Number_Of_Spectrum];
                for (int i = 0; i < Number_Of_Spectrum; i++)
                {
                    float fabc = BytetoFloat(MainArr, CtrToStart);
                    YData[i] = Convert.ToDouble(fabc);
                    //CH1.Add(fabc);
                    CtrToStart += 4;

                }
                if (Is2Channel)
                {
                    //Reading ch2 float FFT[size] or int   F(t)[size] ---- Channel2 range or function. time
                    CtrToStart = 248 + Buf2;
                    fs = new byte[Buf3];
                    byteval = null;
                    YData2 = new double[Number_Of_Spectrum];
                    for (int i = 0; i < Number_Of_Spectrum; i++)
                    {
                        float fabc = BytetoFloat(MainArr, CtrToStart);
                        YData2[i] = Convert.ToDouble(fabc);
                        CtrToStart += 4;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void GetDevicestructure(byte[] devicedata)
        {
            int ctrStructure = 0;
            try
            {
                //Read RMS
                _RMS = BytetoFloat(devicedata, ctrStructure);
                if (Is2Channel)
                {
                    ctrStructure = 4;
                    _RMS2 = BytetoFloat(devicedata, ctrStructure);
                }

                //Read P_P
                ctrStructure = 12;
                _P_P = BytetoFloat(devicedata, ctrStructure);
                if (Is2Channel)
                {
                    ctrStructure = 16;
                    _P_P2 = BytetoFloat(devicedata, ctrStructure);
                }

                //Read dF
                ctrStructure = 24;
                _dF = Math.Round(Convert.ToDouble(BytetoFloat(devicedata, ctrStructure)), 3);

                //Read Window
                ctrStructure = 54;
                _Window = Convert.ToInt32(devicedata[ctrStructure].ToString());
                if (Is2Channel)
                {
                    ctrStructure = 55;
                    _Window2 = Convert.ToInt32(devicedata[ctrStructure].ToString());
                }

                //Read pwr2
                ctrStructure = 57;
                _pwr2 = Convert.ToInt32(devicedata[ctrStructure].ToString());

                //Read Measurement
                ctrStructure = 58;
                _Measurement = Convert.ToInt32(devicedata[ctrStructure].ToString());
                if (Is2Channel)
                {
                    ctrStructure = 59;
                    _Measurement2 = Convert.ToInt32(devicedata[ctrStructure].ToString());
                }

                //Read channel A
                ctrStructure = 61;
                _ChnlA = Convert.ToInt32(devicedata[ctrStructure].ToString());

                if (_Measurement == 0) // For Time
                {
                    Number_Of_Spectrum = 1 << _pwr2;
                }
                else if (_Measurement == 1) // For FFT
                {
                    Number_Of_Spectrum = (1 << (_pwr2 - 6)) * 25;
                }

                HighestFrequency = (float)(dF * Number_Of_Spectrum);
                XData = new double[Number_Of_Spectrum];
                for (int i = 0; i < Number_Of_Spectrum; i++)
                {
                    XData[i] = Convert.ToDouble(dF * i);
                }

                //Read channel B
                ctrStructure = 62;
                _ChnlB = Convert.ToInt32(devicedata[ctrStructure].ToString());


                //Read Trigger
                ctrStructure = 63;
                _Trig = Convert.ToInt32(devicedata[ctrStructure].ToString());


                //Read Averaging Mode
                ctrStructure = 64;
                _Avgm = Convert.ToInt32(devicedata[ctrStructure].ToString());


                //Read Averaging Number
                ctrStructure = 65;
                _Navg = Convert.ToInt32(devicedata[ctrStructure].ToString());


                //Read EPC
                ctrStructure = 66;
                _EPC = Convert.ToInt32(devicedata[ctrStructure].ToString());

                //Read Amplifier Mode
                ctrStructure = 70;
                _Ampmode = Convert.ToInt32(devicedata[ctrStructure].ToString());
                if (Is2Channel)
                {
                    ctrStructure = 71;
                    _Ampmode2 = Convert.ToInt32(devicedata[ctrStructure].ToString());
                }

                //Read Low Frequency Cut Off
                ctrStructure = 76;
                _Amphpf = Convert.ToInt32(devicedata[ctrStructure].ToString());
                if (Is2Channel)
                {
                    ctrStructure = 77;
                    _Amphpf2 = Convert.ToInt32(devicedata[ctrStructure].ToString());
                }

                //Read Carrier for channel
                ctrStructure = 79;
                _Ampenvcr = Convert.ToInt32(devicedata[ctrStructure].ToString());
                if (Is2Channel)
                {
                    ctrStructure = 80;
                    _Ampenvcr2 = Convert.ToInt32(devicedata[ctrStructure].ToString());
                }

                //Read Transducer factor/Sensitivity
                ctrStructure = 90;
                _Sens = BytetoFloat(devicedata, ctrStructure);
                if (Is2Channel)
                {
                    ctrStructure = 94;
                    _Sens2 = BytetoFloat(devicedata, ctrStructure);
                }
                ctrStructure = 226;
                ulong serialNumber = Bytetoulong(devicedata, ctrStructure);
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>    
        /// To fill all the data for hierarchy in the database on the basis of uploaded file. 
        /// It uses stored procedure AddPlant,AddArea,AddTrain,AddMachine and AddPoint respectivly with all required parameters.
        /// </summary>
        public string FillRouteDetail(string filename)
        {
            string PID = null;
            try
            {
                string[] fname = filename.Split(new string[] { "@" }, StringSplitOptions.RemoveEmptyEntries);
                using (DBClass _dbConnection = new DBClass())
                {
                    _dbConnection.AddParameter("@FileID", FileID);
                    _dbConnection.AddParameter("@NoofMachine", 1);
                    _dbConnection.AddParameter("@FileName", fname[fname.Length - 1]);
                    _dbConnection.ExecuteNonQuery("AddRouteData", CommandType.StoredProcedure);
                }
                using (DBClass context = new DBClass())
                {
                    context.AddParameter("@FileID", FileID);
                    DataTable dt = context.getData("GetPointIDForCurrentRoute", CommandType.StoredProcedure);
                    PID = Convert.ToString(dt.Rows[0]["NodeID"]);
                }
            }
            catch { }
            return PID;
        }

        /// <summary>
        /// This function is used to send recorede data to the database after reading a file that uploaded by the user.
        /// </summary>
        public bool FillRecordedData(int Fileid, string UserID, string PointID)
        {
            bool status = false;
            string Pid = null;
            try
            {
                ChnlA = _ChnlA;
                ChnlB = _ChnlB;
                Channel1AmplifierMode = _Ampmode;
                Channel2AmplifierMode = _Ampmode2;
                EnvelopingFrequency = _Ampenvcr;
                WindowType = _Window;
                Fmin = _Amphpf;
                Fmax = HighestFrequency;
                TriggerMode = _Trig;
                AverageMode = _Avgm;
                Number = _Navg;
                Comments = null;
                StringBuilder DataXY = new StringBuilder();
                StringBuilder DataXY2 = new StringBuilder();
                StringBuilder DataX = new StringBuilder();
                StringBuilder DataY = new StringBuilder();
                StringBuilder DataX2 = new StringBuilder();
                StringBuilder DataY2 = new StringBuilder();
                ArrayList arlNewTime = new ArrayList();
                string XYFinalData = null;
                string ValueX = ""; string ValueY = "";
                string XY2FinalData = null;
                string ValueX2 = ""; string ValueY2 = "";
                string[] sarrXTData = new string[0];
                DataXY.Append("0|0,");
                DataX.Append("0");
                DataX.Append("|");
                DataY.Append("0");
                DataY.Append("|");
                for (int i = 1; i < XData.Length; i++)
                {
                    DataXY.Append(Convert.ToString(Math.Round(XData[i], 2)));
                    DataXY.Append("|");
                    DataX.Append(Convert.ToString(Math.Round(XData[i], 2)));
                    DataX.Append("|");
                    DataXY.Append(Convert.ToString(Math.Round(YData[i], 3)));
                    DataXY.Append(",");
                    DataY.Append(Convert.ToString(Math.Round(YData[i], 3)));
                    DataY.Append("|");
                }
                XYFinalData = Convert.ToString(DataXY);
                ValueX = Convert.ToString(DataX);
                ValueY = Convert.ToString(DataY);
                if (Is2Channel)
                {
                    DataXY2.Append("0|0,");
                    DataX2.Append("0");
                    DataX2.Append("|");
                    DataY2.Append("0");
                    DataY2.Append("|");
                    for (int i = 1; i < XData.Length; i++)
                    {
                        DataXY2.Append(Convert.ToString(XData[i]));
                        DataXY2.Append("|");
                        DataX2.Append(Convert.ToString(XData[i]));
                        DataX2.Append("|");
                        DataXY2.Append(Convert.ToString(YData2[i]));
                        DataXY2.Append(",");
                        DataY2.Append(Convert.ToString(YData2[i]));
                        DataY2.Append("|");
                    }
                }
                XY2FinalData = Convert.ToString(DataXY2);
                ValueX2 = Convert.ToString(DataX2);
                ValueY2 = Convert.ToString(DataY2);

                Chnl1FFT_X = ValueX;
                Chnl1FFT_Y = ValueY;
                Chnl2FFT_X = ValueX2;
                Chnl2FFT_Y = ValueY2;
                if (Channel1AmplifierMode == 0)
                {
                    OverallValueChnl1 = _P_P;
                    OverallUnitChnl1 = "m/s2";
                }
                else if (Channel1AmplifierMode == 1)
                {
                    OverallValueChnl1 = _RMS;
                    OverallUnitChnl1 = "mm/sec";
                }
                if (Channel2AmplifierMode == 0)
                {
                    OverallValueChnl2 = _RMS2;
                    OverallUnitChnl2 = "mm/sec";
                }
                else if (Channel2AmplifierMode == 1)
                {
                    OverallValueChnl2 = _P_P2;
                    OverallUnitChnl2 = "m/s2";
                }
                string mTime = Convert.ToString(MeasureTime);

                //using (DBClass context = new DBClass())
                //{
                //    Pid = context.getPointID(FileID, UserID);
                //}
                using (DBClass context = new DBClass())
                {

                    context.AddParameter("@Fileid", FileID);
                    context.AddParameter("@Userid", UserID);
                    context.AddParameter("@Alarmid", AlarmID);
                    context.AddParameter("@ChnlA", ChnlA);
                    context.AddParameter("@Radio1", Ampmode);
                    context.AddParameter("@ChnlB", ChnlB);
                    context.AddParameter("@Radio2", Ampmode2);
                    context.AddParameter("@EVfreq", EnvelopingFrequency);
                    context.AddParameter("@SLine", SpectralLines);
                    context.AddParameter("@WType", WindowType);
                    context.AddParameter("@Fmin", Fmin);
                    context.AddParameter("@Fmax", Fmax);
                    context.AddParameter("@TMode", TriggerMode);
                    context.AddParameter("@AMode", AverageMode);
                    context.AddParameter("@N", Navg);
                    context.AddParameter("@Comment", Comments);
                    context.AddParameter("@PointID", PointID);
                    context.AddParameter("@MTime", Convert.ToString(MeasureTime));
                    context.AddParameter("@OverallValueChnlA", OverallValueChnl1);
                    context.AddParameter("@OvUnitChnlA", OverallUnitChnl1);
                    context.AddParameter("@OverallValueChnlB", OverallValueChnl2);
                    context.AddParameter("@OvUnitChnlB", OverallUnitChnl2);
                    context.AddParameter("@Chnl1FFTX", Chnl1FFT_X);
                    context.AddParameter("@Chnl1FFTY", Chnl1FFT_Y);
                    context.AddParameter("@Chnl2FFTX", Chnl2FFT_X);
                    context.AddParameter("@Chnl2FFTY", Chnl2FFT_Y);
                    int i = context.ExecuteNonQuery("addRecordedDataForKohtect", CommandType.StoredProcedure);
                    if (i > 0)
                    {
                        status = true;
                    }
                }

            }
            catch (Exception e) { throw e; }
            return status;
        }

        private float BytetoFloat(byte[] MainArr, int CtrToStart)
        {
            float returnfloat = 0;
            try
            {
                byte[] newbyte = new byte[4];
                newbyte[0] = MainArr[CtrToStart];
                newbyte[1] = MainArr[CtrToStart + 1];
                newbyte[2] = MainArr[CtrToStart + 2];
                newbyte[3] = MainArr[CtrToStart + 3];

                newbyte = newbyte.Reverse().ToArray();

                returnfloat = BitConverter.ToSingle(newbyte, 0);
            }
            catch
            {
            }
            return returnfloat;
        }

        private ulong Bytetoulong(byte[] MainArr, int CtrToStart)
        {
            ulong returnval = 0;
            try
            {
                byte[] newbyte = new byte[8];
                for (int i = 0; i < 8; i++)
                {
                    newbyte[i] = MainArr[CtrToStart + i];
                }

                newbyte = newbyte.Reverse().ToArray();

                returnval = BitConverter.ToUInt64(newbyte, 0);
            }
            catch
            {
            }
            return returnval;
        }

        public List<ResponseClass> ReturnData { get; set; }
    }

    public class ResponseClass
    {
        public bool Status { get; set; }
        public int FileID { get; set; }
        public string UserID { get; set; }
        public DateTime MeasurementTime { get; set; }
        public DateTime UploadTime { get; set; }
    }
}