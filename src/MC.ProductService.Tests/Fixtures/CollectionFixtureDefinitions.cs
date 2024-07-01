using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC.ProductService.Tests.Fixtures
{
    public static class TestCollections
    {
        /// <summary>
        /// Generic test collection where most tests should be able to live.
        /// Ensures that only one instance of the service and its dependencies are used
        /// for all test classes that use this fixture.
        /// </summary>
        public const string Integration = nameof(Integration);
    }

    /// <summary>
    /// All test classes marked with this collection name will run with a shared
    /// instance of the integration fixture, which is useful for saving time on the expensive
    /// operation of re-upping the db if we don't need to for each test class.
    ///
    /// It can also prevent wonkiness from deleting the db between test runs.
    /// </summary>
    [CollectionDefinition(TestCollections.Integration)]
    public class DatabaseCollection : ICollectionFixture<ProductApplicationFixture>
    {
    }
}
