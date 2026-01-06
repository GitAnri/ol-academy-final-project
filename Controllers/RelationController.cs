using Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.Shared.Command;
using Project.Shared.Model;


[ApiController]
[Route("api/relations")]
[Authorize]
public class RelationController : ControllerBase
{
    private readonly IRelationService _relationService;

    public RelationController(IRelationService relationService)
    {
        _relationService = relationService;
    }

    [HttpPost("{individualId}/add")]
    public async Task<IActionResult> AddRelation(int individualId, [FromBody] AddRelationCommandDto dto)
    {
            var relation = new Relation
            {
                IndividualId = individualId,
                RelatedIndividualId = dto.RelatedIndividualId,
                RelationType = dto.RelationType
            };
            
            await _relationService.AddRelationAsync(relation);
            return Ok();
    }

    [HttpDelete("{individualId}/{relationId}")]
    public async Task<IActionResult> DeleteRelation(int individualId, int relationId)
    {
        var relation = await _relationService.GetRelationAsync(individualId, relationId);
        if (relation == null) return NotFound();

        await _relationService.DeleteRelationAsync(relation);
        return NoContent();
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpGet("GetRelationsReport")]
    public async Task<IActionResult> GetRelationsReport()
    {
        var report = await _relationService.GetRelationsReportAsync();
        return Ok(report);
    }
}
