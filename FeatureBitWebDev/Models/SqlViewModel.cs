using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeatureBitWebDev.Models
{
    public class SqlViewModel
    {
        public int Id { get; set; }

        public string AllowedUsers { get; set; }

        public string CreatedByUser { get; set; }


        public DateTime CreatedDateTime { get; set; }


        public string ExcludedEnvironments { get; set; }
        public string LastModifiedByuser { get; set; }
        public DateTime LastModifiedDateTime { get; set; }
        public int MinimumAllowedPermissionLevel { get; set; }
        public string Name { get; set; }
        public bool OnOff { get; set; }
        public int? ExactAllowedPermissionLevel { get; set; }
    }
}
