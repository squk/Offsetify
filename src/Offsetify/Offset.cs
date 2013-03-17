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
        public string Notes;

        public Offset()
        {
            Name = "";
            memOffset = "";
            Type = "";
            AssignedValue = "";
            DefaultValue = "";
            Notes = "";
        }

        public Offset(string name)
        {
            Name = name;
            memOffset = "";
            Type = "";
            AssignedValue = "";
            DefaultValue = "";
            Notes = "";
        }

        public Offset(string name, string offset, string type, string assignedValue, string defaultValue, string notes)
        {
            Name = name;
            memOffset = offset;
            Type = type;
            AssignedValue = assignedValue;
            DefaultValue = defaultValue;
            Notes = notes;
            CheckForEmptyFields();
        }

        private void CheckForEmptyFields()
        {
            if (Name == "")
            {
                Name = "Not Assigned";
            }
            if (AssignedValue == "")
            {
                AssignedValue = "Not Assigned";
            }
            if (DefaultValue == "")
            {
                DefaultValue = "Not Assigned";
            }
        }
    }
}

