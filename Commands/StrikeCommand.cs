using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Collections;
using System.Text;
using System.Threading.Tasks;
using Rocket.API;
using Rocket.Core;
using Rocket.Unturned;
using Rocket.Core.Logging;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Rocket.Core.Plugins;
using UP = Rocket.Unturned.Player.UnturnedPlayer;
using Rocket.API.Serialisation;
using Rocket.Unturned.Chat;
using SDG.Provider;
using Steamworks;
using UnityEngine;
using System.Globalization;
using System.Net;

namespace StrikesPlugin.Commands
{
    class StrikeCommand : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Both;

        public string Name => "strike";

        public string Help => "";

        public string Syntax => "";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions
        {
            get { return new List<string>() { "strikesplugin.strike" }; }
        }

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UP player = UP.FromName(command[0]);
            var reason = string.Join(" ", command.Where(s => !string.IsNullOrEmpty(s) && s != command[0]));

            if (player != null)
            {
                if (command.Length > 1)
                {
                    String warnFolder = Rocket.Core.Environment.PluginsDirectory + "/StrikesPlugin/Databases/Warnings/";
                    XDocument warnPass = XDocument.Load(warnFolder + "Warnings.xml");

                    var Strikes = warnPass.Descendants("Warning").FirstOrDefault(el => (string)el.Attribute("player") == player.CSteamID.ToString()).Element("Strikes");
                    var ConfStrike = StrikesPlugin.Instance.Configuration.Instance.StrikeSequence.Find(el => el.SequenceNumber == (int.Parse(Strikes.Value) + 1));

                    if (ConfStrike.Action.ToUpper() == "BAN")
                    {
                        player.Ban(ConfStrike.Reason, ConfStrike.BanTime);

                        if (StrikesPlugin.Instance.Configuration.Instance.AnnounceKickAndBanGlobally)
                        {
                            ChatSystem.sendGlobalMessage($"[<color=red> Strike </color>] {player.DisplayName} has been banned for reaching {ConfStrike.SequenceNumber} strikes");
                        }
                        else
                        {
                            caller.sendMessage($"[<color=red> Strike </color>] {player.DisplayName} has been banned for reaching {ConfStrike.SequenceNumber} strikes");
                        }
                    }

                    if (ConfStrike.Action.ToUpper() == "KICK")
                    {
                        player.Kick(ConfStrike.Reason);

                        if (StrikesPlugin.Instance.Configuration.Instance.AnnounceKickAndBanGlobally)
                        {
                            ChatSystem.sendGlobalMessage($"[<color=red> Strike </color>] {player.DisplayName} has been kicked for reaching {ConfStrike.SequenceNumber} strikes");
                        } else
                        {
                            caller.sendMessage($"[<color=red> Strike </color>] {player.DisplayName} has been kicked for reaching {ConfStrike.SequenceNumber} strikes");
                        }
                    }

                    if (ConfStrike.ClearWarnings)
                    {
                        Strikes.Value = "0";
                        warnPass.Save(warnFolder + "Warnings.xml");
                        caller.sendMessage($"[<color=red> Strike </color>] {player.DisplayName} has been issued another strike. This has wiped all previous");
                        caller.sendMessage($"[<color=red> Strike </color>] You have been striked for {reason}");
                        if (StrikesPlugin.Instance.Configuration.Instance.AnnounceStrikesGlobally) ChatSystem.sendGlobalMessage($"[<color=red> Strike </color>] {player.DisplayName} has been issued another strike for '{reason}'");
                    } else
                    {
                        Strikes.Value = (int.Parse(Strikes.Value) + 1).ToString();
                        warnPass.Save(warnFolder + "Warnings.xml");
                        caller.sendMessage($"[<color=red> Strike </color>] {player.DisplayName} has been issued another strike");
                        caller.sendMessage($"[<color=red> Strike </color>] You have been striked for {reason}");
                        if (StrikesPlugin.Instance.Configuration.Instance.AnnounceStrikesGlobally) ChatSystem.sendGlobalMessage($"[<color=red> Strike </color>] {player.DisplayName} has been issued another strike for '{reason}'");
                    }
                } else
                {
                    caller.sendMessage($"[<color=red> Strike </color>] No reason provided");
                }
            } else
            {
                caller.sendMessage($"[<color=red> Strike </color>] No user found");
            }
        }
    }
}
