namespace OnlineShopeProject.Models
{
    using System;
    using System.Web;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Product")]
    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        [Key]
        public int PRODUCT_ID { get; set; }

        [StringLength(50)]
        public string PRODUCT_NAME { get; set; }

        [StringLength(4000)]
        public string PRODUCT_DESCRIPTION { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? PRODUCT_PURCHASEPRICE { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? PRODUCT_SALEPRICE { get; set; }

        [StringLength(100)]
        public string PRODUCT_PICTURE { get; set; }

        [StringLength(100)]
        public string PRODUCT_PICTURE2 { get; set; }

        [StringLength(100)]
        public string PRODUCT_PICTURE3 { get; set; }

        public int? CATEGORY_FID { get; set; }

        public virtual Category Category { get; set; }

        [NotMapped]

        public int PRO_QUANTITY { get; set; }

        [NotMapped]

        public HttpPostedFileBase PRO_PIC { get; set; }

        [NotMapped]

        public HttpPostedFileBase PRO_PIC2{ get; set; }

        [NotMapped]

        public HttpPostedFileBase PRO_PIC3 { get; set; }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
