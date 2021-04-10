using KitProjects.Fixtures;
using KitProjects.MasterChef.Kernel.Recipes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace KitProjects.MasterChef.Tests.Editors
{
    [Collection("Db")]
    public class RecipeStepEditorTests : IDisposable
    {
        private readonly List<DbContext> _dbContexts = new();
        private readonly RecipeStepEditor _sut;
        private readonly DbFixture _fixture;

        public RecipeStepEditorTests(DbFixture fixture)
        {
            _fixture = fixture;
            var queryDbContext = _fixture.DbContext;
            _sut = new RecipeStepEditor()
        }


        public void Dispose()
        {
            foreach (var dbContext in _dbContexts)
            {
                dbContext.Dispose();
            }
        }
    }
}
