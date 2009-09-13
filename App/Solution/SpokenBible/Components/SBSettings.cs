using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace SpokenBible.Components
{
    public class SBSettings : ApplicationSettingsBase
    {
        [UserScopedSetting]
        public string Referencia
        {
            get
            {
                if (this["Referencia"] != null)
                {
                    return ((string)this["Referencia"]);
                }
                return string.Empty;
            }
            set
            {
                this["Referencia"] = value;
            }
        }
    }
}
