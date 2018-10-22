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
            int steps = 2;
            int step = 0;
            string err;
            string separator = "---------------------";
            bool OK = true;

            // step 1 - SQL
            //progressBar1.Step = (step / steps) * 100;
            //progressBar1.PerformStep();
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
            step++;
            //progressBar1.Step = (step / steps) * 100;
            //progressBar1.PerformStep();

            // step 2 - Mail Server
            progressBar1.Step = (step / steps) * 100;
            progressBar1.PerformStep();
            currentTaskTb.Text = "Checking Mail Server";
            UpdateOutput(separator);
            UpdateOutput("Starting Mail Server Tests");

            try
            {
                var ip = Dns.GetHostEntry(Options.MailServer);
                UpdateOutput(string.Format("Mail server '{0}' successfully resolved to '{1}'.", Options.MailServer, ip.AddressList.First()));
            }
            catch (Exception ex)
            {
                OK = false;
                err = ex.Message;
                UpdateOutput(string.Format("Mail server '{0}' failed to resolve. Error: {1}", Options.MailServer, err));
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
            string connStr = string.Format("Server={0};Database=master;Trusted_Connection=True",Options.SQLInstance);

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
