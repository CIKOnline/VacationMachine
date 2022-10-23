namespace VacationMachine.AppSettings;

public class VacationDaysLimitSettings
{
    public DefaultValues Default { get; init; }
    public AdditionalValues Additional { get; init; }

    public class DefaultValues
    {
        public long Default { get; init; }
        public long Slackers { get; init; }
    }
    
    public class AdditionalValues
    {
        public long Default { get; init; }
        public long Performers { get; init; }
    }
}