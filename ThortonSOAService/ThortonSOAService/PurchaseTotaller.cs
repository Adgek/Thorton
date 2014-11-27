using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThortonSOAService
{
    class PurchaseTotaller
    {
        private readonly string[] provinceCode = new string[] { "NL", "NS", "NB", "PE", "QC", "ON", "MB", "SK", "AB", "BC", "YT", "NT", "NU" };
        private readonly double[,] gsthstpst = new double[,] 
            { 
            { 0,        0.13,   0 },    //NL
            { 0,        0.15,   0 },    //NS
            { 0,        0.13,   0 },    //NB
            { 0.1,      0,      0.05 }, //PE
            { 0.095,    0,      0.05 }, //QC
            { 0,        0.13,   0 },    //ON
            { 0.07,     0,      0.05},  //MB
            { 0.05,     0,      0.05},  //SK
            { 0,        0,      0.05},  //AB
            { 0,        0.12,   0},     //BC
            { 0,        0,      0.05},  //YT
            { 0,        0,      0.05},  //NT
            { 0,        0,      0.05}   //NU
            };
        private const int PST_INDEX = 0;
        private const int HST_INDEX = 1;
        private const int GST_INDEX = 2;

        private const int PE_INDEX = 3;
        private const int QC_INDEX = 4;

        private double principal { get; set; }
        private int province;

        public PurchaseTotaller() { }
        public PurchaseTotaller(string Province, string Principal)
        {
            province = Array.IndexOf(provinceCode, Province);
            double temp;
            double.TryParse(Principal, out temp);
            principal = temp;
        }

        private double GetPST()
        {            
            double pst = 0;

            if (gsthstpst[province, HST_INDEX] == 0)
            {
                if (province == PE_INDEX || province == QC_INDEX)
                {
                    pst = (GetGST() + principal) * (gsthstpst[province, PST_INDEX]);
                }
                else
                {
                    pst = principal * (gsthstpst[province, PST_INDEX]);
                }
            }

            return pst;
        }

        private double GetHST()
        {            
            double hst = 0;

            if (gsthstpst[province, PST_INDEX] == 0 && gsthstpst[province, GST_INDEX] == 0)
            {               
                hst = principal * (gsthstpst[province, PST_INDEX]);                
            }

            return hst;
        }
        
        private double GetGST()
        {            
            double gst = 0;

            if (gsthstpst[province, HST_INDEX] == 0)
            {               
                gst = principal * (gsthstpst[province, GST_INDEX]);              
            }

            return gst;
        }

        private double GetTotal()
        {
            double total;

            total = principal + GetPST() + GetHST() + GetGST();

            return total;
        }
    }
}
