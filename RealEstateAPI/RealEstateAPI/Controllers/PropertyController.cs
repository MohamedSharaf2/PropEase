﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateAPI.DTOs;
using RealEstateAPI.Services;
using System.Security.Claims;

[Route("api/properties")]
[ApiController]
public class PropertyController : ControllerBase
{
    private readonly IPropertyService _propertyService;

    public PropertyController(IPropertyService propertyService)
    {
        _propertyService = propertyService;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromForm] CreatePropertyDTO dto)
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized();

        var property = await _propertyService.CreateProperty(userId.Value, dto);
        return Ok(property);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var properties = await _propertyService.GetAllProperties();
        return Ok(properties);
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> Update(int id, [FromForm] UpdatePropertyDTO dto)
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized();

        var updated = await _propertyService.UpdateProperty(id, userId.Value, dto);
        if (updated == null) return NotFound();

        return Ok(updated);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized();

        var deleted = await _propertyService.DeleteProperty(id, userId.Value);
        if (!deleted) return NotFound();

        return NoContent();
    }


    private int? GetUserId()
    {
        var userIdClaim = User.FindFirst("UserId")?.Value
                       ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.TryParse(userIdClaim, out int id) ? id : null;
    }
}
