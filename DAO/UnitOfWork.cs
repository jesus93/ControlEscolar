using System;
using System.Collections.Generic;
using Entities;

namespace DAL
{
    public class UnitOfWork : IDisposable
    {
        private GESTION_SHEntities _context = new GESTION_SHEntities();
        private GenericRepository<ALUMNOS> _alumnoRepository;
        private GenericRepository<CAT_MATERIAS> _catMateriasRepository;
        private GenericRepository<MATERIAS> _materiasRepository;

        public GenericRepository<ALUMNOS> AlumnoRepository
        {
            get
            {
                if (this._alumnoRepository == null)
                {
                    this._alumnoRepository = new GenericRepository<ALUMNOS>(_context);
                }

                return _alumnoRepository;
            }
        }

        public GenericRepository<CAT_MATERIAS> CatMateriasRepository
        {
            get
            {
                if (this._catMateriasRepository == null)
                {
                    this._catMateriasRepository = new GenericRepository<CAT_MATERIAS>(_context);
                }
                return _catMateriasRepository;
            }
        }

        public GenericRepository<MATERIAS> MateriasRepository
        {
            get
            {
                if (this._materiasRepository == null)
                {
                    this._materiasRepository = new GenericRepository<MATERIAS>(_context);
                }
                return _materiasRepository;
            }
        }

        public void Save()
        {
            _context.SaveChanges();
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
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}