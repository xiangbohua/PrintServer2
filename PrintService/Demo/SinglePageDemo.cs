using PrintService.Template;

namespace PrintService.Demo
{


    /// <summary>
    /// Summary description for RepairProductLabel.
    /// </summary>
    public partial class SinglePageDemo : PdfPrintBase
    {
        public string product_code;
        public string product_name;
        public string quantity;
        public string qr_code;

        public SinglePageDemo()
        {
            InitializeComponent();
        }

        public override void SetReportData()
        {
            txtProductCode.Value = this.product_code;
            txtProductName.Value = this.product_name;
            txtQuantity.Value = this.quantity;
            barcode1.Value = this.qr_code;
        }
    }
}