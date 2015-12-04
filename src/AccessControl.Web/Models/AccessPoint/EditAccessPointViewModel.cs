using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AccessControl.Contracts.Dto;
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
        public string Department { get; set; }

        public IList<IDepartment> AvailableDepartments { get; set; }
    }
}