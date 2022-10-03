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

namespace SuperScan
{
    public partial class FormChooseGalaxyTarget : Form
    {
        public FormChooseGalaxyTarget()
        {
            InitializeComponent();
            //Create name list for target dropdown box
            Configuration cfg = new Configuration();
            string tgtDirPath = cfg.ImageBankFolder;
            TargetNameList.Items.Clear();
            List<string> tgtDirList = Directory.GetDirectories(tgtDirPath).ToList();
            foreach (string dir in tgtDirList)
                if (Directory.GetFiles(dir, "*.fit").Count() > 0)
                    TargetNameList.Items.Add(Path.GetFileName(dir));
            return;

        }

        private void NGCOKButton_Click(object sender, EventArgs e)
        {
            Close();
        }


    }
}
