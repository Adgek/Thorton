using HL7Lib.ServiceData;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ThortonSOAClient.UI_Elements
{
    public class UiArgument : Argument
    {
        public UiArgument(int pos, string name, string datatype, bool man)
            : base(pos,  name,  datatype,  man)
        {
            Tb = new TextBox();
            Tb.Name = ArgumentName + "TB";
            Tb.Font = new Font(FontFamily.GenericSansSerif, 12f);
            Tb.Size = new Size(300,10);
            Lbl = new Label();
            Lbl.Font = new Font(FontFamily.GenericSansSerif, 12f);
            Lbl.Name = ArgumentName + "LBL";
            Lbl.Text = ArgumentName;
            Err = new ErrorProvider();
        }

        private TextBox _tb;

        public TextBox Tb
        {
            get { return _tb; }
            set { _tb = value; }
        }

        private Label _lbl;

        public Label Lbl
        {
            get { return _lbl; }
            set { _lbl = value; }
        }

        private ErrorProvider _err;

        public ErrorProvider Err
        {
            get { return _err; }
            set { _err = value; }
        }




    }
}
