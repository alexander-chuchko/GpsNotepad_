using Acr.UserDialogs;
using GpsNotepad.Model;
using GpsNotepad.Service.Settings;
using GpsNotepad.Services.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GpsNotepad.Services.Pin
{
    public class PinServices : IPinServices
    {
        private readonly IRepository _repository;
        private readonly ISettingsManager _settingsManager;
        public PinServices(IRepository repository, ISettingsManager settingsManager)
        {
            _repository = repository;
            _settingsManager = settingsManager;
        }
        public async Task<List<PinModel>> GetPinListAsync(string keyWord=null)
        {
            List<PinModel> pinViewModelsById = null;
            try
            {
                var resultOfGettingAllPins = await _repository.GetAllAsync<PinModel>();
                if (string.IsNullOrWhiteSpace(keyWord))
                {
                    pinViewModelsById = resultOfGettingAllPins.Where(x => x.UserId == _settingsManager.AuthorizedUserID).ToList();
                }
                else
                {
                    pinViewModelsById = resultOfGettingAllPins.Where(x =>(x.UserId == _settingsManager.AuthorizedUserID) &&
                    x.Label.StartsWith(keyWord, StringComparison.OrdinalIgnoreCase)&&
                    x.Label.Contains(keyWord)).ToList();
                }
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.Alert(ex.Message);
            }

            return pinViewModelsById;
        }
        public async Task<bool> SaveOrUpdatePinModelToStorageAsync(PinModel pinModel)
        {
            bool resultOfAction = false;
            try
            {
                if (pinModel != null)
                {
                    if (pinModel.UserId == 0)
                    {
                        pinModel.UserId = _settingsManager.AuthorizedUserID;
                        await _repository.InsertAsync<PinModel>(pinModel);
                        resultOfAction = true;
                    }
                    else
                    {
                        await _repository.UpdateAsync<PinModel>(pinModel);
                        resultOfAction = true;
                    }
                }
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.Alert(ex.Message);
            }
            return resultOfAction;
        }
        public async Task<bool> DeletePinModelToStorageAsync(PinModel  pinModel)
        {
            bool resultOfAction = false;
            try
            {
                if (pinModel != null)
                {
                    await _repository.DeleteAsync(pinModel);
                    resultOfAction = true;
                }
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.Alert(ex.Message);
            }
            return resultOfAction;
        }
        public async Task<bool> UpdatePinModelToStorageAsync(PinModel  pinModel)
        {
            bool resultOfAction = false;
            try
            {
                if (pinModel != null)
                {
                    resultOfAction = true;
                    await _repository.UpdateAsync<PinModel>(pinModel);
                }
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.Alert(ex.Message);
            }
            return resultOfAction;
        }
        public void SetStateOfTextInSearchBar(string value)
        {
            _settingsManager.StateOfTextInSearchBar = value;
        }
        public string GetStatusPermission()
        {
            return _settingsManager.StateOfTextInSearchBar;
        }
    }
}
