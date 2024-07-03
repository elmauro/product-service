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
        /// A common place for most tests to use. This setup helps ensure that tests are not 
        /// stepping on each other by sharing one setup of the service and its dependencies.
        /// </summary>
        public const string Integration = nameof(Integration);
    }

    /// <summary>
    /// When test classes use this label, they share one setup of the database.
    /// This is faster because we don't have to set up the database from scratch for each test class.
    /// It also avoids problems caused by deleting and recreating the database each time.
    /// </summary>
    [CollectionDefinition(TestCollections.Integration)]
    public class DatabaseCollection : ICollectionFixture<ProductApplicationFixture>
    {
    }
}
