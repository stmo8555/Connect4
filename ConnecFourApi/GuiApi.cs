using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConnectFourApi
{
    public class GuiApi
    {
        private readonly Communication _com = new Communication("GUI");

        public GuiApi(Action<int,int,string> onMoveReceived, Action<string> onWinReceived )
        {
            _com.MoveReceived += onMoveReceived;
            _com.WinReceived += onWinReceived;
        }
        
        public List<string> GetPlayers()
        {
            var task = GetPlayersAsync();
            task.Wait();
            return task.Result; 
        }
        
        public async Task<List<string>> GetPlayersAsync()
        {
            var responseTask = WaitForPlayersAsync();
            _com.PlayerRequest();
            return await responseTask;
        }
        
        private Task<List<string>> WaitForPlayersAsync()
        {
            var tcs = new TaskCompletionSource<List<string>>();

            _com.PlayersReceived += players =>
            {
                tcs.TrySetResult(players);
            };

            return tcs.Task;
        }
    }
}