using Market.Application.Dto;
using Market.Application.Services.Interfaces;
using Market.Core.Entities.Base;
using Market.Core.Global;
using Market.Core.Repositories.Base;
using Market.Core.UnitOfWork;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Market.Application.Services
{
    public class BaseService<TEntity, TDto, TGetAllDto>: IService<TEntity, TDto, TGetAllDto>
        where TEntity : class
        where TDto : EntityDto
        where TGetAllDto : class
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IRepository<TEntity> repository;

        public BaseService(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
            repository = (IRepository<TEntity>)unitOfWork.GetRepositoryByName(typeof(TEntity).Name);

        }
        //Task<IPagingResultDto<TGetAllDto>> IService<TEntity, TDto, TGetAllDto>.GetAll(int pageNumber, int pageSize, string orderByField, string orderType)
        //{
        //    throw new NotImplementedException();
        //}
        public virtual async Task<PagingResultDto<TGetAllDto>> GetAll(PagingInputDto pagingInputDto)
        {
            var result = await repository.GetAllAsync(pagingInputDto);

            var list = Mapper.MapperObject.Mapper.Map<IList<TGetAllDto>>(result.Item1);

            var response = new PagingResultDto<TGetAllDto>
            {
                Result = list,
                Total = result.Item2
            };

            return response;
        }

        public virtual async Task<TDto> GetById(long id)
        {
            var entity = await repository.GetByIdAsync(id);

            var response = Mapper.MapperObject.Mapper.Map<TDto>(entity);

            return response;
        }

        public virtual async Task Delete(long id)
        {
            var entity = await repository.GetByIdAsync(id);

            if (entity == null)
                throw new AppException(ExceptionEnum.RecordNotExist);

            var isSoftDelete = typeof(TEntity).GetInterfaces().Any(x => x ==typeof(ISoftDelete));

            if (isSoftDelete)
            {
                ((ISoftDelete)entity).IsDeleted = true;
            }
            else
            {
                await repository.DeleteAsync(entity);
            }

            await unitOfWork.CompleteAsync();
        }

        public virtual async Task<TDto> Create(TDto input)
        {
            var employeeEntitiy = Mapper.MapperObject.Mapper.Map<TEntity>(input);
            if (employeeEntitiy is null)
            {
                throw new AppException(ExceptionEnum.MapperIssue);
            }

            var newEmployee = await repository.AddAsync(employeeEntitiy);
            await unitOfWork.CompleteAsync();

            var response = Mapper.MapperObject.Mapper.Map<TDto>(newEmployee);
            return response;
        }

        public virtual async Task<TDto> Update(TDto input)
        {
            var entitiy = await repository.GetByIdAsync(input.Id);

            if (entitiy == null)
                throw new AppException(ExceptionEnum.RecordNotExist);

            Mapper.MapperObject.Mapper.Map(input, entitiy, typeof(TDto), typeof(TEntity));

            await unitOfWork.CompleteAsync();

            return input;
        }


    }

    public class BaseService<TEntity, TDto, TUpdatDto, TGetAllDto> : IService<TEntity, TDto, TUpdatDto, TGetAllDto>
    where TEntity : class
    where TDto : EntityDto
    where TUpdatDto : EntityDto
    where TGetAllDto : class
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IRepository<TEntity> repository;

        public BaseService(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
            repository = (IRepository<TEntity>)unitOfWork.GetRepositoryByName(typeof(TEntity).Name);

        }
        //Task<IPagingResultDto<TGetAllDto>> IService<TEntity, TDto, TGetAllDto>.GetAll(int pageNumber, int pageSize, string orderByField, string orderType)
        //{
        //    throw new NotImplementedException();
        //}
        public virtual async Task<PagingResultDto<TGetAllDto>> GetAll(PagingInputDto pagingInputDto)
        {
            var result = await repository.GetAllAsync(pagingInputDto);

            var list = Mapper.MapperObject.Mapper.Map<IList<TGetAllDto>>(result.Item1);

            var response = new PagingResultDto<TGetAllDto>
            {
                Result = list,
                Total = result.Item2
            };

            return response;
        }

        public virtual async Task<TDto> GetById(long id)
        {
            var entity = await repository.GetByIdAsync(id);

            var response = Mapper.MapperObject.Mapper.Map<TDto>(entity);

            return response;
        }

        public virtual async Task Delete(long id)
        {
            var entity = await repository.GetByIdAsync(id);

            if (entity == null)
                throw new AppException(ExceptionEnum.RecordNotExist);

            var isSoftDelete = typeof(TEntity).GetInterfaces().Any(x => x == typeof(ISoftDelete));

            if (isSoftDelete)
            {
                ((ISoftDelete)entity).IsDeleted = true;
            }
            else
            {
                await repository.DeleteAsync(entity);
            }

            await unitOfWork.CompleteAsync();
        }

        public virtual async Task<TDto> Create(TDto input)
        {
            var employeeEntitiy = Mapper.MapperObject.Mapper.Map<TEntity>(input);
            if (employeeEntitiy is null)
            {
                throw new AppException(ExceptionEnum.MapperIssue);
            }

            var newEmployee = await repository.AddAsync(employeeEntitiy);
            await unitOfWork.CompleteAsync();

            var response = Mapper.MapperObject.Mapper.Map<TDto>(newEmployee);
            return response;
        }

        public virtual async Task<TDto> Update(TUpdatDto input)
        {
            var entitiy = await repository.GetByIdAsync(input.Id);

            if (entitiy == null)
                throw new AppException(ExceptionEnum.RecordNotExist);

            Mapper.MapperObject.Mapper.Map(input, entitiy, typeof(TDto), typeof(TEntity));

            await unitOfWork.CompleteAsync();

            var response = Mapper.MapperObject.Mapper.Map<TDto>(entitiy);

            return response;
        }
    }
}
