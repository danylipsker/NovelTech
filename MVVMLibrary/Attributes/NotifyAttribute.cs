using System;
using System.Collections.Generic;
using System.Text;

namespace MVVMLibrary.Attributes
{
    [System.AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public class NotifyAttribute : Attribute
    {
        readonly string[] notifyNames;
        public NotifyAttribute(string[] notifyNames)
        {
            this.notifyNames = notifyNames;
        }

        public NotifyAttribute(string notifyName)
        {
            this.notifyNames = new[] { notifyName };
        }

        public string[] NotifyNames
        {
            get { return notifyNames; }
        }
    }
}
