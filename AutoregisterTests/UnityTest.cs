namespace IoCComparison.AutoregisterTests
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Practices.Unity;
    using NUnit.Framework;

    // need the "Unity Auto Registration" addon
    // http://autoregistration.codeplex.com/
    [TestFixture]
    public class UnityTest
    {
        [Test]
        public void CanMakeBusinessProcess()
        {
            UnityContainer container = new UnityContainer();
        }
    }
}
