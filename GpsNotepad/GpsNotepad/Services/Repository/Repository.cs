using GpsNotepad.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace GpsNotepad.Services.Repository
{
    public class Repository: IRepository
    {
        #region   ---    PrivateFields   ---

        private readonly Lazy<SQLiteAsyncConnection> _database;

        #endregion

        public Repository()
        {
            _database = new Lazy<SQLiteAsyncConnection>(() =>
            {
                //Get the path to local folder 
                var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "pinbook.db3");
                //Create a connection
                var database_ = new SQLiteAsyncConnection(path);
                //Create the table UserModel
                database_.CreateTableAsync<UserModel>();
                //Create the table PinModel
                database_.CreateTableAsync<PinModel>();
                //Create the table ImagesPin
                database_.CreateTableAsync<ImagesPin>();
                return database_;
            });
        }

        #region    ---   Methods   ---

        public async Task<int> DeleteAsync<T>(T entity) where T : IEntityBase, new()
        {
            return await _database.Value.DeleteAsync(entity);
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>() where T : IEntityBase, new()
        {
            return await _database.Value.Table<T>().ToListAsync();
        }

        public async Task<int> InsertAsync<T>(T entity) where T : IEntityBase, new()
        {
            return await _database.Value.InsertAsync(entity);
        }

        public async Task<int> UpdateAsync<T>(T entity) where T : IEntityBase, new()
        {
            return await _database.Value.UpdateAsync(entity);
        }

        #endregion
    }
}
