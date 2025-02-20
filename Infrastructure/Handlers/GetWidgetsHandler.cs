using FinancialAssistent.Entities;
using FinancialAssistent.Infrastructure.Commands;
using FinancialAssistent.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinancialAssistent.Infrastructure.Handlers
{
    public class GetWidgetsHandler : IRequestHandler<GetWidgetsCommand, List<Widgets>>
    {
        private readonly Database dbContext;
        private readonly UserInfoService userInfoService;

        public GetWidgetsHandler(Database dbContext, UserInfoService userInfoService)
        {
            this.dbContext = dbContext;
            this.userInfoService = userInfoService;
        }

        public async Task<List<Widgets>> Handle(GetWidgetsCommand request, CancellationToken cancellationToken)
        {
            return await dbContext.Widgets
                .Include(w => w.Icon)
                .Where(w => w.UserInfoId == userInfoService.GetUser().UserInfo.Id)
                .ToListAsync(cancellationToken);
        }
    }
}
