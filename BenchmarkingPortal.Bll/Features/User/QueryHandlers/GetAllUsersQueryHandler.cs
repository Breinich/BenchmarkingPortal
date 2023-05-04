using BenchmarkingPortal.Bll.Features.User.Queries;
using BenchmarkingPortal.Dal;
using BenchmarkingPortal.Dal.Dtos;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BenchmarkingPortal.Bll.Features.User.QueryHandlers;

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, IEnumerable<UserHeader>>
{
    private readonly BenchmarkingDbContext _context;
    private readonly UserManager<Dal.Entities.User> _userManager;

    public GetAllUsersQueryHandler(BenchmarkingDbContext context, UserManager<Dal.Entities.User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }


    public async Task<IEnumerable<UserHeader>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var list = await _context.Users.Select(u => new UserHeader()
        {
            Id = u.Id,
            UserName = u.UserName,
            Email = u.Email,
            Subscription = u.Subscription
        }).ToListAsync(cancellationToken);
        foreach (var userHeader in list)
        {
            var u = await _userManager.FindByIdAsync(userHeader.Id.ToString());
            if(u == null) continue;
            
            userHeader.Role = (await _userManager.GetRolesAsync(u)).FirstOrDefault();
        }

        return list;
    }
        
}