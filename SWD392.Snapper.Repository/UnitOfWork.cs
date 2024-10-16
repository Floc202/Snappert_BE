using SWD392.Snapper.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD392.Snapper.Repository
{
    public class UnitOfWork : IDisposable
    {
        private readonly SNAPPETContext _context;
        public UnitOfWork(SNAPPETContext context)
        {
            _context = context;
        }
        
        private GenericRepository<User> _users;
        private GenericRepository<Pet> _pets;
        private GenericRepository<AdminAction> _actions;
        private GenericRepository<AdminUser> _adminUsers;
        private GenericRepository<Emotion> _emotions;
        private GenericRepository<Order> _orders;
        private GenericRepository<Package> _packages;
        private GenericRepository<Payment> _payments;
        private GenericRepository<PetCategory> _petsCategory;
        private GenericRepository<Photo> _photo;
        private GenericRepository<Post> _post;
        private GenericRepository<PostEmotion> _postEmotions;
        private GenericRepository<Report> _reports;

        
        public GenericRepository<User> Users
        {
            get
            {
                if (_users == null)
                    _users = new GenericRepository<User>(_context);
                return _users;
            }
        }
        public GenericRepository<Pet> Pets
        {
            get
            {
                if (_pets == null)
                    _pets = new GenericRepository<Pet>(_context);
                return _pets;
            }
        }
        public GenericRepository<AdminAction> Actions
        {
            get
            {
                if (_actions == null)
                    _actions = new GenericRepository<AdminAction>(_context);
                return _actions;
            }
        }
        public GenericRepository<AdminUser> AdminUsers
        {
            get
            {
                if (_adminUsers == null)
                    _adminUsers = new GenericRepository<AdminUser>(_context);
                return _adminUsers;
            }
        }
        public GenericRepository<Emotion> Emotions
        {
            get
            {
                if (_emotions == null)
                    _emotions = new GenericRepository<Emotion>(_context);
                return _emotions;
            }
        }
        public GenericRepository<Order> Orders
        {
            get
            {
                if (_orders == null)
                    _orders = new GenericRepository<Order>(_context);
                return _orders;
            }
        }
        public GenericRepository<Package> Packages
        {
            get
            {
                if (_packages == null)
                    _packages = new GenericRepository<Package>(_context);
                return _packages;
            }
        }
        public GenericRepository<Payment> Payments
        {
            get
            {
                if (_payments == null)
                    _payments = new GenericRepository<Payment>(_context);
                return _payments;
            }
        }
        public GenericRepository<PetCategory> PetCategories
        {
            get
            {
                if (_petsCategory == null)
                    _petsCategory = new GenericRepository<PetCategory>(_context);
                return _petsCategory;
            }
        }
        public GenericRepository<Photo> Photos
        {
            get
            {
                if (_photo == null)
                    _photo = new GenericRepository<Photo>(_context);
                return _photo;
            }
        }
        public GenericRepository<Post> Posts
        {
            get
            {
                if (_post == null)
                    _post = new GenericRepository<Post>(_context);
                return _post;
            }
        }
        public GenericRepository<PostEmotion> PostEmotions
        {
            get
            {
                if (_postEmotions == null)
                    _postEmotions = new GenericRepository<PostEmotion>(_context);
                return _postEmotions;
            }
        }
        public GenericRepository<Report> Reports
        {
            get
            {
                if (_reports == null)
                    _reports = new GenericRepository<Report>(_context);
                return _reports;
            }
        }
        public void Save()
        {
            _context.SaveChanges();
        }
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                this.disposed = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
