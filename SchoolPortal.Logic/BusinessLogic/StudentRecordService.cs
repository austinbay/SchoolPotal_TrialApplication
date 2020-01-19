using SchoolPortal.Data.Entities;
using SchoolPortal.Data.Helpers;
using SchoolPortal.Data.Repositories;
using SchoolPortal.Models.DataModels;
using AutoMapper;
using GeneralHelper.Lib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SchoolPortal.Logic.Utilities;
using Microsoft.Extensions.Configuration;
using StudentRecord = SchoolPortal.Data.Entities.StudentRecord;

namespace SchoolPortal.Logic.BusinessLogic
{
    
    public interface IStudentRecordService   
    {
        Task<ServiceResponseData> DeleteAsync(int id);
        Task<ServiceResponseData> UpdateAsync(StudentRecordModel model);
        Task<ServiceResponseData> AddAsync(StudentRecordModel model, int currentUserId);
        Task<ServiceResponseData<StudentRecordItem>> GetItemAsync(int id);
        Task<ServiceResponseData<StudentRecordDetails>> GetDetailsAsync(int id);
        Task<ServiceResponseData<List<LookupModel>>> GetLookupAsync();

        Task<ServiceResponseData<List<StudentRecordItem>>> GetAllAsync();

    }
    public class StudentRecordService : IStudentRecordService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStudentRecordRepository _studentRecordRepository;
        private readonly IMapper _mapper;
        private readonly IErrorService _errorService;
       

        public StudentRecordService(IUnitOfWork unitOfWork, IStudentRecordRepository studentRecordRepository, IMapper mapper, 
            IErrorService errorService, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _studentRecordRepository = studentRecordRepository;
            _errorService = errorService;
           
        }
      
        public async Task<ServiceResponseData<List<LookupModel>>> GetLookupAsync()
        {
            var responseData = new ServiceResponseData<List<LookupModel>>();
            try
            {
                
                var entities = _studentRecordRepository.LoadAll();
                var returnData = entities.Select(x => new LookupModel
                {
                    Id = x.Id,
                    Name = x.LastName + " " + x.FirstName

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
                var entity = GetStudentRecordEntity(id);
                _studentRecordRepository.Delete(entity);
                _unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                responseData.ErrorMessage = ex.ProcessException(_errorService);
                responseData.IsSuccess = false;
            }

            return await Task.FromResult(responseData);
        }
        public async Task<ServiceResponseData<StudentRecordDetails>> GetDetailsAsync(int id)
        {
            var responseData = new ServiceResponseData<StudentRecordDetails>();
            try
            {
                var entity = GetStudentRecordEntity(id);
                responseData.Data = _mapper.Map<StudentRecordDetails>(entity);
            }
            catch (Exception ex)
            {
                responseData.ErrorMessage = ex.ProcessException(_errorService);
                responseData.IsSuccess = false;
            }

            return await Task.FromResult(responseData);
        }

        public async Task<ServiceResponseData<StudentRecordItem>> GetItemAsync(int id)
        {
          
            var responseData = new ServiceResponseData<StudentRecordItem>();
            try
            {

                var entity = GetStudentRecordEntity(id);
                responseData.Data = _mapper.Map<StudentRecordItem>(entity);
                
            }
            catch (Exception ex)
            {
                responseData.ErrorMessage = ex.ProcessException(_errorService);
                responseData.IsSuccess = false;
            }

            return await Task.FromResult(responseData);
        }
        private List<StudentRecordItem> ProcessMapping(IEnumerable<StudentRecord> entities)
        {
           
            return _mapper.Map<List<StudentRecordItem>>(entities);
        }
        
        public async Task<ServiceResponseData<List<StudentRecordItem>>> GetAllAsync()
        {
          
            var responseData = new ServiceResponseData<List<StudentRecordItem>>();
            try
            {

                var entities = _studentRecordRepository.LoadAll();
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

        private void RunValidation(StudentRecordModel model)
        {
            var nameExists = _studentRecordRepository.Get().Any(c => c.PhoneNumber == model.PhoneNumber &&
                                   c.Id != model.Id);
            if (nameExists)
                throw new Exception("Sorry! This Phone Number already exists.");
        }
        public async Task<ServiceResponseData> AddAsync(StudentRecordModel model, int currentUserI)
        {
           
            var responseData = new ServiceResponseData();
            try
            {

                RunValidation(model);
                var entity = _mapper.Map<StudentRecord>(model);
                entity.CreatedBy = currentUserI;
                entity.DateCreated = DateTime.Now;
                _studentRecordRepository.Add(entity);
                _unitOfWork.Commit();

            }
            catch (Exception ex)
            {
                responseData.ErrorMessage = ex.ProcessException(_errorService);
                responseData.IsSuccess = false;
            }

            return await Task.FromResult(responseData);
        }
        public async Task<ServiceResponseData> UpdateAsync(StudentRecordModel model)
        {
            var responseData = new ServiceResponseData();
            try
            {

                RunValidation(model);
                var StudentRecord = GetStudentRecordEntity(model.Id);
                if (StudentRecord != null)
                {
                    _mapper.Map(model, StudentRecord);
                    _studentRecordRepository.Update(StudentRecord);
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
        private StudentRecord GetStudentRecordEntity(int id)
        {
            return _studentRecordRepository.Find(id);
        }

    }
}
