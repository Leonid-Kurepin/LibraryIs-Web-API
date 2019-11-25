using System;

namespace LibraryIS.CommonLayer.FilteringModels
{
    public abstract class FilterModelBase : ICloneable
    {
        public int Page { get; set; }
        public int Limit { get; set; }

        protected FilterModelBase()
        {
            Page = 1;
            Limit = 100;
        }

        public abstract object Clone();
    }
}