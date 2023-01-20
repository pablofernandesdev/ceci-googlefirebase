using AutoMapper;
using CeciGoogleFirebase.Domain.DTO.Address;
using CeciGoogleFirebase.Domain.DTO.Commons;
using CeciGoogleFirebase.Domain.Entities;
using CeciGoogleFirebase.Domain.Interfaces.Repository;
using CeciGoogleFirebase.Domain.Interfaces.Service;
using CeciGoogleFirebase.Domain.Interfaces.Service.External;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CeciGoogleFirebase.Service.Services
{
    public class AddressService : IAddressService
    {
        private readonly IViaCepService _viaCepService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;

        public AddressService (IUnitOfWork uow, 
            IViaCepService viaCepService, 
            IMapper mapper)
        {
            _uow = uow;
            _viaCepService = viaCepService;
            _mapper = mapper;
        }

        public async Task<ResultResponse<AddressResultDTO>> GetAddressByZipCodeAsync(AddressZipCodeDTO obj)
        {
            var response = new ResultResponse<AddressResultDTO>();

            try
            {
                var addressRequest = await _viaCepService.GetAddressByZipCodeAsync(obj.ZipCode);

                response.StatusCode = addressRequest.StatusCode;

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    response.Message = "Unable to get address. Check that the zip code was entered correctly.";
                    return response;
                }

                response.Data = _mapper.Map<AddressResultDTO>(addressRequest.Data);
            }
            catch (Exception ex)
            {
                response.Message = "Could not get address.";
                response.Exception = ex;
            }

            return response;
        }

        public async Task<ResultResponse> AddAsync(AddressAddDTO obj)
        {
            var response = new ResultResponse();

            try
            {
                await _uow.Address.AddAsync(_mapper.Map<Address>(obj));
                await _uow.CommitAsync();

                response.Message = "Address successfully added.";

            }
            catch (Exception ex)
            {
                response.Message = "Could not add address.";
                response.Exception = ex;
            }

            return response;
        }

        public async Task<ResultResponse> UpdateAsync(AddressUpdateDTO obj)
        {
            var response = new ResultResponse();

            try
            {
                var address = await _uow.Address.GetFirstOrDefaultAsync(x=> x.Id == obj.AddressId);

                address = _mapper.Map(obj, address);

                _uow.Address.Update(address);
                await _uow.CommitAsync();

                response.Message = "Address successfully updated.";

            }
            catch (Exception ex)
            {
                response.Message = "Could not updated address.";
                response.Exception = ex;
            }

            return response;
        }

        public async Task<ResultResponse> DeleteAsync(int id)
        {
            var response = new ResultResponse();

            try
            {
                var address = await _uow.Address.GetFirstOrDefaultAsync(x => x.Id == id);

                _uow.Address.Delete(address);
                await _uow.CommitAsync();

                response.Message = "Address successfully deleted.";

            }
            catch (Exception ex)
            {
                response.Message = "Could not deleted address.";
                response.Exception = ex;
            }

            return response;
        }

        public async Task<ResultDataResponse<IEnumerable<AddressResultDTO>>> GetAsync(AddressFilterDTO filter)
        {
            var response = new ResultDataResponse<IEnumerable<AddressResultDTO>>();

            try
            {
                response.Data = _mapper.Map<IEnumerable<AddressResultDTO>>(await _uow.Address.GetByFilterAsync(filter));
                response.TotalItems = await _uow.Address.GetTotalByFilterAsync(filter);
                response.TotalPages = (int)Math.Ceiling((double)response.TotalItems / filter.PerPage);
            }
            catch (Exception ex)
            {
                response.Exception = ex;
            }

            return response;
        }

        public async Task<ResultResponse<AddressResultDTO>> GetByIdAsync(int id)
        {
            var response = new ResultResponse<AddressResultDTO>();

            try
            {
                response.Data = _mapper.Map<AddressResultDTO>(await _uow.Address.GetFirstOrDefaultNoTrackingAsync(x=> x.Id == id));
            }
            catch (Exception ex)
            {
                response.Exception = ex;
            }

            return response;
        }
    }
}
