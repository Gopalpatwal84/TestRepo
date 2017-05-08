namespace BusinessPlatform.Jobs.CommonModules
{
    public class KnownTimerConfigs
    {
        public const string EveryThirtyMinutes = "0 */30 * * * *";
        public const string EveryFiveMinutes = "0 */5 * * * *";
        public const string OnTheHour = "0 0 * * * *";
        public const string HalfPast = "0 30 * * * *";
        public const string AtMidnight = "0 0 0 * * *";
    }
}

// Six-part expression format:
//
// * * * * * *
// - - - - - -
// | | | | | |
// | | | | | +--- day of week (0 - 6) (Sunday=0)
// | | | | +----- month (1 - 12)
// | | | +------- day of month (1 - 31)
// | | +--------- hour (0 - 23)
// | +----------- min (0 - 59)
// +------------- sec (0 - 59)
//
// The six-part expression behaves similarly to the traditional 
// crontab format except that it can denotate more precise schedules 
// that use a seconds component.
// 
// Star (*) in the value field above means all legal values as in 
// braces for that column. The value column can have a * or a list 
// of elements separated by commas. An element is either a number in 
// the ranges shown above or two numbers in the range separated by a 
// hyphen (meaning an inclusive range). 