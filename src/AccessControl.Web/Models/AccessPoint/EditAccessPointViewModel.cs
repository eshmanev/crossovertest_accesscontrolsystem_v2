using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AccessControl.Web.Models.Validation;

namespace AccessControl.Web.Models.AccessPoint
{
    public class EditAccessPointViewModel
    {
        [RequiredGuid]
        public Guid AccessPointId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [RequiredSelect]
        public string Site { get; set; }

        [RequiredSelect]
        public string Department { get; set; }

        public List<Tuple<string, string, string[]>> SiteDepartments { get; set; }

        public IEnumerable<string> GetVisibleDepartments()
        {
            if (string.IsNullOrWhiteSpace(Site))
                return Enumerable.Empty<string>();

            var entry = SiteDepartments.FirstOrDefault(x => x.Item1 == Site);
            return entry != null ? entry.Item3 : Enumerable.Empty<string>();
        }
    }
}