﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Common
{
    public class Page
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
    }
}