namespace TrainsProblem.Domain
{
    public class Route
    {
        public int Id { get; set; }
        public char Origin { get; set; }
        public char Destination { get; set; }
        public int Weighting { get; set; }
    }
}