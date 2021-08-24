﻿using System;

namespace Eliza.UI.Helpers
{
    public class Ref<T>
    {
        private Func<T> getter;
        private Action<T> setter;
        public Ref(Func<T> getter, Action<T> setter)
        {
            this.getter = getter;
            this.setter = setter;
        }
        public T Value
        {
            get { return this.getter(); }
            set { this.setter(value); }
        }
    }
}
