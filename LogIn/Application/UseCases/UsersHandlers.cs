using LogIn.Application.Dtos;
using LogIn.Domain.Hashing;
using LogInDll;
using LogInLibrary;
using MediatR;
using RabbitMQAndGenericRepository.RabbitMq;
using RabbitMQAndGenericRepository.Repositorio.DbEntities;
using SellStocks.Application.Dtos;

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

            await _rabbitMessageService.SendMessage<UsersDbAddDto>(userToAdd, "add");
        }
    }
    public record UserLogInQuery(string name, string password) : IRequest<string?>;

    public class UserLogInHandler : IRequestHandler<UserLogInQuery, string?>
    {
        private readonly UserRepository _userRepository;
        private readonly JwtService _tokenService;

        public UserLogInHandler(UserRepository userRepository, JwtService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        public async Task<string?> Handle(UserLogInQuery request, CancellationToken cancellationToken)
        {
            UsersDb? user = await _userRepository.GetOneByNameAsync(request.name);
            if (user == null)
                return null;

            bool isValid = PasswordHasher.VerifyPassword(request.password, user.password_hash);
            if (!isValid)
                return null;

            var token = _tokenService.GenerateToken(user.name);

            return token;
        }
    }

    public record AddFundsQuery(string name, double amount, string currency) : IRequest;
    public class AddFundsHandler : IRequestHandler<AddFundsQuery>
    {
        private readonly RabbitMessageService _rabbitMessageService;
        private readonly UserRepository _userRepository;
        public AddFundsHandler(RabbitMessageService rabbitMessageService, UserRepository userRepository)
        {
            _rabbitMessageService = rabbitMessageService;
            _userRepository = userRepository;
        }
        public async Task Handle(AddFundsQuery request, CancellationToken cancellationToken)
        {
            UsersDb? userDb = await _userRepository.GetOneByNameAsync(request.name);
            UserFundsDb usersDbDto = new UserFundsDb
            {
                user_id = userDb.id,
                funds = request.amount,
                currency = request.currency
            };
            await _rabbitMessageService.SendMessage<UserFundsDb>(usersDbDto, "add");
        }
    }
    public record SellFundsQuery(string name, double amount, string currency) : IRequest;
    public class SellFundsHandler : IRequestHandler<SellFundsQuery>
    {
        private readonly RabbitMessageService _rabbitMessageService;
        private readonly UserRepository _userRepository;
        private readonly UserFundsRepository _userFundsRepository;
        public SellFundsHandler(RabbitMessageService rabbitMessageService, UserRepository userRepository, UserFundsRepository userFundsRepository)
        {
            _rabbitMessageService = rabbitMessageService;
            _userRepository = userRepository;
            _userFundsRepository = userFundsRepository;
        }
        public async Task Handle(SellFundsQuery request, CancellationToken cancellationToken)
        {
            UsersDb? userDb = await _userRepository.GetOneByNameAsync(request.name);
            UserFundsDb userFundsDb = await _userFundsRepository.GetByIdAsync(new UserFundsStruct(userDb.id, request.currency));
            if (userFundsDb.funds < request.amount)
            {
                throw new Exception("Insufficient funds");
            }
            UserFundsDb usersDbDto = new UserFundsDb
            {
                user_id = userDb.id,
                funds = request.amount,
                currency = request.currency
            };
            await _rabbitMessageService.SendMessage<UserFundsDb>(usersDbDto, "sell");
        }
    }
}