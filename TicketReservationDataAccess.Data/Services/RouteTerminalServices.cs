using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketReservationDataAccess.Data.Core;
using TicketReservationDataAccess.Data.Dtos;
using TicketReservationDataAccess.Data.Entites;
using TicketReservationDataAccess.Data.Entites.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TicketReservationDataAccess.Data.Services
{
    public class RouteTerminalServices : IRouteTerminalServices
    {
        private readonly TicketReservationDBContext _reservationDBContext;
        private readonly IDataMappingAndAssociationHandler Services;

        public RouteTerminalServices(TicketReservationDBContext reservationDBContext, IDataMappingAndAssociationHandler _Services)
        {
            _reservationDBContext = reservationDBContext;
            Services = _Services;
        }
        public async Task<List<TerminalDto>> getallAvaliabletermainls()
        {
            List<Terminal> terminals = _reservationDBContext.Terminals.Where(r => r.TerminalStatus == true).ToList();
            List<TerminalDto> result = new List<TerminalDto>();
            foreach (var terminal in terminals)
            {
                TerminalDto terminaldto = Services.MapTermianlEntityToDto(terminal);
                result.Add(terminaldto);
            }
            return result;

        }
        public async Task<TerminalDto> GetTerminal(int Id)
        {
            if (Id != 0)
            {
                var terminal = _reservationDBContext.Terminals.FirstOrDefault(u => u.Id == Id);
                TerminalDto terminaldto = Services.MapTermianlEntityToDto(terminal);
                return terminaldto;
            }
            return new TerminalDto();
        }
        public async Task<List<TerminalDto>> GetAlltermainls()
        {
            List<Terminal> terminals = _reservationDBContext.Terminals.ToList();
            List<TerminalDto> result = new List<TerminalDto>();
            foreach (var terminal in terminals)
            {
                TerminalDto terminaldto = Services.MapTermianlEntityToDto(terminal);
                result.Add(terminaldto);
            }
            return result;

        }
        public async Task<bool> AddTerminal(TerminalDto terminalDto)
        {
            // Validate or perform any necessary checks on the input data
            //mention addind Enum Is 1 2 3
            var newTerminal = new Terminal
            {
                TerminalName = terminalDto.TerminalName,
                TerminalStatus = terminalDto.TerminalStatus,
                Location = terminalDto.Location,

            };
            _reservationDBContext.Terminals.Add(newTerminal);
            try
            {
                await _reservationDBContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<bool> EditTerminal(int terminalId, TerminalDto terminalDto)
        {
            try
            {
                var existingTerminal = _reservationDBContext.Terminals.FirstOrDefault(t => t.Id == terminalId);
                if (existingTerminal != null)
                {
                    existingTerminal.TerminalName = terminalDto.TerminalName;
                    existingTerminal.Location = terminalDto.Location;
                    existingTerminal.TerminalStatus = terminalDto.TerminalStatus;
                    _reservationDBContext.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        public async Task<bool> DeleteTerminal(int TerminalId)
        {
            try
            {
                var terminal = await _reservationDBContext.Terminals.FindAsync(TerminalId);
                if (terminal != null)
                {
                    _reservationDBContext.Terminals.Remove(terminal);
                    await _reservationDBContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }



        public Route GetRouteById(int id)
        {
            return _reservationDBContext.Routes.FirstOrDefault(x => x.Id == id);

        }
        public RouteModelDto RouteModel(int id)
        {
            Route route = _reservationDBContext.Routes.FirstOrDefault(x => x.Id == id);
            return new RouteModelDto()
            {
                RouteId = route.Id,
                RouteName = route.RouteName,
                fare = route.BaseFare
            };

        }
        public async Task<bool> AddRoute(RouteDto routeDto)
        {
            // Validate or perform any necessary checks on the input data
            //check entered Termainal Exist or not
            var newRoute = new Route
            {
                RouteName = routeDto.RouteName,
                DepatureDestinationId = routeDto.DepartureDestinationId,
                ArrivalDestinationId = routeDto.ArrivalDestinationId,
                BaseFare = routeDto.BaseFare,
                RouteStatus = routeDto.RouteStatus,
                DepartureTerminalId = routeDto.DepartureTerminalId,
                ArrivalTerminalId = routeDto.ArrivalTerminalId
            };
            _reservationDBContext.Routes.Add(newRoute);
            try
            {
                await _reservationDBContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }


        }
        public async Task<bool> EditRoute(int RouteId, RouteDto modifiedRoute)
        {
            try
            {
                var existingRoute = _reservationDBContext.Routes.Find(RouteId);
                if (existingRoute != null)
                {
                    existingRoute.RouteName = modifiedRoute.RouteName;
                    existingRoute.DepatureDestinationId = modifiedRoute.DepartureDestinationId;
                    existingRoute.ArrivalDestinationId = modifiedRoute.ArrivalDestinationId;
                    existingRoute.BaseFare = modifiedRoute.BaseFare;
                    existingRoute.RouteStatus = modifiedRoute.RouteStatus;
                    existingRoute.DepartureTerminalId = modifiedRoute.DepartureTerminalId;
                    existingRoute.ArrivalTerminalId = modifiedRoute.ArrivalTerminalId;
                    _reservationDBContext.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<RouteDto> GetRoute(int Id)
        {
            var route = _reservationDBContext.Routes.FirstOrDefault(u => u.Id == Id);
            RouteDto routedto = Services.MapRouteEntityToDto(route);
            return routedto;
        }
        public async Task<List<RouteDto>> GetAllroutes()
        {
            List<Route> routes = _reservationDBContext.Routes.ToList();
            List<RouteDto> result = new List<RouteDto>();
            foreach (var route in routes)
            {
                RouteDto routedto = Services.MapRouteEntityToDto(route);
                routedto.terminals = await GetAlltermainls();
                routedto.destinations = await GetAllDestinations();
                result.Add(routedto);

            }
            return result;

        }
        public async Task<FareRequsetDto> Routes()
        {
            List<Route> routes = _reservationDBContext.Routes.ToList();
            FareRequsetDto fareRequsetDto = new FareRequsetDto();
            fareRequsetDto.routes = routes.Select(route => new RouteModelDto
            {
                RouteId = route.Id,
                RouteName = route.RouteName
            }).ToList();


            return fareRequsetDto;
        }
        public async Task<List<RouteDto>> GetAllAvaliableRoutes()
        {
            List<Route> routes = _reservationDBContext.Routes.Where(r => r.RouteStatus == true).ToList();
            List<RouteDto> result = new List<RouteDto>();
            foreach (var route in routes)
            {
                RouteDto routedto = Services.MapRouteEntityToDto(route);
                result.Add(routedto);
            }
            return result;
        }
        public async Task<int> GetRouteIdByLocation(int startingFrom, int goingTo)
        {
            Route route = _reservationDBContext.Routes
                .FirstOrDefault(r => r.DepatureDestinationId == startingFrom && r.ArrivalDestinationId == goingTo);
            if (route == null)
            {
                return 0;
            }
            return route.Id;
        }
        public async Task<bool> DeleteRoute(int RouteId)
        {

            try
            {
                var route = await _reservationDBContext.Routes.FindAsync(RouteId);
                if (route != null)
                {
                    _reservationDBContext.Routes.Remove(route);
                    await _reservationDBContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> DeleteRoutes(List<RouteDto> routeDtos)
        {

            try
            {
                foreach (RouteDto routeDto in routeDtos)
                {
                    var route = await _reservationDBContext.Routes.FindAsync(routeDto.id);
                    if (route != null)
                    {
                        _reservationDBContext.Routes.Remove(route);
                        await _reservationDBContext.SaveChangesAsync();
                        return true;
                    }
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public List<int> GetUniqueRoutesForSchedule(int scheduleId)
        {
            try
            {

                var routes = _reservationDBContext.Schedules
                    .Where(s => s.Id == scheduleId)
                    .Select(s => s.RouteId)
                    .Distinct()
                    .ToList();

                return routes;
            }
            catch (Exception ex)
            {
                return new List<int>();
            }
        }




        public async Task<int> GetCapacity(int VehicleId)
        {
            var Vehicle = _reservationDBContext.Vehicles.FirstOrDefault(x => x.Id == VehicleId);
            return Vehicle.Capacity;
        }
        public async Task<List<VehicleDto>> GetAllVehicles()
        {
            List<VehicleDto> result = _reservationDBContext.Vehicles
                .Select(vehicle => new VehicleDto
                {
                    Id = vehicle.Id,
                    VehicleName = vehicle.VehicleName,
                    Capacity = vehicle.Capacity,
                    Description = vehicle.Description
                })
                .ToList();
            return result;
        }
        public async Task<bool> RemoveVehicle(int id)
        {
            try
            {
                var vehicle = await _reservationDBContext.Vehicles.FindAsync(id);
                if (vehicle != null)
                {
                    _reservationDBContext.Vehicles.Remove(vehicle);
                    await _reservationDBContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> AddVehicle(VehicleDto vehicledto)
        {
            try
            {
                var vehicle = new Vehicle
                {
                    Id = vehicledto.Id,
                    VehicleName = vehicledto.VehicleName,
                    Capacity = vehicledto.Capacity,
                    Description = vehicledto.Description
                };
                _reservationDBContext.Vehicles.Add(vehicle);
                await _reservationDBContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<List<destinationDto>> GetAllDestinations()
        {
            List<destinationDto> destinationDtos = _reservationDBContext.Destinations
                .Select(dest => new destinationDto
                {
                    Id = dest.Id,
                    Destination_name = dest.Destination_name
                })
                .ToList();
            return destinationDtos;
        }
        public async Task<bool> RemoveDestination(int id)
        {
            try
            {
                var destination = await _reservationDBContext.Destinations.FindAsync(id);
                if (destination != null)
                {
                    _reservationDBContext.Destinations.Remove(destination);
                    await _reservationDBContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> AddDestination(destinationDto destinationDto)
        {
            try
            {
                var destination = new Destination
                {
                    Id = destinationDto.Id,
                    Destination_name = destinationDto.Destination_name,
                };
                _reservationDBContext.Destinations.Add(destination);
                await _reservationDBContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
