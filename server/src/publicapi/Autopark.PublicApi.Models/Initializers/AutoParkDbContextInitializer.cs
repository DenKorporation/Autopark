using Autopark.PublicApi.Models.EventHistories;
using Autopark.PublicApi.Models.EventTypes;
using Autopark.PublicApi.Models.FuelTypes;
using Autopark.PublicApi.Models.Insurances;
using Autopark.PublicApi.Models.MaintenanceRecords;
using Autopark.PublicApi.Models.OdometerHistories;
using Autopark.PublicApi.Models.PartReplacements;
using Autopark.PublicApi.Models.Parts;
using Autopark.PublicApi.Models.Permissions;
using Autopark.PublicApi.Models.RefuelingHistories;
using Autopark.PublicApi.Models.Statuses;
using Autopark.PublicApi.Models.TechnicalPassports;
using Autopark.PublicApi.Models.Vehicles;
using Autopark.PublicApi.Models.VehicleStatusHistories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Autopark.PublicApi.Models.Initializers;

public class AutoParkDbContextInitializer(
    ILogger<AutoParkDbContextInitializer> logger,
    PublicApiDbContext context)
{
    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await context.Database.MigrateAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while initializing the database.");

            throw;
        }
    }

    public async Task SeedAsync(IList<Guid> groupIds, CancellationToken cancellationToken = default)
    {
        try
        {
            await TrySeedAsync(groupIds, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database.");

            throw;
        }
    }

    private static List<EventType> GetEventTypes(IList<Guid> groupIds)
    {
        return
        [
            new EventType
            {
                Id = Guid.NewGuid(),
                Name = "ДТП",
                GroupId = groupIds.First()
            }
        ];
    }

    private static List<Part> GetParts(IList<Guid> groupIds)
    {
        return
        [
            new Part
            {
                Id = Guid.NewGuid(),
                Name = "Фильтр масляный",
                Category = "Двигатель",
                Manufacturer = "Bosch",
                ServiceLife = 20000,
                GroupId = groupIds.First()
            }
        ];
    }

    private static List<Status> GetStatuses(IList<Guid> groupIds)
    {
        return
        [
            new Status
            {
                Id = Guid.NewGuid(),
                Name = "В эксплуатации",
                GroupId = groupIds.First()
            }
        ];
    }
    
    private static List<FuelType> GetFuelTypes(IList<Guid> groupIds)
    {
        return
        [
            new FuelType
            {
                Id = Guid.NewGuid(),
                Name = "Бензин",
                GroupId = groupIds.First()
            },
            new FuelType
            {
                Id = Guid.NewGuid(),
                Name = "Дизель",
                GroupId = groupIds.First()
            },
        ];
    }

    private static List<Vehicle> GetPreconfiguredVehicles(IList<Guid> groupIds, List<FuelType> fuelType)
    {
        return
        [
            new Vehicle
            {
                Id = Guid.NewGuid(),
                PurchaseDate = DateOnly.Parse("2023-01-01"),
                Cost = 70_000.00M,
                GroupId = groupIds.First(),
                FuelTypeId = fuelType.First().Id,
            },
        ];
    }

    private static List<TechnicalPassport> GetPreconfiguredTechnicalPassports(IList<Guid> groupIds, List<Vehicle> vehicles)
    {
        return
        [
            new TechnicalPassport
            {
                Id = Guid.NewGuid(),
                VehicleId = vehicles.First().Id,
                Number = "AAA128400",
                FirstName = "Иван",
                FirstNameLatin = "Ivan",
                LastName = "Иванов",
                LastNameLatin = "Ivanov",
                Patronymic = "Иванович",
                Address = "Минская обл., Советский район, г. Минск, ул. Гикало, д. 9",
                IssueDate = DateOnly.Parse("2023-01-01"),
                SAICode = "102-18",
                LicensePlate = "1234AA-7",
                Brand = "Audi",
                Model = "A6",
                CreationYear = 2022,
                Color = "СЕРЕБРИСТЫЙ МЕТАЛЛИК",
                VIN = "WAUZZZ4G7FN123456",
                VehicleType = "Легковой",
                MaxWeight = 1905,
                GroupId = groupIds.First()
            },
        ];
    }

    private static List<Insurance> GetPreconfiguredInsurances(IList<Guid> groupIds, List<Vehicle> vehicles)
    {
        return
        [
            new Insurance
            {
                Id = Guid.NewGuid(),
                VehicleId = vehicles.First().Id,
                Series = "MA",
                Number = "1234567",
                VehicleType = "A4",
                Provider = "ЗАСО \"Белнефтестрах\"",
                IssueDate = DateOnly.Parse("2024-01-01"),
                StartDate = DateOnly.Parse("2024-01-02"),
                EndDate = DateOnly.Parse("2025-01-02"),
                Cost = 30.00M,
                GroupId = groupIds.First()
            },
        ];
    }

    private static List<MaintenanceRecord> GetPreconfiguredMaintenanceRecords(IList<Guid> groupIds, List<Vehicle> vehicles)
    {
        return
        [
            new MaintenanceRecord
            {
                Id = Guid.NewGuid(),
                VehicleId = vehicles.First().Id,
                Description = "ТО-2",
                Type = "Плановое обслуживание",
                StartDate = DateOnly.Parse("2023-05-02"),
                EndDate = DateOnly.Parse("2023-05-04"),
                Odometer = 14_000,
                ServiceCenter = "Техцентр Авто",
                Cost = 120.00M,
                GroupId = groupIds.First()
            },
        ];
    }

    private static List<Permission> GetPreconfiguredPermissions(IList<Guid> groupIds, List<Vehicle> vehicles)
    {
        return
        [
            new Permission
            {
                Id = Guid.NewGuid(),
                VehicleId = vehicles.First().Id,
                Number = "AB1234567",
                ExpiryDate = DateOnly.Parse("2025-05-01"),
                GroupId = groupIds.First()
            },
        ];
    }

    private static List<VehicleStatusHistory> GetPreconfiguredVehicleStatusHistories(IList<Guid> groupIds, List<Vehicle> vehicles, List<Status> statuses)
    {
        return
        [
            new VehicleStatusHistory
            {
                Id = Guid.NewGuid(),
                VehicleId = vehicles.First().Id,
                StatusId = statuses.First().Id,
                Date = DateOnly.FromDateTime(DateTime.Now),
                GroupId = groupIds.First()
            },
        ];
    }

    private static List<OdometerHistory> GetPreconfiguredOdometerHistories(IList<Guid> groupIds, List<Vehicle> vehicles)
    {
        return
        [
            new OdometerHistory
            {
                Id = Guid.NewGuid(),
                VehicleId = vehicles.First().Id,
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(-10)),
                Amount = 30_000,
                GroupId = groupIds.First()
            },
        ];
    }
    
    private static List<PartReplacement> GetPreconfiguredPartReplacements(IList<Guid> groupIds, List<Vehicle> vehicles, List<Part> parts)
    {
        return
        [
            new PartReplacement
            {
                Id = Guid.NewGuid(),
                VehicleId = vehicles.First().Id,
                PartId = parts.First().Id,
                Date = DateOnly.FromDateTime(DateTime.Now.AddMonths(-2)),
                Odometer = 140000,
                GroupId = groupIds.First()
            },
        ];
    }

    private static List<EventHistory> GetPreconfiguredEventHistories(IList<Guid> groupIds, List<Vehicle> vehicles, List<EventType> eventTypes)
    {
        return
        [
            new EventHistory
            {
                Id = Guid.NewGuid(),
                VehicleId = vehicles.First().Id,
                EventTypeId = eventTypes.First().Id,
                Date = DateOnly.FromDateTime(DateTime.Now.AddMonths(-3)),
                Description = "Легкое столкновение на парковке",
                GroupId = groupIds.First()
            },
        ];
    }

    private static List<RefuelingHistory> GetPreconfiguredRefuelingHistories(IList<Guid> groupIds, List<Vehicle> vehicles)
    {
        return
        [
            new RefuelingHistory
            {
                Id = Guid.NewGuid(),
                VehicleId = vehicles.First().Id,
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(-5)),
                Amount = 80,
                TotalCost = 200,
                GroupId = groupIds.First()
            },
        ];
    }

    private async Task TrySeedAsync(IList<Guid> groupIds, CancellationToken cancellationToken = default)
    {
        var fuelTypes = GetFuelTypes(groupIds);
        if (!await context.FuelTypes.AnyAsync(cancellationToken))
        {
            await context.FuelTypes.AddRangeAsync(fuelTypes, cancellationToken);
        }
        
        var statuses = GetStatuses(groupIds);
        if (!await context.Statuses.AnyAsync(cancellationToken))
        {
            await context.Statuses.AddRangeAsync(statuses, cancellationToken);
        }
        
        var parts = GetParts(groupIds);
        if (!await context.Parts.AnyAsync(cancellationToken))
        {
            await context.Parts.AddRangeAsync(parts, cancellationToken);
        }
        
        var eventTypes = GetEventTypes(groupIds);
        if (!await context.EventTypes.AnyAsync(cancellationToken))
        {
            await context.EventTypes.AddRangeAsync(eventTypes, cancellationToken);
        }
        
        await context.SaveChangesAsync(cancellationToken);

        var vehicles = GetPreconfiguredVehicles(groupIds, fuelTypes);
        if (!await context.Vehicles.AnyAsync(cancellationToken))
        {
            await context.Vehicles.AddRangeAsync(vehicles, cancellationToken);
        }
        await context.SaveChangesAsync(cancellationToken);

        if (!await context.TechnicalPassports.AnyAsync(cancellationToken))
        {
            await context.TechnicalPassports.AddRangeAsync(GetPreconfiguredTechnicalPassports(groupIds, vehicles), cancellationToken);
        }

        if (!await context.Insurances.AnyAsync(cancellationToken))
        {
            await context.Insurances.AddRangeAsync(GetPreconfiguredInsurances(groupIds, vehicles), cancellationToken);
        }

        if (!await context.Permissions.AnyAsync(cancellationToken))
        {
            await context.Permissions.AddRangeAsync(GetPreconfiguredPermissions(groupIds, vehicles), cancellationToken);
        }
        
        if (!await context.VehicleStatusHistories.AnyAsync(cancellationToken))
        {
            await context.VehicleStatusHistories.AddRangeAsync(GetPreconfiguredVehicleStatusHistories(groupIds, vehicles, statuses), cancellationToken);
        }

        if (!await context.OdometerHistories.AnyAsync(cancellationToken))
        {
            await context.OdometerHistories.AddRangeAsync(GetPreconfiguredOdometerHistories(groupIds, vehicles), cancellationToken);
        }
        
        if (!await context.MaintenanceRecords.AnyAsync(cancellationToken))
        {
            await context.MaintenanceRecords.AddRangeAsync(GetPreconfiguredMaintenanceRecords(groupIds, vehicles), cancellationToken);
        }

        if (!await context.PartReplacements.AnyAsync(cancellationToken))
        {
            await context.PartReplacements.AddRangeAsync(GetPreconfiguredPartReplacements(groupIds, vehicles, parts), cancellationToken);
        }
        
        if (!await context.EventHistories.AnyAsync(cancellationToken))
        {
            await context.EventHistories.AddRangeAsync(GetPreconfiguredEventHistories(groupIds, vehicles, eventTypes), cancellationToken);
        }
        
        if (!await context.RefuelingHistories.AnyAsync(cancellationToken))
        {
            await context.RefuelingHistories.AddRangeAsync(GetPreconfiguredRefuelingHistories(groupIds, vehicles), cancellationToken);
        }
        await context.SaveChangesAsync(cancellationToken);
    }
}
