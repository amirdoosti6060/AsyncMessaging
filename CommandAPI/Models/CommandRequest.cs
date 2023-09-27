namespace CommandAPI.Models
{
    public class CommandRequest
    {
        public string Command { get; set; }
        public int[] Params { get; set; }
    }
}
