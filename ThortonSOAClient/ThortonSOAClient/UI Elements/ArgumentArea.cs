using HL7Lib.ServiceData;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ValidationLib;

namespace ThortonSOAClient.UI_Elements
{
    /// <summary>
    /// Class to represent an argument UI element
    /// </summary>
    public class UiArgument : HL7Lib.ServiceData.Message
    {
        /// <summary>
        ///  constructs a UI arg
        /// </summary>
        /// <param name="pos">passed to arg base class, see argument base class for info</param>
        /// <param name="name">passed to arg base class, see argument base class for info</param>
        /// <param name="datatype">passed to arg base class, see argument base class for info</param>
        /// <param name="man">passed to arg base class, see argument base class for info</param>
        public UiArgument(int pos, string name, string datatype, bool man)
            : base(pos,  name,  datatype,  man)
        {
            Tb = new TextBox();
            Tb.Name = Name + "TB";
            Tb.Font = new Font(FontFamily.GenericSansSerif, 12f);
            Tb.Size = new Size(300,10);
            Lbl = new Label();
            Lbl.Font = new Font(FontFamily.GenericSansSerif, 12f);
            Lbl.Name = Name + "LBL";
            Lbl.Text = Name;
            Err = new ErrorProvider();
            Validator = ValidationLib.ValidatorFactory.GetValidator(datatype);
        }

        /// <summary>
        /// The textbox that holds the arguments value
        /// </summary>
        private TextBox _tb;

        public TextBox Tb
        {
            get { return _tb; }
            set { _tb = value; }
        }

        /// <summary>
        /// the label for the text box
        /// </summary>
        private Label _lbl;

        public Label Lbl
        {
            get { return _lbl; }
            set { _lbl = value; }
        }

        /// <summary>
        /// the error provider for the textbox
        /// </summary>
        private ErrorProvider _err;

        public ErrorProvider Err
        {
            get { return _err; }
            set { _err = value; }
        }

        /// <summary>
        /// the validator class for the textbox
        /// </summary>
        private MessageValidator _validator;

        public MessageValidator Validator
        {
            get { return _validator; }
            set { _validator = value; }
        }
    }
}
