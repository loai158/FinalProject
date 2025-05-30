﻿using FinalProject.Data.Models.AppModels;
using FinalProject.Data.Models.IdentityModels;
using FinalProject.Data.Models.PaymentModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Infrastructure.DataAccess
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        DbSet<Patient> Patients { get; set; }
        DbSet<Doctor> Doctors { get; set; }
        DbSet<Appointment> Appointments { get; set; }
        DbSet<Nurse> Nurses { get; set; }
        DbSet<Department> Departments { get; set; }
        DbSet<PreviousCondition> PreviousConditions { get; set; }
        DbSet<PreviousMedicine> PreviousMedicines { get; set; }
        DbSet<Interaction> Interactions { get; set; }
        DbSet<Perscribtion> Perscribtions { get; set; }
        DbSet<DoctorSchedule> DoctorSchedules { get; set; }
        DbSet<MedicalRecord> MedicalRecords { get; set; }
        DbSet<Medicine> Medicines { get; set; }
        DbSet<Cart> Carts { get; set; }
        DbSet<Order> Orders { get; set; }
        DbSet<OrderItem> OrderItems { get; set; }
        //DbSet<ApplyRequest> ApplyRequestes { get; set; }
        DbSet<ApplyRequest> ApplyRequests { get; set; }
        public DbSet<Message> Messages { get; set; }


        public DbSet<PerscribtionMedicine> PerscribtionMedicines { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Doctor>()
               .Property(d => d.IntialPrice)
               .HasPrecision(18, 2);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Doctor)
                .WithMany(d => d.Appointments)
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Department)
                .WithMany(dep => dep.Appointments)
                .HasForeignKey(a => a.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Patient)
                .WithMany(p => p.Appointments)
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.Cascade);



            modelBuilder.Entity<Perscribtion>()
                .HasMany(p => p.PerscribtionMedicines)
                .WithOne(pm => pm.Perscribtion)
                .HasForeignKey(pm => pm.PerscribtionId)
                .OnDelete(DeleteBehavior.Cascade); // لو حذفت الروشتة، يتم حذف الأدوية المرتبطة بها


            modelBuilder.Entity<Medicine>()
                 .HasMany(m => m.PerscribtionMedicines)
                 .WithOne(pm => pm.Medicine)
                 .HasForeignKey(pm => pm.MedicineId)
                 .OnDelete(DeleteBehavior.Restrict); // لو حذفت دواء، لا تحذف السجلات المرتبطة تلقائيًا


            modelBuilder.Entity<Appointment>()
               .HasOne(a => a.Perscribtion)
               .WithOne(p => p.Appointment)
               .HasForeignKey<Perscribtion>(p => p.AppointmentId)
               .OnDelete(DeleteBehavior.Cascade); // لو حذفت الموعد، يحذف الروشتة


            modelBuilder.Entity<Patient>()
                .HasOne(p => p.ApplicationUser)
                .WithMany()
                .HasForeignKey(p => p.IdentityUserId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Doctor>()
                .HasOne(p => p.ApplicationUser)
                .WithMany()
                .HasForeignKey(p => p.IdentityUserId)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Nurse>()
                .HasOne(p => p.ApplicationUser)
                .WithMany()
                .HasForeignKey(p => p.IdentityUserId)
                .OnDelete(DeleteBehavior.NoAction);



            modelBuilder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany()
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Receiver)
                .WithMany()
                .HasForeignKey(m => m.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);
        }

    }
}

