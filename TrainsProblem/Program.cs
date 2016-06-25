using StructureMap;
using StructureMap.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using TrainsProblem.DataService;
using TrainsProblem.Domain;
using TrainsProblem.Logging;

namespace TrainsProblem
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = new Container(c => c.Scan(scanner =>
            {
                scanner.TheCallingAssembly();
                scanner.WithDefaultConventions();
                scanner.AssemblyContainingType<IInMemoryTrainData>();
            }));
            var app = container.GetInstance<Application>();
            app.Run();
            Console.ReadLine();
        }


        public class Application
        {
            private readonly IConsoleLogger _consoleLogger;
            private readonly IInMemoryTrainData _inMemoryTrainData;

            public Application( IConsoleLogger consoleLogger, IInMemoryTrainData inMemoryTrainData)
            {
                _consoleLogger = consoleLogger;
                _inMemoryTrainData = inMemoryTrainData;
            }

            public void Run()
            {
                _consoleLogger.Info("Question 1 - The distance of the route A-B-C.");
                var getAnswerResult=GetDistanceByRoute("ABC");
                _consoleLogger.Info(getAnswerResult.IsSuccess ? getAnswerResult.LogMessage : getAnswerResult.Error);

                _consoleLogger.Info("Question 2 - The distance of the route A-D.");
                 getAnswerResult = GetDistanceByRoute("AD");
                _consoleLogger.Info(getAnswerResult.IsSuccess ? getAnswerResult.LogMessage : getAnswerResult.Error);

                _consoleLogger.Info("Question 3 - The distance of the route A-D-C.");
                getAnswerResult = GetDistanceByRoute("ADC");
                _consoleLogger.Info(getAnswerResult.IsSuccess ? getAnswerResult.LogMessage : getAnswerResult.Error);

                _consoleLogger.Info("Question 4 -The distance of the route A-E-B-C-D.");
                getAnswerResult = GetDistanceByRoute("AEBCD");
                _consoleLogger.Info(getAnswerResult.IsSuccess? getAnswerResult.LogMessage: getAnswerResult.Error);

                _consoleLogger.Info("Question 5 - The distance of the route A-E-D.");
                getAnswerResult = GetDistanceByRoute("AED");
                _consoleLogger.Info(getAnswerResult.IsSuccess ? getAnswerResult.LogMessage : getAnswerResult.Error);

                _consoleLogger.Info("Question 5 - The number of trips starting at C and ending at C with a maximum of 3 stops.");
                getAnswerResult =  _inMemoryTrainData.FindPath("C","C");
                _consoleLogger.Info(getAnswerResult.IsSuccess ? getAnswerResult.LogMessage : getAnswerResult.Error);
            }

            private Result GetDistanceByRoute(string route)
            {
                var routeMap = GetOriginAndDestination(route);
                var distance = 0;
                foreach (var keyValuePair in routeMap)
                {
                    var getRouteResult = _inMemoryTrainData.GetByOriginAndDestination(keyValuePair.Key);
                    if (getRouteResult.IsFailure)
                        return getRouteResult;
                   distance+= getRouteResult.Value.Weighting;
                }

                return Result.Ok($"Output #1: {distance}");
            }
            

            private List<KeyValuePair<string, int>> GetOriginAndDestination(string route)
            {
                var routeLegs = WholeChunks(route);
                var routes = new List<KeyValuePair<string, int>>();
                foreach (var routeLeg in routeLegs)
                {
                    routes.Add(new KeyValuePair<string, int>(routeLeg, 0));
                }

                return routes;
            }
            private IEnumerable<string> WholeChunks(string str)
            {
                for (int i = 0; i < str.Length-1; i += 1)
                    yield return str.Substring(i, 2);
            }
        }
        
    }
}
