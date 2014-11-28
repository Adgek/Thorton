using NLog;
// By: Kyle Fowler and Matthew Anselmo
using Soa2.NestedConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soa2.Validation
{
    public static class ValidationSetup
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        /// <summary>
        /// Setup the validators for all parameters on the methods for all services
        /// </summary>
        /// <param name="services">the services read in from the config file</param>
        public static void SetupValidators(ServiceCollection services)
        {
            try
            {
                for (int x = 0; x < services.Count; x++)
                {
                    SetupValidator(services[x]);
                }
            }
            catch(Exception e)
            {
                logger.Trace(e.Message);
            }
        }

        /// <summary>
        /// Setup the validators for a service's method parameters
        /// </summary>
        /// <param name="services">the services read in from the config file</param>
        private static void SetupValidator(Service s)
        {
            foreach(Method m in  s.Methods)
            {
                foreach(Parameter p in m.Parameters)
                {
                    if(p.validatorSetup.Type.Equals("boolean",StringComparison.OrdinalIgnoreCase))
                    {
                        p.Validator = new BooleanValidator();
                    }
                    else if(p.validatorSetup.Type.Equals("int",StringComparison.OrdinalIgnoreCase))
                    {
                        p.Validator = new IntValidator(ValidationUtils.ParseIntFromString(p.validatorSetup.LengthMax), ValidationUtils.ParseIntFromString(p.validatorSetup.LengthMin));
                    }
                    else if (p.validatorSetup.Type.Equals("float", StringComparison.OrdinalIgnoreCase))
                    {
                        p.Validator = new FloatValidator(ValidationUtils.ParseIntFromString(p.validatorSetup.LengthMax), ValidationUtils.ParseIntFromString(p.validatorSetup.LengthMin));
                    }
                    else if (p.validatorSetup.Type.Equals("string", StringComparison.OrdinalIgnoreCase))
                    {
                        p.Validator = new StringValidator(ValidationUtils.ParseIntFromString(p.validatorSetup.LengthMax), ValidationUtils.ParseIntFromString(p.validatorSetup.LengthMin));
                    }
                    else
                    {
                        throw (new NotImplementedException("Non implemented type was used in the app.config file for validation: " + p.validatorSetup.Type));
                    }
                }
            }
        }
    }
}
