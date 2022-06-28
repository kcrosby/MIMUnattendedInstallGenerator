using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

using static MIMUnattendedInstallGenerator.Objects;
using Newtonsoft.Json;
using System.Diagnostics;

namespace MIMUnattendedInstallGenerator
{
    public partial class MainApp : Form
    {
        private InstallOptions o = new InstallOptions();
        private string configFile = "config.json";
        private string encryptionKey = null;
        private DecryptedValues d = new DecryptedValues();

        public MainApp()
        {
            InitializeComponent();
            MIMSvcEmailPwd.Visible = false;
            label11.Visible = false;

            if (File.Exists(configFile))
            {
                //prompt for encryption key
                var form1 = new GetKey(false);
                form1.ShowDialog();
                encryptionKey = form1.Key;

                //load config
                o = JsonConvert.DeserializeObject<InstallOptions>(File.ReadAllText(configFile));

                //decrypt values
                if (encryptionKey != null)
                {
                    d.MIMSvcAcctEmailPwd = Security.Decrypt(encryptionKey, o.MIMSvcAcctEmailPwd);
                    d.MIMSvcAcctPwd = Security.Decrypt(encryptionKey, o.MIMSvcAcctPwd);
                    d.PAMAPISvcAcctPwd = Security.Decrypt(encryptionKey, o.PAMAPISvcAcctPwd);
                    d.PAMComSvcAcctPwd = Security.Decrypt(encryptionKey, o.PAMComSvcAcctPwd);
                    d.PAMMonSvcAcctPwd = Security.Decrypt(encryptionKey, o.PAMMonSvcAcctPwd);
                    d.RegSvcAcctPwd = Security.Decrypt(encryptionKey, o.RegSvcAcctPwd);
                    d.ResSvcAcctPwd = Security.Decrypt(encryptionKey, o.ResSvcAcctPwd);
                }

                //populate the form
                IList<string> selectedSvcs = o.ServicesToInstall;

                //hide fields
                label11.Visible = false;
                MIMSvcEmailPwd.Visible = false;
                copyBtn.Visible = false;

                foreach (var s in selectedSvcs)
                {

                    switch (s)
                    {
                        case "CommonServices":
                            svcs_mimsvcCb.Checked = true;
                            break;
                        case "WebPortals":
                            checkBox10.Checked = true;
                            break;
                        case "RegistrationPortal":
                            checkBox8.Checked = true;
                            break;
                        case "ResetPortal":
                            checkBox9.Checked = true;
                            break;
                        case "PAMServices":
                            checkBox7.Checked = true;
                            break;
                        case "FIMReporting":
                            checkBox6.Checked = true;
                            break;
                    }
                }

                //general
                logDirTxt.Text = o.LogPath;
                msiPathTxt.Text = o.MSIPath;
                comboBox1.Text = o.InstallDrive;

                //mim service
                sqlDbNameTxt.Text = o.SQLDBName;
                sqlInstanceTxt.Text = o.SQLInstance;
                checkBox2.Checked = o.SQLUseExisting;

                mailSrvTxt.Text = o.MailServer;
                checkBox1.Checked = o.MailUseSSL;
                checkBox3.Checked = o.MailIsExch;
                checkBox4.Checked = o.MailPollExch;
                checkBox5.Checked = o.MailIsExchOnline;

                MIMSvcAcctNameTxt.Text = o.MIMSvcAcctName;
                MIMSvcAcctPwdTxt.Text = d.MIMSvcAcctPwd;
                MIMSvcAccountDomain.Text = o.MIMSvcAcctDomain;
                MIMSvcEmailAddrTxt.Text = o.MIMSvcAcctEmail;
                MIMSvcEmailPwd.Text = d.MIMSvcAcctEmailPwd;

                MIMSyncServerTxt.Text = o.SyncServer;
                MIMSvcMAAcctTxt.Text = o.MIMSvcMaAcct;

                MIMSvcNameTxt.Text = o.MIMSvcAddr;

                MIMPortalUrlTxt.Text = o.MIMPortalURL;
                SpTimeoutTxt.Text = o.SPTimeOut;
                ssprRegUrlTxt.Text = o.MIMSSPRRegURL;

                regSvcAcctTxt.Text = o.RegSvcAcctName;
                regSvcAcctPwdTxt.Text = d.RegSvcAcctPwd;
                regMIMSvcNameTxt.Text = o.RegMIMSvcName;
                regExtCb.Checked = o.RegSvcIsExtranet;
                regPortalUrlTxt.Text = o.RegSvcHostName;
                regPortTxt.Text = o.RegSvcPort;
                regCofgFwTxt.Checked = o.RegConfigFW;

                resSvcAcctTxt.Text = o.ResSvcAcctName;
                resSvcAcctPwdTxt.Text = d.ResSvcAcctPwd;
                resMIMSvcNameTxt.Text = o.ResMIMSvcName;
                resExtCb.Checked = o.ResSvcIsExtranet;
                resPortalUrlTxt.Text = o.ResSvcHostName;
                resPortTxt.Text = o.ResSvcPort;
                resCfgFwTxt.Checked = o.ResConfigFW;

                pamMonAcctName.Text = o.PAMMonSvcAcctName;
                pamMonAcctDomain.Text = o.PAMMonSvcAcctDomain;
                pamMonAcctPwd.Text = d.PAMMonSvcAcctPwd;
                pamComAcctName.Text = o.PAMComSvcAcctName;
                pamComAcctDomain.Text = o.PAMComSvcAcctDomain;
                pamComAcctPwd.Text = d.PAMComSvcAcctPwd;
                pamApiAcctName.Text = o.PAMAPISvcAcctName;
                pamApiAcctDomain.Text = o.PAMAPISvcAcctDomain;
                pamApiAcctPwd.Text = d.PAMAPISvcAcctPwd;
                pamApiPort.Text = o.PAMAPIPort;
                pamCfgFwCb.Checked = o.PAMAPIConfigFW;

                scsmServerName.Text = o.SCSMServerName;

                //populate defaults
                if (string.IsNullOrWhiteSpace(comboBox1.Text)) comboBox1.Text = "C:";
                if (string.IsNullOrWhiteSpace(sqlDbNameTxt.Text)) sqlDbNameTxt.Text = "FIMService";
                if (string.IsNullOrWhiteSpace(SpTimeoutTxt.Text)) SpTimeoutTxt.Text = "180";
            }

            //to do

            //get current domain and use as default in all domain fields
            //change passwords to secure, encrypt with a keyword (not saved), prompt for keyword on file load
            //validate accounts
            //validate Exch connectivity
            //handle cert thumbprint prop 'CERTIFICATE_THUMBPRINT=1236454AVCD'
            //add SPNs, configure delegation

        }


        private void msiPathTxt_TextChanged(object sender, EventArgs e)
        {
            o.MSIPath = msiPathTxt.Text;
        }

        private void logDirTxt_TextChanged(object sender, EventArgs e)
        {
            o.LogPath = logDirTxt.Text;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            o.InstallDrive = comboBox1.Text;
        }

        private void sqlInstanceTxt_TextChanged(object sender, EventArgs e)
        {
            o.SQLInstance = sqlInstanceTxt.Text;
        }

        private void sqlDbNameTxt_TextChanged(object sender, EventArgs e)
        {
            o.SQLDBName = sqlDbNameTxt.Text;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            o.SQLUseExisting = checkBox2.Checked;
        }

        private void mailSrvTxt_TextChanged(object sender, EventArgs e)
        {
            o.MailServer = mailSrvTxt.Text;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            o.MailUseSSL = checkBox1.Checked;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            o.MailIsExch = checkBox3.Checked;
            if (!checkBox3.Checked)
            {
                checkBox4.Checked = false;
                checkBox4.Enabled = false;
                checkBox5.Checked = false;
            }
            else if (!checkBox5.Checked)
            {
                checkBox4.Enabled = true;
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            o.MailPollExch = checkBox4.Checked;
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            o.MailIsExchOnline = checkBox5.Checked;

            if (checkBox5.Checked)
            {
                label11.Visible = true;
                MIMSvcEmailPwd.Visible = true;
                checkBox4.Enabled = false;
                checkBox3.Enabled = false;
                checkBox1.Enabled = false;
                checkBox4.Checked = true;
                checkBox3.Checked = true;
                checkBox1.Checked = true;
                mailSrvTxt.Text = "outlook.office365.com";
                mailSrvTxt.Enabled = false;
            }
            else
            {
                label11.Visible = false;
                MIMSvcEmailPwd.Visible = false;
                MIMSvcEmailPwd.Text = null;
                checkBox4.Enabled = true;
                checkBox3.Enabled = true;
                checkBox1.Enabled = true;
                mailSrvTxt.Enabled = true;
                mailSrvTxt.Text = null;
            }
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void MIMSvcAcctNameTxt_TextChanged(object sender, EventArgs e)
        {
            o.MIMSvcAcctName = MIMSvcAcctNameTxt.Text;
            if (MIMSvcAcctNameTxt.Text.EndsWith("$"))
            {
                o.MIMSvcAcctIsGMSA = true;
                MIMSvcAcctPwdTxt.Text = null;
                MIMSvcAcctPwdTxt.Enabled = false;
            }
            else
            {
                o.MIMSvcAcctIsGMSA = false;
                MIMSvcAcctPwdTxt.Enabled = true;
            }
            if (MIMSvcAcctNameTxt.Text.EndsWith("$") || checkBox5.Checked)
            {
                label11.Visible = true;
                MIMSvcEmailPwd.Visible = true;
                label11.Visible = true;
            }
            else
            {
                label11.Visible = false;
                MIMSvcEmailPwd.Visible = false;
                label11.Visible = false;
                MIMSvcEmailPwd.Text = null;
            }
        }

        private void MIMSvcAcctPwdTxt_TextChanged(object sender, EventArgs e)
        {
            d.MIMSvcAcctPwd = MIMSvcAcctPwdTxt.Text;
        }

        private void MIMSvcAccountDomain_TextChanged(object sender, EventArgs e)
        {
            o.MIMSvcAcctDomain = MIMSvcAccountDomain.Text;
        }

        private void MIMSvcEmailAddrTxt_TextChanged(object sender, EventArgs e)
        {
            o.MIMSvcAcctEmail = MIMSvcEmailAddrTxt.Text;
        }

        private void MIMSvcEmailPwd_TextChanged(object sender, EventArgs e)
        {
            d.MIMSvcAcctEmailPwd = MIMSvcEmailPwd.Text;
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void MIMSyncServerTxt_TextChanged(object sender, EventArgs e)
        {
            o.SyncServer = MIMSyncServerTxt.Text;
        }

        private void MIMSvcMAAcctTxt_TextChanged(object sender, EventArgs e)
        {
            o.MIMSvcMaAcct = MIMSvcMAAcctTxt.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveConfig();
        }

        private void SaveConfig()
        {
            if (encryptionKey == null)
            {
                //prompt for encryption key
                var form1 = new GetKey(true);
                form1.ShowDialog();
                encryptionKey = form1.Key;
            }

            if (encryptionKey != null)
            {
                o.MIMSvcAcctEmailPwd = Security.Encrypt(encryptionKey, d.MIMSvcAcctEmailPwd);
                o.MIMSvcAcctPwd = Security.Encrypt(encryptionKey, d.MIMSvcAcctPwd);
                o.PAMAPISvcAcctPwd = Security.Encrypt(encryptionKey, d.PAMAPISvcAcctPwd);
                o.PAMComSvcAcctPwd = Security.Encrypt(encryptionKey, d.PAMComSvcAcctPwd);
                o.PAMMonSvcAcctPwd = Security.Encrypt(encryptionKey, d.PAMMonSvcAcctPwd);
                o.RegSvcAcctPwd = Security.Encrypt(encryptionKey, d.RegSvcAcctPwd);
                o.ResSvcAcctPwd = Security.Encrypt(encryptionKey, d.ResSvcAcctPwd);
            }
            else
            {
                o.MIMSvcAcctEmailPwd = null;
                o.MIMSvcAcctPwd = null;
                o.PAMAPISvcAcctPwd = null;
                o.PAMComSvcAcctPwd = null;
                o.PAMMonSvcAcctPwd = null;
                o.RegSvcAcctPwd = null;
                o.ResSvcAcctPwd = null;
            }

            string config = JsonConvert.SerializeObject(o);
            File.WriteAllText(configFile, config);
        }
        private void MIMSvcNameTxt_TextChanged(object sender, EventArgs e)
        {
            o.MIMSvcAddr = MIMSvcNameTxt.Text;
        }
        private string BooleanToString(bool Input)
        {
            if (Input)
            {
                return "1";
            }
            else
            {
                return "0";
            }
        }

        private void MIMPortalUrlTxt_TextChanged(object sender, EventArgs e)
        {
            o.MIMPortalURL = MIMPortalUrlTxt.Text;
        }

        private void SpTimeoutTxt_TextChanged(object sender, EventArgs e)
        {
            o.SPTimeOut = SpTimeoutTxt.Text;
        }

        private void ssprRegUrlTxt_TextChanged(object sender, EventArgs e)
        {
            o.MIMSSPRRegURL = ssprRegUrlTxt.Text;
        }

        private void regSvcAcctTxt_TextChanged(object sender, EventArgs e)
        {
            o.RegSvcAcctName = regSvcAcctTxt.Text;
        }

        private void regSvcAcctPwdTxt_TextChanged(object sender, EventArgs e)
        {
            d.RegSvcAcctPwd = regSvcAcctPwdTxt.Text;
        }

        private void regMIMSvcNameTxt_TextChanged(object sender, EventArgs e)
        {
            o.RegMIMSvcName = regMIMSvcNameTxt.Text;
        }

        private void regExtCb_CheckedChanged(object sender, EventArgs e)
        {
            o.RegSvcIsExtranet = regExtCb.Checked;
        }

        private void regPortalUrlTxt_TextChanged(object sender, EventArgs e)
        {
            o.RegSvcHostName = regPortalUrlTxt.Text;
        }

        private void regPortTxt_TextChanged(object sender, EventArgs e)
        {
            o.RegSvcPort = regPortTxt.Text;
        }

        private void regCofgFwTxt_CheckedChanged(object sender, EventArgs e)
        {
            o.RegConfigFW = regCofgFwTxt.Checked;
        }

        private void resSvcAcctTxt_TextChanged(object sender, EventArgs e)
        {
            o.ResSvcAcctName = resSvcAcctTxt.Text;
        }

        private void resSvcAcctPwdTxt_TextChanged(object sender, EventArgs e)
        {
            d.ResSvcAcctPwd = resSvcAcctPwdTxt.Text;
        }

        private void resMIMSvcTxt_TextChanged(object sender, EventArgs e)
        {
            o.ResMIMSvcName = resMIMSvcNameTxt.Text;
        }

        private void resExtCb_CheckedChanged(object sender, EventArgs e)
        {
            o.ResSvcIsExtranet = resExtCb.Checked;
        }

        private void resPortalUrlTxt_TextChanged(object sender, EventArgs e)
        {
            o.ResSvcHostName = resPortalUrlTxt.Text;
        }

        private void resPortTxt_TextChanged(object sender, EventArgs e)
        {
            o.ResSvcPort = resPortTxt.Text;
        }

        private void resCfgFwTxt_CheckedChanged(object sender, EventArgs e)
        {
            o.ResConfigFW = resCfgFwTxt.Checked;
        }

        private void pamMonAcctName_TextChanged(object sender, EventArgs e)
        {
            o.PAMMonSvcAcctName = pamMonAcctName.Text;

        }

        private void pamMonAcctDomain_TextChanged(object sender, EventArgs e)
        {
            o.PAMMonSvcAcctDomain = pamMonAcctDomain.Text;

        }

        private void pamMonAcctPwd_TextChanged(object sender, EventArgs e)
        {
            d.PAMMonSvcAcctPwd = pamMonAcctPwd.Text;
        }

        private void pamComAcctName_TextChanged(object sender, EventArgs e)
        {
            o.PAMComSvcAcctName = pamComAcctName.Text;
        }

        private void pamComAcctDomain_TextChanged(object sender, EventArgs e)
        {
            o.PAMComSvcAcctDomain = pamComAcctDomain.Text;
        }

        private void pamComAcctPwd_TextChanged(object sender, EventArgs e)
        {
            d.PAMComSvcAcctPwd = pamComAcctPwd.Text;
        }

        private void pamApiAcctName_TextChanged(object sender, EventArgs e)
        {
            o.PAMAPISvcAcctName = pamApiAcctName.Text;
        }

        private void pamApiAcctDomain_TextChanged(object sender, EventArgs e)
        {
            o.PAMAPISvcAcctDomain = pamApiAcctDomain.Text;
        }

        private void pamApiAcctPwd_TextChanged(object sender, EventArgs e)
        {
            d.PAMAPISvcAcctPwd = pamApiAcctPwd.Text;
        }

        private void pamApiPort_TextChanged(object sender, EventArgs e)
        {
            o.PAMAPIPort = pamApiPort.Text;
        }

        private void pamCfgFwCb_CheckedChanged(object sender, EventArgs e)
        {
            o.PAMAPIConfigFW = pamCfgFwCb.Checked;
        }

        private void svcs_mimsvcCb_CheckedChanged(object sender, EventArgs e)
        {
            if (svcs_mimsvcCb.Checked)
            {
                if (o.ServicesToInstall == null || !o.ServicesToInstall.Contains("CommonServices"))
                    o.ServicesToInstall.Add("CommonServices");
            }
            else
            {
                if (o.ServicesToInstall.Contains("CommonServices"))
                    o.ServicesToInstall.Remove("CommonServices");
            }
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox6.Checked)
            {
                if (!o.ServicesToInstall.Contains("FIMReporting"))
                    o.ServicesToInstall.Add("FIMReporting");
                svcs_mimsvcCb.Checked = true;
                svcs_mimsvcCb.Enabled = false;
            }
            else
            {
                if (o.ServicesToInstall.Contains("FIMReporting"))
                    o.ServicesToInstall.Remove("FIMReporting");
                if (!checkBox7.Checked)
                {
                    svcs_mimsvcCb.Enabled = true;
                }
            }
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox7.Checked)
            {
                if (!o.ServicesToInstall.Contains("PAMServices"))
                    o.ServicesToInstall.Add("PAMServices");
                svcs_mimsvcCb.Checked = true;
                svcs_mimsvcCb.Enabled = false;
            }
            else
            {
                if (o.ServicesToInstall.Contains("PAMServices"))
                    o.ServicesToInstall.Remove("PAMServices");
                if (!checkBox6.Checked)
                {
                    svcs_mimsvcCb.Enabled = true;
                }
            }
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox8.Checked)
            {
                if (!o.ServicesToInstall.Contains("RegistrationPortal"))
                    o.ServicesToInstall.Add("RegistrationPortal");
            }
            else
            {
                if (o.ServicesToInstall.Contains("RegistrationPortal"))
                    o.ServicesToInstall.Remove("RegistrationPortal");
            }
        }

        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox9.Checked)
            {
                if (!o.ServicesToInstall.Contains("ResetPortal"))
                    o.ServicesToInstall.Add("ResetPortal");
            }
            else
            {
                if (o.ServicesToInstall.Contains("ResetPortal"))
                    o.ServicesToInstall.Remove("ResetPortal");
            }
        }

        private void checkBox10_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox10.Checked)
            {
                if (!o.ServicesToInstall.Contains("WebPortals"))
                    o.ServicesToInstall.Add("WebPortals");
            }
            else
            {
                if (o.ServicesToInstall.Contains("WebPortals"))
                    o.ServicesToInstall.Remove("WebPortals");
            }
        }

        private void scsmServerName_TextChanged(object sender, EventArgs e)
        {
            o.SCSMServerName = scsmServerName.Text;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //prompt for encryption key
            var form1 = new GetKey(true);
            form1.ShowDialog();
            encryptionKey = form1.Key;
        }

        private void msiBtn_Click(object sender, EventArgs e)
        {
            var result = msiFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                if (msiFileDialog1.FileName.ToLower().EndsWith("service and portal.msi"))
                {
                    msiPathTxt.Text = Path.GetDirectoryName(msiFileDialog1.FileName);
                }
                else
                {
                    Utils.ShowErrorMessage("Invalid file, please browse to 'Service and Portal.msi'.");
                }
            }
        }

        private void logDirBtn_Click(object sender, EventArgs e)
        {
            var result = logDirBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                logDirTxt.Text = logDirBrowserDialog1.SelectedPath;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var validationForm = new Validation(o, d);
            validationForm.Show();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 6)
            {
                copyBtn.Visible = true;

                string logFile = Path.Combine(o.LogPath, "MIM_Service_Install_Log_%yyyy%_%mn%_%dd%_%hh%_%MM%_%ss%.log");
                string installDir = o.InstallDrive + @"\Program Files\Microsoft Forefront Identity Manager\2010\";
                string msiPath = Path.Combine(o.MSIPath, "Service and Portal.msi");

                string output = "";

                output += "@echo off";
                output += "for /f \"tokens = 2 delims == \" %%a in ('wmic OS Get localdatetime /value') do set \"dt =%% a\"" + Environment.NewLine;
                output += "set \"yyyy=%dt:~0,4%\" & set \"mn=%dt:~4,2%\" & set \"dd=%dt:~6,2%\"" + Environment.NewLine;
                output += "set \"hh=%dt:~8,2%\" & set \"MM=%dt:~10,2%\" & set \"ss=%dt:~12,2%\"" + Environment.NewLine;

                output += "net start spadminv4" + Environment.NewLine;

                output += "msiexec.exe /i \"" + msiPath + "\" INSTALLDIR=\"" + installDir + "\"" +
                    " ACCEPT_EULA=1 FIREWALL_CONF=1 SQMOPTINSETTING=0 REBOOT=ReallySupress";

                output += " ADDLOCAL=" + string.Join(",", o.ServicesToInstall);

                if (o.ServicesToInstall.Contains("CommonServices"))
                {
                    if (!string.IsNullOrEmpty(o.MailServer)) { output += " MAIL_SERVER=" + o.MailServer; }
                    output += " MAIL_SERVER_USE_SSL=" + BooleanToString(o.MailUseSSL);
                    output += " MAIL_SERVER_IS_EXCHANGE=" + BooleanToString(o.MailIsExch);
                    output += " POLL_EXCHANGE_ENABLED=" + BooleanToString(o.MailPollExch);
                    output += " MAIL_SERVER_IS_EXCHANGE_ONLINE=" + BooleanToString(o.MailIsExchOnline);

                    if (!string.IsNullOrEmpty(o.MIMPortalURL)) { output += " SQLSERVER_SERVER=" + o.SQLInstance; }
                    if (!string.IsNullOrEmpty(o.SQLInstance)) { output += " SQLSERVER_DATABASE=" + o.SQLDBName; }
                    if (!string.IsNullOrEmpty(o.SQLDBName)) { output += " SHAREPOINT_URL=" + o.MIMPortalURL; }
                    output += " EXISTINGDATABASE=" + BooleanToString(o.SQLUseExisting);

                    if (!string.IsNullOrEmpty(o.MIMSvcAcctName)) { output += " SERVICE_ACCOUNT_NAME=" + o.MIMSvcAcctName; }
                    if (!string.IsNullOrEmpty(d.MIMSvcAcctPwd)) { output += " SERVICE_ACCOUNT_PASSWORD=\"" + d.MIMSvcAcctPwd + "\""; }
                    if (!string.IsNullOrEmpty(o.MIMSvcAcctDomain)) { output += " SERVICE_ACCOUNT_DOMAIN=" + o.MIMSvcAcctDomain; }
                    if (o.MIMSvcAcctIsGMSA) { output += " USE_MANAGED_ACCOUNT_FOR_SERVICE=1"; }
                    if (!string.IsNullOrEmpty(o.MIMSvcAcctEmail)) { output += " SERVICE_ACCOUNT_EMAIL=" + o.MIMSvcAcctEmail; }
                    if (!string.IsNullOrEmpty(d.MIMSvcAcctEmailPwd)) { output += " SERVICE_ACCOUNT_EMAIL_PASSWORD=\"" + d.MIMSvcAcctEmailPwd + "\""; }

                    if (!string.IsNullOrEmpty(o.MIMSvcMaAcct)) { output += " SYNCHRONIZATION_SERVER_ACCOUNT=" + o.MIMSvcMaAcct; }
                    if (!string.IsNullOrEmpty(o.SyncServer)) { output += " SYNCHRONIZATION_SERVER=" + o.SyncServer; }
                    if (!string.IsNullOrEmpty(o.MIMSvcAddr)) { output += " SERVICEADDRESS=" + o.MIMSvcAddr; }

                }

                if (o.ServicesToInstall.Contains("WebPortals"))
                {
                    output += " SHAREPOINTUSERS_CONF=1";
                    if (!string.IsNullOrEmpty(o.SPTimeOut)) { output += " SHAREPOINTTIMEOUT=" + o.SPTimeOut; }
                    if (!string.IsNullOrEmpty(o.MIMSSPRRegURL)) { output += " REGISTRATION_PORTAL_URL=" + o.MIMSSPRRegURL; }
                }

                if (o.ServicesToInstall.Contains("RegistrationPortal"))
                {
                    if (!string.IsNullOrEmpty(o.RegSvcAcctName)) { output += " REGISTRATION_ACCOUNT=" + o.RegSvcAcctName; }
                    if (!string.IsNullOrEmpty(o.RegSvcPort)) { output += " REGISTRATION_PORT=" + o.RegSvcPort; }
                    if (!string.IsNullOrEmpty(o.RegSvcAcctPwd)) { output += " REGISTRATION_ACCOUNT_PASSWORD=\"" + d.RegSvcAcctPwd + "\""; }
                    if (!string.IsNullOrEmpty(o.RegMIMSvcName)) { output += " REGISTRATION_SERVERNAME=" + o.RegMIMSvcName; }
                    output += " IS_REGISTRATION_EXTRANET=" + BooleanToString(o.RegSvcIsExtranet);
                    if (!string.IsNullOrEmpty(o.RegSvcHostName)) { output += " REGISTRATION_PORTAL_URL=" + o.RegSvcHostName; }
                    output += " REGISTRATION_FIREWALL_CONFIG=" + BooleanToString(o.RegConfigFW);
                }

                if (o.ServicesToInstall.Contains("ResetPortal"))
                {
                    if (!string.IsNullOrEmpty(o.ResSvcAcctName)) { output += " RESET_ACCOUNT=" + o.ResSvcAcctName; }
                    if (!string.IsNullOrEmpty(o.ResSvcPort)) { output += " RESET_ACCOUNT_PASSWORD=\"" + d.ResSvcAcctPwd + "\""; }
                    if (!string.IsNullOrEmpty(o.ResSvcAcctPwd)) { output += " RESET_PORT=" + o.ResSvcPort; }
                    if (!string.IsNullOrEmpty(o.ResMIMSvcName)) { output += " RESET_SERVERNAME=" + o.ResMIMSvcName; }
                    output += " IS_RESET_EXTRANET=" + BooleanToString(o.ResSvcIsExtranet);
                    if (!string.IsNullOrEmpty(o.ResSvcHostName)) { output += " RESET_PORTAL_URL=" + o.ResSvcHostName; }
                    output += " RESET_FIREWALL_CONFIG=" + BooleanToString(o.ResConfigFW);
                }

                if (o.ServicesToInstall.Contains("PAMServices"))
                {
                    if (!string.IsNullOrEmpty(o.PAMMonSvcAcctDomain)) { output += " PAM_MONITORING_SERVICE_ACCOUNT_DOMAIN=" + o.PAMMonSvcAcctDomain; }
                    if (!string.IsNullOrEmpty(o.PAMMonSvcAcctName)) { output += " PAM_MONITORING_SERVICE_ACCOUNT_NAME=" + o.PAMMonSvcAcctName; }
                    if (!string.IsNullOrEmpty(d.PAMMonSvcAcctPwd)) { output += " PAM_MONITORING_SERVICE_ACCOUNT_PASSWORD=\"" + d.PAMMonSvcAcctPwd + "\""; }
                    if (!string.IsNullOrEmpty(o.PAMComSvcAcctDomain)) { output += " PAM_COMPONENT_SERVICE_ACCOUNT_DOMAIN=" + o.PAMComSvcAcctDomain; }
                    if (!string.IsNullOrEmpty(o.PAMComSvcAcctName)) { output += " PAM_COMPONENT_SERVICE_ACCOUNT_NAME=" + o.PAMComSvcAcctName; }
                    if (!string.IsNullOrEmpty(d.PAMComSvcAcctPwd)) { output += " PAM_COMPONENT_SERVICE_ACCOUNT_PASSWORD=\"" + d.PAMComSvcAcctPwd + "\"" + ""; }
                    if (!string.IsNullOrEmpty(o.PAMAPISvcAcctDomain)) { output += " PAM_REST_API_APPPOOL_ACCOUNT_DOMAIN=" + o.PAMAPISvcAcctDomain; }
                    if (!string.IsNullOrEmpty(o.PAMAPISvcAcctName)) { output += " PAM_REST_API_APPPOOL_ACCOUNT_NAME=" + o.PAMAPISvcAcctName; }
                    if (!string.IsNullOrEmpty(d.PAMAPISvcAcctPwd)) { output += " PAM_REST_API_APPPOOL_ACCOUNT_PASSWORD=\"" + d.PAMAPISvcAcctPwd + "\""; }
                    if (!string.IsNullOrEmpty(o.PAMAPIPort)) { output += " MIMPAM_REST_API_PORT=" + o.PAMAPIPort; }
                    output += " CONFIG_FIREWALL=" + BooleanToString(o.PAMAPIConfigFW);
                }

                if (o.ServicesToInstall.Contains("FIMReporting"))
                {
                    if (!string.IsNullOrEmpty(o.SCSMServerName)) { output += " SERVICE_MANAGER_SERVER=" + o.SCSMServerName; }
                }

                if (!string.IsNullOrWhiteSpace(o.LogPath))
                {
                    output += " /l*v \"" + logFile +"\"";
                }

                output += " /qn";

                outputTb.Text = output;
            }
            else
            {
                copyBtn.Visible = false;
            }
        }

        private void copyBtn_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(outputTb.Text);
        }

        private void outputTb_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
