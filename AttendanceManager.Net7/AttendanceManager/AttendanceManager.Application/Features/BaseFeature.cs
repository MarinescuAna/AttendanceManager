using AttendanceManager.Application.Contracts.UnitOfWork;
using AutoMapper;

namespace AttendanceManager.Application.Features
{
    public abstract class BaseFeature
    {
        protected readonly IUnitOfWork unitOfWork;
        protected readonly IMapper mapper;

        public BaseFeature(IUnitOfWork unit, IMapper mapper)
        {
            unitOfWork = unit;
            this.mapper = mapper;
        }
    }
}
