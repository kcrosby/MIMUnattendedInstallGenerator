using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIMUnattendedInstallGenerator
{
    public class Objects
    {
        public class InstallOptions
        {
            //initializations
            private IList<string> servicesToInstall = new List<string>();

            //general
            public string InstallDrive { get; set; }
            public string LogPath { get; set; }
            public IList<string> ServicesToInstall
            {
                get
                {
                    return servicesToInstall;
                }
                set
                {
                    servicesToInstall = value;
                }
            }
            public string MSIPath { get; set; }

            //mim service
            public string MailServer { get; set; }
            public bool MailUseSSL { get; set; }
            public bool MailIsExch { get; set; }
            public bool MailPollExch { get; set; }
            public bool MailIsExchOnline { get; set; }

            public string SQLInstance { get; set; }
            public string SQLDBName { get; set; }
            public bool SQLUseExisting { get; set; }
            
            public string MIMSvcAcctName { get; set; }
            public string MIMSvcAcctPwd { get; set; }
            public string MIMSvcAcctDomain { get; set; }
            public string MIMSvcAcctEmail { get; set; }
            public string MIMSvcAcctEmailPwd { get; set; }

            public string SyncServer { get; set; }
            public string MIMSvcMaAcct { get; set; }
            public string MIMSvcAddr { get; set; }

            public string MIMSSPRRegURL { get; set; }
            public string MIMPortalURL { get; set; }
            public string SPTimeOut { get; set; }

            public string RegSvcAcctName { get; set; }
            public string RegSvcAcctPwd { get; set; }
            public string RegSvcPort { get; set; }
            public bool RegSvcIsExtranet { get; set; }
            public string RegMIMSvcName { get; set; }
            public string RegSvcHostName { get; set; }
            public bool RegConfigFW { get; set; }

            public string ResSvcAcctName { get; set; }
            public string ResSvcAcctPwd { get; set; }
            public string ResSvcPort { get; set; }
            public bool ResSvcIsExtranet { get; set; }
            public string ResMIMSvcName { get; set; }
            public string ResSvcHostName { get; set; }
            public bool ResConfigFW { get; set; }

            public string PAMMonSvcAcctName { get; set; }
            public string PAMMonSvcAcctDomain { get; set; }
            public string PAMMonSvcAcctPwd { get; set; }
            public string PAMComSvcAcctName { get; set; }
            public string PAMComSvcAcctDomain { get; set; }
            public string PAMComSvcAcctPwd { get; set; }
            public string PAMAPISvcAcctName { get; set; }
            public string PAMAPISvcAcctDomain { get; set; }
            public string PAMAPISvcAcctPwd { get; set; }
            public string PAMAPIPort { get; set; }
            public bool PAMAPIConfigFW { get; set; }

            public string SCSMServerName { get; set; }

        }

        public class DecryptedValues
        {
            public string MIMSvcAcctPwd { get; set; }
            public string MIMSvcAcctEmailPwd { get; set; }
            public string RegSvcAcctPwd { get; set; }
            public string ResSvcAcctPwd { get; set; }
            public string PAMMonSvcAcctPwd { get; set; }
            public string PAMComSvcAcctPwd { get; set; }
            public string PAMAPISvcAcctPwd { get; set; }
        }
    }
}
