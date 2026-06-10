using System.Collections.Generic;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace MarriageOverhaul
{
    public partial class ModEntry
    {
        // ── Multiplayer persistence ───────────────────────────────
        // Only the host can read/write save data, so the host owns every player's ModData in one
        // dictionary keyed by multiplayer ID. Each player (host + farmhands) runs their own marriage
        // logic locally against Game1.player; farmhands sync their data to the host over messages.
        private const string SaveKeyAll = "players-data";
        private const string SaveKeyLegacy = "main-data";   // pre-multiplayer single-player blob
        private const string MsgRequestData = "RequestData";
        private const string MsgProvideData = "ProvideData";
        private const string MsgUpdateData = "UpdateData";

        /// <summary>Host-only authoritative store of every player's data (null on farmhands).</summary>
        private Dictionary<long, ModData> AllData;

        /// <summary>True once this machine's data is authoritative: always for the host, and for a farmhand once the host has sent its saved copy. Gates per-day logic so we don't act on (or persist) placeholder data.</summary>
        private bool dataReady;

        /// <summary>Load this player's data. Host reads from the save (migrating the legacy blob); farmhands request theirs from the host.</summary>
        private void Multiplayer_OnSaveLoaded()
        {
            if (Context.IsMainPlayer)
            {
                this.AllData = this.Helper.Data.ReadSaveData<Dictionary<long, ModData>>(SaveKeyAll);
                if (this.AllData == null)
                {
                    this.AllData = new Dictionary<long, ModData>();

                    // Migrate an existing single-player save (stored as one ModData under the old key).
                    ModData legacy = this.Helper.Data.ReadSaveData<ModData>(SaveKeyLegacy);
                    if (legacy != null)
                        this.AllData[Game1.player.UniqueMultiplayerID] = legacy;
                }

                this.Data = this.GetOrCreateData(Game1.player.UniqueMultiplayerID);
                this.dataReady = true;
            }
            else
            {
                // Farmhand: can't touch save data. Start fresh, then ask the host for our saved copy.
                this.Data = new ModData();
                this.dataReady = false;
                this.Helper.Multiplayer.SendMessage(
                    true, MsgRequestData,
                    modIDs: new[] { this.ModManifest.UniqueID });
            }
        }

        /// <summary>Persist all data. Host writes the dictionary; farmhands have already pushed theirs each night.</summary>
        private void Multiplayer_OnSaving()
        {
            if (!Context.IsMainPlayer)
                return; // farmhands persist via the host (see SendFarmhandDataToHost on day end)

            if (this.AllData == null)
                this.AllData = new Dictionary<long, ModData>();
            this.AllData[Game1.player.UniqueMultiplayerID] = this.Data;
            this.Helper.Data.WriteSaveData(SaveKeyAll, this.AllData);
        }

        /// <summary>Farmhands send their latest data to the host to be persisted (called at day end).</summary>
        private void SendFarmhandDataToHost()
        {
            // Don't push placeholder data: it would overwrite the player's real saved progress on the host.
            if (Context.IsMainPlayer || this.Data == null || !this.dataReady)
                return;
            this.Helper.Multiplayer.SendMessage(
                this.Data, MsgUpdateData,
                modIDs: new[] { this.ModManifest.UniqueID });
        }

        private void OnModMessageReceived(object sender, ModMessageReceivedEventArgs e)
        {
            if (e.FromModID != this.ModManifest.UniqueID)
                return;

            switch (e.Type)
            {
                // Host: a farmhand wants their saved data — send their slice back.
                case MsgRequestData when Context.IsMainPlayer:
                {
                    ModData d = this.GetOrCreateData(e.FromPlayerID);
                    this.Helper.Multiplayer.SendMessage(
                        d, MsgProvideData,
                        modIDs: new[] { this.ModManifest.UniqueID },
                        playerIDs: new[] { e.FromPlayerID });
                    break;
                }

                // Farmhand: the host sent our saved data.
                case MsgProvideData when !Context.IsMainPlayer:
                    this.Data = e.ReadAs<ModData>() ?? new ModData();
                    this.dataReady = true;
                    break;

                // Host: a farmhand pushed their latest data to persist.
                case MsgUpdateData when Context.IsMainPlayer:
                    if (this.AllData == null)
                        this.AllData = new Dictionary<long, ModData>();
                    this.AllData[e.FromPlayerID] = e.ReadAs<ModData>() ?? new ModData();
                    break;
            }
        }

        /// <summary>Host helper: fetch (or create) the stored data for a player ID.</summary>
        private ModData GetOrCreateData(long playerId)
        {
            if (this.AllData == null)
                this.AllData = new Dictionary<long, ModData>();
            if (!this.AllData.TryGetValue(playerId, out ModData d) || d == null)
            {
                d = new ModData();
                this.AllData[playerId] = d;
            }
            return d;
        }
    }
}
