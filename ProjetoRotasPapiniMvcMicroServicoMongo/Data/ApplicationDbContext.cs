using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ProjetoRotasPapiniMvcMicroServicoMongo.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ProjetoRotasPapiniMvcMicroServicoMongo.Models.Pessoa> Pessoa { get; set; }

        public DbSet<ProjetoRotasPapiniMvcMicroServicoMongo.Models.Cidade> Cidade { get; set; }

        public DbSet<ProjetoRotasPapiniMvcMicroServicoMongo.Models.Time> Time { get; set; }

        public DbSet<ProjetoRotasPapiniMvcMicroServicoMongo.Models.Arquivo> Arquivo { get; set; }

        public DbSet<ProjetoRotasPapiniMvcMicroServicoMongo.Models.Usuario> Usuario { get; set; }
    }
}
