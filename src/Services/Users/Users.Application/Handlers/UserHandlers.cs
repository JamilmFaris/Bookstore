using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Users.Application.Commands;
using Users.Application.DTOs;
using Users.Application.Queries;
using Users.Domain.Entities;
using Users.Domain.Interfaces;
using Users.Application.Common;
public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, AuthResponse>
{
    private readonly IUserRepository _repository;
    private readonly JwtSettings _jwtSettings;
    private readonly IMapper _mapper;

    public RegisterUserCommandHandler(IUserRepository repository, IOptions<JwtSettings> jwtSettings, IMapper mapper)
    {
        _repository = repository;
        _jwtSettings = jwtSettings.Value;
        _mapper = mapper;
    }

    public async Task<AuthResponse> Handle(RegisterUserCommand request, CancellationToken ct)
    {
        if (await _repository.GetByUsernameAsync(request.Dto.Username) != null)
            throw new Exception("Username already exists");
            
        if (await _repository.GetByEmailAsync(request.Dto.Email) != null)
            throw new Exception("Email already exists");
        
        var user = new User
        {
            Username = request.Dto.Username,
            Email = request.Dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Dto.Password),
            FirstName = request.Dto.FirstName,
            LastName = request.Dto.LastName
        };
        
        await _repository.AddAsync(user);
        
        var token = GenerateJwtToken(user);
        
        return new AuthResponse(
            Token: token,
            User: _mapper.Map<UserDto>(user)
        );
    }
    
    private string GenerateJwtToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            }),
            Expires = DateTime.UtcNow.AddDays(_jwtSettings.ExpiryDays),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key), 
                SecurityAlgorithms.HmacSha256Signature)
        };
        
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, AuthResponse>
{
    private readonly IUserRepository _repository;
    private readonly JwtSettings _jwtSettings;
    private readonly IMapper _mapper;

    public LoginUserCommandHandler(IUserRepository repository, IOptions<JwtSettings> jwtSettings, IMapper mapper)
    {
        _repository = repository;
        _jwtSettings = jwtSettings.Value;
        _mapper = mapper;
    }

    public async Task<AuthResponse> Handle(LoginUserCommand request, CancellationToken ct)
    {
        var user = await _repository.GetByUsernameAsync(request.Dto.Username);
        
        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Dto.Password, user.PasswordHash))
            throw new Exception("Invalid username or password");
        
        var token = GenerateJwtToken(user);
        
        return new AuthResponse(
            Token: token,
            User: _mapper.Map<UserDto>(user)
        );
    }
    
    private string GenerateJwtToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            }),
            Expires = DateTime.UtcNow.AddDays(_jwtSettings.ExpiryDays),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key), 
                SecurityAlgorithms.HmacSha256Signature)
        };
        
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserDto>
{
    private readonly IUserRepository _repository;
    private readonly IMapper _mapper;

    public UpdateUserCommandHandler(IUserRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<UserDto> Handle(UpdateUserCommand request, CancellationToken ct)
    {
        var user = await _repository.GetByIdAsync(request.UserId);
        if (user == null) throw new Exception("User not found");

        user.FirstName = request.Dto.FirstName;
        user.LastName = request.Dto.LastName;
        user.Email = request.Dto.Email;
        user.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(user);
        return _mapper.Map<UserDto>(user);
    }
}

public class LogoutCommandHandler : IRequestHandler<LogoutCommand, Unit>
{
    private readonly IUserRepository _repository;

    public LogoutCommandHandler(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(LogoutCommand request, CancellationToken ct)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.ReadJwtToken(request.Token);

        await _repository.AddTokenToBlacklistAsync(new BlacklistedToken
        {
            Token = request.Token,
            Expiry = token.ValidTo
        });

        return Unit.Value;
    }
}

public class UpdateSubscriptionCommandHandler : IRequestHandler<UpdateSubscriptionCommand, UserDto>
{
    private readonly IUserRepository _repository;
    private readonly IMapper _mapper;

    public UpdateSubscriptionCommandHandler(IUserRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<UserDto> Handle(UpdateSubscriptionCommand request, CancellationToken ct)
    {
        var user = await _repository.GetByIdAsync(request.UserId);
        if (user == null) throw new Exception("User not found");

        if (request.Dto.Subscribe)
        {
            user.IsSubscribed = true;
            user.SubscriptionExpiry = DateTime.UtcNow.AddMonths(request.Dto.Months);
        }
        else
        {
            user.IsSubscribed = false;
            user.SubscriptionExpiry = null;
        }

        await _repository.UpdateAsync(user);
        return _mapper.Map<UserDto>(user);
    }
}

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto>
{
    private readonly IUserRepository _repository;
    private readonly IMapper _mapper;

    public GetUserByIdQueryHandler(IUserRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken ct)
    {
        var user = await _repository.GetByIdAsync(request.UserId);
        if (user == null) throw new Exception("User not found");
        return _mapper.Map<UserDto>(user);
    }
}