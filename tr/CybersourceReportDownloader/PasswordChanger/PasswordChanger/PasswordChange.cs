using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CybersourceReportDownloader;
using System.Configuration;
using WatiN.Core;



namespace PasswordChanger
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class PasswordChange
    {
        private static string reportDate = ConfigurationSettings.AppSettings["ReportDate"].ToString();
        public PasswordChange()
        {
            if (string.IsNullOrEmpty(reportDate))
            {
                reportDate = DateTime.Today.AddDays(-1).ToString("MM/dd/yyyy");
            }
        }
        private TestContext testContextInstance;
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }


        [TestMethod]
        public void FirstimeLoginChangePassword()
        {
            TreasuryDataContext tdc = new TreasuryDataContext();
            List<string> defaultpassword = new List<string>() { "NCOgateway09", "NCOgateway10", "NCOgateway20", null,"invalid password","Account locked","" };
            var passwordchangeusersnames = (from u in tdc.CybersourceGatewayTranDownloadTrackings
                                            where u.ReportDate.Equals(DateTime.Parse(reportDate)) && u.isDownLoadSuccess.Value.Equals(false) && !(defaultpassword.Contains(u.NewPasswordSet))
                                            select u);

            foreach (CybersourceGatewayTranDownloadTracking t in passwordchangeusersnames)
            {
                string organizationId = t.UserName;
                string username = t.UserName;
                string password = t.NewPasswordSet;
                string newpassword = "NCOgateway09";


                using (var browser = new IE("https://ebc.cybersource.com/ebc/login/Login.do#"))
                {
                   
                    browser.Link(Find.ByText("change")).Click();
                    browser.TextField(Find.ByName("organizationId")).TypeText(organizationId);
                    browser.TextField(Find.ByName("username")).TypeText(username);
                    browser.TextField(Find.ByName("password")).TypeText(password);
                    browser.Button(Find.ByValue("Login")).Click();
                    List<string> validmessage = new List<string>();
                    validmessage.Add("Welcome to the CyberSource Business Center");
                    validmessage.Add("The specified login credentials are incorrect");
                    validmessage.Add("The specified user name has been locked.");


                    if ( browser.ContainsText(validmessage[0]) || browser.ContainsText(validmessage[1]) || browser.ContainsText(validmessage[2]) )
                    {

                        if (browser.ContainsText(validmessage[0]))
                            newpassword = password;
                        else if (browser.ContainsText(validmessage[1]))
                            newpassword = "invalid password";
                        else
                            newpassword = "Account locked";
                    }
                    else
                    {

                        browser.TextField(Find.ByName("password")).TypeText(password);
                        browser.TextField(Find.ByName("newPassword")).TypeText(newpassword);
                        browser.TextField(Find.ByName("newPasswordConfirm")).TypeText(newpassword);
                        browser.Button(Find.ByValue("Submit")).Click();


                    
                        if (browser.ContainsText("Your new password cannot be a previously used password."))
                        {
                            browser.Back();
                            //browser.Link(Find.ByText("Close")).Click();
                            newpassword = "NCOgateway10";
                            browser.TextField(Find.ByName("password")).TypeText(password);
                            browser.TextField(Find.ByName("newPassword")).TypeText(newpassword);
                            browser.TextField(Find.ByName("newPasswordConfirm")).TypeText(newpassword);
                            browser.Button(Find.ByValue("Submit")).Click();

                        }

                        if (browser.ContainsText("Your new password cannot be a previously used password."))
                        {
                            browser.Back();
                            //browser.Link(Find.ByText("Close")).Click();
                            newpassword = "NCOgateway20";
                            browser.TextField(Find.ByName("password")).TypeText(password);
                            browser.TextField(Find.ByName("newPassword")).TypeText(newpassword);
                            browser.TextField(Find.ByName("newPasswordConfirm")).TypeText(newpassword);
                            browser.Button(Find.ByValue("Submit")).Click();

                        }



                        if (browser.ContainsText("Secret Profile Setup"))
                        {
                            if (!(browser.TextField(Find.ByName("answer")).Text.Length > 1))
                            {
                                browser.TextField(Find.ByName("answer")).TypeText("NCO");
                            }

                            
                            browser.Button(Find.ByValue("Submit Request")).Click();
                        }
                    }
                }
                string sql = "Update dbo.T_CybersourceGatewayTranDownloadTracking set NewPasswordSet= '" + newpassword + "' where CybersourceGatewayTranDownloadTracking_id=" + t.CybersourceGatewayTranDownloadTracking_id;
                tdc.ExecuteCommand(sql); 
            }


        }
    }
}
