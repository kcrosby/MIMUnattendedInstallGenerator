using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MIMUnattendedInstallGenerator
{
    class Utils
    {
        public static bool ValidateAdAccount (string UserName, string password)
        {
            bool result = false;
            using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, "YOURDOMAIN"))
            {
                // validate the credentials
                result = pc.ValidateCredentials("myuser", "mypassword");
            }
            return result;
        }

        public static void ShowErrorMessage(string message)
        {
            var r = MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void ShowWarningMessage(string message)
        {
            var r = MessageBox.Show(message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

    }
}
