using System.Collections.Generic;

namespace GourdUI
{
    [System.Serializable]
    public class UIViewFilterConfigurationModule
    {
        public List<UIViewFilterBaseConfigData> positiveFilters = 
            new List<UIViewFilterBaseConfigData>();
        public List<UIViewFilterBaseConfigData> negativeFilters = 
            new List<UIViewFilterBaseConfigData>();
    }
}