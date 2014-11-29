using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HL7Lib;
using HL7Lib.ServiceData;

namespace ThortonSOAService
{
    class PurchaseTotaller
    {
        public const string TAG_NAME = "GIORP-TOTAL";
        public const int SECURITY_LEVEL = 3;
        public const string DESCRIPTION = "Calculate the PST, HST and GST of a principal amount";

        public double principal { get; set; }
        public int province { get; set; }

        public List<Message> arguments;
        public List<Message> responses;

        public List<Message> results;

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

        public PurchaseTotaller() 
        {
            arguments = new List<Message>();
            SetArguments();

            responses = new List<Message>();
            SetResponses();

            results = new List<Message>();
        }

        public PurchaseTotaller(string Province, double Principal) : this()
        {
            province = Array.IndexOf(provinceCode, Province);           
            principal = Principal;
        }

        public double GetPST()
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

        public double GetHST()
        {            
            double hst = 0;

            if (gsthstpst[province, PST_INDEX] == 0 && gsthstpst[province, GST_INDEX] == 0)
            {               
                hst = principal * (gsthstpst[province, HST_INDEX]);                
            }

            return hst;
        }

        public double GetGST()
        {            
            double gst = 0;

            if (gsthstpst[province, HST_INDEX] == 0)
            {               
                gst = principal * (gsthstpst[province, GST_INDEX]);              
            }

            return gst;
        }

        public double GetTotal()
        {
            double total;

            total = principal + GetPST() + GetHST() + GetGST();

            return total;
        }

        private void SetArguments()
        {
            Message arg1 = new Message(1, "province", "string", true);
            Message arg2 = new Message(2, "principal", "double", true);

            arguments.Add(arg1);
            arguments.Add(arg2);
        }

        private void SetResponses()
        {
            Message res1 = new Message(1, "SubTotalAmount", "double");
            Message res2 = new Message(2, "PSTAmount", "double");
            Message res3 = new Message(3, "HSTAmount", "double");
            Message res4 = new Message(4, "GSTAmount", "double");
            Message res5 = new Message(5, "TotalPurchaseAmount", "double");

            responses.Add(res1);
            responses.Add(res2);
            responses.Add(res3);
            responses.Add(res4);
            responses.Add(res5);
        }

        public void AddResult(int pos, string name, string datatype, double value)
        {
            Message result = new Message(pos, name, datatype, value:value.ToString());
            results.Add(result);
        }        
    }
}
