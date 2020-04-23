using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User
{
    public class ServiceLocator
    {
        public static IServiceProvider Instance { get; set; }
    }
}
