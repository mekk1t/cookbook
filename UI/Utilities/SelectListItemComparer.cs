using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace UI.Utilities
{
    public class SelectListItemComparer : IEqualityComparer<SelectListItem>
    {
        public bool Equals(SelectListItem x, SelectListItem y)
        {
            if (x.Value == y.Value)
                return true;

            return false;
        }

        public int GetHashCode([DisallowNull] SelectListItem obj) => obj.Value.GetHashCode();
    }
}
