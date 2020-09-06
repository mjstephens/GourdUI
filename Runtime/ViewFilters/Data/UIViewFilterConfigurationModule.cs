using System.Collections.Generic;

namespace GourdUI
{
    [System.Serializable]
    public class UIViewFilterConfigurationModule
    {
        public List<UIViewFilterBaseConfigDataTemplate> positiveFilters = 
            new List<UIViewFilterBaseConfigDataTemplate>();
        public List<UIViewFilterBaseConfigDataTemplate> negativeFilters = 
            new List<UIViewFilterBaseConfigDataTemplate>();
    }
}