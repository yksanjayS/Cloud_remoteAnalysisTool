using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibExchange.Areas.RemoteAnalysis.Models
{
    interface BearingFF_Interface
    {
        double _NumberOfBalls
        {
            set;
        }
        double _BearingPitchDiameter
        {
            set;
        }
        double _RollingElementDiameter
        {
            set;
        }
        double _ContactAngle
        {
            set;
        }
        double _ShaftSpeed
        {
            set;
        }

        double _BPFO
        {
            get;
        }
        double _BPFI
        {
            get;
        }
        double _BSF
        {
            get;
        }
        double _FTF
        {
            get;
        }


        void CalculateBearingFaultFrequencies();
        ArrayList CalculateBearingFaultFrequencies(double ShaftSpeed, double NumberOfBalls, double BearingPitchDiameter, double RollingElementDiameter, double ContactAngle);
    }
}
