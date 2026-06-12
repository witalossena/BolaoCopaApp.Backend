using BolaoCopaApp.Application.Commands;
using BolaoCopaApp.Application.DTOs;
using BolaoCopaApp.Domain.Entities;
using BolaoCopaApp.Domain.Interfaces;
using BolaoCopaApp.Domain.Interfaces.Repositories;
using BolaoCopaApp.Application.Interfaces;
using MediatR;
using BCrypt.Net;

namespace BolaoCopaApp.Application.Handlers;

public class AuthHandlers : 
    IRequestHandler<RegisterUserCommand, AuthResponse>,
    IRequestHandler<LoginCommand, AuthResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _uow;
    private readonly IJwtService _jwtService;

    public AuthHandlers(IUserRepository userRepository, IUnitOfWork uow, IJwtService jwtService)
    {
        _userRepository = userRepository;
        _uow = uow;
        _jwtService = jwtService;
    }

    public async Task<AuthResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var existingEmail = await _userRepository.GetByEmailAsync(request.Request.Email, cancellationToken);
        if (existingEmail != null) throw new Exception("Email em uso");

        var existingHandle = await _userRepository.GetByHandleAsync(request.Request.Handle, cancellationToken);
        if (existingHandle != null) throw new Exception("Username em uso");

        var user = new User
        {
            Name = request.Request.Name,
            Email = request.Request.Email,
            Handle = request.Request.Handle,
            PasswordHash = global::BCrypt.Net.BCrypt.HashPassword(request.Request.Password)
        };

        await _userRepository.AddAsync(user, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);

        var token = _jwtService.GenerateToken(user.Id, user.Email, user.Role.ToString());
        return new AuthResponse(token, new UserDto(user.Id, user.Name, user.Handle, user.IsPaid, user.Role.ToString(), new PointsDto(0,0,0,0,0,0), user.IsPredictionUnlocked));
    }

    public async Task<AuthResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Request.Email, cancellationToken);
        if (user == null || !global::BCrypt.Net.BCrypt.Verify(request.Request.Password, user.PasswordHash))
        {
            throw new Exception("Credenciais inválidas");
        }

        var token = _jwtService.GenerateToken(user.Id, user.Email, user.Role.ToString());
        return new AuthResponse(token, new UserDto(user.Id, user.Name, user.Handle, user.IsPaid, user.Role.ToString(), new PointsDto(0,0,0,0,0,0), user.IsPredictionUnlocked));
    }
}
