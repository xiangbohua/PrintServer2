
using PrintService.Template;

namespace ReportTemplates.Transport
{
    /// <summary>
    /// Summary description for PickOrderProductLabel.
    /// </summary>
    public partial class SinglePageDemo : PdfPrintBase
    {
        public SinglePageDemo()
        {
            InitializeComponent();
        }

        public string after_sale_no;
        public string reciver;
        public string tel;
        public string address;
        public string warehouse_name;
        public string supplier_code;


        public override void SetReportData()
        {
            barCodeNumber.Value = this.after_sale_no;
            txtNumber.Value = this.after_sale_no;
            txtNumber2.Value = this.after_sale_no;
            txtName.Value = this.reciver;
            txtTel.Value = this.tel;
            txtAddress.Value = this.address;
            txtWarehouseName.Value = this.warehouse_name;
            txtSupplierCode.Value = this.supplier_code;
        }
    }
}