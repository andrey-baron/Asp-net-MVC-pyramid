//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DBFirstDAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class Products
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Products()
        {
            this.PointOnImgs = new HashSet<PointOnImgs>();
            this.ProductImages = new HashSet<ProductImages>();
            this.ProductOrders = new HashSet<ProductOrders>();
            this.ProductValues = new HashSet<ProductValues>();
            this.Reviews = new HashSet<Reviews>();
            this.Events = new HashSet<Events>();
            this.HomeEntity = new HashSet<HomeEntity>();
            this.Categories = new HashSet<Categories>();
            this.EnumValues = new HashSet<EnumValues>();
        }
    
        public int Id { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        public int TypePrice { get; set; }
        public bool IsSEOReady { get; set; }
        public System.DateTime DateCreation { get; set; }
        public System.DateTime DateChange { get; set; }
        public Nullable<int> PointOnImg_Id { get; set; }
        public Nullable<bool> SeasonOffer { get; set; }
        public string OneCId { get; set; }
        public Nullable<int> PopularCount { get; set; }
        public bool IsPriority { get; set; }
        public bool IsFilled { get; set; }
        public int TypeStatusProduct { get; set; }
        public string Content { get; set; }
        public bool IsNotUnloading1C { get; set; }
        public Nullable<int> SeoId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PointOnImgs> PointOnImgs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductImages> ProductImages { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductOrders> ProductOrders { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductValues> ProductValues { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Reviews> Reviews { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Events> Events { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HomeEntity> HomeEntity { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Categories> Categories { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EnumValues> EnumValues { get; set; }
        public virtual Seo Seo { get; set; }
    }
}
