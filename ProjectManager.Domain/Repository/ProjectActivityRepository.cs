using Newtonsoft.Json;
using ProjectManager.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ProjectManager.Repositories
{
    public class ProjectActivityRepository : IProjectActivityRepository
    {
        protected List<IProjectActivity> activities;

        public ProjectActivityRepository()
        {
            Load();
        }

        public IProjectActivity Add(IProjectActivity entry)
        {
            if (entry.Id == Guid.Empty) entry.Id = Guid.NewGuid();

            activities.Add(entry);

            Save();

            return entry;
        }

        public IProjectActivity Get(Guid id)
        {
            return activities.FirstOrDefault(a => a.Id == id);
        }

        public IQueryable<IProjectActivity> GetAll()
        {
            return activities.AsQueryable();
        }

        public bool Update(IProjectActivity entry)
        {
            var activity = activities.FirstOrDefault(a => a.Id == entry.Id);
            activity.Name = entry.Name;
            activity.Description = entry.Description;
            activity.ProjectId = entry.ProjectId;
            activity.Deadline = entry.Deadline;
            activity.TimeBudget = entry.TimeBudget;

            Save();

            return true;
        }

        public bool Delete(IProjectActivity entry)
        {
            return Delete(entry.Id);
        }

        public bool Delete(Guid id)
        {
            var activity = activities.FirstOrDefault(a => a.Id == id);
            activities.Remove(activity);

            Save();

            return true;
        }

        protected void Load()
        {
            var filePath = "activities.json";
            activities = Load(filePath);
        }

        protected void Save()
        {
            var filePath = "activities.json";
            Save(filePath, activities);
        }

        public List<IProjectActivity> Load(string filePath)
        {
            if (File.Exists(filePath))
            {
                var data = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<List<IProjectActivity>>(data, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Objects
                });
            }
            return new List<IProjectActivity>();
        }

        public void Save(string filePath, List<IProjectActivity> data)
        {
            var json = JsonConvert.SerializeObject(data, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects,
                TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple
            });
            File.WriteAllText(filePath, json);
        }
    }
}
