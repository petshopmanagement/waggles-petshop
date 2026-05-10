using AutoMapper;
using FluentAssertions;
using Moq;
using PetManagementSystem.Api.DTOs;
using PetManagementSystem.Api.Exceptions;
using PetManagementSystem.Api.Models;
using PetManagementSystem.Api.Repositories;
using PetManagementSystem.Api.Services;

namespace PetControllerTest
{
    public class UnitTest1
    {
        private readonly Mock<IPetRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly PetService _service;

        public UnitTest1()
        {
            _repositoryMock = new Mock<IPetRepository>();
            _mapperMock = new Mock<IMapper>();

            _service = new PetService(
                _repositoryMock.Object,
                _mapperMock.Object
            );
        }

        // =====================================================
        // POSITIVE TEST CASES
        // =====================================================

        [Fact]
        public async Task GetAllPets_ShouldReturnAllPets_WhenPetsExist()
        {
            // Arrange
            var pets = new List<Pet>
            {
                new Pet { PetId = 1, Name = "Dog" }
            };

            var petDtos = new List<PetDTO>
            {
                new PetDTO { PetId = 1, Name = "Dog" }
            };

            _repositoryMock.Setup(r => r.GetAllPets())
                .ReturnsAsync(pets);

            _mapperMock.Setup(m => m.Map<IEnumerable<PetDTO>>(pets))
                .Returns(petDtos);

            // Act
            var result = await _service.GetAllPets();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
        }

        [Fact]
        public async Task GetPetById_ShouldReturnPet_WhenPetExists()
        {
            // Arrange
            var pet = new Pet
            {
                PetId = 1,
                Name = "Cat"
            };

            var petDto = new PetDTO
            {
                PetId = 1,
                Name = "Cat"
            };

            _repositoryMock.Setup(r => r.GetPetById(1))
                .ReturnsAsync(pet);

            _mapperMock.Setup(m => m.Map<PetDTO>(pet))
                .Returns(petDto);

            // Act
            var result = await _service.GetPetById(1);

            // Assert
            result.Should().NotBeNull();
            result.PetId.Should().Be(1);
        }

        [Fact]
        public async Task AddPet_ShouldAddPetSuccessfully()
        {
            // Arrange
            var createDto = new PetCreate
            {
                Name = "Rabbit"
            };

            var pet = new Pet
            {
                PetId = 1,
                Name = "Rabbit"
            };

            var petDto = new PetDTO
            {
                PetId = 1,
                Name = "Rabbit"
            };

            _mapperMock.Setup(m => m.Map<Pet>(createDto))
                .Returns(pet);

            _mapperMock.Setup(m => m.Map<PetDTO>(pet))
                .Returns(petDto);

            // Act
            var result = await _service.AddPet(createDto);

            // Assert
            result.Should().NotBeNull();

            _repositoryMock.Verify(r => r.AddPet(pet), Times.Once);
        }

        [Fact]
        public async Task UpdatePet_ShouldUpdateSuccessfully_WhenPetExists()
        {
            // Arrange
            var dto = new PetUpdate
            {
                PetId = 1,
                Name = "Updated Dog"
            };

            var existingPet = new Pet
            {
                PetId = 1,
                Name = "Old Dog"
            };

            _repositoryMock.Setup(r => r.GetPetById(1))
                .ReturnsAsync(existingPet);

            // Act
            await _service.UpdatePet(1, dto);

            // Assert
            _repositoryMock.Verify(r => r.UpdatePet(existingPet), Times.Once);
        }

        [Fact]
        public async Task GetPetByName_ShouldReturnPets_WhenNameExists()
        {
            // Arrange
            var pets = new List<Pet>
            {
                new Pet { PetId = 1, Name = "Tommy" }
            };

            var petDtos = new List<PetDTO>
            {
                new PetDTO { PetId = 1, Name = "Tommy" }
            };

            _repositoryMock.Setup(r => r.GetPetByName("Tommy"))
                .ReturnsAsync(pets);

            _mapperMock.Setup(m => m.Map<IEnumerable<PetDTO>>(pets))
                .Returns(petDtos);

            // Act
            var result = await _service.GetPetByName("Tommy");

            // Assert
            result.Should().HaveCount(1);
        }

        // =====================================================
        // NEGATIVE TEST CASES
        // =====================================================

        [Fact]
        public async Task GetAllPets_ShouldThrowNotFoundException_WhenNoPetsExist()
        {
            // Arrange
            _repositoryMock.Setup(r => r.GetAllPets())
                .ReturnsAsync(new List<Pet>());

            // Act
            Func<Task> act = async () => await _service.GetAllPets();

            // Assert
            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task GetPetById_ShouldThrowBadRequestException_WhenIdIsZero()
        {
            // Act
            Func<Task> act = async () => await _service.GetPetById(0);

            // Assert
            await act.Should().ThrowAsync<BadRequestException>();
        }

        [Fact]
        public async Task GetPetById_ShouldThrowNotFoundException_WhenPetDoesNotExist()
        {
            // Arrange
            _repositoryMock.Setup(r => r.GetPetById(1))
                .ReturnsAsync((Pet)null);

            // Act
            Func<Task> act = async () => await _service.GetPetById(1);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task AddPet_ShouldThrowBadRequestException_WhenDtoIsNull()
        {
            // Act
            Func<Task> act = async () => await _service.AddPet(null);

            // Assert
            await act.Should().ThrowAsync<BadRequestException>();
        }

        [Fact]
        public async Task UpdatePet_ShouldThrowBadRequestException_WhenIdsDoNotMatch()
        {
            // Arrange
            var dto = new PetUpdate
            {
                PetId = 2
            };

            // Act
            Func<Task> act = async () => await _service.UpdatePet(1, dto);

            // Assert
            await act.Should().ThrowAsync<BadRequestException>();
        }
    }
}
