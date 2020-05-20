using Newtonsoft.Json;
using ProjectManager.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ProjectManager.Repositories
{
    public class ActivityTimeRepository : IActivityTimeRepository
    {
        protected List<IActivityTime> times;

        public ActivityTimeRepository()
        {
            Load();
        }

        public IActivityTime Add(IActivityTime entry)
        {
            if (entry.Id == Guid.Empty) entry.Id = Guid.NewGuid();

            times.Add(entry);

            Save();

            return entry;
        }

        public IActivityTime Get(Guid id)
        {
            return times.FirstOrDefault(a => a.Id == id);
        }

        public IQueryable<IActivityTime> GetAll()
        {
            return times.AsQueryable();
        }

        public bool Update(IActivityTime entry)
        {
            var time = times.FirstOrDefault(a => a.Id == entry.Id);
            time.ActivityId = entry.ActivityId;
            time.Comment = entry.Comment;
            time.Hours = entry.Hours;
            time.Timestamp = entry.Timestamp;
            time.Reported = entry.Reported;

            Save();

            return true;
        }

        public bool Delete(IActivityTime entry)
        {
            return Delete(entry.Id);
        }

        public bool Delete(Guid id)
        {
            var time = times.FirstOrDefault(a => a.Id == id);
            times.Remove(time);

            Save();

            return true;
        }

        protected void Load()
        {
            var filePath = "times.json";
            times = Load(filePath);
        }

        protected void Save()
        {
            var filePath = "times.json";
            Save(filePath, times);
        }

        public List<IActivityTime> Load(string filePath)
        {
            if (File.Exists(filePath))
            {
                var data = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<List<IActivityTime>>(data, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Objects
                });
            }
            return new List<IActivityTime>();
        }

        public void Save(string filePath, List<IActivityTime> data)
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
