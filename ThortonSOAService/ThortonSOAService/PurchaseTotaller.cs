//***********************
//Authors: Kyle Fowler, Matt Anselmo, Adrian Krebs
//Project: ThortonSoa
//File: PurchaseTotaller.cs
//Date: 23/11/14
//Purpose: This file cotains the logic for the service, taking in a province and principal value and calculating the pst, hst, gst and total price.
//***********************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HL7Lib;
using HL7Lib.ServiceData;

namespace ThortonSOAService
{
    /// <summary>
    /// calucates the gst, pst hst, and total amount for a given province and principal value
    /// </summary>
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

        /// <summary>
        /// constructor
        /// </summary>
        public PurchaseTotaller() 
        {
            arguments = new List<Message>();
            SetArguments();

            responses = new List<Message>();
            SetResponses();

            results = new List<Message>();
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="Province">the province to get tax values for</param>
        /// <param name="Principal">principal value</param>
        public PurchaseTotaller(string Province, double Principal) : this()
        {
            province = Array.IndexOf(provinceCode, Province);           
            principal = Principal;
        }

        /// <summary>
        /// calulates the pst for the principal 
        /// </summary>
        /// <returns>the pst amount</returns>
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

        /// <summary>
        /// calculates the hst for the principal
        /// </summary>
        /// <returns>the hst amount</returns>
        public double GetHST()
        {            
            double hst = 0;

            if (gsthstpst[province, PST_INDEX] == 0 && gsthstpst[province, GST_INDEX] == 0)
            {               
                hst = principal * (gsthstpst[province, HST_INDEX]);                
            }

            return hst;
        }

        /// <summary>
        /// calulates the gst for the principal
        /// </summary>
        /// <returns>the gst amount</returns>
        public double GetGST()
        {            
            double gst = 0;

            if (gsthstpst[province, HST_INDEX] == 0)
            {               
                gst = principal * (gsthstpst[province, GST_INDEX]);              
            }

            return gst;
        }

        /// <summary>
        /// calcuates the total value
        /// </summary>
        /// <returns>the total value</returns>
        public double GetTotal()
        {
            double total;

            total = principal + GetPST() + GetHST() + GetGST();

            return total;
        }
        
        /// <summary>
        /// sets the arguments the service expects
        /// </summary>
        private void SetArguments()
        {
            Message arg1 = new Message(1, "province", "string", true);
            Message arg2 = new Message(2, "principal", "double", true);

            arguments.Add(arg1);
            arguments.Add(arg2);
        }

        /// <summary>
        /// sets the responses the service will send
        /// </summary>
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

        /// <summary>
        /// add result to result list
        /// </summary>
        /// <param name="pos">the position of the value</param>
        /// <param name="name">name of the value</param>
        /// <param name="datatype">datatype of the value</param>
        /// <param name="value">the value</param>
        public void AddResult(int pos, string name, string datatype, double value)
        {
            Message result = new Message(pos, name, datatype, value:value.ToString());
            results.Add(result);
        }        
    }
}
