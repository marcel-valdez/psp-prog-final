//p.Setup=20
namespace Programa.Core.Setup
{
    using System;
    using Programa.CollectionLibraries;
    using Programa.Core;
    using Programa.CollectionLibraries.Impl;

    public class Setup
    {
        static Setup()
        {
            ACollectionFactory.Factory = new CollectionFactory();
        }

        public static void Load()
        {

        }
    }
}
