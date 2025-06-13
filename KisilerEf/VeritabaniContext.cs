using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore; //EntityFrameworkCore eklenmelidir.
using KisilerEf.Models; //models eklenmelidir.

namespace KisilerEf
{
    public class VeritabaniContext:DbContext //public olmalı ve DbContext'ten miras almalı
    {
        //DbSet eklenmelidir (Tablo örneği)
        public DbSet<Kisi> Kisiler { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder secenekler)
        {
            //Sql Server veri tabanı bağlantı cümlesini ekle
            secenekler.UseSqlServer(@"Server=(LocalDb)\MSSQLLocalDb;Database=Veriler2;Trusted_Connection=True;");
        }
    }
}
