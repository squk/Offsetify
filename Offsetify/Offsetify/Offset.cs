using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Offsetify
{
    using System;

    internal class Offset
    {
        public string AssignedValue;
        public string DefaultValue;
        public string memOffset;
        public string Name;
        public string Type;

        public Offset(string name, string offset, string type, string assignedValue, string defaultValue)
        {
            this.Name = name;
            this.memOffset = offset;
            this.Type = type;
            this.AssignedValue = assignedValue;
            this.DefaultValue = defaultValue;
            this.CheckForEmptyFields();
        }

        private void CheckForEmptyFields()
        {
            if (this.Name == "")
            {
                this.Name = "Not Assigned";
            }
            if (this.AssignedValue == "")
            {
                this.AssignedValue = "Not Assigned";
            }
            if (this.DefaultValue == "")
            {
                this.DefaultValue = "Not Assigned";
            }
        }
    }
}

