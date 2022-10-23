namespace VacationMachine.AppSettings;

public class GenericOptions<T> : IOptions<T>
{
    public GenericOptions(AppSettingsReader appSettingsReader)
    {
        Current = appSettingsReader.Read<T>();
    }
    public T Current { get; }
}