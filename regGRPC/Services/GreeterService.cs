using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Repository;
using Scaffolds;

namespace regGRPC
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly IConfiguration _configuration;
        public GreeterService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply
            {
                Message = "Hello " + request.Name
            });
        }

        public override async Task<LevelTypeReply> SendLevelType(LevelTypeRequest request, ServerCallContext context)
        {
            var conn = _configuration.GetConnectionString("DefaultConnection");
            try
            {
            }
            catch (Exception ex)
            {

                throw;
            }
            return new LevelTypeReply { Status = false };
            
                
            //try
            //{
            //    var count = _levelTypeRepository.CountAsync().Result + 1;

            //    _levelTypeRepository.AddAsync(new LevelType
            //    {
            //        CreatedDate = DateTimeOffset.UtcNow,
            //        Name = request.Name,
            //        NormalizedName = request.Normalized,
            //        ID = (byte)count
            //    }).Wait();

            //    _levelTypeRepository.SaveChangeAsync().Wait();

            //    return Task.FromResult(new LevelTypeReply { Status = true });

            //}
            //catch (Exception)
            //{
            //    return Task.FromResult(new LevelTypeReply { Status = false });
            //}
           
        }
    }
}
