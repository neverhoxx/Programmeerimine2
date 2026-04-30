using KooliProjekt.WindowsForms.Api;
using Moq;
using Xunit;

namespace KooliProjekt.WindowsForms.UnitTests
{
    public class MainViewPresenterTests
    {
        private readonly Mock<IApiClient> _apiClientMock;
        private readonly Mock<IMainView> _mainViewMock;
        private readonly MainViewPresenter _presenter;

        public MainViewPresenterTests()
        {
            _apiClientMock = new Mock<IApiClient>();
            _mainViewMock = new Mock<IMainView>();
            _presenter = new MainViewPresenter(_apiClientMock.Object, _mainViewMock.Object);
        }

        [Fact]
        public async Task LoadData_should_call_ShowError_with_faulty_response()
        {
            // Arrange
            var faultyResponse = new OperationResult<PagedResult<Project>>();
            faultyResponse.Errors.Add("An error occurred while fetching data.");

            _apiClientMock
                .Setup(client => client.List(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(faultyResponse)
                .Verifiable();
            _mainViewMock
                .Setup(view => view.ShowError(It.IsAny<string>(), It.IsAny<OperationResult>()))
                .Verifiable();
            _mainViewMock
                .SetupSet(view => view.DataSource = null)
                .Verifiable();

            // Act
            await _presenter.LoadData();

            // Assert
            _apiClientMock.VerifyAll();
            _mainViewMock.VerifyAll();
        }

        [Fact]
        public async Task LoadData_should_set_DataSource_with_valid_response()
        {
            // Arrange
            var validResponse = new OperationResult<PagedResult<Project>>
            {
                Value = new PagedResult<Project>
                {
                    Results = new List<Project>
                    {
                        new Project { Id = 1, Name = "Test List 1" },
                        new Project { Id = 2, Name = "Test List 2" },

                    }
                }
            };

            _apiClientMock
                .Setup(client => client.List(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(validResponse)
                .Verifiable();
            _mainViewMock
                .SetupSet(view => view.DataSource = validResponse.Value.Results)
                .Verifiable();

            // Act
            await _presenter.LoadData();

            // Assert
            _apiClientMock.VerifyAll();
            _mainViewMock.VerifyAll();
        }

        [Fact]
        public void SetSelection_should_set_fields_with_valid_selection()
        {
            // Arrange
            var selectedList = new Project
            {
                Id = 5,
                Name = "Test Project",
                Budget = 1000,
                PricePerHour = 50,
                StartDate = new DateTime(2026, 1, 1),
                DueDate = new DateTime(2026, 12, 31)
            };

            _mainViewMock.SetupSet(view => view.CurrentId = selectedList.Id).Verifiable();
            _mainViewMock.SetupSet(view => view.CurrentName = selectedList.Name).Verifiable();
            _mainViewMock.SetupSet(view => view.CurrentBudget = selectedList.Budget).Verifiable();
            _mainViewMock.SetupSet(view => view.CurrentPricePerHour = selectedList.PricePerHour).Verifiable();
            _mainViewMock.SetupSet(view => view.CurrentStartDate = selectedList.StartDate).Verifiable();
            _mainViewMock.SetupSet(view => view.CurrentDueDate = selectedList.DueDate).Verifiable();

            // Act
            _presenter.SetSelection(selectedList);

            // Assert
            _mainViewMock.VerifyAll();
        }

        [Fact]
        public async Task Save_should_call_LoadData_with_valid_response()
        {
            // Arrange
            var validResponse = new OperationResult();

            _apiClientMock
                .Setup(client => client.Save(It.IsAny<Project>()))
                .ReturnsAsync(validResponse)
                .Verifiable();

            var expectedList = new List<Project>();

            _apiClientMock
                .Setup(client => client.List(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new OperationResult<PagedResult<Project>>
                {
                    Value = new PagedResult<Project>
                    {
                        Results = expectedList
                    }
                });

            _mainViewMock
                .SetupSet(view => view.DataSource = expectedList)
                .Verifiable();

            // Act
            await _presenter.Save();

            // Assert
            _apiClientMock.VerifyAll();
            _mainViewMock.VerifyAll();
        }

        [Fact]
        public async Task Delete_should_return_when_user_didnot_confirmed()
        {
            // Arrange
            _mainViewMock
                .Setup(view => view.ConfirmDelete())
                .Returns(false)
                .Verifiable();

            // Act
            await _presenter.Delete();

            // Assert
            _mainViewMock.VerifyAll();
            _apiClientMock.Verify(client => client.Delete(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task Delete_should_call_ShowError_with_faulty_response()
        {
            // Arrange
            var faultyResponse = new OperationResult();
            faultyResponse.Errors.Add("Delete failed");

            _mainViewMock
                .Setup(view => view.ConfirmDelete())
                .Returns(true);

            _apiClientMock
                .Setup(client => client.Delete(It.IsAny<int>()))
                .ReturnsAsync(faultyResponse)
                .Verifiable();

            _mainViewMock
                .Setup(view => view.ShowError(It.IsAny<string>(), It.IsAny<OperationResult>()))
                .Verifiable();

            // Act
            await _presenter.Delete();

            // Assert
            _apiClientMock.VerifyAll();
            _mainViewMock.VerifyAll();
        }

        [Fact]
        public async Task Delete_should_call_LoadData_with_valid_response()
        {
            // Arrange
            var validResponse = new OperationResult();

            _mainViewMock
                .Setup(view => view.ConfirmDelete())
                .Returns(true);

            _apiClientMock
                .Setup(client => client.Delete(It.IsAny<int>()))
                .ReturnsAsync(validResponse)
                .Verifiable();

            var expectedList = new List<Project>();

            _apiClientMock
                .Setup(client => client.List(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new OperationResult<PagedResult<Project>>
                {
                    Value = new PagedResult<Project>
                    {
                        Results = expectedList
                    }
                });

            _mainViewMock
                .SetupSet(view => view.DataSource = expectedList)
                .Verifiable();

            // Act
            await _presenter.Delete();

            // Assert
            _apiClientMock.VerifyAll();
            _mainViewMock.VerifyAll();
        }
    }
}
