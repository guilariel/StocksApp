using MediatR;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Polly;
using RabbitMQAndGenericRepository.Repositorio;
using RabbitMQAndGenericRepository.Repositorio.DbEntities;
using StocksApp.Application.Dtos.DbDtos;
using StocksApp.Infrastructure.ExternalServices;
using StocksDll;

namespace StocksApp.Application.UseCases.DbUseCases
{
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
                UsersDbDto userDbDto = new UsersDbDto(us.name);
                result.Add(userDbDto);
            }
            return await Task.FromResult(result);
        }
    }

    // --- GET ONE ---
    public record GetUserByIdQuery(int Id, string currency) : IRequest<UserFundsDto?>;

    public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, UserFundsDto?>
    {
        private readonly UserRepository _userRepository;
        private readonly UserFundsRepository _userFundsRepository;
        public GetUserByIdHandler(UserRepository userRepository, UserFundsRepository userFundsRepository)
        {
            _userRepository = userRepository;
            _userFundsRepository = userFundsRepository;
        }

        public async Task<UserFundsDto?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            UsersDb user = await _userRepository.GetByIdAsync(request.Id);
            UserFundsDb funds = await _userFundsRepository.GetByIdAsync(user.id, request.currency);
            UserFundsDto result = new UserFundsDto(user.name, funds.funds, request.currency);
            return await Task.FromResult(result);
        }
    }
    public record GetUserByNameQuery(string name, string currency) : IRequest<UserFundsDto?>;
    public class GetUserByNameHandler : IRequestHandler<GetUserByNameQuery, UserFundsDto?>
    {
        private readonly UserRepository _userRepository;
        private readonly UserFundsRepository _userFundsRepository;
        public GetUserByNameHandler(UserRepository userRepository, UserFundsRepository userFundsRepository)
        {
            _userRepository = userRepository;
            _userFundsRepository = userFundsRepository;
        }
        public async Task<UserFundsDto?> Handle(GetUserByNameQuery request, CancellationToken cancellationToken)
        {
            UsersDb user = await _userRepository.GetOneByNameAsync(request.name);
            UserFundsDb funds = await _userFundsRepository.GetByIdAsync(user.id, request.currency);
            UserFundsDto result = new UserFundsDto(user.name, funds.funds, request.currency);
            return await Task.FromResult(result);
        }
    }

    public record GetYearlyProffitQuery(string user, string currency) : IRequest<double>;

    public class GetYearlyProffitHandler : IRequestHandler<GetYearlyProffitQuery, double>
    {
        private readonly DbContext _dbContext;
        private readonly UserRepository _userRepository;
        public GetYearlyProffitHandler(DbContext genericDbContext, UserRepository userRepository)
        {
            _dbContext = genericDbContext;
            _userRepository = userRepository;
        }
        public async Task<double> Handle(GetYearlyProffitQuery request, CancellationToken cancellationToken)
        {
            UsersDb usersDb = await _userRepository.GetOneByNameAsync(request.user);
            var userIdParam = new NpgsqlParameter("owner_id", usersDb.id);
            var currencyParam = new NpgsqlParameter("currency", request.currency);
            var profit = await _dbContext.Database
             .SqlQueryRaw<int>(
        "select get_yearly_profit(@owner_id, @currency) as \"Value\"",
        userIdParam,
        currencyParam
    )
         .SingleAsync(cancellationToken);
            return await Task.FromResult(profit);
        }
    }

    public record GetMonthlyProffitQuery(string user, string currency) : IRequest<double>;
    public class GetMonthlyProffitHandler : IRequestHandler<GetMonthlyProffitQuery, double>
    {
        private readonly DbContext _dbContext;
        private readonly UserRepository _userRepository;
        public GetMonthlyProffitHandler(DbContext genericDbContext, UserRepository userRepository)
        {
            _dbContext = genericDbContext;
            _userRepository = userRepository;
        }
        public async Task<double> Handle(GetMonthlyProffitQuery request, CancellationToken cancellationToken)
        {
            UsersDb usersDb = await _userRepository.GetOneByNameAsync(request.user);
            var userIdParam = new NpgsqlParameter("owner_id", usersDb.id);
            var currencyParam = new NpgsqlParameter("currency", request.currency);
            var profit = await _dbContext.Database
             .SqlQueryRaw<int>(
        "select get_monthly_profit(@owner_id, @currency) as \"Value\"",
        userIdParam,
        currencyParam
    )
         .SingleAsync(cancellationToken);
            return await Task.FromResult(profit);
        }
    }
}
/*    public record AddUserCommand(UsersDbDto User) : IRequest;

    public class AddUserHandler : IRequestHandler<AddUserCommand>
    {
        private readonly UserRepository userRepository;

        public AddUserHandler(UserRepository UserRepository)
        {
            userRepository = UserRepository;
        }

        public async Task Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            await userRepository.AddAsync(request.User);
        }
    }

    // --- UPDATE ---
    public record UpdateUserCommand(UsersDbDto User) : IRequest;

    public class UpdateUserHandler : IRequestHandler<UpdateUserCommand>
    {
        private readonly UserRepository userRepository;

        public UpdateUserHandler(UserRepository UserRepository)
        {
            userRepository = UserRepository;
        }

        public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            await userRepository.UpdateAsync(request.User);
        }
    }

    // --- DELETE ---
    public record DeleteUserCommand(int Id) : IRequest;

    public class DeleteUserHandler : IRequestHandler<DeleteUserCommand>
    {
        private readonly UserRepository userRepository;

        public DeleteUserHandler(UserRepository UserRepository)
        {
            userRepository = UserRepository;
        }

        public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            await userRepository.DeleteAsync(request.Id);
        }
    }*/