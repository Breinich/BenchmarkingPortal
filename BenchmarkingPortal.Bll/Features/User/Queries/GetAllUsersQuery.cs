using BenchmarkingPortal.Dal.Dtos;
using MediatR;

namespace BenchmarkingPortal.Bll.Features.User.Queries;

public class GetAllUsersQuery : IRequest<IEnumerable<UserHeader>>
{
}