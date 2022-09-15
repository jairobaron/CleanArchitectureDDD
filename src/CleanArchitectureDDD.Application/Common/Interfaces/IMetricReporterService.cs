﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitectureDDD.Application.Common.Interfaces;

public interface IMetricReporterService
{
    void RegisterRequest();
    void RegisterResponseTime(int statusCode, string requestPath, string method, TimeSpan elapsed);
}
