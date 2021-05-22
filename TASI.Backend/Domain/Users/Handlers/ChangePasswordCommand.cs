﻿using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TASI.Backend.Infrastructure.Database;
using TASI.Backend.Infrastructure.Resources;

namespace TASI.Backend.Domain.Users.Handlers
{
    public class ChangePasswordCommand : IRequest<IActionResult>
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string NewPassword { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string RepeatPassword { get; set; }
    }

    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, IActionResult>
    {
        private readonly ILogger<ChangePasswordCommandHandler> _logger;
        private readonly TasiContext _context;

        public ChangePasswordCommandHandler(ILogger<ChangePasswordCommandHandler> logger, TasiContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Change password attempt for {0}", request.UserId);

            var user = await _context.Users.FindAsync(request.UserId);
            if (user == null)
            {
                _logger.LogDebug("Change password attempt for {0} error, user not found", request.UserId);
                return new NotFoundObjectResult(new ErrorModel(ErrorMessages.NotFound, ErrorCodes.NotFound));
            }

            if (request.NewPassword != request.RepeatPassword)
            {
                _logger.LogDebug("Change password attempt for {0} error, new and repeated password didn't match", request.UserId);
                return new BadRequestObjectResult(new ErrorModel("Password baru tidak cocok",
                    ErrorCodes.ModelValidation));
            }

            if (!BCrypt.Net.BCrypt.Verify(request.OldPassword, user.Password))
            {
                _logger.LogDebug("Change password attempt for {0} error, old password didn't match existing password", request.UserId);
                return new UnauthorizedObjectResult(new ErrorModel("Password lama tidak cocok", ErrorCodes.Forbidden));
            }

            user.Password = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            _context.Users.Update(user);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Change password attempt for {0} success", request.UserId);
            return new OkResult();
        }
    }
}
