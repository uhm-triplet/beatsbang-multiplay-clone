using BeatsBang.Core;
using BeatsBang.Core.BeatsBang.Manager;
using System.Collections.Generic;
using System.Threading.Tasks;

public class GameLobbyManager : Singleton<GameLobbyManager>
{
    // Start is called before the first frame update
    public async Task<bool> CreateLobby()
    {
        Dictionary<string, string> playerData = new Dictionary<string, string>()
        {
            {"GameTag", "HostPlayer"}
        };
        bool succeeded = await LobbyManager.Instance.CreateLobby(2, true, playerData);
        return succeeded;
    }

    public string GetLobbyCode()
    {
        return LobbyManager.Instance.GetLobbyCode();
    }
}
