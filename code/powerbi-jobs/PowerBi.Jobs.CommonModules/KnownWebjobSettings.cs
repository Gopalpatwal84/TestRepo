using Acom.Configuration;

namespace PowerBI.Jobs.CommonModules
{
    public class KnownWebjobSettings
    {
        public const string SlotNameField = "%" + KnownSlots.FieldNames.AcomConfigurationSlotName + "%";
    }
    
    public class KnownStorageKeys
    {
        public const string GlobalSingleton = "GlobalSingleton";
    }
}
