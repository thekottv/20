//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace _20.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class user
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public user()
        {
            this.done_service = new HashSet<done_service>();
            this.insurance_bills = new HashSet<insurance_bills>();
        }
    
        public int ID { get; set; }
        public string name { get; set; }
        public string login { get; set; }
        public string password { get; set; }
        public Nullable<System.DateTime> lastseen { get; set; }
        public string services { get; set; }
        public int type { get; set; }
        public byte[] avatar { get; set; }
        public string ip { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<done_service> done_service { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<insurance_bills> insurance_bills { get; set; }
        public virtual role role { get; set; }
    }
}
