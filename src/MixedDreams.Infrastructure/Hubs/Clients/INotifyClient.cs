﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Application.Hubs.Clients
{
    public interface INotifyClient
    {
        public Task LowWaterNotification(float currectFullness);
    }
}
