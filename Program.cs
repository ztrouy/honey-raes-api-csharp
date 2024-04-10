using HoneyRaesAPI.Models;
using HoneyRaesAPI.Models.DTOs;
List<Customer> customers = new List<Customer>()
{
    new Customer()
    {
        Id = 1,
        Name = "Zavier",
        Address = "123 Loopy Lane"
    },
    new Customer()
    {
        Id = 2,
        Name = "Chad",
        Address = "345 Birch Boulevard"
    },
    new Customer()
    {
        Id = 3,
        Name = "Ezra",
        Address = "678 Colander Cove"
    }
};
List<Employee> employees = new List<Employee>()
{
    new Employee()
    {
        Id = 1,
        Name = "Zachary",
        Specialty = "PCs and Phones"
    },
    new Employee()
    {
        Id = 2,
        Name = "Lincoln",
        Specialty = "Instruments"
    }
};
List<ServiceTicket> serviceTickets = new List<ServiceTicket>()
{
    new ServiceTicket()
    {
        Id = 1,
        CustomerId = 1,
        EmployeeId = 1,
        Description = "Laptop battery is bulging",
        Emergency = true,
        DateCompleted = new DateTime(2024, 03, 19)
    },
    new ServiceTicket()
    {
        Id = 2,
        CustomerId = 1,
        EmployeeId = 2,
        Description = "Keyboard keys inconsistent",
        Emergency = false,
    },
    new ServiceTicket()
    {
        Id = 3,
        CustomerId = 2,
        EmployeeId = 2,
        Description = "Guitar strings won't stay taut",
        Emergency = false,
        DateCompleted = new DateTime(2024, 04, 08)
    },
    new ServiceTicket()
    {
        Id = 4,
        CustomerId = 3,
        Description = "Computer suddenly running too slow, can't do coursework",
        Emergency = true,
    },
    new ServiceTicket()
    {
        Id = 5,
        CustomerId = 2,
        Description = "RGB not working",
        Emergency = false
    }
};

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/servicetickets", () =>
{
    return serviceTickets.Select(ticket => new ServiceTicketDTO
    {
        Id = ticket.Id,
        CustomerId = ticket.CustomerId,
        EmployeeId = ticket.EmployeeId,
        Description = ticket.Description,
        Emergency = ticket.Emergency,
        DateCompleted = ticket.DateCompleted
    });
});

app.MapGet("/servicetickets/{id}", (int id) => 
{
    ServiceTicket serviceTicket = serviceTickets.FirstOrDefault(ticket => ticket.Id == id);

    return new ServiceTicketDTO()
    {
        Id = serviceTicket.Id,
        CustomerId = serviceTicket.CustomerId,
        EmployeeId = serviceTicket.EmployeeId,
        Description = serviceTicket.Description,
        Emergency = serviceTicket.Emergency,
        DateCompleted = serviceTicket.DateCompleted
    };
});

app.MapGet("/employees", () => 
{
    return employees.Select(employee => new EmployeeDTO
    {
        Id = employee.Id,
        Name = employee.Name,
        Specialty = employee.Specialty
    });
});

app.Run();

