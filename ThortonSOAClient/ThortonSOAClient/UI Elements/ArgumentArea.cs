using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ThortonSOAClient.UI_Elements
{
    public class ArgumentArea
    {
        public ArgumentArea(string argumentName)
        {
            Tb = new TextBox();
            Tb.Name = argumentName + "TB";
            Tb.Font = new Font(FontFamily.GenericSansSerif, 12f);
            Tb.Size = new Size(300,10);
            Lbl = new Label();
            Lbl.Font = new Font(FontFamily.GenericSansSerif, 12f);
            Lbl.Name = argumentName + "LBL";
            Lbl.Text = argumentName;
            Err = new ErrorProvider();
        }

        private string _argumentName;

        public string ArgumentName
        {
            get { return _argumentName; }
            set { _argumentName = value; }
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
