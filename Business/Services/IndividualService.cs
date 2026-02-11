using Business.Services;
using DAL.Infrastructure;
using Project.Business.Exceptions;
using Project.DAL.Repositories;
using Project.Shared.Command;
using Project.Shared.Model;
using Shared.Query;
using System.Linq.Expressions;

namespace Project.Business.Services
{
    public class IndividualService : IIndividualService
    {
        private readonly IIndividualRepository _repository;
        private readonly IPhoneNumberService _phoneService;
        private readonly IRelationService _relationService;
        private readonly IFileService _fileService;
        private readonly IUnitOfWork _unitOfWork;

        public IndividualService(
            IUnitOfWork unitOfWork,
            IIndividualRepository repository,
            IPhoneNumberService phoneService,
            IRelationService relationService,
            IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
            _phoneService = phoneService;
            _relationService = relationService;
            _fileService = fileService;
        }

        public async Task<int> AddIndividualAsync(CreateIndividualCommandDto dto)
        {
            if (!await _repository.CityExistsAsync((int)dto.City))
                throw new NotFoundException("City does not exist");

            if (await _repository.ExistsByPersonalNumberAsync(dto.PersonalNumber))
                throw new ValidationException("Personal number must be unique");

            foreach (var phone in dto.PhoneNumbers)
            {
                if (await _phoneService.ExistsAsync(phone.Number))
                    throw new ConflictException($"Phone number {phone.Number} already exists");
            }

            var individual = new Individual
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Gender = dto.Gender,
                PersonalNumber = dto.PersonalNumber,
                DateOfBirth = dto.DateOfBirth,
                CityId = (int)dto.City,
            };

            await _repository.AddAsync(individual);

            foreach (var phone in dto.PhoneNumbers)
            {
                await _phoneService.AddPhoneNumberAsync(new PhoneNumber
                {
                    Number = phone.Number,
                    Type = phone.Type,
                    IndividualId = individual.Id
                });
            }

            return individual.Id;
        }

        public async Task UpdateIndividualAsync(int id, UpdateIndividualCommandDto dto)
        {
            var individual = await _repository.GetByIdAsync(id);
            var existingPhones = await _phoneService.GetByIndividualIdAsync(individual.Id);
            var existingNumbers = existingPhones.Select(p => p.Number).ToHashSet();
            var newNumbers = dto.PhoneNumbers.Select(p => p.Number).ToHashSet();

            if (individual == null)
                throw new NotFoundException("Individual not found");

            if (await _repository.ExistsByPersonalNumberAsync(dto.PersonalNumber, id))
                throw new ValidationException("Personal number must be unique");

            foreach (var phone in dto.PhoneNumbers)
            {
                if (!existingNumbers.Contains(phone.Number) && await _phoneService.ExistsAsync(phone.Number))
                    throw new ConflictException($"Phone number {phone.Number} already exists");
            }

            individual.FirstName = dto.FirstName;
            individual.LastName = dto.LastName;
            individual.Gender = dto.Gender;
            individual.PersonalNumber = dto.PersonalNumber;
            individual.DateOfBirth = dto.DateOfBirth;
            individual.CityId = (int)dto.City;

            await _repository.UpdateAsync(individual);


            foreach (var phone in existingPhones.Where(p => !newNumbers.Contains(p.Number)))
                await _phoneService.DeletePhoneNumberAsync(phone);

            foreach (var phone in dto.PhoneNumbers.Where(p => !existingNumbers.Contains(p.Number)))
                await _phoneService.AddPhoneNumberAsync(new PhoneNumber
                {
                    Number = phone.Number,
                    Type = phone.Type,
                    IndividualId = individual.Id
                });

            foreach (var phone in existingPhones)
            {
                var updatedPhone = dto.PhoneNumbers.FirstOrDefault(p => p.Number == phone.Number);
                if (updatedPhone != null)
                {
                    phone.Type = updatedPhone.Type;
                    await _phoneService.UpdatePhoneNumberAsync(phone);
                }
            }
        }

        public async Task DeleteIndividualAsync(int id)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var individual = await _repository.GetByIdAsync(id);

                if (individual == null)
                    throw new NotFoundException("Individual not found");

                await _relationService.MarkDeletionByIndividualIdAsync(individual.Id);

                var phones = await _phoneService.GetByIndividualIdAsync(individual.Id);
                foreach (var phone in phones)
                    await _phoneService.MarkDeletionAsync(phone);

                if (!string.IsNullOrEmpty(individual.ImagePath))
                    await _fileService.DeleteIndividualImageAsync(individual.ImagePath);

                await _repository.MarkIndividualDeletionAsync(individual);

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<IndividualGetResponseDto?> GetByIdAsync(int id)
        {
            var individual = await _repository.GetByIdAsync(id);
            if (individual == null) return null;

            return MapToDto(individual);
        }

        public async Task<PagedResult<IndividualGetResponseDto>> GetAllAsync(IndividualQueryDto query)
        {
            if (query.PageNumber <= 0) query.PageNumber = 1; // Negatives are handledd. todo: absurdly high value validation
            if (query.PageSize <= 0) query.PageSize = 10;

            var filter = BuildFilter(query);

            var totalCount = await _repository.CountAsync(filter);

            var items = await _repository.GetAllAsync(filter,
                skip: (query.PageNumber - 1) * query.PageSize,
                take: query.PageSize);

            return new PagedResult<IndividualGetResponseDto>
            {
                TotalCount = totalCount,
                Items = items.Select(MapToDto).ToList()
            };
        }

        private static Expression<Func<Individual, bool>> BuildFilter(IndividualQueryDto query)
        {
            Expression<Func<Individual, bool>> filter = i => true;

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                var search = query.Search.ToLower();
                filter = filter.AndAlso(i =>
                    i.FirstName.ToLower().Contains(search) ||
                    i.LastName.ToLower().Contains(search) ||
                    i.PersonalNumber.Contains(search));
            }

            if (!string.IsNullOrWhiteSpace(query.FirstName))
                filter = filter.AndAlso(i => i.FirstName.Contains(query.FirstName));
            if (!string.IsNullOrWhiteSpace(query.LastName))
                filter = filter.AndAlso(i => i.LastName.Contains(query.LastName));
            if (!string.IsNullOrWhiteSpace(query.PersonalNumber))
                filter = filter.AndAlso(i => i.PersonalNumber.Contains(query.PersonalNumber));
            if (query.Gender.HasValue)
                filter = filter.AndAlso(i => i.Gender == query.Gender.Value);
            if (query.CityId.HasValue)
                filter = filter.AndAlso(i => i.CityId == query.CityId.Value);
            if (query.DateOfBirthFrom.HasValue)
                filter = filter.AndAlso(i => i.DateOfBirth >= query.DateOfBirthFrom.Value);
            if (query.DateOfBirthTo.HasValue)
                filter = filter.AndAlso(i => i.DateOfBirth <= query.DateOfBirthTo.Value);

            return filter;
        }

        private static IndividualCityDto MapCity(City city) =>
            new IndividualCityDto
            {
                Id = city.Id,
                Name = city.Name
            };

        private static IndividualGetResponseDto MapToDto(Individual individual)
        {
            return new IndividualGetResponseDto
            {
                Id = individual.Id,
                FirstName = individual.FirstName,
                LastName = individual.LastName,
                Gender = individual.Gender,
                PersonalNumber = individual.PersonalNumber,
                DateOfBirth = individual.DateOfBirth,
                City = MapCity(individual.City),
                ImagePath = individual.ImagePath,
                PhoneNumbers = individual.PhoneNumbers.Select(p => new PhoneNumberDto
                {
                    Number = p.Number,
                    Type = p.Type
                }).ToList(),
                Relations = individual.Relations.Select(r => new RelationDto
                {
                    RelationId = r.Id,
                    RelatedIndividualId = r.RelatedIndividualId,
                    RelationType = r.RelationType
                }).ToList()
            };
        }
    }
}
