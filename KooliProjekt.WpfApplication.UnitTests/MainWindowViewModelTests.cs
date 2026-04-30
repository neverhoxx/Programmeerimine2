using Moq;

namespace KooliProjekt.WpfApplication.UnitTests
{
    public class MainWindowViewModelTests
    {
        private readonly Mock<IApiClient> _apiClientMock;
        private readonly Mock<IDialogProvider> _dialogProviderMock;
        private readonly MainWindowViewModel _viewModel;

        public MainWindowViewModelTests()
        {
            _apiClientMock = new Mock<IApiClient>();
            _dialogProviderMock = new Mock<IDialogProvider>();
            _viewModel = new MainWindowViewModel(_apiClientMock.Object, _dialogProviderMock.Object);
        }

        [Fact]
        public void SelectedItem_should_return_correct_item()
        {
            // Arrange
            var item = new Project { Id = 1, Name = "Test" };

            // Act
            _viewModel.SelectedItem = item;

            // Assert
            Assert.Equal(item, _viewModel.SelectedItem);
        }

        [Fact]
        public void SelectedItem_should_call_notify_property_changed()
        {
            // Arrange
            var item = new Project { Id = 1, Name = "Test" };
            var propertyChangedRaised = false;
            _viewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(MainWindowViewModel.SelectedItem))
                {
                    propertyChangedRaised = true;
                }
            };

            // Act
            _viewModel.SelectedItem = item;

            // Assert
            Assert.True(propertyChangedRaised);
        }

        [Fact]
        public async Task LoadData_should_load_data_from_api_client()
        {
            // Arrange
            var apiResult = new OperationResult<PagedResult<Project>>
            {
                Value = new PagedResult<Project>
                {
                    Results = new List<Project>
                    {
                        new Project { Id = 1, Name = "Test 1" },
                        new Project { Id = 2, Name = "Test 2" }
                    }
                }
            };

            _apiClientMock.Setup(client => client.List(1, 100))
                .ReturnsAsync(apiResult)
                .Verifiable();

            // Act            
            await _viewModel.LoadData();

            // Assert
            _apiClientMock.VerifyAll();
            Assert.Equal(2, _viewModel.Data.Count);
            Assert.Equal(1, _viewModel.Data[0].Id);
            Assert.Equal(2, _viewModel.Data[1].Id);
        }

        [Fact]
        public async Task LoadData_should_show_error_when_api_client_fails()
        {
            // Arrange
            var apiResult = new OperationResult<PagedResult<Project>>
            {
                Errors = new List<string> { "Error" }
            };

            _apiClientMock.Setup(client => client.List(1, 100))
                .ReturnsAsync(apiResult)
                .Verifiable();

            // Act            
            await _viewModel.LoadData();

            // Assert
            _apiClientMock.VerifyAll();
            Assert.Empty(_viewModel.Data);
        }

        [Fact]
        public void AddNew_Command_Should_Set_Empty_SelectedItem()
        {
            // Arrange
            _viewModel.SelectedItem = new Project { Id = 10, Name = "Old Project" };

            // Act
            _viewModel.AddNewCommand.Execute(null);

            // Assert
            Assert.NotNull(_viewModel.SelectedItem);
            Assert.Equal(0, _viewModel.SelectedItem.Id);
            Assert.Null(_viewModel.SelectedItem.Name);
        }


        [Fact]
        public void SaveCommand_should_load_data_if_no_errors()
        {
            // Arrange
            var loadDataApiResult = new OperationResult<PagedResult<Project>>
            {
                Value = new PagedResult<Project>
                {
                    Results = new List<Project>
                    {
                        new Project { Id = 1, Name = "Test 1" },
                        new Project { Id = 2, Name = "Test 2" }
                    }
                }
            };
            var saveDataApiResult = new OperationResult();
            var projectToSave = new Project { Id = 1, Name = "Test" };

            _apiClientMock.Setup(client => client.Save(It.IsAny<Project>()))
                .ReturnsAsync(saveDataApiResult)
                .Verifiable();
            _apiClientMock.Setup(client => client.List(1, 100))
                .ReturnsAsync(loadDataApiResult)
                .Verifiable();

            // Act
            _viewModel.SaveCommand.Execute(projectToSave);

            // Arrange
            _apiClientMock.VerifyAll();
        }

        [Fact]
        public async Task SaveCommand_should_return_when_api_gave_error()
        {
            // Arrange
            var projectToSave = new Project { Id = 1, Name = "Test" };
            var saveDataApiResult = new OperationResult { Errors = new List<string> { "API Error" } };

            _apiClientMock.Setup(client => client.Save(It.IsAny<Project>()))
                .ReturnsAsync(saveDataApiResult)
                .Verifiable();

            // Act
            _viewModel.SaveCommand.Execute(projectToSave);

            // Assert
            _apiClientMock.VerifyAll();
            // Проверяем, что List не вызывался (загрузка данных не пошла дальше ошибки)
            _apiClientMock.Verify(client => client.List(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task SaveCommand_can_execute_when_selected_item_is_not_null()
        {
            // Act & Assert
            // Case 1: SelectedItem is null
            _viewModel.SelectedItem = null;
            Assert.False(_viewModel.SaveCommand.CanExecute(null));

            // Case 2: SelectedItem is not null
            _viewModel.SelectedItem = new Project { Name = "New Project" };
            Assert.True(_viewModel.SaveCommand.CanExecute(null));
        }

        [Fact]
        public async Task DeleteCommand_should_return_when_no_confirmation()
        {
            // Arrange
            var projectToDelete = new Project { Id = 1 };
            _viewModel.SelectedItem = projectToDelete;

            _dialogProviderMock.Setup(d => d.Confirm(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(false); // Пользователь нажал "Нет"

            // Act
            _viewModel.DeleteCommand.Execute(projectToDelete);

            // Assert
            // Проверяем, что вызов API Delete не произошел
            _apiClientMock.Verify(client => client.Delete(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task DeleteCommand_should_load_data_if_no_errors()
        {
            // Arrange
            var projectToDelete = new Project { Id = 1 };
            _viewModel.SelectedItem = projectToDelete;

            _dialogProviderMock.Setup(d => d.Confirm(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(true);
            // Also setup the single-argument overload to be safe (some code paths or overload resolution may call this one)
            _dialogProviderMock.Setup(d => d.Confirm(It.IsAny<string>()))
                .Returns(true);

            _apiClientMock.Setup(client => client.Delete(projectToDelete.Id))
                .ReturnsAsync(new OperationResult())
                .Verifiable();

            _apiClientMock.Setup(client => client.List(1, 100))
                .ReturnsAsync(new OperationResult<PagedResult<Project>> { Value = new PagedResult<Project>() })
                .Verifiable();

            // Act
            _viewModel.DeleteCommand.Execute(projectToDelete);

            // Assert
            _apiClientMock.VerifyAll();
        }

        [Fact]
        public async Task DeleteCommand_should_return_when_api_gave_error()
        {
            // Arrange
            var projectToDelete = new Project { Id = 1 };
            _viewModel.SelectedItem = projectToDelete;

            _dialogProviderMock.Setup(d => d.Confirm(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(true);

            _apiClientMock.Setup(client => client.Delete(projectToDelete.Id))
                .ReturnsAsync(new OperationResult { Errors = new List<string> { "Delete failed" } })
                .Verifiable();

            // Act
            _viewModel.DeleteCommand.Execute(projectToDelete);

            // Assert
            _apiClientMock.VerifyAll();
            // List не должен вызываться после ошибки удаления
            _apiClientMock.Verify(client => client.List(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task DeleteCommand_can_execute_when_selected_item_is_not_null_and_id_is_not_zero()
        {
            // Case 1: SelectedItem is null
            _viewModel.SelectedItem = null;
            Assert.False(_viewModel.DeleteCommand.CanExecute(null));

            // Case 2: SelectedItem is new (Id = 0)
            _viewModel.SelectedItem = new Project { Id = 0 };
            Assert.False(_viewModel.DeleteCommand.CanExecute(null));

            // Case 3: SelectedItem exists (Id > 0)
            _viewModel.SelectedItem = new Project { Id = 5 };
            Assert.True(_viewModel.DeleteCommand.CanExecute(null));
        }
    }
}
