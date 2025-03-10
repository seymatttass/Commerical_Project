using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;
using SagaStateMachine.Service.StateMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SagaStateMachine.Service.StateDbContext
{
    public class BasketStateDbContext : SagaDbContext
    {
        public BasketStateDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override IEnumerable<ISagaClassMap> Configurations
        {
            get
            {
                //db oluştur ve db oluştururken state ınstancelrın validasyn
                //kurallarını da burada map den al.
                yield return new OrderStateMap();
            }
        }

    }
}
