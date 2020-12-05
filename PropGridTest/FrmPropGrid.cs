using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PropGridTest
{
    public partial class FrmPropGrid : Form
    {
        public FrmPropGrid()
        {
            InitializeComponent();
            this.propGrid1.IsDisplayInGroup = true;
        }

        private void FrmPropGrid_Load(object sender, EventArgs e)
        {
            this.InitPropGrid();
        }

        private void InitPropGrid()
        {
            propGrid1.SetValue("外观", "Text", (object)"This is a test string", "Text");
            propGrid1.SetValue("外观", "Color", (object)Color.Red, "Color");
            propGrid1.SetValue("外观", "Font", (object)"宋体", "Font");
            propGrid1.SetValue("服务器", "ServerIP", (object)"172.168.9.9", "IP");
            propGrid1.SetValue("服务器", "ServerPort", (object)8888, "Int");
            propGrid1.SetValue("IsRight", (object)true, "YesNoCheckBox");
            propGrid1.SetValue("Percent", (object)80, "Progress");

            List<string> slist = new List<string>();
            slist.Add("男");
            slist.Add("女");
            propGrid1.SetValue("人员", "person", "女", "CustomEnum", slist, "");
            
            //propGrid1.SetValue("人员", "sex", "男,女", "Option", slist, "");
        }
    }
}
