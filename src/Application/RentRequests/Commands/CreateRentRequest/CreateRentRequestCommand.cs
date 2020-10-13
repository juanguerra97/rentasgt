using MediatR;
using Microsoft.EntityFrameworkCore;
using rentasgt.Application.Common.Exceptions;
using rentasgt.Application.Common.Interfaces;
using Application.Common.Extensions;
using rentasgt.Domain.Entities;
using rentasgt.Domain.Enums;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace rentasgt.Application.RentRequests.Commands.CreateRentRequest
{
    public class CreateRentRequestCommand : IRequest<long>
    {

        public long ProductId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Place { get; set; }

    }

    public class CreateRentRequestCommandHandler : IRequestHandler<CreateRentRequestCommand, long>
    {

        private readonly IApplicationDbContext context;
        private readonly IDateTime timeService;
        private readonly ICurrentUserService currentUserService;
        private readonly UserManager<AppUser> userManager;

        public CreateRentRequestCommandHandler(IApplicationDbContext context, IDateTime timeService, ICurrentUserService currentUserService, UserManager<AppUser> userManager)
        {
            this.context = context;
            this.timeService = timeService;
            this.currentUserService = currentUserService;
            this.userManager = userManager;
        }

        public async Task<long> Handle(CreateRentRequestCommand request, CancellationToken cancellationToken)
        {

            var productEntity = await this.context.Products.Include(p => p.Owner).SingleOrDefaultAsync(p => p.Id == request.ProductId);

            if (productEntity == null)
            {
                throw new NotFoundException(nameof(Product), request.ProductId);
            }

            if (productEntity.Status == ProductStatus.Inactive || productEntity.Status == ProductStatus.Incomplete)
            {
                throw new InvalidStateException("El estado actual del producto no permite realizar la operacion");
            }

            request.StartDate = request.StartDate.ToCentralAmericaStandardTime();
            request.EndDate = request.EndDate.ToCentralAmericaStandardTime();

            // one month from now is the maximum start date a rent request can be made
            DateTime maxStartDate = this.timeService.Now.AddMonths(1);
            if (request.StartDate.CompareTo(maxStartDate) > 0)
            {
                throw new InvalidRentRequestException("No se permite reservar mas de un mes desde la fecha actual");
            }

            // var dif = request.EndDate - request.StartDate;
            // if (dif.Days < 1)
            // {
            //     throw new InvalidRentRequestException("No puedes rentar un artículo por menos de 12 horas");
            // }

            var conflicts = await this.context.RentRequests.Where(rq => rq.Product.Id == productEntity.Id && rq.Status == RequestStatus.Accepted).Where(rq => (request.StartDate.CompareTo(rq.StartDate) >= 0 && request.StartDate.CompareTo(rq.EndDate) <= 0) || (request.EndDate.CompareTo(rq.StartDate) >= 0 && request.EndDate.CompareTo(rq.EndDate) <= 0) || (request.StartDate.CompareTo(rq.StartDate) <= 0 && (request.EndDate.CompareTo(rq.EndDate) >= 0))).ToListAsync();
            if (conflicts.Any())
            {
                throw new InvalidRentRequestException("Conflicto de fechas");
            }

            if (await this.context.RentRequests.Where(rq => rq.RequestorId == this.currentUserService.UserId).Where(rq => rq.Product.Id == productEntity.Id  && (rq.Status == RequestStatus.Pending || rq.Status == RequestStatus.Viewed)).AnyAsync())
            {
                throw new OperationForbidenException();
            }

            var currentUser = await this.userManager.FindByIdAsync(this.currentUserService.UserId);

            if (currentUser.Id == productEntity.Owner.Id)
            {
                throw new OperationForbidenException("No puedes rentar tu propio articulo");
            }

            var newRentRequest = new RentRequest
            {
                Status = RequestStatus.Pending,
                RequestDate = this.timeService.Now,
                Product = productEntity,
                Requestor = currentUser,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Place = request.Place,
                EstimatedCost = EstimateCost(request, productEntity),
            };

            await this.context.RentRequests.AddAsync(newRentRequest);
            await this.context.SaveChangesAsync(cancellationToken);

            return newRentRequest.Id;
        }

        private decimal EstimateCost(CreateRentRequestCommand command, Product product)
        {
            decimal total = 0;
            var span = command.EndDate.Date - command.StartDate.Date;
            var days = span.Days + 1;

            var months = days / 31;
            if (product.CostPerMonth != null)
            {
                total += months * (decimal)product.CostPerMonth;
                days = days - months * 31;
            }
            
            var weeks = days / 7;
            if (product.CostPerWeek != null)
            {
                total += weeks * (decimal)product.CostPerWeek;
                days = days - weeks * 7;
            }
            total += days * product.CostPerDay;
            return total;
        }

    }

}