//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NavigationDrawerPopUpMenu2.db
{
    using System;
    using System.Collections.Generic;
    
    public partial class Kontrakt
    {
        public int Id { get; set; }
        public Nullable<int> IdZayavki { get; set; }
        public Nullable<int> IdForeman { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<int> IdBrigadir { get; set; }
    
        public virtual Foreman Foreman { get; set; }
        public virtual Users Users { get; set; }
        public virtual Zayavki Zayavki { get; set; }
    }
}
