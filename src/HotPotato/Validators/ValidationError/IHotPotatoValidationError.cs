using System;
using System.Collections.Generic;
using System.Text;

namespace HotPotato.Validators
{
    public interface IHotPotatoValidationError
    {
        string Message { get; set; }
        string Kind { get; set; }
        string Property { get; set; }
        int LineNumber { get; set; }
        int LinePosition { get; set; }

    }
}
