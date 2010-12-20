using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using NLog;
using NLog.Targets;
using NLog.Config;
using System.IO;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Data;
using System.Data.Linq;
using System.Web; 
using System.Net;
using WatiN.Core;

using LINQtoCSV;

namespace CybersourceReportDownloader
{
    public class Program
    {

        private static Logger logger = LogManager.GetCurrentClassLogger();
        private static TreasuryDataContext tdc = new TreasuryDataContext();
        private static EpaymentsConfigDataContext edc = new EpaymentsConfigDataContext();
        private static string reportDate;
        
        [STAThread]
        public static void Main(string[] args)
        {
            string username = "";
            DateTime rptDt;
            string Password = "";
            string merchantid = "";
            DateTime reportFromdate;
            DateTime reportTodate;

            reportFromdate = DateTime.Parse(ConfigurationSettings.AppSettings["ReportFromDate"]);
            reportTodate = DateTime.Parse(ConfigurationSettings.AppSettings["ReportToDate"].ToString());
          
            //Import Merchant data
            importMerchantConfig();
          //  tdc.ExecuteCommand("Truncate table dbo.T_CybersourceGatewayTranDownloadTracking");

            foreach (DateTime day in EachDay(reportFromdate, reportTodate))
            {
                reportDate = day.ToString();
                if (string.IsNullOrEmpty(reportDate))
                {
                    reportDate = DateTime.Today.AddDays(-1).ToString("MM/dd/yyyy");
                }
                //DateTime  rptDt1 = DateTime.Parse(reportDate);
                //CyberSourceReport("n109233110475", "NCOgateway09", rptDt1, "cp109233-110475"); 
                
                ProcessReport();
                PasswordChange();
                FirstimeLoginChangePassword();
                ReProcessRemainingReports();
            }
            LoadTransactionData();
        }
        
        [MTAThread]
        private static void LoadTransactionData()
        {
            
            
            CsvContext cc = new CsvContext();
            IEnumerable<Transaction> dataRows_namesUs = null;
            CsvFileDescription fileDescription_namesUs = new CsvFileDescription
            {
                SeparatorChar = '~', // default is ','
                FirstLineHasColumnNames = false,
                EnforceCsvColumnAttribute = true, // default is false
                FileCultureName = "en-US", // default is the current culture,
                MaximumNbrExceptions =10

            };
            string sourcePath = ConfigurationSettings.AppSettings["ReportDownloadLocation"];
            string fileName = "";
            int rownumber;
            
                if (System.IO.Directory.Exists(sourcePath))
                {
                    string[] files = System.IO.Directory.GetFiles(sourcePath);

                    // Copy the files and overwrite destination files if they already exist. 
                    foreach (string s in files)
                    {
                        // Use static Path methods to extract only the file name from the path. 
                        fileName = s;

                        dataRows_namesUs =
                        cc.Read<Transaction>(fileName, fileDescription_namesUs);
                        rownumber = 1;
                       
                        foreach (Transaction row in dataRows_namesUs)
                        {
                           try
                            {
                                if (rownumber > 2)
                                {
                                    STG_CybersourceTransaction t = new STG_CybersourceTransaction()
                                    {
                                        FileName = System.IO.Path.GetFileName(fileName),
                                        Row_descriptor = row.TransactionRow ?? "" 
                                    };
                                    tdc.STG_CybersourceTransactions.InsertOnSubmit(t);
                                    tdc.SubmitChanges();
                                }
                                rownumber++;
                           }
                            catch (Exception e)
                            {
                                Utils.OutputException(e);
                            }
                        }
                       
                        File.Delete(s);
                    }
                }


           


        }

        private static void ProcessReport()
        {
            //call report for each username
            List<CybersourceGatewayMerchantConfig> usersnames = (from usr in tdc.CybersourceGatewayMerchantConfigs
                                                                 where !(
                                                                            (from dr in tdc.CybersourceGatewayTranDownloadTrackings
                                                                             where dr.ReportDate.Equals(DateTime.Parse(reportDate))
                                                                             select dr.UserName).Contains(usr.Username)
                                                                         )
                                                                 select usr).Distinct().ToList();
             
            downloadreport(usersnames);

            tdc.ExecuteCommand("delete from dbo.T_CybersourceGatewayTranDownloadTracking where isDownLoadSuccess = 1");

            //List<CybersourceGatewayMerchantConfig> errusersnames = (from u in tdc.CybersourceGatewayTranDownloadTrackings
            //                                                        join usr in tdc.CybersourceGatewayMerchantConfigs
            //                                                        on u.MerchantId equals usr.MerchantId
            //                                                        where u.ReportDate.Equals(DateTime.Parse(reportDate)) && u.isDownLoadSuccess.Value.Equals(false) && (u.NewPasswordSet == null)
            //                                                        select usr).Distinct().ToList();

            //downloadreport(errusersnames, "NCOgateway10");

            //tdc.ExecuteCommand("delete from dbo.T_CybersourceGatewayTranDownloadTracking where isDownLoadSuccess = 1");
            
            ////manual step
            //errusersnames = (from u in tdc.CybersourceGatewayTranDownloadTrackings
            //                 join usr in tdc.CybersourceGatewayMerchantConfigs
            //                 on u.MerchantId equals usr.MerchantId
            //                 where u.ReportDate.Equals(DateTime.Parse(reportDate)) && u.isDownLoadSuccess.Value.Equals(false) == null
            //                 select usr).Distinct().ToList();
            
            //downloadreport(errusersnames, "NCOgateway20");

            //tdc.ExecuteCommand("delete from dbo.T_CybersourceGatewayTranDownloadTracking where isDownLoadSuccess = 1");

        }

        private static void ReProcessRemainingReports()
        {
            //call report for each username
            List<string> defaultpassword = new List<string>() { "NCO", null, "invalid password", "Account locked", "" };

            List<CybersourceGatewayTranDownloadTracking> usersnames = (from u in tdc.CybersourceGatewayTranDownloadTrackings
                                                                 join usr in tdc.CybersourceGatewayMerchantConfigs
                                                                 on u.MerchantId equals usr.MerchantId
                                                                       where u.ReportDate.Equals(DateTime.Parse(reportDate)) && u.isDownLoadSuccess.Value.Equals(false) && (u.NewPasswordSet != null) && (u.NewPasswordSet.Trim().Length > 0) && !(defaultpassword.Contains(u.NewPasswordSet))
                                                                 select u).Distinct().ToList();
            string username = "";
            DateTime rptDt;
            string Password = "";
            string merchantid = "";

            foreach (CybersourceGatewayTranDownloadTracking c in usersnames)
            {
                username = c.UserName;
                rptDt = DateTime.Parse(reportDate);
                merchantid = c.MerchantId;
                Password = c.NewPasswordSet; 
                CyberSourceReport(username, Password, rptDt, merchantid);
            }
            tdc.ExecuteCommand("delete from dbo.T_CybersourceGatewayTranDownloadTracking where isDownLoadSuccess = 1");
            
        }
        
        private static void PasswordChange()
        {
            bool nextstage = false;

            var passwordchangeusersnames = (from u in tdc.CybersourceGatewayTranDownloadTrackings
                                            where u.ReportDate.Equals(DateTime.Parse(reportDate)) && u.isDownLoadSuccess.Value.Equals(false) && u.NewPasswordSet.Equals(null) 
                                 select u);
            foreach (CybersourceGatewayTranDownloadTracking t in passwordchangeusersnames)
            {
                string merchantid = t.MerchantId ;
                string username = t.UserName;
                string password="NCOgateway09";
                string organizationId = t.UserName;
                string newpassword="";

                List<string> validmessage = new List<string>();
                validmessage.Add("Welcome to the CyberSource Business Center");
                validmessage.Add("The specified user name has been locked.");

                using (var browser = new IE("https://ebc.cybersource.com/ebc/login/Login.do#"))
                {

                    browser.Link(Find.ByText("change")).Click();
                    browser.TextField(Find.ByName("organizationId")).TypeText(organizationId);
                    browser.TextField(Find.ByName("username")).TypeText(username);
                    browser.TextField(Find.ByName("password")).TypeText(password);
                    browser.Button(Find.ByValue("Login")).Click();

                    if (browser.ContainsText(validmessage[0]) || browser.ContainsText(validmessage[1]) )
                    {

                        if (browser.ContainsText(validmessage[0]))
                        { newpassword = password; }
                        else if (browser.ContainsText(validmessage[1]))
                        { newpassword = "Account locked"; }


                        tdc.ExecuteCommand("update dbo.T_CybersourceGatewayTranDownloadTracking set NewPasswordSet='" + newpassword + "' where MerchantId = '" + merchantid + "'");
                        tdc.ExecuteCommand("update dbo.T_CybersourceGatewayMerchantConfig set Password='" + password + "' where MerchantId = '" + merchantid + "'");
                        
                    }
                    else if (browser.ContainsText("Secret Profile Setup"))
                    {
                        if (!(browser.TextField(Find.ByName("answer")).Text.Length > 1))
                        {
                            browser.TextField(Find.ByName("answer")).TypeText("NCO");
                        }

                        browser.Button(Find.ByValue("Submit Request")).Click();
                        string sql = "Update dbo.T_CybersourceGatewayTranDownloadTracking set NewPasswordSet= '" + newpassword + "' where CybersourceGatewayTranDownloadTracking_id=" + t.CybersourceGatewayTranDownloadTracking_id;
                        tdc.ExecuteCommand(sql);
                        tdc.ExecuteCommand("update dbo.T_CybersourceGatewayMerchantConfig set Password='" + newpassword + "' where MerchantId = '" + merchantid + "'");
                        
                    }
                    else
                    {
                        browser.Back();
                        password = "NCOgateway10";
                        browser.Link(Find.ByText("change")).Click();
                        browser.TextField(Find.ByName("organizationId")).TypeText(organizationId);
                        browser.TextField(Find.ByName("username")).TypeText(username);
                        browser.TextField(Find.ByName("password")).TypeText(password);
                        browser.Button(Find.ByValue("Login")).Click();

                        if (browser.ContainsText(validmessage[0]) || browser.ContainsText(validmessage[1]))
                        {

                            if (browser.ContainsText(validmessage[0]))
                            { newpassword = password; }
                            else if (browser.ContainsText(validmessage[1]))
                            { newpassword = "Account locked"; }

                            tdc.ExecuteCommand("update dbo.T_CybersourceGatewayTranDownloadTracking set NewPasswordSet='" + newpassword + "' where MerchantId = '" + merchantid + "'");
                            tdc.ExecuteCommand("update dbo.T_CybersourceGatewayMerchantConfig set Password='" + password + "' where MerchantId = '" + merchantid + "'");
                        
                        }
                        else if (browser.ContainsText("Secret Profile Setup"))
                        {
                            if (!(browser.TextField(Find.ByName("answer")).Text.Length > 1))
                            {
                                browser.TextField(Find.ByName("answer")).TypeText("NCO");
                            }

                            browser.Button(Find.ByValue("Submit Request")).Click();
                            string sql = "Update dbo.T_CybersourceGatewayTranDownloadTracking set NewPasswordSet= '" + newpassword + "' where CybersourceGatewayTranDownloadTracking_id=" + t.CybersourceGatewayTranDownloadTracking_id;
                            tdc.ExecuteCommand(sql);
                            tdc.ExecuteCommand("update dbo.T_CybersourceGatewayMerchantConfig set Password='" + newpassword + "' where MerchantId = '" + merchantid + "'");
                        
                        }
                        else
                        {

                            nextstage = step1_RequestForgottenPassword(organizationId, username);

                            if (nextstage)
                                nextstage = step2_RecoverPassword(organizationId, username, ref password);

                            string sql = "Update dbo.T_CybersourceGatewayTranDownloadTracking set NewPasswordSet= '" + password + "' where CybersourceGatewayTranDownloadTracking_id=" + t.CybersourceGatewayTranDownloadTracking_id;
                            tdc.ExecuteCommand(sql);
                            
                            tdc.ExecuteCommand("update dbo.T_CybersourceGatewayMerchantConfig set Password='" + newpassword + "' where MerchantId = '" + merchantid + "'");
                        
                        }
                    }
                }
            }


        }
        private static void downloadreport(List<CybersourceGatewayMerchantConfig> usersnames)
        {
            string username = "";
            DateTime rptDt;
            string password = "";
            string merchantid = "";

            foreach (CybersourceGatewayMerchantConfig c in usersnames)
            {
                try
                {
                    username = c.Username;
                    password = c.Password; 
                    rptDt = DateTime.Parse(reportDate);
                    merchantid = c.MerchantId;
                    CyberSourceReport(username, password, rptDt, merchantid);
                }
                catch (Exception ex)
                {
                    logger.LogException(LogLevel.Error, "Error occured " + ex.Message.ToString(), ex);
                }
            }
        }
        private static bool step1_RequestForgottenPassword(string organizationid, string username)
        {
            WebRequest request;
            string postdata;

            try
            {
                request = WebRequest.Create("https://ebc.cybersource.com/ebc/login/RequestForgottenPassword.do ");
                request.Method = "POST";
                postdata = "organizationId=" + organizationid + "&username=" + username;
                byte[] byteArray = Encoding.UTF8.GetBytes(postdata);
                string sql="";

                request.ContentType = "application/x-www-form-urlencoded";
              
                request.ContentLength = byteArray.Length;
               
                Stream dataStream = request.GetRequestStream();
              
                dataStream.Write(byteArray, 0, byteArray.Length);
              
                dataStream.Close();
             
                WebResponse response = request.GetResponse();
            
                dataStream = response.GetResponseStream();
            
                StreamReader reader = new StreamReader(dataStream);
            
                string responseFromServer = reader.ReadToEnd();
           
           
                // Clean up the streams.
                reader.Close();
                dataStream.Close();
                response.Close();

                if (responseFromServer.Contains("Recover Your Password"))
                    return true;
                else if (responseFromServer.Contains("The specified user name has been locked"))
                {
                    sql = "Update dbo.T_CybersourceGatewayTranDownloadTracking set NewPasswordSet= 'Account locked' where username=" + username;
                    tdc.ExecuteCommand(sql);
                    return false;
                }
                else
                    return false;


            }
            catch (Exception ex)
            {
                return false;
            }


        }
        private static bool step2_RecoverPassword(string organizationid, string username, ref string newglbpassword)
        {
            WebRequest request;
            string postdata;

            try
            {
                request = WebRequest.Create("https://ebc.cybersource.com/ebc/login/RecoverPassword.do ");
                request.Method = "POST";
                postdata = "answer=NCO&question=1&organizationId=" + organizationid + "&username=" + username;
                byte[] byteArray = Encoding.UTF8.GetBytes(postdata);
                
                request.ContentType = "application/x-www-form-urlencoded";
              
                request.ContentLength = byteArray.Length;
              
                Stream dataStream = request.GetRequestStream();
               
                dataStream.Write(byteArray, 0, byteArray.Length);
               
                dataStream.Close();
               
                WebResponse response = request.GetResponse();
                
                dataStream = response.GetResponseStream();
               
                StreamReader reader = new StreamReader(dataStream);
             
                string responseFromServer = reader.ReadToEnd();
               
                // Clean up the streams.
                reader.Close();
                dataStream.Close();
                response.Close();

                if (responseFromServer.Contains("Your password has been reset as follows"))
                {
                    int i = responseFromServer.IndexOf("<b>", 0, responseFromServer.Length);
                    int j = responseFromServer.IndexOf("</b>", 0, responseFromServer.Length);
                    newglbpassword = responseFromServer.Substring(i + 3, j - 3 - i);
                    return true;
                }
                else
                    return false;


            }
            catch (Exception ex)
            {
                return false;
            }
        }
        private static void importMerchantConfig()
        {
             try
            {
                //truncate table and import T_CybersourceGatewayMerchantConfig from epaymentsconfig
                List<string> tdcMerchant = new List<string>();
                tdcMerchant = (from c in tdc.CybersourceGatewayMerchantConfigs
                       select c.MerchantId).ToList(); 

                var config = from CybersourceGatewayMerchantConfiguration mc in edc.CybersourceGatewayMerchantConfigurations
                             where !(tdcMerchant.Contains(mc.MerchantId))
                             select mc;
                
                foreach (CybersourceGatewayMerchantConfiguration c in config)
                {

                    CybersourceGatewayMerchantConfig cfg = new CybersourceGatewayMerchantConfig()
                    {
                        CybersourceGatewayMerchantConfigID = c.CybersourceGatewayMerchantConfigID
                        ,
                        CybersourceGatewayProcessorID = c.CybersourceGatewayProcessorID
                        ,
                        IgnoreCVResult = c.IgnoreCVResult
                        ,
                        Level2or3Enabled = c.Level2or3Enabled
                        ,
                        MerchantId = c.MerchantId
                        ,
                        Password = c.Password
                        ,
                        ProfileName = c.ProfileName
                        ,
                        Username = c.Username
                    };

                    tdc.CybersourceGatewayMerchantConfigs.InsertOnSubmit(cfg);
             }
                tdc.SubmitChanges();
            }
            catch (Exception ex)
            {
                logger.LogException(LogLevel.Error, "Error occured " + ex.Message.ToString(), ex);
            }

        }
        private static void CyberSourceReport(string strUsername, string password, DateTime reportDate, string merchantid)
        {
            string strReportURL="";
            string strReportName="";
            string strReportFormat="";
            string reportURL="";
            string authInfo="";
            string strReportDownloadLocation="";
            string filepath="";
            try
            {
                strReportName = ConfigurationSettings.AppSettings["reportName"].ToString();
                strReportFormat = ConfigurationSettings.AppSettings["reportFormat"].ToString();
                reportURL =  ConfigurationSettings.AppSettings["reportURL"].ToString();
                strReportURL = reportURL + getYear(reportDate) + "/" + getMonth(reportDate) + "/" + getDay(reportDate) + "/" + strUsername + "/" + strReportName + "." + strReportFormat;
               
                strReportDownloadLocation = ConfigurationSettings.AppSettings["ReportDownloadLocation"].ToString(); 

                if (string.IsNullOrEmpty(strReportName))
                    throw new NullReferenceException("ReportName should not be null");

                if (string.IsNullOrEmpty(password))
                    throw new NullReferenceException("Password should not be null");

                if (string.IsNullOrEmpty(strReportFormat))
                    throw new NullReferenceException("Report Format should not be null"); 

                if (string.IsNullOrEmpty(reportURL))
                    throw new NullReferenceException("Report URL should not be null"); 

            }
            catch(Exception ex)
            {
                logger.LogException(LogLevel.Error, "Validation Error occured " + ex.Message.ToString(), ex);
            }
  
            HttpWebResponse webresponse=null;
            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(strReportURL);
            Stream str;
           

            authInfo = strUsername + ":" + password;
            authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
            
            webrequest.Headers["Authorization"] = "Basic " + authInfo;
            
            webrequest.PreAuthenticate = false;
            webrequest.KeepAlive = false;
            webrequest.Method = "GET";
            webrequest.ContentType = "application/csv";

            try
            {
                webresponse = (HttpWebResponse)webrequest.GetResponse();

                if (webresponse.ContentLength < 1)
                    throw new NullReferenceException("Web response is null");
                else
                    str = webresponse.GetResponseStream();

                filepath = strReportDownloadLocation + strReportName + "_" + strUsername + "_" + reportDate.ToString("MM/dd/yyyy").Replace("/", "-") + "." + strReportFormat;

                byte[] inBuf = new byte[10000000];
                int bytesToRead = (int)inBuf.Length;
                int bytesRead = 0;
                while (bytesToRead > 0)
                {
                    int n = str.Read(inBuf, bytesRead, bytesToRead);
                    if (n == 0)
                        break;
                    bytesRead += n;
                    bytesToRead -= n;
                }
                FileStream fstr = new FileStream(filepath, FileMode.Create, FileAccess.Write);
                fstr.Write(inBuf, 0, bytesRead);
                str.Close();
                fstr.Close();

                tdc.ExecuteCommand("delete from dbo.T_CybersourceGatewayTranDownloadTracking where MerchantId= '" + merchantid + "'");

                CybersourceGatewayTranDownloadTracking trandwld = new CybersourceGatewayTranDownloadTracking()
                {
                    FileName = filepath,
                    isDownLoadSuccess = true,
                    MerchantId = merchantid,
                    Password = password,
                    UserName = strUsername,
                    ReportDate = reportDate,
                    Message = "success"
                };
              

                tdc.CybersourceGatewayTranDownloadTrackings.InsertOnSubmit(trandwld);
                tdc.SubmitChanges();
                tdc.ExecuteCommand("update dbo.T_CybersourceGatewayMerchantConfig set Password='" + password + "' where MerchantId = '" + merchantid + "'");

            }
            catch (Exception ex)
            {

                tdc.ExecuteCommand("delete from dbo.T_CybersourceGatewayTranDownloadTracking where MerchantId= '" + merchantid + "'");

                if (!((System.Net.HttpWebResponse)(((System.Net.WebException)(ex)).Response)).StatusDescription.ToString().Contains("The report requested cannot be found on this server"))
                {
                    CybersourceGatewayTranDownloadTracking trandwld = new CybersourceGatewayTranDownloadTracking()
                    {
                        FileName = filepath,
                        isDownLoadSuccess = false,
                        MerchantId = merchantid,
                        Password = password,
                        UserName = strUsername,
                        ReportDate = reportDate,
                        Message = ((System.Net.HttpWebResponse)(((System.Net.WebException)(ex)).Response)).StatusDescription.ToString()
                    };
                        tdc.CybersourceGatewayTranDownloadTrackings.InsertOnSubmit(trandwld);
                        tdc.SubmitChanges();

                    //logger.LogException(LogLevel.Error, "Error occured while downloading report " + ex.Message.ToString(), ex);
                }
           }
            
        }
        private static string getYear(DateTime DatadownloadDate)
        {
            return DatadownloadDate.Year.ToString();  
        }
        private static string getMonth(DateTime DatadownloadDate)
        {
            string month = "0" + DatadownloadDate.Month.ToString();
            return month.Substring(month.Length - 2,2);  
        }
        private static string getDay(DateTime DatadownloadDate)
        {
            string day = "0" + DatadownloadDate.Day.ToString();
            return day.Substring(day.Length - 2,2);  
        }

        [STAThread]
        private static void FirstimeLoginChangePassword()
        {
            TreasuryDataContext tdc = new TreasuryDataContext();
            List<string> defaultpassword = new List<string>() { "NCO", null, "invalid password", "Account locked", "" };
            var passwordchangeusersnames = (from u in tdc.CybersourceGatewayTranDownloadTrackings
                                            where u.ReportDate.Equals(DateTime.Parse(reportDate)) && u.isDownLoadSuccess.Value.Equals(false) && !(defaultpassword.Contains(u.NewPasswordSet))
                                            select u);

            foreach (CybersourceGatewayTranDownloadTracking t in passwordchangeusersnames)
            {
                
                string organizationId = t.UserName;
                string username = t.UserName;
                string password = t.NewPasswordSet;
                string newpassword = "";
                string merchantid = t.MerchantId; 
                
                List<string> validmessage = new List<string>();
                validmessage.Add("Welcome to the CyberSource Business Center");
                validmessage.Add("The specified login credentials are incorrect");
                validmessage.Add("The specified user name has been locked.");
                
                using (var browser = new IE("https://ebc.cybersource.com/ebc/login/Login.do#"))
                {

                    browser.Link(Find.ByText("change")).Click();
                    browser.TextField(Find.ByName("organizationId")).TypeText(organizationId);
                    browser.TextField(Find.ByName("username")).TypeText(username);
                    browser.TextField(Find.ByName("password")).TypeText(password);
                    browser.Button(Find.ByValue("Login")).Click();
                   
                   if (browser.ContainsText(validmessage[0]) || browser.ContainsText(validmessage[1]) || browser.ContainsText(validmessage[2]))
                    {
                        if (browser.ContainsText(validmessage[0]))
                            newpassword = password;
                        else if (browser.ContainsText(validmessage[1]))
                            newpassword = "invalid password";
                        else
                            newpassword = "Account locked";

                        tdc.ExecuteCommand("update dbo.T_CybersourceGatewayMerchantConfig set Password='" + newpassword + "' where MerchantId = '" + merchantid + "'");
                    }
                    else
                    {
                        //newpassword = "NCOgateway09" ;
                        //browser.TextField(Find.ByName("password")).TypeText(password);
                        //browser.TextField(Find.ByName("newPassword")).TypeText(newpassword);
                        //browser.TextField(Find.ByName("newPasswordConfirm")).TypeText(newpassword);
                        //browser.Button(Find.ByValue("Submit")).Click();

                        //if (browser.ContainsText("Secret Profile Setup"))
                        //{
                        //    if (!(browser.TextField(Find.ByName("answer")).Text.Length > 1))
                        //    {
                        //        browser.TextField(Find.ByName("answer")).TypeText("NCO");
                        //    }

                        //    browser.Button(Find.ByValue("Submit Request")).Click();

                        //    string sql = "Update dbo.T_CybersourceGatewayTranDownloadTracking set NewPasswordSet= '" + newpassword + "' where CybersourceGatewayTranDownloadTracking_id=" + t.CybersourceGatewayTranDownloadTracking_id;
                        //    tdc.ExecuteCommand(sql);

                        //    tdc.ExecuteCommand("update dbo.T_CybersourceGatewayMerchantConfig set Password='" + newpassword + "' where MerchantId = '" + merchantid + "'");

                        //}
                        //else
                        //{
                        //    browser.Back();
                        //    newpassword = "NCOgateway10";
                        //    browser.TextField(Find.ByName("password")).TypeText(password);
                        //    browser.TextField(Find.ByName("newPassword")).TypeText(newpassword);
                        //    browser.TextField(Find.ByName("newPasswordConfirm")).TypeText(newpassword);
                        //    browser.Button(Find.ByValue("Submit")).Click();


                        //    if (browser.ContainsText("Secret Profile Setup"))
                        //    {
                        //        if (!(browser.TextField(Find.ByName("answer")).Text.Length > 1))
                        //        {
                        //            browser.TextField(Find.ByName("answer")).TypeText("NCO");
                        //        }

                        //        browser.Button(Find.ByValue("Submit Request")).Click();

                        //        string sql = "Update dbo.T_CybersourceGatewayTranDownloadTracking set NewPasswordSet= '" + newpassword + "' where CybersourceGatewayTranDownloadTracking_id=" + t.CybersourceGatewayTranDownloadTracking_id;
                        //        tdc.ExecuteCommand(sql);

                        //        tdc.ExecuteCommand("update dbo.T_CybersourceGatewayMerchantConfig set Password='" + newpassword + "' where MerchantId = '" + merchantid + "'");

                        //    }
                        //    else
                        //    {
                                //browser.Back();

                                for (int i = 12; i <= 30; i++)
                                {
                                    
                                    newpassword = "NCOgateway" + i.ToString();
                                    browser.TextField(Find.ByName("password")).TypeText(password);
                                    browser.TextField(Find.ByName("newPassword")).TypeText(newpassword);
                                    browser.TextField(Find.ByName("newPasswordConfirm")).TypeText(newpassword);
                                    browser.Button(Find.ByValue("Submit")).Click();

                                    if (browser.ContainsText("Secret Profile Setup"))
                                    {
                                        if (!(browser.TextField(Find.ByName("answer")).Text.Length > 1))
                                        {
                                            browser.TextField(Find.ByName("answer")).TypeText("NCO");
                                        }

                                        browser.Button(Find.ByValue("Submit Request")).Click();

                                        string sql = "Update dbo.T_CybersourceGatewayTranDownloadTracking set NewPasswordSet= '" + newpassword + "' where CybersourceGatewayTranDownloadTracking_id=" + t.CybersourceGatewayTranDownloadTracking_id;
                                        tdc.ExecuteCommand(sql);

                                        tdc.ExecuteCommand("update dbo.T_CybersourceGatewayMerchantConfig set Password='" + newpassword + "' where MerchantId = '" + merchantid + "'");
                                        break;
                                    }
                                    else if (browser.ContainsText("Your new password cannot be a previously used password."))
                                    {
                                        browser.Back();

                                    }

                                    else if (browser.ContainsText("You cannot change your password more than three times in 24 hours"))
                                    {
                                        browser.Back();
                                        browser.Back();
                                        break;

                                    }
                                //}
                            //}
                       }
                    }
               }
            }
        }

        private static IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        } 

    }
}
