using System;
using System.Collections.Generic;
using System.Text;

namespace MVVMLibrary.Models
{
    public class BaseMChildModel : BaseChildModel
    {
        #region Properties
        public string parentType { get; set; }
        #endregion
    }
}
