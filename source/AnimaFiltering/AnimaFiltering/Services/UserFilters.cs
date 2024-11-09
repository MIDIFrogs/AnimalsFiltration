// Copyright 2024 (c) MIDIFrogs (contact https://github.com/MIDIFrogs)
// Distributed under AGPL v3.0 license. See LICENSE.md file in the project root for more information
using AnimaFiltering.Services.Filters;
using System.Collections.ObjectModel;

namespace AnimaFiltering.Services
{
    /// <summary>
    /// Represents a collection for the user defined filters.
    /// </summary>
    public class UserFilters : Collection<IPostProcessingFilter>
    {
        public UserFilters(AppPreferences preferences)
        {
            Add(new SizeFilter(preferences));
            Add(new UnFocus(preferences));
            Add(new HeadFilter(preferences));
        }
    }
}