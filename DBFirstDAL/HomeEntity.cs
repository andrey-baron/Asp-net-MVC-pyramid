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
    
    public partial class HomeEntity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public HomeEntity()
        {
            this.Products = new HashSet<Products>();
            this.Categories = new HashSet<Categories>();
        }
    
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string TitleVideoGuide { get; set; }
        public string LinkYouTobe { get; set; }
        public Nullable<int> ThumbnailId { get; set; }
        public string CallToAction { get; set; }
    
        public virtual BannerWithPoints BannerWithPoints { get; set; }
        public virtual Images Images { get; set; }
        public virtual Faq Faq { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Products> Products { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Categories> Categories { get; set; }
    }
}
