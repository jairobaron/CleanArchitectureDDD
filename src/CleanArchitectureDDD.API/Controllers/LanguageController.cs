using CleanArchitectureDDD.Application.Common.Models;
using CleanArchitectureDDD.Application.Languages.Commands.CreateLanguage;
using CleanArchitectureDDD.Application.Languages.Commands.DeleteLanguage;
using CleanArchitectureDDD.Application.Languages.Commands.UpdateLanguage;
using CleanArchitectureDDD.Application.Languages.Queries.GetLanguages;
using CleanArchitectureDDD.Application.Languages.Queries.ExportLanguages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CleanArchitectureDDD.Application.Languages.Commands.ImportLanguages;

namespace CleanArchitectureDDD.API.Controllers;

/// <summary>
/// Language Controller
/// </summary>
[Authorize]
//[AllowAnonymous]
public class LanguageController : ApiControllerBase
{
    /// <summary>
    /// Get Languages with pagination
    /// </summary>
    /// <remarks>
    /// Sample Request:
    /// 
    /// GET
    /// {
    ///     "PageNumber" : 1,
    ///     "PageSize" : 1
    /// }
    /// </remarks>
    /// <param name="query">Page number and Page size</param>
    /// <returns>A List of Languages with pagination</returns>
    /// <response code="200">Returns A List of Languages with pagination</response>
    [HttpGet]
    public async Task<ActionResult<PaginatedList<LanguageBriefDto>>> GetLanguagesWithPagination([FromQuery] GetLanguagesWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    /// <summary>
    /// Create new Language
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    /// <response code="200">Success create a new language. Returns language id</response>
    [HttpPost]
    public async Task<ActionResult<Guid>> Create(CreateLanguageCommand command)
    {
        return await Mediator.Send(command);
    }

    /// <summary>
    /// Update Language
    /// </summary>
    /// <param name="id">Language Id</param>
    /// <param name="command"></param>
    /// <returns></returns>
    /// <response code="200">No response</response>
    /// <response code="204">Success Update a Language</response>
    /// <response code="400">Language Id not found</response>
    [HttpPut("{id}")]
    public async Task<ActionResult> Update(Guid id, UpdateLanguageCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }
    /// <summary>
    /// Delete Language
    /// </summary>
    /// <param name="id">Language Id</param>
    /// <returns></returns>
    /// <response code="200">No response</response>
    /// <response code="204">Success Delete Language</response>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        await Mediator.Send(new DeleteLanguageCommand(id));

        return NoContent();
    }

    /// <summary>
    /// Get Languages Export to CSV
    /// </summary>
    /// <returns>A List of Languages in Csv</returns>
    /// <response code="200">Returns A List of Languages in Csv</response>
    [HttpGet]
    public async Task<FileResult> ExportCsv()
    {
        var vm = await Mediator.Send(new ExportCsvLanguagesQuery());

        return File(vm.Content, vm.ContentType, vm.FileName);
    }

    /// <summary>
    /// Get Languages Export to Excel
    /// </summary>
    /// <returns>A List of Languages in Excel</returns>
    /// <response code="200">Returns A List of Languages in Excel</response>
    [HttpGet]
    public async Task<FileResult> ExportExcel()
    {
        var vm = await Mediator.Send(new ExportExcelLanguagesQuery());

        return File(vm.Content, vm.ContentType, vm.FileName);
    }

    /// <summary>
    /// Get Languages Export to Excel
    /// </summary>
    /// <returns>A List of Languages in Excel</returns>
    /// <response code="200">Returns A List of Languages in Excel</response>
    [HttpPost]
    public async Task<ActionResult<ICollection<LanguagesRecord>>> ImportExcel([FromForm] ImportLanguagesCommand excelFile)
    {
        var vm = await Mediator.Send(excelFile);
        return Ok(vm);
    }

}