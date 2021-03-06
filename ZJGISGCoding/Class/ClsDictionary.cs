﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZJGISGCoding
{
    /// <summary>
    /// 定义和初始化字典
    /// </summary>
    class ClsDictionary
    {
        #region
        /*
        public static Dictionary<int, string> SecondGridD1 = new Dictionary<int, string>
        {
            {00,"IN"},
            {01,"IO"},
            {02,"IP"},
            {03,"IQ"},
            {04,"IR"},
            {05,"IS"},
            {10,"JN"},
            {11,"JO"},
            {12,"JP"},
            {13,"JQ"},
            {14,"JR"},
            {15,"JS"},
            {20,"KN"},
            {21,"KO"},
            {22,"KP"},
            {23,"KQ"},
            {24,"KR"},
            {25,"KS"},
            {30,"LN"},
            {31,"LO"},
            {32,"LP"},
            {33,"LQ"},
            {34,"LR"},
            {35,"LS"}
        };
        public static Dictionary<int, string> SecondGridD2 = new Dictionary<int, string>
        {
            {00,"IF"},
            {01,"IG"},
            {02,"IH"},
            {03,"II"},
            {04,"IJ"},
            {05,"IK"},
            {10,"JF"},
            {11,"JG"},
            {12,"JH"},
            {13,"JI"},
            {14,"JJ"},
            {15,"JK"},
            {20,"KF"},
            {21,"KG"},
            {22,"KH"},
            {23,"KI"},
            {24,"KJ"},
            {25,"KK"},
            {30,"LF"},
            {31,"LG"},
            {32,"LH"},
            {33,"LI"},
            {34,"LJ"},
            {35,"LK"}
        };
        public static Dictionary<int, string> SecondGridD3 = new Dictionary<int, string>
        {
            {00,"DE"},
            {01,"DF"},
            {02,"DG"},
            {03,"DH"},
            {04,"DI"},
            {05,"DJ"},
            {06,"DK"},
            {07,"DL"},
            {10,"EE"},
            {11,"EF"},
            {12,"EG"},
            {13,"EH"},
            {14,"EI"},
            {15,"EJ"},
            {16,"EK"},
            {17,"EL"},
            {20,"FE"},
            {21,"FF"},
            {22,"FG"},
            {23,"FH"},
            {24,"FI"},
            {25,"FJ"},
            {26,"FK"},
            {27,"FL"},
            {30,"GE"},
            {31,"GF"},
            {32,"GG"},
            {33,"GH"},
            {34,"GI"},
            {35,"GJ"},
            {36,"GK"},
            {37,"GL"},
            {40,"(H)HE"},
            {41,"(H)HF"},
            {42,"(H)HG"},
            {43,"(H)HH"},
            {44,"(H)HI"},
            {45,"(H)HJ"},
            {46,"(H)HK"},
            {47,"(H)HL"},
            {50,"(H)HE"},
            {51,"(H)HF"},
            {52,"(H)HG"},
            {53,"(H)HH"},
            {54,"(H)HI"},
            {55,"(H)HJ"},
            {56,"(H)HK"},
            {57,"(H)HL"}
        };
        public static Dictionary<int, string> SecondGridD4 = new Dictionary<int, string>
        {
            {00,"DM"},
            {01,"DN"},
            {02,"DO"},
            {03,"DP"},
            {04,"DQ"},
            {05,"DR"},
            {06,"DS"},
            {07,"DT"},
            {10,"EM"},
            {11,"EN"},
            {12,"EO"},
            {13,"EP"},
            {14,"EQ"},
            {15,"ER"},
            {16,"ES"},
            {17,"ET"},
            {20,"FM"},
            {21,"FN"},
            {22,"FO"},
            {23,"FP"},
            {24,"FQ"},
            {25,"FR"},
            {26,"FS"},
            {27,"FT"},
            {30,"GM"},
            {31,"GN"},
            {32,"GO"},
            {33,"GP"},
            {34,"GQ"},
            {35,"GR"},
            {36,"GS"},
            {37,"GT"},
            {40,"(H)HM"},
            {41,"(H)HN"},
            {42,"(H)HO"},
            {43,"(H)HP"},
            {44,"(H)HQ"},
            {45,"(H)HR"},
            {46,"(H)HS"},
            {47,"(H)HT"},
            {50,"(H)HM"},
            {51,"(H)HN"},
            {52,"(H)HO"},
            {53,"(H)HP"},
            {54,"(H)HQ"},
            {55,"(H)HR"},
            {56,"(H)HS"},
            {57,"(H)HT"}
        };
         * */
        #endregion
        /// <summary>
        /// 二级格网第一象限
        /// </summary>
        public static Dictionary<int, string> SecondGridD1 = new Dictionary<int, string>
        {
            {00,"IN"},
            {10,"IO"},
            {20,"IP"},
            {30,"IQ"},
            {40,"IR"},
            {50,"IS"},
            {01,"JN"},
            {11,"JO"},
            {21,"JP"},
            {31,"JQ"},
            {41,"JR"},
            {51,"JS"},
            {02,"KN"},
            {12,"KO"},
            {22,"KP"},
            {32,"KQ"},
            {42,"KR"},
            {52,"KS"},
            {03,"LN"},
            {13,"LO"},
            {23,"LP"},
            {33,"LQ"},
            {43,"LR"},
            {53,"LS"}
        };
        /// <summary>
        /// 二级格网第二象限
        /// </summary>
        public static Dictionary<int, string> SecondGridD2 = new Dictionary<int, string>
        {
            {00,"IF"},
            {10,"IG"},
            {20,"IH"},
            {30,"II"},
            {40,"IJ"},
            {50,"IK"},
            {01,"JF"},
            {11,"JG"},
            {21,"JH"},
            {31,"JI"},
            {41,"JJ"},
            {51,"JK"},
            {02,"KF"},
            {12,"KG"},
            {22,"KH"},
            {32,"KI"},
            {42,"KJ"},
            {52,"KK"},
            {03,"LF"},
            {13,"LG"},
            {23,"LH"},
            {33,"LI"},
            {43,"LJ"},
            {53,"LK"}
        };
        /// <summary>
        /// 二级格网第三象限
        /// </summary>
        public static Dictionary<int, string> SecondGridD3 = new Dictionary<int, string>
        {
            {00,"DE"},
            {10,"DF"},
            {20,"DG"},
            {30,"DH"},
            {40,"DI"},
            {50,"DJ"},
            {60,"DK"},
            {70,"DL"},
            {01,"EE"},
            {11,"EF"},
            {21,"EG"},
            {31,"EH"},
            {41,"EI"},
            {51,"EJ"},
            {61,"EK"},
            {71,"EL"},
            {02,"FE"},
            {12,"FF"},
            {22,"FG"},
            {32,"FH"},
            {42,"FI"},
            {52,"FJ"},
            {62,"FK"},
            {72,"FL"},
            {03,"GE"},
            {13,"GF"},
            {23,"GG"},
            {33,"GH"},
            {43,"GI"},
            {53,"GJ"},
            {63,"GK"},
            {73,"GL"},
            {04,"(H)HE"},
            {14,"(H)HF"},
            {24,"(H)HG"},
            {34,"(H)HH"},
            {44,"(H)HI"},
            {54,"(H)HJ"},
            {64,"(H)HK"},
            {74,"(H)HL"},
            {05,"(H)HE"},
            {15,"(H)HF"},
            {25,"(H)HG"},
            {35,"(H)HH"},
            {45,"(H)HI"},
            {55,"(H)HJ"},
            {65,"(H)HK"},
            {75,"(H)HL"}
        };
        /// <summary>
        /// 二级格网第四象限
        /// </summary>
        public static Dictionary<int, string> SecondGridD4 = new Dictionary<int, string>
        {
            {00,"DM"},
            {10,"DN"},
            {20,"DO"},
            {30,"DP"},
            {40,"DQ"},
            {50,"DR"},
            {60,"DS"},
            {70,"DT"},
            {01,"EM"},
            {11,"EN"},
            {21,"EO"},
            {31,"EP"},
            {41,"EQ"},
            {51,"ER"},
            {61,"ES"},
            {71,"ET"},
            {02,"FM"},
            {12,"FN"},
            {22,"FO"},
            {32,"FP"},
            {42,"FQ"},
            {52,"FR"},
            {62,"FS"},
            {72,"FT"},
            {03,"GM"},
            {13,"GN"},
            {23,"GO"},
            {33,"GP"},
            {43,"GQ"},
            {53,"GR"},
            {63,"GS"},
            {73,"GT"},
            {04,"(H)HM"},
            {14,"(H)HN"},
            {24,"(H)HO"},
            {34,"(H)HP"},
            {44,"(H)HQ"},
            {54,"(H)HR"},
            {64,"(H)HS"},
            {74,"(H)HT"},
            {05,"(H)HM"},
            {15,"(H)HN"},
            {25,"(H)HO"},
            {35,"(H)HP"},
            {45,"(H)HQ"},
            {55,"(H)HR"},
            {65,"(H)HS"},
            {75,"(H)HT"}
        };
        /// <summary>
        /// 比例尺
        /// </summary>
        public static Dictionary<string, string> ScaleDic = new Dictionary<string, string>
         {
             {"1:10000","A"},
             {"1:50000","B"},
             {"1:250000","C"},
         };

    }
}
