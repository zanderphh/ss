//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace 销售管理.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class DaContractGood
    {
        public DaContractGood()
        {
            this.DaInvoices = new HashSet<DaInvoice>();
            this.DaStockIns = new HashSet<DaStockIn>();
            this.DaExpenses = new HashSet<DaExpense>();
        }
    
        public int IntID { get; set; }
        public int IntContractID { get; set; }
        public int IntGoodsID { get; set; }
        public decimal MonPrice { get; set; }
        public int NumCount { get; set; }
        public decimal MonSum { get; set; }
        public System.DateTime DtCreate { get; set; }
        public long IntEmpNum { get; set; }
    
        public virtual DaPurchaseContract DaPurchaseContract { get; set; }
        public virtual T_Users T_Users { get; set; }
        public virtual DaGoods DaGood { get; set; }
        public virtual ICollection<DaInvoice> DaInvoices { get; set; }
        public virtual ICollection<DaStockIn> DaStockIns { get; set; }
        public virtual ICollection<DaExpense> DaExpenses { get; set; }
    }
}
