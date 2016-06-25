using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using System.Linq;
using System.Reflection;
using QuickGraph;
using QuickGraph.Data;
using TrainsProblem.Domain;

namespace TrainsProblem.DataService
{
    public interface IInMemoryTrainData
    {
        Result<Route> GetById(int id);
        Result<List<Route>> GetByOrigin(char origin);
        Result<Route> GetByOriginAndDestination(string route);
        Result FindPath(string origin, string destination);
    }

    public class InMemoryTrainData : IInMemoryTrainData
    {
        private static List<Route> _routes;
        private static AdjacencyGraph<string, Edge<string>> _graph;
        private static Dictionary<Edge<string>, double> _edgeCost;

        public InMemoryTrainData()
        {
            _routes=new List<Route>
            {
                new Route {Id=1,Origin='A',Destination = 'B', Weighting = 5},
                new Route {Id=2,Origin='B',Destination = 'C', Weighting = 4},
                new Route {Id=3,Origin='C',Destination = 'D', Weighting = 8},
                new Route {Id=4,Origin='D',Destination = 'C', Weighting = 8},
                new Route {Id=5,Origin='D',Destination = 'E', Weighting = 6},
                new Route {Id=6,Origin='A',Destination = 'D', Weighting = 5},
                new Route {Id=7,Origin='C',Destination = 'E', Weighting = 2},
                new Route {Id=8,Origin='E',Destination = 'B', Weighting = 3},
                new Route {Id=9,Origin='A',Destination = 'E', Weighting = 7}
            };
            _graph = new AdjacencyGraph<string, Edge<string>>(true);
            _graph.AddVertex("A");
            _graph.AddVertex("B");
            _graph.AddVertex("C");
            _graph.AddVertex("D");
            _graph.AddVertex("E");

            Edge<string> AB=new Edge<string>("A","B");
            Edge<string> BC = new Edge<string>("B", "C");
            Edge<string> CD = new Edge<string>("C", "D");
            Edge<string> DC = new Edge<string>("D", "C");
            Edge<string> DE = new Edge<string>("D", "E");
            Edge<string> AD = new Edge<string>("A", "D");
            Edge<string> CE = new Edge<string>("C", "E");
            Edge<string> EB = new Edge<string>("E", "B");
            Edge<string> AE = new Edge<string>("A", "E");

            _graph.AddEdge(AB);
            _graph.AddEdge(BC);
            _graph.AddEdge(CD);
            _graph.AddEdge(DC);
            _graph.AddEdge(DE);
            _graph.AddEdge(AD);
            _graph.AddEdge(CE);
            _graph.AddEdge(EB);
            _graph.AddEdge(AE);

            _edgeCost = new Dictionary<Edge<string>, double>(_graph.EdgeCount)
            {
                {AB, 5},
                {BC, 4},
                {CD, 8},
                {DC, 8},
                {DE, 6},
                {AD, 5},
                {CE, 2},
                {EB, 3},
                {AE, 7}
            };

        }
        private static DataTable ToDataTable<T>(IList<T> routes)
        {
            DataTable dt = new DataTable("DataTable");
            Type t = typeof(T);
            PropertyInfo[] pia = t.GetProperties();

            //Inspect the properties and create the columns in the DataTable
            foreach (PropertyInfo pi in pia)
            {
                Type ColumnType = pi.PropertyType;
                if ((ColumnType.IsGenericType))
                {
                    ColumnType = ColumnType.GetGenericArguments()[0];
                }
                dt.Columns.Add(pi.Name, ColumnType);
            }

            //Populate the data table
            foreach (T route in routes)
            {
                DataRow dr = dt.NewRow();
                dr.BeginEdit();
                foreach (PropertyInfo pi in pia)
                {
                    if (pi.GetValue(route, null) != null)
                    {
                        dr[pi.Name] = pi.GetValue(route, null);
                    }
                }
                dr.EndEdit();
                dt.Rows.Add(dr);
            }
            return dt;
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
                return Result.Fail<Route>("NO SUCH ROUTE");
            }
        }

        public Result FindPath(string origin, string destination)
        {
            try
            {


            }
            catch (Exception)
            {
                return Result.Fail<Route>("NO SUCH ROUTE");
            }
        }
    }
}