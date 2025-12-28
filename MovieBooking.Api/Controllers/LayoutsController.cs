using Microsoft.AspNetCore.Mvc;
using MovieBooking.Api.Contracts.Requests;
using MovieBooking.Api.Contracts.Responses;
using MovieBooking.Application.Services;

namespace MovieBooking.Api.Controllers;

[ApiController]
[Route("api/layouts")]
public class LayoutsController : ControllerBase
{
    private readonly LayoutService _layoutService;

    public LayoutsController(LayoutService layoutService)
    {
        _layoutService = layoutService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateLayoutRequest request, CancellationToken ct)
    {
        var id = await _layoutService.CreateAsync(
            request.Name,
            request.RowCount,
            request.SeatsPerRow,
            request.VipRows,
            request.PremiumRows,
            ct);

        return CreatedAtAction(nameof(GetById), new { layoutId = id }, null);
    }

    [HttpGet("{layoutId:guid}")]
    public async Task<ActionResult<LayoutResponse>> GetById(Guid layoutId, CancellationToken ct)
    {
        var layout = await _layoutService.GetByIdAsync(layoutId, ct);
        if (layout is null) return NotFound();

        var seatTypes = layout.SeatTypes
            .OrderBy(s => s.RowNumber)
            .ThenBy(s => s.SeatNumber)
            .Select(s => new LayoutSeatTypeResponse(s.RowNumber, s.SeatNumber, s.SeatType.ToString()))
            .ToList();

        return Ok(new LayoutResponse(
            layout.LayoutId,
            layout.Name,
            layout.RowCount,
            layout.SeatsPerRow,
            seatTypes));
    }
}
