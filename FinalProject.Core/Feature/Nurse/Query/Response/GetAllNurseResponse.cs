﻿namespace FinalProject.Core.Feature.Nurse.Query.Response
{
    public class GetAllNurseResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string? Image { get; set; }
        public string Email { get; set; }
        public string Department { get; set; }
    }
}
