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
    
    public partial class patient
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public patient()
        {
            this.order = new HashSet<order>();
        }
    
        public int ID { get; set; }
        public string login { get; set; }
        public string password { get; set; }
        public string FIO { get; set; }
        public System.DateTime birthday { get; set; }
        public string passport { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public int insurance_number { get; set; }
        public string insurance_type { get; set; }
        public int insurance_company_id { get; set; }
    
        public virtual insurance_company insurance_company { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<order> order { get; set; }
    }
}
