using Acr.UserDialogs;
using GpsNotepad.Model;
using GpsNotepad.Service.Settings;
using GpsNotepad.Services.Pin;
using GpsNotepad.Services.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GpsNotepad.Services.ImagesOfPin
{
    public class ImagesPinService: IImagesPinService
    {
        #region   ---    PrivateFields   ---

        private readonly IRepository _repository;
        private readonly ISettingsManager _settingsManager;
        private readonly IPinServices _pinServices;

        #endregion

        public ImagesPinService(IRepository repository, ISettingsManager settingsManager, IPinServices pinServices)
        {
            _repository = repository;
            _settingsManager = settingsManager;
            _pinServices = pinServices;
        }

        #region    ---   Methods   ---
        public async Task<IEnumerable<ImagesPin>> GetAllImagePinModelAsync(int pinId)
        {
            IEnumerable<ImagesPin> imagePinModel = null;
            try
            {

                var resultOfGettingAllImages = await _repository.GetAllAsync<ImagesPin>();

                imagePinModel = resultOfGettingAllImages.Where(x => x.PinId == pinId).ToList();

            }
            catch (Exception ex)
            {
                UserDialogs.Instance.Alert(ex.Message);
            }
            return imagePinModel;
        }

        public async Task<bool> DeleteImagePinModelAsync(ImagesPin imagePinModel)
        {
            bool resultOfAction = false;
            int countDeletedRow = 0;

            try
            {
                if (imagePinModel != null)
                {
                    countDeletedRow= await _repository.DeleteAsync(imagePinModel);

                    if(countDeletedRow==1)
                    {
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

        public async Task<bool> DeleteAllImagePinModelAsync(int pinId)
        {
            bool resultOfActionDelete = false;
            int countDeletedRow = 0;
            try
            {
                var resultOfGettingAllImages = await _repository.GetAllAsync<ImagesPin>();

                var imagesPinModel = resultOfGettingAllImages.Where(x => x.PinId == pinId).ToList();

                if(imagesPinModel.ToList().Count!=0)
                {
                    foreach (var imagePin in imagesPinModel)
                    {
                        countDeletedRow =await _repository.DeleteAsync(imagePin);

                        if(countDeletedRow==1)
                        {
                            resultOfActionDelete = true;
                        }
                    }
                }
                else
                {
                    resultOfActionDelete = true;
                }
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.Alert(ex.Message);
            }

            return resultOfActionDelete;
        }
        public async Task<bool> SaveImagePinModelAsync(ImagesPin imagePinModel)
        {
            bool resultOfAction = false;
            int countSavedRow = 0;

            try
            {
                if (imagePinModel != null)
                {
                    countSavedRow=await _repository.InsertAsync<ImagesPin>(imagePinModel);

                    if(countSavedRow==1)
                    {
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

        public async Task<bool> UpdateImagePinModelAsync(ImagesPin imagePinModel)
        {
            bool resultOfAction = false;
            int countUpdatedRow = 0;

            try
            {
                if (imagePinModel != null)
                {
                    countUpdatedRow=await _repository.UpdateAsync<ImagesPin>(imagePinModel);

                    if(countUpdatedRow==1)
                    {
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

        #endregion
    }
}
