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
    
    public partial class Pages
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public Nullable<int> ImageId { get; set; }
        public Nullable<int> SeoId { get; set; }
    
        public virtual Seo Seo { get; set; }
    }
}
