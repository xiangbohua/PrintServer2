
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

        [TemplatePara("String", "RBRB81612221333")]
        public string after_sale_no;

        [TemplatePara("String", "Alex .xiang")]
        public string reciver;
        [TemplatePara("String", "13585860114")]
        public string tel;
        [TemplatePara("String", "ShangHaiChina ")]
        public string address;
        [TemplatePara("String", "The Main distribute center")]
        public string warehouse_name;
        [TemplatePara("String", "RBRB81612221333")]
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