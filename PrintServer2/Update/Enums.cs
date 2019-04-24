namespace PrintServer2.Update
{
    public enum StepEnum
    {
        OnPrepare = 0,
        OnCheck = 1,
        OnBackup = 2,
        OnDownload = 3,
        OnUpdate = 4,
        OnRecover = 5
    }

    public enum UpdateType
    {
        NewFile = 1,
        NewVersion = 2,
        DeleteFile = 3
    }
}
