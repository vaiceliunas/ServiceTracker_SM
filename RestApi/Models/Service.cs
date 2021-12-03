using RestApi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace KinlySmartMonitoringAssignment.Models
{
    public partial class Service
    {
        public Service()
        {
            Labels = new HashSet<Label>();
        }
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Port { get; set; }
        public string MaintainerEmail { get; set; }

        public virtual ICollection<Label> Labels { get; set; }

        public void SetId(int id)
        {
            this.Id = id;
        }
    }
}
