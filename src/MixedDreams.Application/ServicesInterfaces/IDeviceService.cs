﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Infrastructure.Hubs.Clients
{
    public interface IDeviceService
    {
        public Task ConnectAsync();
    }
}
