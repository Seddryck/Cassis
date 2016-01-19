using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cassis.Core
{
    public interface IPackageEvents
    {
        IList<string> Errors { get; set; }
    }
}
