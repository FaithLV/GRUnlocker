namespace GRUnlocker {
    class Program {

        static void Main(string[] args) {
            System.Console.Title = "Ghostrunner Unlocker";
            // load config if any
            Config.getInstance().Load(args);
            System.Console.WriteLine(Config.getInstance().saveLocation);

            // handle input
            InputHandler handler = new InputHandler();
            handler.Handle();
        }
    }
}