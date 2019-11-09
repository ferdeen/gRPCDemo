using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcServer.Services
{
    public class CustomersService : Customer.CustomerBase
    {
        private readonly ILogger<CustomersService> _logger;

        public CustomersService(ILogger<CustomersService> logger)
        {
            _logger = logger;
        }

        public override Task<CustomerModel> GetCustomerInfo(CustomerLookupModel request, ServerCallContext context)
        {
            CustomerModel output = new CustomerModel();

            if (request.UserId == 1)
            {
                output.FirstName = "Jamie";
                output.LastName = "Smith";
            }
            else if (request.UserId == 2)
            {
                output.FirstName = "Jane";
                output.LastName = "Doe";
            }
            else
            {
                output.FirstName = "Gregg";
                output.LastName = "Thomas";
            }

            output.UserId = request.UserId;

            return Task.FromResult(output);
        }

        public override async Task GetNewCustomers(NewCustomerRequest request, IServerStreamWriter<CustomerModel> responseStream, ServerCallContext context)
        {
            List<CustomerModel> customers = new List<CustomerModel>
            {
                new CustomerModel
                {
                    FirstName = "Ferdeen",
                    LastName = "Mughal",
                    EmailAddress = "ferdeen@gmail.com",
                    Age = 46,
                    IsAlive = true
                },
                new CustomerModel
                {
                    FirstName = "Faron",
                    LastName = "Mughal",
                    EmailAddress = "Faron@gmail.com",
                    Age = 16,
                    IsAlive = true
                },
                new CustomerModel
                {
                    FirstName = "Callum",
                    LastName = "Mughal",
                    EmailAddress = "callum@gmail.com",
                    Age = 14,
                    IsAlive = true
                },
            };

            foreach(var cust in customers)
            {
                await Task.Delay(1000); // 1 second (mimicking extra time taken duing streaming)
                await responseStream.WriteAsync(cust);
            }
        }
    }
}
