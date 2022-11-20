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
    class ClearStrikesCommand : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Both;

        public string Name => "clearstrikes";

        public string Help => "";

        public string Syntax => "";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions
        {
            get { return new List<string>() { "strikesplugin.clearstrikes" }; }
        }

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UP player = UP.FromName(command[0]);
            var reason = string.Join(" ", command.Where(s => !string.IsNullOrEmpty(s) && s != command[0]));

            if (player != null)
            {
                String warnFolder = Rocket.Core.Environment.PluginsDirectory + "/StrikesPlugin/Databases/Warnings/";
                XDocument warnPass = XDocument.Load(warnFolder + "Warnings.xml");

                var Strikes = warnPass.Descendants("Warning").FirstOrDefault(el => (string)el.Attribute("player") == player.CSteamID.ToString()).Element("Strikes");

                Strikes.Value = "0";
                warnPass.Save(warnFolder + "Warnings.xml");

                caller.sendMessage($"[<color=red> Strike </color>] Successfully wiped {player.DisplayName} strikes");
            }
            else
            {
                caller.sendMessage($"[<color=red> Strike </color>] No player found");
            }
        }
    }
}
