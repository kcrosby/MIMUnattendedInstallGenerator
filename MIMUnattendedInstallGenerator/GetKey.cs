using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MIMUnattendedInstallGenerator
{
    public partial class GetKey : Form
    {
        public string Key { get; set; }
        private bool ShowEncryptionText = false;

        public GetKey(bool EncryptionText)
        {
            InitializeComponent();
            bOK.Enabled = false;
            Key = null;
            ShowEncryptionText = EncryptionText;
        }

        private void bCancel_Click(object sender, EventArgs e)
        {
            Key = null;
            this.Close();
        }

        private void bOK_Click(object sender, EventArgs e)
        {
            Key = textBox2.Text;
            this.Close();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text.Length > 7 )
            {
                bOK.Enabled = true;
            }
            else
            {
                bOK.Enabled = false;
            }
        }

        private void GetKey_Load(object sender, EventArgs e)
        {
            if (ShowEncryptionText)
            {
                textBox1.Text = "Enter the key to be used for password encryption. Press 'Cancel' to skip password encryption (passwords will not be saved).";
               
            }
        }
    }
}
