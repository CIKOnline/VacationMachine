namespace VacationMachine.AppSettings;

public interface IOptions<out TOption>
{
    TOption Current { get; }
}