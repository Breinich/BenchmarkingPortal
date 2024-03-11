using System.Net.NetworkInformation;
using BenchmarkingPortal.Bll.Exceptions;
using BenchmarkingPortal.Bll.Features.ComputerGroup.Commands;
using BenchmarkingPortal.Dal;
using BenchmarkingPortal.Dal.Dtos;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace BenchmarkingPortal.Bll.Features.ComputerGroup.CommandHandlers;

/// <summary>
/// Command handler for the <see cref="UpdateComputerGroupCommand"/>
/// </summary>
// ReSharper disable once UnusedType.Global
public class UpdateComputerGroupCommandHandler : IRequestHandler<UpdateComputerGroupCommand, ComputerGroupHeader>
{
    private readonly BenchmarkingDbContext _context;
    private readonly UserManager<Dal.Entities.User> _userManager;

    public UpdateComputerGroupCommandHandler(BenchmarkingDbContext context, UserManager<Dal.Entities.User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }


    public async Task<ComputerGroupHeader> Handle(UpdateComputerGroupCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(request.InvokerName);

        if (user == null || !await _userManager.IsInRoleAsync(user, Roles.Admin))
            throw new ArgumentException(new ExceptionMessage<Dal.Entities.ComputerGroup>().NoPrivilege);

        var computerGroup = await _context.ComputerGroups.FindAsync(new object?[] { request.Id }, 
            cancellationToken: cancellationToken);
        if (computerGroup == null)
            throw new ArgumentException(new ExceptionMessage<Dal.Entities.ComputerGroup>().ObjectNotFound);

        if (request.Description != null)
            computerGroup.Description = request.Description;

        if (request.Name != null)
            computerGroup.Name = request.Name;

        if (request.Hostname != null)
        {
            // check if the given hostname is reachable
            Ping pingSender = new Ping ();
            int timeout = 60;
            PingReply reply = pingSender.Send(request.Hostname, timeout: timeout);
            if (reply.Status == IPStatus.Success)
            {
                computerGroup.Hostname = request.Hostname;
            }
            else
            {
                throw new ArgumentException("The given hostname is not reachable!");
            }
        }

        await _context.SaveChangesAsync(cancellationToken);
        return new ComputerGroupHeader(computerGroup);
    }
}