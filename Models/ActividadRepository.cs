using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace Models
{
    public class ActividadRepository
    {
        public List<Actividad> GetAll()
        {
            using (var context = new zenithzoneEntities())
            {
                return context.Actividad.ToList();
            }
        }

        public Actividad GetById(int id)
        {
            using (var context = new zenithzoneEntities())
            {
                return context.Actividad.Find(id);
            }
        }

        public void Add(Actividad actividad)
        {
            using (var context = new zenithzoneEntities())
            {
                context.Actividad.Add(actividad);
                var result = context.SaveChanges();
                
                System.Diagnostics.Debug.WriteLine($"Actividad agregada con ID: {actividad.Id}, Rows affected: {result}");
            }
        }

        public void Update(Actividad actividad)
        {
            using (var context = new zenithzoneEntities())
            {
                var existing = context.Actividad.Find(actividad.Id);
                if (existing != null)
                {
                    existing.Nombre = actividad.Nombre;
                    existing.AforoMaximo = actividad.AforoMaximo;
                    context.SaveChanges();
                }
            }
        }

        public void Delete(int id)
        {
            using (var context = new zenithzoneEntities())
            {
                var actividad = context.Actividad.Find(id);
                if (actividad != null)
                {
                    context.Actividad.Remove(actividad);
                    context.SaveChanges();
                }
            }
        }
    }
}
