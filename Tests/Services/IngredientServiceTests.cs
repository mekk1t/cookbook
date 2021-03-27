using DatabaseTestsConnector;
using KitProjects.Fixtures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace KitProjects.MasterChef.Tests.Services
{
    [Collection("Db")]
    public class IngredientServiceTests
    {
        private readonly DbFixture _fixture;

        public IngredientServiceTests(DbFixture fixture)
        {
            _fixture = fixture;
        }
    }
}
