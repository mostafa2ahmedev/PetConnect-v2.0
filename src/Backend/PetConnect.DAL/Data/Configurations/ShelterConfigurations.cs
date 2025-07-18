using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetConnect.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetConnect.DAL.Data.Configurations
{
    public class ShelterConfigurations : IEntityTypeConfiguration<Shelter>
    {
        public void Configure(EntityTypeBuilder<Shelter> builder)
        {
          
            builder.HasOne(S=>S.ShelterOwner).WithMany(SO=>SO.Shelters).HasForeignKey(SO=>SO.OwnerId);
        
        }
    }
}
