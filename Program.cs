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

app.MapPost("/servicetickets", (ServiceTicket serviceTicket) => 
{
    // Get the Customer data to check that the CustomerId for the Service Ticket is valid
    Customer? customer = customers.FirstOrDefault(customer => customer.Id == serviceTicket.CustomerId);

    // If the client did not provide a valid CustomerId, this is a bad request
    if (customer == null)
    {
        return Results.BadRequest();
    }

    // Create a new Id (SQL will do this for us like JSON Server did!)
    serviceTicket.Id = serviceTickets.Max(ticket => ticket.Id) + 1;
    serviceTickets.Add(serviceTicket);

    // Created returns a 201 Status Code with a link in the headers to where the new resource can be accessed
    return Results.Created($"/servicetickets/{serviceTicket.Id}", new ServiceTicketDTO
    {
        Id = serviceTicket.Id,
        CustomerId = serviceTicket.CustomerId,
        Customer = new CustomerDTO
        {
            Id = customer.Id,
            Name = customer.Name,
            Address = customer.Address
        },
        Description = serviceTicket.Description,
        Emergency = serviceTicket.Emergency
    });
});

app.MapGet("/servicetickets/{id}", (int id) => 
{
    ServiceTicket? serviceTicket = serviceTickets.FirstOrDefault(ticket => ticket.Id == id);

    if (serviceTicket == null)
    {
        return Results.NotFound();
    }

    Employee? employee = employees.FirstOrDefault(employee => employee.Id == serviceTicket.EmployeeId);
    Customer? customer = customers.FirstOrDefault(customer => customer.Id == serviceTicket.CustomerId);

    return Results.Ok(new ServiceTicketDTO()
    {
        Id = serviceTicket.Id,
        CustomerId = serviceTicket.CustomerId,
        Customer = customer == null ? null : new CustomerDTO
        {
            Id = customer.Id,
            Name = customer.Name,
            Address = customer.Address
        },
        EmployeeId = serviceTicket.EmployeeId,
        Employee = employee == null ? null : new EmployeeDTO
        {
            Id = employee.Id,
            Name = employee.Name,
            Specialty = employee.Specialty
        },
        Description = serviceTicket.Description,
        Emergency = serviceTicket.Emergency,
        DateCompleted = serviceTicket.DateCompleted
    });
});

app.MapDelete("/servicetickets/{id}", (int id) =>
{
    ServiceTicket serviceTicketToDelete = serviceTickets.FirstOrDefault(serviceTicket => serviceTicket.Id == id);

    if (serviceTicketToDelete == null)
    {
        return Results.BadRequest();
    }

    serviceTickets.Remove(serviceTicketToDelete);

    return Results.NoContent();
});

app.MapPut("servicetickets/{id}", (int id, ServiceTicket serviceTicket) =>
{
    ServiceTicket? ticketToUpdate = serviceTickets.FirstOrDefault(ticket => ticket.Id == id);

    if (ticketToUpdate == null)
    {
        return Results.NotFound();
    }
    if (id != serviceTicket.Id)
    {
        return Results.BadRequest();
    }

    ticketToUpdate.CustomerId = serviceTicket.CustomerId;
    ticketToUpdate.EmployeeId = serviceTicket.EmployeeId;
    ticketToUpdate.Description = serviceTicket.Description;
    ticketToUpdate.Emergency = serviceTicket.Emergency;
    ticketToUpdate.DateCompleted = serviceTicket.DateCompleted;

    return Results.NoContent();
});

app.MapPut("servicetickets/{id}/complete", (int id) =>
{
    ServiceTicket? ticketToComplete = serviceTickets.FirstOrDefault(ticket => ticket.Id == id);
    if (ticketToComplete == null)
    {
        return Results.NotFound();
    }
    if (ticketToComplete.EmployeeId == null)
    {
        return Results.BadRequest();
    }
    if (ticketToComplete.DateCompleted != null)
    {
        return Results.BadRequest();
    }

    ticketToComplete.DateCompleted = DateTime.Today;

    return Results.NoContent();
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

app.MapGet("/employees/{id}", (int id) => 
{
    Employee? employee = employees.FirstOrDefault(employee => employee.Id == id);

    if (employee == null)
    {
        return Results.NotFound();
    }

    List<ServiceTicket> tickets = serviceTickets.Where(serviceTicket => serviceTicket.EmployeeId == id).ToList();

    return Results.Ok(new EmployeeDTO
    {
        Id = employee.Id,
        Name = employee.Name,
        Specialty = employee.Specialty,
        ServiceTickets = tickets.Select(ticket => new ServiceTicketDTO
        {
            Id = ticket.Id,
            CustomerId = ticket.CustomerId,
            EmployeeId = ticket.EmployeeId,
            Description = ticket.Description,
            Emergency = ticket.Emergency,
            DateCompleted = ticket.DateCompleted
        }).ToList()
    });
});

app.MapGet("/customers", () => 
{
    return customers.Select(customer => new CustomerDTO
    {
        Id = customer.Id,
        Name = customer.Name,
        Address = customer.Address
    });
});

app.MapGet("/customers/{id}", (int id) =>
{
    Customer? customer = customers.FirstOrDefault(customer => customer.Id == id);

    if (customer == null)
    {
        return Results.NotFound();
    }

    List<ServiceTicket> tickets = serviceTickets.Where(serviceTicket => serviceTicket.CustomerId == id).ToList();

    return Results.Ok(new CustomerDTO
    {
        Id = customer.Id,
        Name = customer.Name,
        Address = customer.Address,
        ServiceTickets = tickets.Select(ticket => new ServiceTicketDTO
        {
            Id = ticket.Id,
            CustomerId = ticket.CustomerId,
            EmployeeId = ticket.EmployeeId,
            Description = ticket.Description,
            Emergency = ticket.Emergency,
            DateCompleted = ticket.DateCompleted
        }).ToList()
    });
});

app.Run();

