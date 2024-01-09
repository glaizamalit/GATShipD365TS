using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
namespace GATShipD365TS.Models
{
    public class EventPayload
    {
        [Required]
        public string action { get; set; }
        [Required]
        public string tenant { get; set; }
        [Required]
        public string entity { get; set; }
        [Required]
        public eventData data { get; set; }      
    }
    public class eventData
    {
        [Required]
        public int id { get; set; }      
        [Required]
        public string typeId { get; set; }
        [Required]      
        public string appointmentId { get; set; }
        [Required]
        public DateTime startDate { get; set; }
        [Required]       
        public string notation { get; set; }
    }
}