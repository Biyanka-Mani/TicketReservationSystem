using TicketReservationDataAccess.Data.Dtos;
using TicketReservationDataAccess.Data.Entites;

namespace TicketReservationDataAccess.Data.Services
{
    public interface IRouteTerminalServices
    {
        Task<bool> AddDestination(destinationDto destinationDto);
        Task<bool> AddRoute(RouteDto routeDto);
        Task<bool> AddTerminal(TerminalDto terminalDto);
        Task<bool> AddVehicle(VehicleDto vehicledto);
        Task<bool> DeleteRoute(int RouteId);
        Task<bool> DeleteRoutes(List<RouteDto> routeDtos);
        Task<bool> DeleteTerminal(int TerminalId);
        Task<bool> EditRoute(int RouteId, RouteDto modifiedRoute);
        Task<bool> EditTerminal(int terminalId, TerminalDto terminalDto);
        Task<List<RouteDto>> GetAllAvaliableRoutes();
        Task<List<TerminalDto>> getallAvaliabletermainls();
        Task<List<destinationDto>> GetAllDestinations();
        Task<List<RouteDto>> GetAllroutes();
        Task<List<TerminalDto>> GetAlltermainls();
        Task<List<VehicleDto>> GetAllVehicles();
        Task<int> GetCapacity(int VehicleId);
        Task<RouteDto> GetRoute(int Id);
        Route GetRouteById(int id);
        Task<int> GetRouteIdByLocation(int startingFrom, int goingTo);
        Task<TerminalDto> GetTerminal(int Id);
        List<int> GetUniqueRoutesForSchedule(int scheduleId);
        Task<bool> RemoveDestination(int id);
        Task<bool> RemoveVehicle(int id);
        RouteModelDto RouteModel(int id);
        Task<FareRequsetDto> Routes();
    }
}