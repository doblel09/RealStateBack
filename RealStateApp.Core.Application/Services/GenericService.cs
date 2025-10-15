using AutoMapper;
using RealStateApp.Core.Application.Interfaces.Repositories;
using RealStateApp.Core.Application.Interfaces.Services;

namespace RealStateApp.Core.Application.Services
{
    public class GenericService<SaveViewModel,ViewModel,Model> : IGenericService<SaveViewModel, ViewModel, Model>
         where SaveViewModel : class
         where ViewModel : class
         where Model : class
    {
        private readonly IGenericRepository<Model> _repository;
        private readonly IMapper _mapper;

        public GenericService(IGenericRepository<Model> repository, IMapper mapper)
        {
            _repository = repository;           
            _mapper = mapper;
        }

        public virtual async Task Update(SaveViewModel vm,int id)
        {
            Model entity = _mapper.Map<Model>(vm);
            await _repository.UpdateAsync(entity, id);
        }

        public virtual async Task<SaveViewModel> Add(SaveViewModel vm)
        {
            Model entity = _mapper.Map<Model>(vm);

            entity = await _repository.AddAsync(entity);

            SaveViewModel entityVm = _mapper.Map<SaveViewModel>(entity);

            return entityVm;
        }

        public virtual async Task<List<SaveViewModel>> AddRange(List<SaveViewModel> vm)
        {
            List<Model> entities = _mapper.Map<List<Model>>(vm);

            entities = await _repository.AddRangeAsync(entities);

            List<SaveViewModel> entitiesVm = _mapper.Map <List<SaveViewModel>>(entities);

            return entitiesVm;
        }

        public virtual async Task Delete(int id)
        {
            var product = await _repository.GetByIdAsync(id);
            await _repository.DeleteAsync(product);
        }

        public virtual async Task<SaveViewModel> GetByIdSaveViewModel(int id)
        {
            var entity = await _repository.GetByIdAsync(id);

            SaveViewModel vm = _mapper.Map<SaveViewModel>(entity);
            return vm;
        }

        public virtual async Task<List<ViewModel>> GetAllListViewModel()
        {
            var entityList = await _repository.GetAllListAsync();

            return _mapper.Map<List<ViewModel>>(entityList);
        }

        public virtual IQueryable<ViewModel> GetAllQueryViewModel()
        {
            var entityList =  _repository.GetAllQuery();

            return _mapper.Map<IQueryable<ViewModel>>(entityList);
        }
    }
}
