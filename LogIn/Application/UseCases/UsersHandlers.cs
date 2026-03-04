using LogIn.Application.Dtos;
using LogIn.Domain.Hashing;
using LogInDll;
using LogInLibrary;
using MediatR;
using Microsoft.AspNetCore.Identity;
using RabbitMQAndGenericRepository.RabbitMq;
using RabbitMQAndGenericRepository.Repositorio.DbEntities;
using SellStocks.Application.Dtos;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LogIn.Application.UseCases
{
    public record AddUserQuery(string name, string password) : IRequest;
    public class AddUserHandler : IRequestHandler<AddUserQuery>
    {
        private readonly RabbitMessageService _rabbitMessageService; 
        public AddUserHandler(RabbitMessageService rabbitMessageService)
        {
            _rabbitMessageService = rabbitMessageService;
        }
        public async Task Handle(AddUserQuery request, CancellationToken cancellationToken)
        {
            var hashedPassword = PasswordHasher.Hasher(request.password);
            UsersDbAddDto userToAdd = new UsersDbAddDto(request.name, 0, hashedPassword);
            IdentityUser user = new IdentityUser
            {
                UserName = request.name,
                Email = string.Empty,
                PasswordHash = hashedPassword,
                SecurityStamp = string.Empty,
                ConcurrencyStamp = string.Empty,
                PhoneNumber = string.Empty,
                TwoFactorEnabled = false,
                LockoutEnabled = false,
                AccessFailedCount = 0

            };

            await _rabbitMessageService.SendMessage<IdentityUser>(user, "add");
            await _rabbitMessageService.SendMessage<UsersDbAddDto>(userToAdd, "add");
        }
    }
    public record AddFundsQuery(double amount, string currency) : IRequest;
    public class AddFundsHandler : IRequestHandler<AddFundsQuery>
    { 
        private readonly RabbitMessageService _rabbitMessageService;
        private readonly UserFundsRepository _userFundsRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ICurrentUserService _currentUser;
        public AddFundsHandler(RabbitMessageService rabbitMessageService,
            UserManager<IdentityUser> userManager, 
            UserFundsRepository userFundsRepository,
            ICurrentUserService currentUser) 
        {
            _rabbitMessageService = rabbitMessageService;
            _userFundsRepository = userFundsRepository;
            _userManager = userManager; _currentUser = currentUser;
        } 
        public async Task Handle(AddFundsQuery request, CancellationToken cancellationToken) 
        { 
            var userId = _currentUser.UserId;
            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedAccessException("UserId not found in token");
            var userFundsDb = await _userFundsRepository.GetByIdAsync(userId, request.currency);
            UserFundsDb userFunds = new UserFundsDb 
            { 
                user_id = userId,
                funds = userFundsDb.funds + request.amount,
                currency = request.currency 
            }; 
            await _rabbitMessageService.SendMessage(userFunds, "add"); 
        } 
    }
    public record SellFundsQuery(double amount, string currency) : IRequest;
    public class SellFundsHandler : IRequestHandler<SellFundsQuery> 
    { 
        private readonly RabbitMessageService _rabbitMessageService;
        private readonly UserFundsRepository _userFundsRepository; 
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ICurrentUserService _currentUser; 
        public SellFundsHandler(
            RabbitMessageService rabbitMessageService,
            UserManager<IdentityUser> userManager,
            UserFundsRepository userFundsRepository,
            ICurrentUserService currentUser)
        {
            _rabbitMessageService = rabbitMessageService;
            _userFundsRepository = userFundsRepository;
            _userManager = userManager;
            _currentUser = currentUser;
        } 
        public async Task Handle(SellFundsQuery request, CancellationToken cancellationToken)
        { 
            var userId = _currentUser.UserId;
            if (userId == null) 
               throw new UnauthorizedAccessException(); 
            UserFundsDb? userFundsDb = await _userFundsRepository.GetByIdAsync(userId, request.currency);
            if (userFundsDb.funds < request.amount) 
            throw new Exception("Insufficient funds"); 
            UserFundsDb usersDbDto = new UserFundsDb 
            { 
                user_id = userId, 
                funds = userFundsDb.funds - request.amount,
                currency = request.currency 
            }; 
            await _rabbitMessageService.SendMessage<UserFundsDb>(usersDbDto, "sell"); 
        } 
    }
}
public interface ICurrentUserService 
{ 
    string? UserId { get; } 
    string? UserName { get; }
}
public class CurrentUserService : ICurrentUserService 
{ 
    private readonly IHttpContextAccessor _httpContextAccessor; 
    public CurrentUserService(IHttpContextAccessor httpContextAccessor) 
    { 
        _httpContextAccessor = httpContextAccessor;
    } 
    public string? UserId => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
    public string? UserName => _httpContextAccessor.HttpContext?.User?.Identity?.Name; 
} 
