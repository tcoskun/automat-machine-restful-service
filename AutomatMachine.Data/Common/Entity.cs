using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutomatMachine.Data.Common
{
    public class Entity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }
    }
}
