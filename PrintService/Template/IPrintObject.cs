namespace PrintService.Template
{
    /// <summary>
    /// Define the print object
    /// </summary>
    public interface IPrintObject
    {
        void Print(bool printNows);

        int Intervel();
    }
}
