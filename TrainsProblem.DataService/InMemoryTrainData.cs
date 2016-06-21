using System;
using System.Collections.Generic;
using System.Linq;
using TrainsProblem.Domain;

namespace TrainsProblem.DataService
{
    public interface IInMemoryTrainData
    {
        Result<Route> GetById(int id);
        Result<List<Route>> GetByOrigin(char origin);
        Result<Route> GetByOriginAndDestination(string route);
    }

    public class InMemoryTrainData : IInMemoryTrainData
    {
        private static List<Route> _routes;

        public InMemoryTrainData()
        {
            _routes=new List<Route>
            {
                new Route {Id=1,Origin='A',Destination = 'B', Weighting = 5},
                new Route {Id=1,Origin='B',Destination = 'C', Weighting = 4},
                new Route {Id=1,Origin='C',Destination = 'D', Weighting = 8},
                new Route {Id=1,Origin='D',Destination = 'C', Weighting = 8},
                new Route {Id=1,Origin='D',Destination = 'E', Weighting = 6},
                new Route {Id=1,Origin='A',Destination = 'D', Weighting = 5},
                new Route {Id=1,Origin='C',Destination = 'E', Weighting = 2},
                new Route {Id=1,Origin='E',Destination = 'B', Weighting = 3},
                new Route {Id=1,Origin='A',Destination = 'E', Weighting = 7}
            };
        }
        public Result<Route> GetById(int id)
        {
            try
            {
                return Result.Ok(_routes.Single(x => x.Id == id));
            }
            catch (Exception ex)
            {
                return Result.Fail($"Could not get route with id {id}", ex.Message) as Result<Route>;
            }
        }

        public Result<List<Route>> GetByOrigin(char origin)
        {
            try
            {
                return Result.Ok(_routes.Where(x => x.Origin == origin).ToList());
            }
            catch (Exception ex)
            {
                return Result.Fail($"Could not get route with origin '{origin}'", ex.Message) as Result<List<Route>>;
            }
        }

        public Result<Route> GetByOriginAndDestination(string route)
        {
            try
            {
                return Result.Ok(_routes.Single(x => x.Origin == route[0] && x.Destination == route[1]));
            }
            catch (Exception)
            {
                var result = Result.Fail("NO SUCH ROUTE") as Result<Route>;
                return result;
            }
        }
    }
}