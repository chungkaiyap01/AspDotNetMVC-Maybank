﻿using Maybank.DomainModelEntity.Entities;
using Maybank.DomainModelEntity.Interface;
using Maybank.InfrastructurePersistent.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maybank.InfrastructurePersistent.Repository
{
    public class RepositoryAdministrator : RepositoryGeneric<Administrator>, IRepositoryAdministrator
    {


        public RepositoryAdministrator(AppDbContext db) : base(db)
        {

        }
    }
}