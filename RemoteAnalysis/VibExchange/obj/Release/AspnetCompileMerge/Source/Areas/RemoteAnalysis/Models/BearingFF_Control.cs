using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VibExchange.Areas.RemoteAnalysis.Models
{
    class BearingFF_Control : BearingFF_Interface
    {
        double NumberOfBalls = 0;
        double BearingPitchDiameter = 0;
        double RollingElementDiameter = 0;
        double ContactAngle = 0;
        double ShaftSpeed = 0;
        double BPFO = 0;
        double BPFI = 0;
        double BSF = 0;
        double FTF = 0;

        #region BearingFF_Interface Members

        public double _NumberOfBalls
        {
            set { NumberOfBalls = value; }
        }

        public double _BearingPitchDiameter
        {
            set { BearingPitchDiameter = value; }
        }

        public double _RollingElementDiameter
        {
            set { RollingElementDiameter = value; }
        }

        public double _ContactAngle
        {
            set { ContactAngle = value; }
        }

        public double _ShaftSpeed
        {
            set { ShaftSpeed = value; }
        }

        public double _BPFO
        {
            get { return BPFO; }
        }

        public double _BPFI
        {
            get { return BPFI; }
        }

        public double _BSF
        {
            get { return BSF; }
        }

        public double _FTF
        {
            get { return FTF; }
        }

        public void CalculateBearingFaultFrequencies()
        {
            try
            {
                double CosAConstant = (double)((RollingElementDiameter / BearingPitchDiameter) * Math.Cos(ContactAngle));//(d/D)*Cos(a)

                BPFO = (double)(((ShaftSpeed * NumberOfBalls) / 2) * (1 - CosAConstant));
                BPFI = (double)(((ShaftSpeed * NumberOfBalls) / 2) * (1 + CosAConstant));
                BSF = (double)((ShaftSpeed / 2) * (BearingPitchDiameter / RollingElementDiameter) * (1 - Math.Pow(CosAConstant, 2)));
                FTF = (double)((ShaftSpeed / 2) * (1 - CosAConstant));
            }
            catch (Exception ex)
            {
                //ErrorLog_Class.ErrorLogEntry(ex);
            }
        }

        public ArrayList CalculateBearingFaultFrequencies(double ShaftSpeed, double NumberOfBalls, double BearingPitchDiameter, double RollingElementDiameter, double ContactAngle)
        {
            ArrayList ReturnValue = new ArrayList();
            try
            {
                double CosAConstant = (double)((RollingElementDiameter / BearingPitchDiameter) * Math.Cos(ContactAngle));//(d/D)*Cos(a)

                BPFO = (double)(((ShaftSpeed * NumberOfBalls) / 2) * (1 - CosAConstant));
                BPFI = (double)(((ShaftSpeed * NumberOfBalls) / 2) * (1 + CosAConstant));
                BSF = (double)((ShaftSpeed / 2) * (BearingPitchDiameter / RollingElementDiameter) * (1 - Math.Pow(CosAConstant, 2)));
                FTF = (double)((ShaftSpeed / 2) * (1 - CosAConstant));
                ReturnValue.Add("BPFO = " + (BPFO / 60).ToString());
                ReturnValue.Add("BPFI = " + (BPFI / 60).ToString());
                ReturnValue.Add("BSF = " + (BSF / 60).ToString());
                ReturnValue.Add("FTF = " + (FTF / 60).ToString());
            }
            catch (Exception ex)
            {
                //ErrorLog_Class.ErrorLogEntry(ex);
            }
            return ReturnValue;
        }



        #endregion
    }
}