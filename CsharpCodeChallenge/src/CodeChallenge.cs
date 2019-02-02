using System;
using System.Threading.Tasks;

namespace code.challenge
{
    public class CodeChallenge
    {
	    volatile bool isRunning = true;
        
	    static void Main(string[] args)
        {
            var codeChallenge = new CodeChallenge();
            EventHandler.register(codeChallenge);
            
	        Task.Delay(1000).ContinueWith(t=> EventHandler.postEvent(EventType.Quit));

            while (codeChallenge.isRunning)
            {
                Console.WriteLine("Looping!");
            } 
            Console.WriteLine("Finishing!");
        }

        [Subscribe((EventType.Quit))]
        public void onQuitEvent()
        {
            isRunning = false;
        }
    }   
}
