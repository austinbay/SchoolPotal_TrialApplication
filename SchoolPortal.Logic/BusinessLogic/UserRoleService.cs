using SchoolPortal.Data.Entities;
using SchoolPortal.Data.Helpers;
using SchoolPortal.Data.Repositories;
using SchoolPortal.Models.DataModels;
using AutoMapper;
using GeneralHelper.Lib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolPortal.Logic.Utilities;

namespace SchoolPortal.Logic.BusinessLogic
{
    
    public interface IUserRoleService   
    {
        Task<ServiceResponseData> DeleteAsync(int id);
        Task<ServiceResponseData> UpdateAsync(UserRoleModel model);
        Task<ServiceResponseData> AddAsync(UserRoleModel model);
        Task<ServiceResponseData<UserRoleItem>> GetItemAsync(int id);
        Task<ServiceResponseData<UserRoleDetails>> GetDetailsAsync(int id);
        Task<ServiceResponseData<List<LookupModel>>> GetLookupAsync();


        Task<ServiceResponseData<List<UserRoleItem>>> GetAllAsync();

    }
    public class UserRoleService : IUserRoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IMapper _mapper;
        private readonly IErrorService _errorService;

        public UserRoleService(IUnitOfWork unitOfWork, IUserRoleRepository userRoleRepository, IMapper mapper, IErrorService errorService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userRoleRepository = userRoleRepository;
            _errorService = errorService;
        }
      
        public async Task<ServiceResponseData<List<LookupModel>>> GetLookupAsync()
        {
            var responseData = new ServiceResponseData<List<LookupModel>>();
            try
            {
                
                var entities = _userRoleRepository.LoadAll();
                var returnData = entities.Select(x => new LookupModel
                {
                    Id = x.Id,
                    Name = x.Name

                }).ToList();

                responseData.Data = returnData;
              
            }
            catch (Exception ex)
            {
                responseData.ErrorMessage = ex.ProcessException(_errorService);
                responseData.IsSuccess = false;
            }

            return await Task.FromResult(responseData);
        }
       
        public async Task<ServiceResponseData> DeleteAsync(int id)
        {
            var responseData = new ServiceResponseData();
            try
            {
                var entity = GetUserRoleEntity(id);
                _userRoleRepository.Delete(entity);
                _unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                responseData.ErrorMessage = ex.ProcessException(_errorService);
                responseData.IsSuccess = false;
            }

            return await Task.FromResult(responseData);
        }
        public async Task<ServiceResponseData<UserRoleDetails>> GetDetailsAsync(int id)
        {
            var responseData = new ServiceResponseData<UserRoleDetails>();
            try
            {
                var entity = GetUserRoleEntity(id);
                responseData.Data = _mapper.Map<UserRoleDetails>(entity);
            }
            catch (Exception ex)
            {
                responseData.ErrorMessage = ex.ProcessException(_errorService);
                responseData.IsSuccess = false;
            }

            return await Task.FromResult(responseData);
        }

        public async Task<ServiceResponseData<UserRoleItem>> GetItemAsync(int id)
        {
          
            var responseData = new ServiceResponseData<UserRoleItem>();
            try
            {

                var entity = GetUserRoleEntity(id);
                responseData.Data = _mapper.Map<UserRoleItem>(entity);
                
            }
            catch (Exception ex)
            {
                responseData.ErrorMessage = ex.ProcessException(_errorService);
                responseData.IsSuccess = false;
            }

            return await Task.FromResult(responseData);
        }
        private List<UserRoleItem> ProcessMapping(IEnumerable<AspNetRoles> entities)
        {
           
            return _mapper.Map<List<UserRoleItem>>(entities);
        }
        
        public async Task<ServiceResponseData<List<UserRoleItem>>> GetAllAsync()
        {
          
            var responseData = new ServiceResponseData<List<UserRoleItem>>();
            try
            {

                var entities = _userRoleRepository.LoadAll();
                var data = ProcessMapping(entities);
                responseData.Data = data;

            }
            catch (Exception ex)
            {
                responseData.ErrorMessage = ex.ProcessException(_errorService);
                responseData.IsSuccess = false;
            }

            return await Task.FromResult(responseData);
        }

        private void RunValidation(UserRoleModel model)
        {
            var nameExists = _userRoleRepository.Get().Any(c => c.Name == model.Name &&
                                   c.Id != model.Id);
            if (nameExists)
                throw new Exception("Sorry! This name already exists.");
        }
        public async Task<ServiceResponseData> AddAsync(UserRoleModel model)
        {
           
            var responseData = new ServiceResponseData();
            try
            {

                RunValidation(model);
                var entity = _mapper.Map<AspNetRoles>(model);

                _userRoleRepository.Add(entity);
                _unitOfWork.Commit();

            }
            catch (Exception ex)
            {
                responseData.ErrorMessage = ex.ProcessException(_errorService);
                responseData.IsSuccess = false;
            }

            return await Task.FromResult(responseData);
        }
        public async Task<ServiceResponseData> UpdateAsync(UserRoleModel model)
        {
            var responseData = new ServiceResponseData();
            try
            {

                RunValidation(model);
                var userRole = GetUserRoleEntity(model.Id);
                if (userRole != null)
                {
                    _mapper.Map(model, userRole);
                    _userRoleRepository.Update(userRole);
                    _unitOfWork.Commit();
                }

            }
            catch (Exception ex)
            {
                responseData.ErrorMessage = ex.ProcessException(_errorService);
                responseData.IsSuccess = false;
            }

            return await Task.FromResult(responseData);
        }
        private AspNetRoles GetUserRoleEntity(int id)
        {
            return _userRoleRepository.Find(id);
        }

    }
}
