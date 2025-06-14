﻿using FinalProject.Data.Models.AppModels;

namespace FinalProject.Services.Abstracts
{
    public interface IDoctorServices
    {
        public IQueryable<Doctor> GetAll();
        public Task<int> Create(Doctor doctor);
        public Task<Doctor> GetById(int id);
        public Task<bool> IsDoctorNameExists(string name);
        public string Edit(Doctor doctor);
        public Task<int> GetTotalCount(string query);
        public Task<string> Delete(int id);
        public IQueryable<Doctor> GetByDeptId(int id);

        Task update(int id);
    }
}
