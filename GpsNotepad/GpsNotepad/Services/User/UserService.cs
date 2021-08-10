using Acr.UserDialogs;
using GpsNotepad.Model;
using GpsNotepad.Services.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GpsNotepad.Service.User
{
    public class UserService: IUserService
    {
        #region   ---    PrivateFields   ---

        private readonly IRepository _repository;

        #endregion

        public UserService(IRepository repository)
        {
            _repository = repository;
        }

        #region    ---   Methods   ---
        public async Task<IEnumerable<UserModel>> GetAllUserModelAsync()
        {
            IEnumerable<UserModel> userModels = null;

            try
            {
                userModels = await _repository.GetAllAsync<UserModel>();
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.Alert(ex.Message);
            }

            return userModels;
        }
        public async Task<bool> DeleteUserModelAsync(UserModel userModel)
        {
            bool resultOfAction = false;
            int countDeletededRow = 0;

            try
            {
                if (userModel != null)
                {
                    countDeletededRow= await _repository.DeleteAsync(userModel);

                    if(countDeletededRow==1)
                    {
                        resultOfAction = true;
                    }

                }
            }
            catch(Exception ex)
            {
                UserDialogs.Instance.Alert(ex.Message);
            }
            return resultOfAction;
        }
        public async Task<bool> SaveUserModelAsync(UserModel userModel)
        {
            bool resultOfAction = false;
            int countSavedRow = 0;
            try
            {
                countSavedRow=await _repository.InsertAsync<UserModel>(userModel);

                if (countSavedRow == 1)
                {
                    resultOfAction = true;
                }

            }
            catch(Exception ex)
            {
                UserDialogs.Instance.Alert(ex.Message);
            }
            return resultOfAction;
        }
        public async Task<bool> UpdateUserModelAsync(UserModel userModel)
        {
            bool resultOfAction = false;
            int countUpdatedRow = 0;
            try
            {
                if(userModel!=null)
                {
                    countUpdatedRow=await _repository.UpdateAsync<UserModel>(userModel);
                } 

                if(countUpdatedRow==1)
                {
                    resultOfAction = true;
                }
            }
            catch(Exception ex)
            {
                UserDialogs.Instance.Alert(ex.Message);
            }
            return resultOfAction;
        }

        #endregion
    }
}
