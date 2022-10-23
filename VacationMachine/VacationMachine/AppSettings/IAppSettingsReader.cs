namespace VacationMachine.AppSettings;

public interface IAppSettingsReader
{
    T Read<T>();
}