using MediatR;
using StocksApp.Infrastructure.ExternalServices;
using RabbitMQAndGenericRepository.Repositorio.DbEntities;
using StocksDll;
using StocksApp.Application.Dtos.DbDtos;

namespace StocksApp.Application.UseCases.DbUseCases
{
    // --- GET ALL ---
    public record GetAllUsersQuery() : IRequest<List<UsersDbDto>>;

    public class GetAllUsersHandler : IRequestHandler<GetAllUsersQuery, List<UsersDbDto>>
    {
        private readonly UserRepository userRepository;

        public GetAllUsersHandler(UserRepository UserRepository)
        {
            userRepository = UserRepository;
        }

        public async Task<List<UsersDbDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<UsersDb> users = await userRepository.GetAllAsync();
            List<UsersDbDto> result = new List<UsersDbDto>();
            foreach (UsersDb us in users)
            {
                UsersDbDto userDbDto = new UsersDbDto(us.name, us.funds);
                result.Add(userDbDto);
            }
            return result;
        }
    }

    // --- GET ONE ---
    public record GetUserByIdQuery(int Id) : IRequest<UsersDbDto?>;

    public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, UsersDbDto?>
    {
        private readonly UserRepository userRepository;

        public GetUserByIdHandler(UserRepository UserRepository)
        {
            userRepository = UserRepository;
        }

        public async Task<UsersDbDto?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            UsersDb user = await userRepository.GetByIdAsync(request.Id);
            UsersDbDto result = new UsersDbDto(user.name,user.funds);
            return result;
        }
    }
    public record GetUserByNameQuery(string name) : IRequest<UsersDbDto?>;
    public class GetUserByNameHandler : IRequestHandler<GetUserByNameQuery, UsersDbDto?>
    {
        private readonly UserRepository userRepository;
        public GetUserByNameHandler(UserRepository UserRepository)
        {
            userRepository = UserRepository;
        }
        public async Task<UsersDbDto?> Handle(GetUserByNameQuery request, CancellationToken cancellationToken)
        {
            UsersDb user = await userRepository.GetOneByNameAsync(request.name);
            UsersDbDto result = new UsersDbDto(user.name, user.funds);
            return result;
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