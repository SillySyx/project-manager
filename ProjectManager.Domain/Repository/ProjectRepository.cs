using Newtonsoft.Json;
using ProjectManager.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ProjectManager.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        protected List<IProject> projects;

        public ProjectRepository()
        {
            Load();
        }

        public IProject Add(IProject entry)
        {
            if (entry.Id == Guid.Empty) entry.Id = Guid.NewGuid();

            projects.Add(entry);

            Save();

            return entry;
        }

        public IProject Get(Guid id)
        {
            return projects.FirstOrDefault(p => p.Id == id);
        }

        public IQueryable<IProject> GetAll()
        {
            return projects.AsQueryable();
        }

        public bool Update(IProject entry)
        {
            var project = projects.FirstOrDefault(p => p.Id == entry.Id);
            project.Name = entry.Name;
            project.Description = entry.Description;
            project.Parent = entry.Parent;

            Save();

            return true;
        }

        public bool Delete(IProject entry)
        {
            return Delete(entry.Id);
        }

        public bool Delete(Guid id)
        {
            var project = projects.FirstOrDefault(p => p.Id == id);
            projects.Remove(project);

            Save();

            return true;
        }

        protected void Load()
        {
            var filePath = "projects.json";
            projects = Load(filePath);
        }

        protected void Save()
        {
            var filePath = "projects.json";
            Save(filePath, projects);
        }

        public List<IProject> Load(string filePath)
        {
            if (File.Exists(filePath))
            {
                var data = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<List<IProject>>(data, new JsonSerializerSettings 
                {
                    TypeNameHandling = TypeNameHandling.Objects
                });
            }
            return new List<IProject>();
        }

        public void Save(string filePath, List<IProject> data)
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
