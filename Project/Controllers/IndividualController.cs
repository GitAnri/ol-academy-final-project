using Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.Business.Services;
using Project.Shared.Command;
using Shared.Query;

[ApiController]
[Route("api/individuals")]
[Authorize]
public class IndividualController : ControllerBase
{
    private readonly IIndividualService _individualService;
    private readonly IFileService _fileService;

    public IndividualController(
        IIndividualService individualService,
        IFileService fileService)
    {
        _individualService = individualService;
        _fileService = fileService;
    }

    [HttpPost("CreateIndividual")]
    public async Task<IActionResult> Create([FromBody] CreateIndividualCommandDto dto)
    {
            var id = await _individualService.AddIndividualAsync(dto);
            return Ok(id);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateIndividualCommandDto dto)
    {
            await _individualService.UpdateIndividualAsync(id, dto);
            return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
            await _individualService.DeleteIndividualAsync(id);
            return NoContent();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var individual = await _individualService.GetByIdAsync(id);
        if (individual == null)
            return NotFound();

        return Ok(individual);
    }

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll([FromQuery] IndividualQueryDto query)
    {
        var result = await _individualService.GetAllAsync(query);
        return Ok(result);
    }


    [HttpPost("{id}/image")]
    public async Task<IActionResult> UploadImage(int id, IFormFile file)
    {
        var path = await _fileService.UploadIndividualImageAsync(id, file);
        return Ok(path);
    }
}
