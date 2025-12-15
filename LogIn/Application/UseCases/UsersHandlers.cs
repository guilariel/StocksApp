using MediatR;
using SellStocks.Application.Dtos;
using RabbitMQAndGenericRepository.Repositorio.DbEntities;
using RabbitMQAndGenericRepository.RabbitMq;
using LogIn.Domain.Hashing;
using LogInLibrary;

namespace LogIn.Application.UseCases
{
    // --- GET ALL ---
    public record GetAllUsersQuery() : IRequest<List<UsersDbDto>>;

    public class GetAllUsersHandler : IRequestHandler<GetAllUsersQuery, List<UsersDbDto>>
    {
        private readonly UserRepository _userRepository;

        public GetAllUsersHandler(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<UsersDbDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<UsersDb> users = await _userRepository.GetAllAsync();
            List<UsersDbDto> result = new List<UsersDbDto>();
            foreach (UsersDb us in users)
            {
                UsersDbDto userDbDto = new UsersDbDto(us.name, us.funds);
                result.Add(userDbDto);
            }
            return await Task.FromResult(result);
        }
    }

    // --- GET ONE ---
    public record GetUserByIdQuery(int Id) : IRequest<UsersDbDto?>;

    public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, UsersDbDto?>
    {
        private readonly UserRepository _userRepository;

        public GetUserByIdHandler(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UsersDbDto?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            UsersDb user = await _userRepository.GetByIdAsync(request.Id);
            UsersDbDto result = new UsersDbDto(user.name,user.funds);
            return await Task.FromResult(result);
        }
    }
    public record GetUserByNameQuery(string name) : IRequest<UsersDbDto?>;
    public class GetUserByNameHandler : IRequestHandler<GetUserByNameQuery, UsersDbDto?>
    {
        private readonly UserRepository _userRepository;

        public GetUserByNameHandler(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<UsersDbDto?> Handle(GetUserByNameQuery request, CancellationToken cancellationToken)
        {
            UsersDb user = await _userRepository.GetOneByNameAsync(request.name);
            UsersDbDto result = new UsersDbDto(user.name, user.funds);
            return await Task.FromResult(result);
        }
    }
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

    public record AddFundsQuery(string name, double amount) : IRequest;
    public class AddFundsHandler : IRequestHandler<AddFundsQuery>
    {
        private readonly RabbitMessageService _rabbitMessageService;
        public AddFundsHandler(RabbitMessageService rabbitMessageService)
        {
            _rabbitMessageService = rabbitMessageService;
        }
        public async Task Handle(AddFundsQuery request, CancellationToken cancellationToken)
        {
            UsersDbAddDto usersDbDto = new UsersDbAddDto
            {
                Name = request.name,
                Funds = request.amount,
                password_hash = ""
            };
            await _rabbitMessageService.SendMessage<UsersDbAddDto>(usersDbDto, "add");
        }
    }
    public record SellFundsQuery(string name, double amount) : IRequest;
    public class SellFundsHandler : IRequestHandler<AddFundsQuery>
    {
        private readonly RabbitMessageService _rabbitMessageService;

        public SellFundsHandler(RabbitMessageService rabbitMessageService)
        {
            _rabbitMessageService = rabbitMessageService;
        }

        public async Task Handle(AddFundsQuery request, CancellationToken cancellationToken)
        {
            UsersDbAddDto usersDbDto = new UsersDbAddDto
            {
                Name = request.name,
                Funds = request.amount,
                password_hash = ""
            };
            await _rabbitMessageService.SendMessage<UsersDbAddDto>(usersDbDto, "add");
        }
    }
}