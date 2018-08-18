using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using VibExchange.Models;

namespace VibExchange.Areas.RemoteAnalysis.Models
{
    public class ChartData
    {
        public static ArrayList GetData(string nodeid, string userid, ArrayList BearingFrequency)
        {
            ArrayList xyData = new ArrayList();
            try
            {
                double[] Xdoubles = new double[1];
                double[] Ydoubles = new double[1];
                //string[] ffData = getFaultFrequencyData(nodeid, userid);//used for frequency name with value..

                //////////////  Chages for bearing fault frequency /////////////////////

                double[] bearingFrequency = new double[BearingFrequency.Count];
                for (int i = 0; i < BearingFrequency.Count; i++)
                {
                    string[] bff = (Convert.ToString(BearingFrequency[i])).Split(new char[] { '=' });
                    bearingFrequency[i] = Convert.ToDouble(bff[1]);
                }
                double[] faultFrequency = getFaultFrequencyData(nodeid, userid);
                using (DBClass context = new DBClass())
                {
                    context.AddParameter("@Nodeid", nodeid);
                    DataTable dtData = context.getData("GetRecordedDataByPointID", CommandType.StoredProcedure);
                    foreach (DataRow dr in dtData.Rows)
                    {
                        string[] xData = Convert.ToString(dr["Chnl1FFT_X"]).Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                        string[] yData = Convert.ToString(dr["Chnl1FFT_Y"]).Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                        Array.Resize(ref Xdoubles, xData.Length);
                        Array.Resize(ref Ydoubles, xData.Length);
                        Xdoubles = Array.ConvertAll(xData, new Converter<string, double>(Double.Parse));
                        Ydoubles = Array.ConvertAll(yData, new Converter<string, double>(Double.Parse));
                        ChartData.xAxisUnit = "Frequency-Hz";
                        ChartData.yAxisUnit = Convert.ToString(dr["OverallUnitChnlA"]);
                        if (ChartData.yAxisUnit == "mm/sec" || ChartData.yAxisUnit == "inch/sec")
                        {
                            ChartData.amplitudeUnit = "Velocity ( " + ChartData.yAxisUnit + " )";
                        }
                        if (ChartData.yAxisUnit == "G" || ChartData.yAxisUnit == "m/s2")
                        {
                            ChartData.amplitudeUnit = "Acceleration ( " + ChartData.yAxisUnit + " )";
                        }
                        if (ChartData.yAxisUnit == "um" || ChartData.yAxisUnit == "mil")
                        {
                            ChartData.amplitudeUnit = "Displacement ( " + ChartData.yAxisUnit + " )";
                        }

                        ChartData.overallValue = Convert.ToDouble(dr["OverallValueChnlA"]);
                    }
                }
                xyData.Add(Xdoubles);
                xyData.Add(Ydoubles);
                xyData.Add(FindNearestPointIndex(faultFrequency, Xdoubles));
                xyData.Add(FindNearestPointIndex(bearingFrequency, Xdoubles));
            }
            catch (Exception ex) { throw ex; }
            return xyData;
        }
        public static double[] FindNearestPointIndex(double[] actualvalue, double[] xData)
        {
            double[] pointIndex = new double[actualvalue.Length];
            double minDistance = 0;
            int minIndex;
            double lValue = 0, rValue = 0, diff;
            for (int j = 0; j < actualvalue.Length; j++)
            {
                minIndex = Array.IndexOf(xData, actualvalue[j]);
                if (minIndex == -1)
                {
                    for (int i = 0; i < xData.Length; i++)
                    {
                        diff = xData[i] - actualvalue[j];
                        if (diff >= 0)
                        {
                            if (i == 0) { lValue = xData[i]; }
                            else { lValue = xData[i - 1]; }
                            lValue = xData[i - 1];
                            rValue = xData[i];
                            break;
                        }
                    }
                    if (Math.Abs(actualvalue[j] - lValue) > Math.Abs(actualvalue[j] - rValue))
                    {
                        minIndex = Array.IndexOf(xData, rValue);
                    }
                    else
                    {
                        minIndex = Array.IndexOf(xData, lValue);
                    }
                    pointIndex[j] = minIndex;
                }
                else
                {
                    pointIndex[j] = minIndex;
                }


            }
            return pointIndex;
        }

        public static double[] getFaultFrequencyData(string Pointid, string Userid)
        {
            double[] lstFF = new double[1];
            try
            {
                using (DBClass context = new DBClass())
                {
                    context.AddParameter("@PointID", Pointid);
                    context.AddParameter("@UserName", Userid);
                    DataTable dtFF = context.getData("getFalutFrequencyListbyPointID", CommandType.StoredProcedure);
                    Array.Resize(ref lstFF, dtFF.Rows.Count);
                    if (dtFF.Rows.Count > 0)
                    {
                        int i = 0;
                        foreach (DataRow dr in dtFF.Rows)
                        {
                            lstFF[i] = Convert.ToDouble(dr["FrequencyValue"]);
                            i++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstFF;
        }

        public ChartData(double xAxis, double yAxis)
        {
            this.xAxis = xAxis;
            this.yAxis = yAxis;
        }
        public double xAxis { get; set; }
        public double yAxis { get; set; }
        public static string NodeID { get; set; }
        public static string xAxisUnit { get; set; }
        public static string yAxisUnit { get; set; }
        public static string amplitudeUnit { get; set; }
        public static double overallValue { get; set; }

    }
}