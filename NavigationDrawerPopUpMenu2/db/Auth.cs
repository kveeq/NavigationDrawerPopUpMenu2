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
    
    public partial class Auth
    {
        public int IdUser { get; set; }
        public Nullable<int> IdRole { get; set; }
        public string Password { get; set; }
    
        public virtual Role Role { get; set; }
        public virtual Users Users { get; set; }
    }
}
