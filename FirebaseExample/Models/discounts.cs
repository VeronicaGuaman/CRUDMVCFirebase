//------------------------------------------------------------------------------
// <auto-generated>
//    Este código se generó a partir de una plantilla.
//
//    Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//    Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FirebaseExample.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class discounts
    {
        public string discounttype { get; set; }
        public string stor_id { get; set; }
        public Nullable<short> lowqty { get; set; }
        public Nullable<short> highqty { get; set; }
        public decimal discount { get; set; }
    
        public virtual stores stores { get; set; }
    }
}
