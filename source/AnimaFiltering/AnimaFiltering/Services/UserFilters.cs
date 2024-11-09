using AnimaFiltering.Services.Filters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimaFiltering.Services
{
    public class UserFilters : Collection<IPostProcessingFilter>
    {
        public UserFilters(AppPreferences preferences)
        {
            Add(new SizeFilter(preferences));
            Add(new UnFocus(preferences));
        }
    }
}
