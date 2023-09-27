namespace CommandAPI.Models
{
    public class CommandMessage
    {
        public string Command { get; set; }
        public int[] Params { get; set; }
        public string Result { get; set; }
    }
}
