using Business.Services;
using Project.Business.Exceptions;
using Project.DAL.Repositories;
using Project.Shared.Model;
using Shared.Query;

namespace Project.Business.Services
{
    public class RelationService : IRelationService
    {
        private readonly IRelationRepository _repo;
        private readonly IIndividualRepository _individualRepo;

        public RelationService(
            IRelationRepository repo,
            IIndividualRepository individualRepo)
        {
            _repo = repo;
            _individualRepo = individualRepo;
        }

        public async Task AddRelationAsync(Relation relation)
        {
            if (relation.IndividualId == relation.RelatedIndividualId)
                throw new ValidationException("Cannot relate an individual to themselves");

            if (!await _individualRepo.IndividualExistsAsync(relation.IndividualId))
                throw new NotFoundException("Individual does not exist");

            if (!await _individualRepo.IndividualExistsAsync(relation.RelatedIndividualId))
                throw new NotFoundException("Related individual does not exist");

            if (await _repo.ExistsAsync(relation.IndividualId, relation.RelatedIndividualId, relation.RelationType))
                throw new ConflictException("This relation already exists.");

            await _repo.AddAsync(relation);
        }

        public async Task DeleteRelationAsync(Relation relation)
        {
            await _repo.DeleteAsync(relation);
        }

        public async Task<Relation?> GetRelationAsync(int individualId, int relationId)
        {
            return await _repo.GetAsync(individualId, relationId);
        }

        public async Task<List<Relation>> GetByIndividualIdAsync(int individualId)
        {
            return await _repo.GetByIndividualIdAsync(individualId);
        }

        public async Task DeleteByIndividualIdAsync(int individualId)
        {
            await _repo.DeleteByIndividualIdAsync(individualId);
        }

        public async Task MarkDeletionByIndividualIdAsync(int individualId)
        {
            await _repo.MarkDeletionByIndividualIdAsync(individualId);
        }

        public async Task<bool> ExistsAsync(int individualId, int relatedId, RelationType type)
        {
            return await _repo.ExistsAsync(individualId, relatedId, type);
        }
        public async Task<List<IndividualRelationsReportDto>> GetRelationsReportAsync()
        {
            var individuals = await _repo.GetAllIndividualsWithRelationsAsync();

            var report = individuals.Select(ind => new IndividualRelationsReportDto
            {
                IndividualId = ind.Id,
                FirstName = ind.FirstName,
                LastName = ind.LastName,
                RelationsCount = ind.Relations
                    .GroupBy(r => r.RelationType)
                    .ToDictionary(g => g.Key, g => g.Count())
            }).ToList();

            return report;
        }
    }
}
