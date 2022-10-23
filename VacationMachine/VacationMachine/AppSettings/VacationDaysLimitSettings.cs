namespace VacationMachine.AppSettings;

public class VacationDaysLimitSettings
{
    public DefaultValues Default { get; init; }
    public AdditionalValues Additional { get; init; }

    public class DefaultValues
    {
        public long Default { get; init; }
        public long Slacker { get; init; }
    }
    
    public class AdditionalValues
    {
        public long Default { get; init; }
        public long Performer { get; init; }
    }
}