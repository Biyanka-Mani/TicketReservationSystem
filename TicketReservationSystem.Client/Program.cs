using TicketReservationDataAccess.Data.Core;
using Microsoft.EntityFrameworkCore;
using TicketReservationDataAccess.Data.Services;

namespace TicketReservationSystem.Client
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<TicketReservationDBContext>(options => options.UseSqlServer(
               builder.Configuration.GetConnectionString("DefaultConnection")
               ));
            builder.Services.AddScoped<IUserServices,UserServices>();
            builder.Services.AddScoped<IValidationServices,ValidationServices>();
            builder.Services.AddScoped<IAdminServices,AdminServices>();
            builder.Services.AddScoped<IRouteTerminalServices,RouteTerminalServices>();
            builder.Services.AddScoped<IBookingServices,BookingServices>();
            builder.Services.AddScoped<BookingManagementService>();
            builder.Services.AddScoped<IScheduleRelatedServices,ScheduleRelatedServices>();
            builder.Services.AddScoped<IDataMappingAndAssociationHandler,DataMappingAndAssociationHandler>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}