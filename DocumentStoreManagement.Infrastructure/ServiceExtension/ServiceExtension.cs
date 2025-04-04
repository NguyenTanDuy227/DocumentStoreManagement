using DocumentStoreManagement.Core.Interfaces;
using DocumentStoreManagement.Core.Models;
using DocumentStoreManagement.Infrastructure.Repositories.SQL;
using DocumentStoreManagement.Services;
using DocumentStoreManagement.Services.Commands.DocumentCommands;
using DocumentStoreManagement.Services.Handlers.DocumentHandlers;
using DocumentStoreManagement.Services.Interfaces;
using DocumentStoreManagement.Services.MessageBroker;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DocumentStoreManagement.Infrastructure.ServiceExtension
{
    /// <summary>
    /// Service Extension Helper
    /// </summary>
    public static class ServiceExtension
    {
        /// <summary>
        /// Add the dependency injections of database and repositories here
        /// </summary>
        public static IServiceCollection AddDIServices(this IServiceCollection services, IConfiguration configuration)
        {
            // SQL context
            services.AddScoped<DbContext, SqlApplicationContext>();
            services.AddScoped<IUnitOfWork, SqlUnitOfWork>();
            services.AddScoped(typeof(IRepository<>), typeof(SqlRepository<>));
            services.AddScoped(typeof(IQueryRepository<>), typeof(SqlQueryRepository<>));
            string connectionString = configuration.GetConnectionString("SqlDbConnection") ?? throw new InvalidOperationException("Connection string 'SqlDbConnection' not found.");
            services.AddTransient<System.Data.IDbConnection>(db => new Microsoft.Data.SqlClient.SqlConnection(connectionString));
            services.AddDbContext<DbContext>(options => options.UseSqlServer(connectionString));

            // MongoDB context
            /*services.Configure<MongoDbSettings>(
                configuration.GetSection("MongoDBDatabase"));
            services.AddSingleton<IMongoDbSettings>(sp =>
                sp.GetRequiredService<IOptions<MongoDbSettings>>().Value);
            services.AddScoped<IMongoApplicationContext, MongoApplicationContext>();
            services.AddScoped<IUnitOfWork, MongoUnitOfWork>();
            services.AddScoped(typeof(IRepository<>), typeof(MongoRepository<>));
            string connectionString = configuration.GetConnectionString("SqlDbConnection") ?? throw new InvalidOperationException("Connection string 'SqlDbConnection' not found.");
            services.AddDbContext<DbContext>(options => options.UseSqlServer(connectionString));*/

            // Postgres context
            /*string connectionString = configuration.GetConnectionString("PostgresConnection");
            services.AddScoped<DbContext, PostgresApplicationContext>();
            services.AddScoped<IUnitOfWork, SqlUnitOfWork>();
            services.AddScoped(typeof(IRepository<>), typeof(SqlRepository<>));
            services.AddScoped(typeof(IQueryRepository<>), typeof(SqlQueryRepository<>));
            services.AddDbContext<DbContext>(options =>
                options.UseNpgsql(connectionString));
            services.AddTransient<IDbConnection>(db =>
                new NpgsqlConnection(connectionString));
            services.Configure<MongoDbSettings>(
                configuration.GetSection("MongoDBDatabase"));
            services.AddSingleton<IMongoDbSettings>(sp =>
                sp.GetRequiredService<IOptions<MongoDbSettings>>().Value);
            services.AddScoped<IMongoApplicationContext, MongoApplicationContext>();
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);*/

            // Register services
            services.AddScoped<IDocumentService, DocumentService>();
            services.AddScoped<IOrderService, OrderService>();

            // Register RabbitMQ
            services.AddScoped<IRabbitMQProducer, RabbitMQProducer>();

            // Register generic request handler of MediatR
            services.AddTransient<IRequestHandler<CreateDocumentCommand<Book>, Book>, CreateDocumentHandler<Book>>();
            services.AddTransient<IRequestHandler<CreateDocumentCommand<Magazine>, Magazine>, CreateDocumentHandler<Magazine>>();
            services.AddTransient<IRequestHandler<CreateDocumentCommand<Newspaper>, Newspaper>, CreateDocumentHandler<Newspaper>>();

            return services;
        }
    }
}
