using AutoMapper;
using Core.Exceptions;
using DataAccess.Repository;
using DataAccess.Repository.Companies;
using Domain.Common.Enums;
using Domain.Entities.Companies;
using Application.Validators.Companies;
using Shared.Resources.Companies;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Utilities.Helpers.Cryptograpies;
using System.Security.Cryptography;

namespace Application.Services.Entities.Companies
{
    public class CompanyService : ICompanyService
    {
        private readonly IMapper _mapper;
        private readonly ICompanyRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly CreateCompanyValidator _createValidationRules;
        private readonly UpdateCompanyValidator _updateDeleteValidationRules;
        public CompanyService(IMapper mapper, ICompanyRepository repository,IUnitOfWork unitOfWork)
        {
            _updateDeleteValidationRules = new UpdateCompanyValidator();
            _createValidationRules = new CreateCompanyValidator();
            _unitOfWork = unitOfWork;
            _repository = repository;
            _mapper = mapper;
        }

        public async Task CreateAsync(CompanyData createData)
        {
            var validate = await _createValidationRules.ValidateAsync(createData).ConfigureAwait(false);
            if (!validate.IsValid)
            {
                throw new EntityValidationException(validate.ToString("~"));
            }
            var company = _mapper.Map<Company>(createData);
            if (createData==null)
            {
                throw new NullReferenceException();
            }

            if (await _repository.IsExistAsync(x => x.Name == company.Name))
            {
                throw new RecordAlreadyExistException();
            }
            await _repository.AddAsync(company).ConfigureAwait(false);
            await _unitOfWork.CompleteAsync().ConfigureAwait(false);
        }

        public async Task DeleteAsync(int id)
        {
            var company =  await _repository.GetSingleFirstAsync(x => x.Id == id).ConfigureAwait(false);
            if (company == null)
            {
                throw new RecordNotFoundException();
            }
            company.Status = RecordStatus.Deleted;
            company.DateDeleted = DateTime.Now;
            await _unitOfWork.CompleteAsync().ConfigureAwait(false);
        }

        public async Task<CompanyGetData> GetAsync(int id)
        {
            var company = await _repository.GetSingleFirstAsync(x => x.Id == id).ConfigureAwait(false);
            if (company==null)
            {
                throw new RecordNotFoundException();
            }
            return _mapper.Map<CompanyGetData>(company);
        }

        public async Task<List<CompanyGetData>> ListAsync()
        {
            var list = await _repository.GetAll().Where(x => x.Status == RecordStatus.Active).ToListAsync().ConfigureAwait(false);
            
            return _mapper.Map<List<CompanyGetData>>(list);
        }

        public async Task<CompanyGetData> UpdateAsync(CompanyData updateData)
        {
            var validate = await _updateDeleteValidationRules.ValidateAsync(updateData).ConfigureAwait(false);
            if (!validate.IsValid)
            {
                throw new EntityValidationException(validate.ToString("~"));
            }
            var company =await _repository.GetSingleFirstAsync(x => x.Id == updateData.Id).ConfigureAwait(false);

            if (company == null)
            {
                throw new RecordNotFoundException();
            }
            var status = company.Status;
            _mapper.Map(updateData, company);

            await _unitOfWork.CompleteAsync().ConfigureAwait(false);

            return _mapper.Map<CompanyGetData>(company);
        }
    }
}
