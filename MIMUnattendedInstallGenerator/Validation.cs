using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static MIMUnattendedInstallGenerator.Objects;
using System.Net;

namespace MIMUnattendedInstallGenerator
{
    public partial class Validation : Form
    {
        private InstallOptions Options;
        private DecryptedValues Decrypted;

        public Validation (InstallOptions o, DecryptedValues d)
        {
            InitializeComponent();
            Options = o;
            Decrypted = d;
            errorPb.Visible = false;
        }

        private void UpdateOutput(string dataLine)
        {
            outputTb.AppendText(DateTime.Now.ToLongTimeString() + "  -  " + dataLine + Environment.NewLine);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            errorPb.Visible = false;
            currentTaskTb.Clear();
            outputTb.Clear();
            string err;
            string separator = "---------------------";
            bool OK = true;

            // step 1 - SQL
            currentTaskTb.Text = "Checking SQL";
            UpdateOutput(separator);
            UpdateOutput("Starting SQL Connection Test");

            if (IsSqlValid(out err))
            {
                UpdateOutput("SQL Connection OK");
            }
            else
            {
                OK = false;
                UpdateOutput(string.Format("SQL Connection Error: {0}", err));
            }

            // step 2 - Resolve Mail Server
            currentTaskTb.Text = "Resolving Mail Server";
            UpdateOutput(separator);
            UpdateOutput("Starting Mail Server Resolution Test");

            string hostname = Options.MailServer;
            if (Options.MailIsExchOnline) { hostname = "outlook.office365.com"; }

            try
            {
                var ip = Dns.GetHostEntry(hostname);
                UpdateOutput(string.Format("Mail server '{0}' successfully resolved to '{1}'.", hostname, ip.AddressList.First()));
            }
            catch (Exception ex)
            {
                OK = false;
                err = ex.Message;
                UpdateOutput(string.Format("Mail server '{0}' failed to resolve. Error: {1}", hostname, err));
            }

            //Step 3 - Mail Server Connectivity
            currentTaskTb.Text = "Testing Mail Server Connectivity";
            UpdateOutput(separator);
            UpdateOutput("Starting Mail Server Connectivity Test");
            
            try
            {
                string url = string.Format("https://{0}/ews/exchange.asmx",hostname);
                UpdateOutput("Target URL: " + url);
                string username = Options.MIMSvcAcctName;
                string domain = Options.MIMSvcAcctDomain;
                string password = Decrypted.MIMSvcAcctPwd;
                string dispName = Options.MIMSvcAcctDomain + @"\" + Options.MIMSvcAcctName;
                NetworkCredential netCred = new NetworkCredential(username, password, domain);
                if (Options.MailIsExchOnline) {
                    username = Options.MIMSvcAcctEmail;
                    domain = null;
                    password = Decrypted.MIMSvcAcctEmailPwd;
                    dispName = Options.MIMSvcAcctEmail;
                    netCred = new NetworkCredential(username, password);
                }
                UpdateOutput("Connecting as " + dispName);
                
                // try accessing the web service directly via it's URL
                HttpWebRequest request =
                    WebRequest.Create(url) as HttpWebRequest;
                request.Credentials = netCred;
                request.Timeout = 5000;

                using (HttpWebResponse response =
                           request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new Exception(String.Format("Status '{0}' returned.",response.StatusDescription));
                }
                UpdateOutput("Connection established successfully!");
            }
            catch (Exception ex)
            {
                OK = false;
                err = ex.Message;
                UpdateOutput(string.Format("Connection to mail server '{0}' failed. Error: {1}", hostname, err));
            }

            // finish
            currentTaskTb.Text = "DONE!";
            if (!OK)
            {
                errorPb.Visible = true;
            }

        }
        private bool IsSqlValid(out string Error)
        {
            Error = null;
            string connStr = string.Format("Server={0};Database=master;Trusted_Connection=True;Connection Timeout=5", Options.SQLInstance);

            using (SqlConnection connection = new SqlConnection(connStr))
            {
                try
                {
                    connection.Open();
                    return true;
                }
                catch (Exception ex)
                {
                    Error = ex.Message;
                    return false;
                }
            }
        }
    }
}
