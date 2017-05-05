using System;

using TestStack.Seleno.Configuration;

namespace Acom.Selenium.Tests.Fixtures
{
    public abstract class ConcernFor<T, TTestClass> : IDisposable
        where TTestClass : ConcernFor<T, TTestClass>
    {
        protected ConcernFor() : this(true)
        {
        }

        protected ConcernFor(bool cacheResult)
        {
            if (lazyResult == null || !cacheResult)
                lazyResult = new Lazy<T>(this.Given);

            Host.SetupAppliTools(this.GetType(), "");
        }

        protected SelenoHost SelenoHost
        {
            get
            {
                return Tests.Host.Instance;
            }
        }

        public T Sut
        {
            get
            {
                return lazyResult.Value;
            }
        }

        public abstract T Given();
        private static Lazy<T> lazyResult = null;

        public void Dispose()
        {
            Host.CloseEyes();
        }
    }
}