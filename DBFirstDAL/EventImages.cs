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
    
    public partial class EventImages
    {
        public int EventId { get; set; }
        public Nullable<int> ImageId { get; set; }
    
        public virtual Events Events { get; set; }
        public virtual Images Images { get; set; }
    }
}
