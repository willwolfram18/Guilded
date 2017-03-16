using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Guilded.Controllers.Forums;
using Guilded.Data.DAL.Forums;
using Guilded.Data.Models.Forums;
using Xunit;

using DataModel = Guilded.Data.Models.Forums.ForumSection;
using ViewModel = Guilded.Data.ViewModels.Forums.ForumSection;

namespace Guilded.Tests.Controllers.Forums
{
    public class ForumSectionControllerUnitTest : ApiControllerUnitTestBase<ForumSectionController>
    {
        #region Properties
        #region Private Properties
        private const int NUM_SECTIONS = 5;

        private readonly List<ForumSection> forumSections = new List<ForumSection>();
        private readonly Mock<IForumSectionReadWriteRepository> mockForumSectionsRepo = new Mock<IForumSectionReadWriteRepository>();
        private readonly Mock<IForumsReadWriteDataContext> mockForumsDb = new Mock<IForumsReadWriteDataContext>();
        #endregion
        #endregion

        #region Test init
        protected override ForumSectionController SetupController()
        {
            mockForumsDb.Setup(c => c.ForumSections).Returns(mockForumSectionsRepo.Object);
            return new ForumSectionController(this.mockForumsDb.Object);
        }

        protected override void AdditionalSetup()
        {
            CreateForumSections();
        }
        private void CreateForumSections()
        {
            for (int i = 0; i < NUM_SECTIONS; i++)
            {
                forumSections.Add(new ForumSection
                {
                    Title = "Forum Section #" + i + 1,
                    DisplayOrder = i,
                    IsActive = i % 2 == 0,
                });
            }
        }
        #endregion

        #region Unit Tests
        #region ForumSectionController.Get(bool)
        [Fact]
        public void Get_ActiveOnly() {
            #region Arrange
            List<ForumSection> expectedSections = forumSections.Where(s => s.IsActive)
                .OrderBy(s => s.DisplayOrder)
                .ToList();
            mockForumSectionsRepo.Setup(r => r.Get(
                It.IsAny<Expression<Func<DataModel, bool>>>(),
                It.IsAny<Func<IQueryable<DataModel>, IOrderedQueryable<DataModel>>>()
            )).Returns(expectedSections.AsQueryable());
            #endregion
        
            #region Act
            JsonResult result = Controller.Get();
            #endregion
        
            #region Assert
            List<ViewModel> data = AssertResultIs<IEnumerable<ViewModel>>(result).ToList();
            Assert.Equal(expectedSections.Count, data.Count);
            for (int i = 0; i < expectedSections.Count; i++)
            {
                Assert.Equal(expectedSections[i].Id, data[i].Id);
                Assert.Equal(expectedSections[i].Title, data[i].Title);
            }
            mockForumSectionsRepo.Verify(r => r.Get(
                It.IsAny<Expression<Func<DataModel, bool>>>(),
                It.IsAny<Func<IQueryable<DataModel>, IOrderedQueryable<DataModel>>>()
            ), Times.Once());
            #endregion
        }

        [Fact]
        public void Get_AllSections() {
            #region Arrange
            List<ForumSection> expectedSections = forumSections.OrderBy(s => s.DisplayOrder)
                .ToList();
            mockForumSectionsRepo.Setup(r => r.Get(
                It.IsAny<Func<IQueryable<DataModel>, IOrderedQueryable<DataModel>>>()
            )).Returns(expectedSections.AsQueryable());
            #endregion
        
            #region Act
            JsonResult result = Controller.Get(false);
            #endregion
        
            #region Assert
            List<ViewModel> data = AssertResultIs<IEnumerable<ViewModel>>(result).ToList();
            Assert.Equal(expectedSections.Count, data.Count);
            for (int i = 0; i < expectedSections.Count; i++)
            {
                Assert.Equal(expectedSections[i].Id, data[i].Id);
                Assert.Equal(expectedSections[i].Title, data[i].Title);
            }
            mockForumSectionsRepo.Verify(r => r.Get(
                It.IsAny<Func<IQueryable<DataModel>, IOrderedQueryable<DataModel>>>()
            ), Times.Once());
            #endregion
        }
        #endregion

        #region Private methods
        private TResult AssertResultIs<TResult>(JsonResult result)
            where TResult : class
        {
            Assert.NotNull(result);
            TResult data = result.Value as TResult;
            Assert.NotNull(data);
            return data;
        }
        #endregion
        #endregion
    }
}