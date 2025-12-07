using MediatR;
using PurchaseStocks.Infrastructure;
using SellStocks.Application.Dtos;
using SellStocks.Domain.Entities;

namespace PurchaseStocks.Application.Handlers
{
    // --- GET ALL ---
    public record GetAllUsersQuery() : IRequest<List<UsersDbDto>>;

    public class GetAllUsersHandler : IRequestHandler<GetAllUsersQuery, List<UsersDbDto>>
    {
        private readonly CrudUsers _userService;

        public GetAllUsersHandler(CrudUsers CrudUsers)
        {
            _userService = CrudUsers;
        }

        public async Task<List<UsersDbDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            List<UsersDb> users = await _userService.GetAllAsync();
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
        private readonly CrudUsers _userService;

        public GetUserByIdHandler(CrudUsers CrudUsers)
        {
            _userService = CrudUsers;
        }

        public async Task<UsersDbDto?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            UsersDb user = await _userService.GetByIdAsync(request.Id);
            UsersDbDto result = new UsersDbDto(user.name,user.funds);
            return await Task.FromResult(result);
        }
    }
    public record GetUserByNameQuery(string name) : IRequest<UsersDbDto?>;
    public class GetUserByNameHandler : IRequestHandler<GetUserByNameQuery, UsersDbDto?>
    {
        private readonly CrudUsers _userService;
        public GetUserByNameHandler(CrudUsers CrudUsers)
        {
            _userService = CrudUsers;
        }
        public async Task<UsersDbDto?> Handle(GetUserByNameQuery request, CancellationToken cancellationToken)
        {
            UsersDb user = await _userService.GetOneByNameAsync(request.name);
            UsersDbDto result = new UsersDbDto(user.name, user.funds);
            return await Task.FromResult(result);
        }
    }
}
/*    public record AddUserCommand(UsersDbDto User) : IRequest;

    public class AddUserHandler : IRequestHandler<AddUserCommand>
    {
        private readonly CrudUsers _userService;

        public AddUserHandler(CrudUsers CrudUsers)
        {
            _userService = CrudUsers;
        }

        public async Task Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            await _userService.AddAsync(request.User);
        }
    }

    // --- UPDATE ---
    public record UpdateUserCommand(UsersDbDto User) : IRequest;

    public class UpdateUserHandler : IRequestHandler<UpdateUserCommand>
    {
        private readonly CrudUsers _userService;

        public UpdateUserHandler(CrudUsers CrudUsers)
        {
            _userService = CrudUsers;
        }

        public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            await _userService.UpdateAsync(request.User);
        }
    }

    // --- DELETE ---
    public record DeleteUserCommand(int Id) : IRequest;

    public class DeleteUserHandler : IRequestHandler<DeleteUserCommand>
    {
        private readonly CrudUsers _userService;

        public DeleteUserHandler(CrudUsers CrudUsers)
        {
            _userService = CrudUsers;
        }

        public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            await _userService.DeleteAsync(request.Id);
        }
    }*/