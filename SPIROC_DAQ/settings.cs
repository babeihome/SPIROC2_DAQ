﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPIROC_DAQ
{
    static class settings
    {
        // CONFIG Variables - ID table
        public readonly static int TRIG_EXT = 0;// change frequently
        public readonly static int FLAG_TDC_EXT = 1;// change frequently
        public readonly static int START_RAMP_ADC_EXT = 2;
        public readonly static int START_RAMP_TDC_EXT = 3;
        public readonly static int ADC_GRAY = 4;
        public readonly static int CHIPID = 5;
        public readonly static int PROBE_OTA = 6;
        public readonly static int ENABLE_ANALOGUE_OUTPUT = 7;
        public readonly static int DISABLE_ANALOGUE_OUTPUT_PP = 8;
        public readonly static int NC = 9;
        public readonly static int EN_OR36 = 10;// change frequently
        public readonly static int ADC_RAMP_SLOPE = 11;
        public readonly static int ADC_RAMP_CURRENT_SOURCE = 12;
        public readonly static int ADC_RAMP_INTEGRATOR = 13;
        public readonly static int[] INDAC = new int[36] {14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,
        40,41,42,43,44,45,46,47,48,49};
        public readonly static int CAP_HG_PA_COMPENSATION = 50;
        public readonly static int NC2 = 51;
        public readonly static int FS = 52;
        public readonly static int NC3 = 53;
        public readonly static int CAP_LG_PA_COMPENSATION = 54;
        public readonly static int ENABLE_PREAMP_PP = 55;
        public readonly static int[] PREAMP_GAIN = new int[36] {56,57,58,59,60,61,62,63,64,65,66,67,68,69,70,71,72,73,74,75,76,77,
        78,79,80,81,82,83,84,85,86,87,88,89,90,91};
        public readonly static int ENABLE_LG_SS_FOLLOWER_PP = 92;
        public readonly static int LG_SS_TIME_CONSTANT = 93;// change frequently
        public readonly static int ENABLE_LG_SS_PP = 94;
        public readonly static int ENABLE_HG_SS_FOLLOWER_PP = 95;
        public readonly static int HG_SS_TIME_CONSTANT = 96;// change frequently
        public readonly static int ENABLE_HG_SS_PP = 97;
        public readonly static int FS_FOLLOWER_PP = 98;
        public readonly static int FS_PP = 99;
        public readonly static int BACKUP_SCA = 100; // change frequently
        public readonly static int SCA_PP = 101;
        public readonly static int EN_BANDGAP = 102;
        public readonly static int BANDGAP_PP = 103;
        public readonly static int EN_DAC = 104;
        public readonly static int DAC_PP = 105;
        public readonly static int TRIG_DAC = 106; // very important
        public readonly static int GAIN_DAC = 107;  // very important
        public readonly static int DELAY_START_RAMP_TDC_PP = 108;
        public readonly static int DELAY_START_RAMP_TDC = 109;
        public readonly static int TDC_RAMP_SLOPE_GC = 110;
        public readonly static int TDC_RAMP_PP = 111;
        public readonly static int ADC_DISCRI_PP = 112;
        public readonly static int GAIN_SELECT_DISCRI_PP = 113;
        public readonly static int AUTO_GAIN = 114; // change frequently
        public readonly static int GAIN_SELECT = 115; // change frequently
        public readonly static int ADC_EXT_INPUT = 116;
        public readonly static int SWITCH_TDC_ON = 117;
        public readonly static int DISCRIMINATOR_MASK1 = 118;
        public readonly static int DISCRIMINATOR_MASK2 = 119;
        public readonly static int NC4 = 120;
        public readonly static int DISCRI_DELAY_PP = 121;
        public readonly static int DELAY_TRIGGER = 122; // change frequently
        public readonly static int[] DISCRI_4BIT_ADJUST = new int[36] {123,124,125,126,127,128,129,130,131,132,133,134,135,136,137,
        138,139,140,141,142,143,144,145,146,147,148,149,150,151,152,153,154,155,156,157,158};
        public readonly static int ADJUST_4BIT_DAC = 159; // value is when select 'fine'
        public readonly static int DAC_4BIT_PP = 160;
        public readonly static int TRIG_DISCRI_PP = 161;
        public readonly static int DELAY_VALIDHOLD_PP = 162;
        public readonly static int DELAY_VALIDHOLD = 163;
        public readonly static int DELAY_RSTCOL_PP = 164;
        public readonly static int DELAY_RSTCOL = 165;
        public readonly static int CLOCK_LVDS_RECEIVE = 166;
        public readonly static int POD = 167;
        public readonly static int END_READOUT = 168;
        public readonly static int START_READOUT = 169;
        public readonly static int CHIPSAT = 170;
        public readonly static int TRANSMITON2 = 171;
        public readonly static int TRANSMITON1 = 172;
        public readonly static int DOUT2 = 173;
        public readonly static int DOUT1 = 174;

        public readonly static string DEFAULT_DIC = @"D:\Experiment_Data\SPIROC2b_xx\";
        public readonly static string AFG_DESCR = "USB[0-9]::0x0699::0x0345::C[0-9]+::INSTR";
        public readonly static int DURATION_SWEEP = 10 * 1000; // 10s for a voltage, unit is ms
    }
}
