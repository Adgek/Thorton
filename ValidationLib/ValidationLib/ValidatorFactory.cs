using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValidationLib
{
    static public class ValidatorFactory
    {
        static public MessageValidator GetValidator(string datatype)
        {
            datatype = datatype.ToLower();
            switch(datatype)
            {
                case "int":
                    return new IntValidator();
                case "char":
                    return new CharValidator();
                case "short":
                    return new ShortValidator();
                case "long":
                    return new LongValidator();
                case "float":
                    return new FloatValidator();
                case "double":
                    return new DoubleValidator();
                case "string":
                    return new StringValidator();
                default:
                    return new MessageValidator();
            }
        }
    }
}
