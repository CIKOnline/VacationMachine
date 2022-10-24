namespace VacationMachine.AppSettings;

public class GenericOptions<T> : IOptions<T>
{
    public GenericOptions(IAppSettingsReader appSettingsReader)
    {
        Current = appSettingsReader.Read<T>();
    }

    public T Current { get; }
}