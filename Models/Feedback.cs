namespace OnlineShopeProject.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Feedback")]
    public partial class Feedback
    {
        [Key]
        public int FEEDBACK_ID { get; set; }

        [StringLength(50)]
        public string FEEDBACK_DETAIL { get; set; }

        [StringLength(50)]
        public string FEEDBACK_EMAIL { get; set; }

        [StringLength(50)]
        public string FEEDBACK_CONTACT { get; set; }

        [StringLength(50)]
        public string FEEDBACK_ADDRESS { get; set; }

        public int? CUSTOMER_FID { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
