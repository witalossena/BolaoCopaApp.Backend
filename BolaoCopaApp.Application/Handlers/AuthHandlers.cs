using BolaoCopaApp.Application.Commands;
using BolaoCopaApp.Application.DTOs;
using BolaoCopaApp.Domain.Entities;
using BolaoCopaApp.Domain.Interfaces;
using BolaoCopaApp.Domain.Interfaces.Repositories;
using MediatR;
using BCrypt.Net;
using System.Security.Cryptography;
using System.Text;

namespace BolaoCopaApp.Application.Handlers;

public class AuthHandlers : 
    IRequestHandler<RegisterUserCommand, AuthResponse>,
    IRequestHandler<LoginCommand, AuthResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _uow;

    public AuthHandlers(IUserRepository userRepository, IUnitOfWork uow)
    {
        _userRepository = userRepository;
        _uow = uow;
    }

    public async Task<AuthResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var existingEmail = await _userRepository.GetByEmailAsync(request.Request.Email, cancellationToken);
        if (existingEmail != null) throw new Exception("Email already in use");

        var existingHandle = await _userRepository.GetByHandleAsync(request.Request.Handle, cancellationToken);
        if (existingHandle != null) throw new Exception("Handle already in use");

        var user = new User
        {
            Name = request.Request.Name,
            Email = request.Request.Email,
            Handle = request.Request.Handle,
            PasswordHash = global::BCrypt.Net.BCrypt.HashPassword(request.Request.Password)
        };

        await _userRepository.AddAsync(user, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);

        // Dummy token for now, real token generated in API or a separate IJwtProvider service
        return new AuthResponse("dummy-token", new UserDto(user.Id, user.Name, user.Handle, user.IsPaid, new PointsDto(0,0,0,0,0,0)));
    }

    public async Task<AuthResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Request.Email, cancellationToken);
        if (user == null || !global::BCrypt.Net.BCrypt.Verify(request.Request.Password, user.PasswordHash))
        {
            throw new Exception("Invalid credentials");
        }

        return new AuthResponse("dummy-token", new UserDto(user.Id, user.Name, user.Handle, user.IsPaid, new PointsDto(0,0,0,0,0,0)));
    }
}
