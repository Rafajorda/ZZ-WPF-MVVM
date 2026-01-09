using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace Models
{
    public class SocioRepository
    {
        public List<Socio> GetAll()
        {
            using (var context = new zenithzoneEntities())
            {
                return context.Socio.ToList();
            }
        }

        public Socio GetById(int id)
        {
            using (var context = new zenithzoneEntities())
            {
                return context.Socio.Find(id);
            }
        }

        public void Add(Socio socio)
        {
            using (var context = new zenithzoneEntities())
            {
                context.Socio.Add(socio);
                var result = context.SaveChanges();
                
                // Después de SaveChanges, el Id debería estar asignado por la BD
                System.Diagnostics.Debug.WriteLine($"Socio agregado con ID: {socio.Id}, Rows affected: {result}");
            }
        }

        public void Update(Socio socio)
        {
            using (var context = new zenithzoneEntities())
            {
                var existing = context.Socio.Find(socio.Id);
                if (existing != null)
                {
                    existing.Nombre = socio.Nombre;
                    existing.Email = socio.Email;
                    existing.Activo = socio.Activo;
                    context.SaveChanges();
                }
            }
        }

        public void Delete(int id)
        {
            using (var context = new zenithzoneEntities())
            {
                var socio = context.Socio.Find(id);
                if (socio != null)
                {
                    context.Socio.Remove(socio);
                    context.SaveChanges();
                }
            }
        }
    }
}
