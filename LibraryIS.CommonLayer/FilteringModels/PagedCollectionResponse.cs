﻿using System;
using System.Collections.Generic;

namespace LibraryIS.CommonLayer.FilteringModels
{
    public class PagedCollectionResponse<T> where T : class
    {
        public IEnumerable<T> Items { get; set; }
        public Uri NextPage { get; set; }
        public Uri PreviousPage { get; set; }
    }
}