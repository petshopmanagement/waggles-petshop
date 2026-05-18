using AutoMapper;
using PetManagementSystem.Api.DTOs;
using PetManagementSystem.Api.Exceptions;
using PetManagementSystem.Api.Models;
using PetManagementSystem.Api.Repositories;

namespace PetManagementSystem.Api.Services
{
    public class PetService : IPetService
    {
        private readonly IPetRepository _repository;
        private readonly IMapper _mapper;

        public PetService(IPetRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PetDto>> GetAllPets(int page = 1, int pageSize = 10)
        {
            var pets = await _repository.GetAllPets(page, pageSize);

            if (pets == null || !pets.Any())
            {
                // We shouldn't necessarily throw an exception for pagination if there are no more results
                return new List<PetDto>();
            }

            return _mapper.Map<IEnumerable<PetDto>>(pets);
        }

        public async Task<PetDto?> GetPetById(int petid)
        {
            if (petid <= 0)
            {
                throw new BadRequestException("Pet Id must be greater than 0.");
            }

            var pet = await _repository.GetPetById(petid);

            if (pet == null)
            {
                throw new NotFoundException("Pet not found.");
            }

            return _mapper.Map<PetDto>(pet);
        }

        public async Task<IEnumerable<PetDto>> GetPetByCategory(int categoryId, int page = 1, int pageSize = 10)
        {
            if (categoryId <= 0)
            {
                throw new BadRequestException("Category Id must be greater than 0.");
            }

            var pets = await _repository.GetPetByCategory(categoryId, page, pageSize);

            if (pets == null || !pets.Any())
            {
                return new List<PetDto>();
            }

            return _mapper.Map<IEnumerable<PetDto>>(pets);
        }

        public async Task<IEnumerable<PetDto>> GetPetByName(string name, int page = 1, int pageSize = 10)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new BadRequestException("Pet name is required.");
            }

            var pets = await _repository.GetPetByName(name, page, pageSize);

            if (pets == null || !pets.Any())
            {
                return new List<PetDto>();
            }

            return _mapper.Map<IEnumerable<PetDto>>(pets);
        }

        public async Task<PetDto> AddPet(PetCreate dto)
        {
            if (dto == null)
            {
                throw new BadRequestException("Pet data is required.");
            }

            var pet = _mapper.Map<Pet>(dto);

            await _repository.AddPet(pet);

            return _mapper.Map<PetDto>(pet);
        }

        public async Task UpdatePet(int petid, PetUpdate dto)
        {
            if (dto == null)
            {
                throw new BadRequestException("Pet update data is required.");
            }

            if (petid <= 0)
            {
                throw new BadRequestException("Pet Id must be greater than 0.");
            }

            if (petid != dto.PetId)
            {
                throw new BadRequestException("Pet ID mismatch.");
            }

            var existingPet = await _repository.GetPetById(petid);

            if (existingPet == null)
            {
                throw new NotFoundException("Pet not found.");
            }

            _mapper.Map(dto, existingPet);

            await _repository.UpdatePet(existingPet);
        }

        public async Task<IEnumerable<SupplierDTO>> GetSuppliersByPetIdService(int petId)
        {
            if (petId <= 0)
            {
                throw new BadRequestException("Pet Id must be greater than 0.");
            }

            var suppliers = await _repository.GetSuppliersByPetId(petId);

            if (suppliers == null || !suppliers.Any())
            {
                throw new NotFoundException("No suppliers found for this pet.");
            }

            return _mapper.Map<IEnumerable<SupplierDTO>>(suppliers);
        }

        public async Task<IEnumerable<EmployeeDto>> GetEmployeeByPetIdService(int petId)
        {
            if (petId <= 0)
            {
                throw new BadRequestException("Pet Id must be greater than 0.");
            }

            var employees = await _repository.GetEmployeeById(petId);

            if (employees == null || !employees.Any())
            {
                throw new NotFoundException("No employees found for this pet.");
            }

            return _mapper.Map<IEnumerable<EmployeeDto>>(employees);
        }

        public async Task<IEnumerable<TransactionDto>> GetTrasactionbypetID(int petId)
        {
            if (petId <= 0)
            {
                throw new BadRequestException("Pet Id must be greater than 0.");
            }

            var transactions = await _repository.GetTransactionByPetId(petId);

            if (transactions == null || !transactions.Any())
            {
                throw new NotFoundException("No transactions found for this pet.");
            }

            return _mapper.Map<IEnumerable<TransactionDto>>(transactions);
        }

        public async Task<IEnumerable<VaccinationDto>> GetVaccinationByPetId(int petId)
        {
            if (petId <= 0)
            {
                throw new BadRequestException("Pet Id must be greater than 0.");
            }

            var vaccinations = await _repository.GetVaccinationByPetId(petId);

            if (vaccinations == null || !vaccinations.Any())
            {
                throw new NotFoundException("No vaccinations found for this pet.");
            }

            return _mapper.Map<IEnumerable<VaccinationDto>>(vaccinations);
        }

        public async Task<IEnumerable<GroomingDTO>> GetGroomingsByPetId(int petId)
        {
            if (petId <= 0)
            {
                throw new BadRequestException("Pet Id must be greater than 0.");
            }

            var groomings = await _repository.GetGroomingsByPetId(petId);

            if (groomings == null || !groomings.Any())
            {
                throw new NotFoundException("No groomings found for this pet.");
            }

            return _mapper.Map<IEnumerable<GroomingDTO>>(groomings);
        }
    }
}
