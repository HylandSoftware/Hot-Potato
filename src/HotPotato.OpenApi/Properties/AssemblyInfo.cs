﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

#if DEBUG
[assembly: InternalsVisibleTo("HotPotato.Integration.Test")]
[assembly: InternalsVisibleTo("HotPotato.OpenApi.Test")]
#endif
