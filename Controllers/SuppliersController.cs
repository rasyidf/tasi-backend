﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TASI.Backend.Domain.Suppliers.Dtos;
using TASI.Backend.Domain.Suppliers.Handlers;
using TASI.Backend.Infrastructure.Resources;

namespace TASI.Backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SuppliersController : ControllerBase
    {
        private readonly ILogger<SuppliersController> _logger;
        private readonly IMediator _mediator;

        public SuppliersController(IMediator mediator, ILogger<SuppliersController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("{supplierId}")]
        public async Task<IActionResult> Get([FromRoute, Required] int supplierId)
        {
            try
            {
                return await _mediator.Send(new GetSupplierCommand
                {
                    SupplierId = supplierId
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in {0}", HttpContext.Request.Path);
                return BadRequest(ErrorMessages.InternalExceptionModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetSuppliersCommand model)
        {
            try
            {
                return await _mediator.Send(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in {0}", HttpContext.Request.Path);
                return BadRequest(ErrorMessages.InternalExceptionModel);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody, Required] CreateSupplierCommand model)
        {
            try
            {
                return await _mediator.Send(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in {0}", HttpContext.Request.Path);
                return BadRequest(ErrorMessages.InternalExceptionModel);
            }
        }

        [HttpPut("{supplierId}")]
        public async Task<IActionResult> Edit([FromRoute, Required] int supplierId, [FromBody] EditSupplierDto dto)
        {
            try
            {
                return await _mediator.Send(new EditSupplierCommand
                {
                    SupplierId = supplierId,
                    Body = dto
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in {0}", HttpContext.Request.Path);
                return BadRequest(ErrorMessages.InternalExceptionModel);
            }
        }

        [HttpDelete("{supplierId}")]
        public async Task<IActionResult> Delete([FromRoute, Required] int supplierId)
        {
            try
            {
                return await _mediator.Send(new DeleteSupplierCommand
                {
                    SupplierId = supplierId
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in {0}", HttpContext.Request.Path);
                return BadRequest(ErrorMessages.InternalExceptionModel);
            }
        }
    }
}
