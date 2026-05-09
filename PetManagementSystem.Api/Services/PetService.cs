using AutoMapper;
using PetManagementSystem.Api.DTOs;
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

        public async Task<IEnumerable<PetDTO>> GetAllPets()
        {
            var pets = await _repository.GetAllPets();
            //return pets.Select(p => new PetDTO
            //{
            //    Name = p.Name,
            //    Price = p.Price,
            //    Age = p.Age,
            //    Description = p.Description,
            //    Breed = p.Breed,
            //    ImageUrl = p.ImageUrl
            //});

            return _mapper.Map<IEnumerable<PetDTO>>(pets);
        }

        public async Task<PetDTO?> GetPetById(int petid)
        {
            var pet = await _repository.GetPetById(petid);

            if (pet == null)
                return null;

            //return new PetDTO
            //{
            //    Name = pet.Name,
            //    Price = pet.Price,
            //    Age = pet.Age,
            //    Description = pet.Description,
            //    Breed = pet.Breed,
            //    ImageUrl = pet.ImageUrl
            //};
            return _mapper.Map<PetDTO>(pet);
        }
        public async Task<IEnumerable<PetDTO>> GetPetByCategory(int categoryId)
        {
            var pets = await _repository.GetPetByCategory(categoryId);

            //return pets.Select(p => new PetDTO
            //{
            //    Name = p.Name,
            //    Price = p.Price,
            //    Age = p.Age,
            //    Description = p.Description,
            //    Breed = p.Breed,
            //    ImageUrl = p.ImageUrl
            //});
            return _mapper.Map<IEnumerable<PetDTO>>(pets);
        }



        public async Task<IEnumerable<PetDTO>> GetPetByName(string name)
        {
            var pets = await _repository.GetPetByName(name);

            //return pets.Select(p => new PetDTO
            //{
            //    Name = p.Name,
            //    Price = p.Price,
            //    Age = p.Age,
            //    Description = p.Description,
            //    Breed = p.Breed,
            //    ImageUrl = p.ImageUrl
            //});
            return _mapper.Map<IEnumerable<PetDTO>>(pets);
        }

        public async Task<PetDTO> AddPet(PetCreate dto)
        {
            var pet = _mapper.Map<Pet>(dto);

            await _repository.AddPet(pet);

            return _mapper.Map<PetDTO>(pet);

        }
        public async Task UpdatePet(int petid, PetUpdate dto)
        {
            if (petid != dto.PetId)
            {
                throw new ArgumentException("Pet ID mismatch");
            }

            var pet = _mapper.Map<Pet>(dto);

            await _repository.UpdatePet(pet);

        }

        public async Task<IEnumerable<SupplierDTO>> GetSuppliersByPetIdService(int petId)
        {
            var suppliers = await _repository.GetSuppliersByPetId(petId);

            if (suppliers == null)
            {
                return Enumerable.Empty<SupplierDTO>();
            }

            return _mapper.Map<IEnumerable<SupplierDTO>>(suppliers);
        }

        public async Task<IEnumerable<EmployeeDTO>> GetEmployeeByPetIdService(int petId)
        {
            var employees = await _repository.GetEmployeeById(petId);
            if (employees == null)
            {
                return Enumerable.Empty<EmployeeDTO>();
            }
            return _mapper.Map<IEnumerable<EmployeeDTO>>(employees);
        }
    }
}
