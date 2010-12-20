using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Web;
using System.IO;
using System.Collections.Specialized;  



namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public string newglbpassword = "";
        public string glbformAccessKey = "";

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            bool nextstage = false;

            //downloadreport("NCOgateway09", "n451047322990", DateTime.Parse("10/08/2010"));

            //nextstage = step1_RequestForgottenPassword("n451047320994", "n451047320994");
            
            //if (nextstage)
            //    nextstage = step2_RecoverPassword("n451047320994", "n451047320994");

            //if (nextstage)
            string organizationid = "51209287";
            string username = "51209287";
            string currentpassword = "NCOgateway09";
            string newpassword = "NCOgateway09";

            //nextstage = step1_RequestForgottenPassword(organizationid, username);

            //nextstage = step2_RecoverPassword(organizationid, username);

            nextstage = step3_LoginProcess(organizationid, username, currentpassword);
            //glbformAccessKey = "44uNYRZRbCX97bmBUuoN4tx+iZM=";
            glbformAccessKey = Uri.EscapeDataString(glbformAccessKey);
            //44uNYRZRbCX97bmBUuoN4tx%2BiZM%3D

            //if (nextstage)
                //nextstage = step4_ChangeExpiredPassword(organizationid, username, currentpassword, newpassword, glbformAccessKey);
            
        }

        public bool step1_RequestForgottenPassword(string organizationid, string username)
        {
            WebRequest request;
            string postdata;
            
            try
            {
                request = WebRequest.Create("https://ebc.cybersource.com/ebc/login/RequestForgottenPassword.do ");
                request.Method = "POST";
                postdata = "organizationId=" + organizationid + "&username=" + username;
                byte[] byteArray = Encoding.UTF8.GetBytes(postdata);
                // Set the ContentType property of the WebRequest.
                request.ContentType = "application/x-www-form-urlencoded";
                // Set the ContentLength property of the WebRequest.
                request.ContentLength = byteArray.Length;
                // Get the request stream.
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();
                // Get the response.
                WebResponse response = request.GetResponse();
                // Display the status.
                //Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                // Get the stream containing content returned by the server.
                dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                string responseFromServer = reader.ReadToEnd();
                // Display the content.
                textBox1.Text = "";  
                textBox1.Text = responseFromServer;
                webBrowser1.DocumentText = responseFromServer;
                // Clean up the streams.
                reader.Close();
                dataStream.Close();
                response.Close();
                
                if (responseFromServer.Contains("Recover Your Password"))
                    return true;
                else
	                return false;


            }
            catch (Exception ex)
            {
                return false;
            }
            
           
        }

        public bool step2_RecoverPassword(string organizationid, string username)
        {
            WebRequest request;
            string postdata;

            try
            {
                request = WebRequest.Create("https://ebc.cybersource.com/ebc/login/RecoverPassword.do ");
                request.Method = "POST";
                postdata = "answer=NCO&question=1&organizationId=" + organizationid + "&username=" + username;
                byte[] byteArray = Encoding.UTF8.GetBytes(postdata);
                // Set the ContentType property of the WebRequest.
                request.ContentType = "application/x-www-form-urlencoded";
                // Set the ContentLength property of the WebRequest.
                request.ContentLength = byteArray.Length;
                // Get the request stream.
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();
                // Get the response.
                WebResponse response = request.GetResponse();
                // Display the status.
                //Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                // Get the stream containing content returned by the server.
                dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                string responseFromServer = reader.ReadToEnd();
                // Display the content.
                textBox1.Text = "";
                textBox1.Text = responseFromServer;
                webBrowser1.DocumentText = responseFromServer;
                               

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

        public bool step3_LoginProcess(string organizationid, string username, string newpassword)
        {
            WebRequest request;
            string postdata;

            try
            {
                request = WebRequest.Create("https://ebc.cybersource.com/ebc/login/LoginProcess.do ");
                request.Method = "POST";
                postdata = "organizationId=" + organizationid + "&username=" + username + "&password=" +  newpassword + "&alreadyVisited=false";
                byte[] byteArray = Encoding.UTF8.GetBytes(postdata);
                // Set the ContentType property of the WebRequest.
                request.ContentType = "application/x-www-form-urlencoded";
                // Set the ContentLength property of the WebRequest.
                request.ContentLength = byteArray.Length;
                // Get the request stream.
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();
                // Get the response.
                WebResponse response = request.GetResponse();
                // Display the status.
                //Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                // Get the stream containing content returned by the server.
                dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                string responseFromServer = reader.ReadToEnd();
                // Display the content.
                textBox1.Text = "";
                textBox1.Text = responseFromServer;
                webBrowser1.DocumentText = responseFromServer;
                // Clean up the streams.
                reader.Close();
                dataStream.Close();
                response.Close();

                if (responseFromServer.Contains("Password Change Request"))
                {
                    
                    string token = "formAccessKey";
                    int i = responseFromServer.IndexOf(token, 0, responseFromServer.Length);
                    int j = responseFromServer.IndexOf("value=", i, 26);
                    int k = responseFromServer.IndexOf(">", j, 50);

                    string formtoken = responseFromServer.Substring(j + 7, k - j - 8);
                    glbformAccessKey = formtoken;

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

        public bool step4_ChangeExpiredPassword(string organizationid, string username, string currentpassword, string newpassword, string formAccessKey)
        {
            WebRequest request;
            string postdata;

            try
            {
                request = WebRequest.Create("https://ebc.cybersource.com/ebc/login/ChangeExpiredPassword.do");
                request.Method = "POST";
                postdata = "password=" + currentpassword + "&newPassword=" + newpassword + "&newPasswordConfirm=" + newpassword + "&organizationId=" + organizationid + "&username=" + username + "&formAccessKey=" + formAccessKey + "";
                               
                                                         
                byte[] byteArray = Encoding.UTF8.GetBytes(postdata);

                Encoding ascii = Encoding.UTF8;

                char[] asciiChars = new char[ascii.GetCharCount(byteArray, 0, byteArray.Length)];
                ascii.GetChars(byteArray, 0, byteArray.Length, asciiChars, 0);
                string asciiString = new string(asciiChars);




                // Set the ContentType property of the WebRequest.
                request.ContentType = "application/x-www-form-urlencoded";
                // Set the ContentLength property of the WebRequest.
                request.ContentLength = byteArray.Length;
                // Get the request stream.
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                                
                // Close the Stream object.
                dataStream.Close();
                // Get the response.

               
                 
                
                WebResponse response = request.GetResponse();
                                 
                
                // Display the status.
                //Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                // Get the stream containing content returned by the server.
                dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                string responseFromServer = reader.ReadToEnd();
                // Display the content.
                textBox1.Text = "";
                textBox1.Text = responseFromServer;
                webBrowser1.DocumentText = responseFromServer;
                // Clean up the streams.
                reader.Close();
                dataStream.Close();
                response.Close();

               // if (responseFromServer.Contains("Password Change Request"))
                    return true;
               // else
               //     return false;


            }
            catch (Exception ex)
            {
                return false;
            }


        }

        public void downloadreport(string password, string strUsername, DateTime reportDate)
        {
            string strReportURL = "https://ebc.cybersource.com/ebc/DownloadReport/";
            HttpWebResponse webresponse=null;
            
            Stream str;
            string authInfo=null;
            string filepath="";
            string strReportDownloadLocation="\\\\cc-n05-04-07\\HexCRM\\TreasuryReconciliation\\CyberSourceGatewayTransactionDownload\\";
            string strReportName = "TransactionDetailReport";
            string strReportFormat = "csv";

            strReportURL = strReportURL + getYear(reportDate) + "/" + getMonth(reportDate) + "/" + getDay(reportDate) + "/" + strUsername + "/" + strReportName + "." + strReportFormat;

            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(strReportURL);

            authInfo = strUsername + ":" + password;
            authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
            
            webrequest.Headers["Authorization"] = "Basic " + authInfo;
            
            webrequest.PreAuthenticate = true;
            webrequest.KeepAlive = false;
            webrequest.Method = "GET";
            webrequest.ContentType = "application/csv";

        
                webresponse = (HttpWebResponse)webrequest.GetResponse();
                
                if (webresponse.ContentLength < 1) 
                    throw new NullReferenceException("Web response is null");    
                else
                    str= webresponse.GetResponseStream();
            
                filepath = strReportDownloadLocation + strReportName + "_" + strUsername + "_" + reportDate.ToString("MM/dd/yyyy").Replace("/","-") + "." +  strReportFormat;
                      
                byte[] inBuf = new byte[100000];
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
                FileStream fstr = new FileStream(filepath, FileMode.OpenOrCreate, FileAccess.Write);
                fstr.Write(inBuf, 0, bytesRead);
                str.Close();
                fstr.Close();

        }

        private static string getYear(DateTime DatadownloadDate)
        {
            return DatadownloadDate.Year.ToString();
        }

        private static string getMonth(DateTime DatadownloadDate)
        {
            string month = "0" + DatadownloadDate.Month.ToString();
            return month.Substring(month.Length - 2, 2);
        }

        private static string getDay(DateTime DatadownloadDate)
        {
            string day = "0" + DatadownloadDate.Day.ToString();
            return day.Substring(day.Length - 2, 2);
        }


    }
}
