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
    class StrikesCommand : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "strikes";

        public string Help => "";

        public string Syntax => "";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions
        {
            get { return new List<string>() { "strikesplugin.strikes" }; }
        }

        public void Execute(IRocketPlayer caller, string[] command)
        {
            var reason = string.Join(" ", command.Where(s => !string.IsNullOrEmpty(s) && s != command[0]));

            if (command.Length > 0)
            {
                UP player = UP.FromName(command[0]);

                String warnFolder = Rocket.Core.Environment.PluginsDirectory + "/StrikesPlugin/Databases/Warnings/";
                XDocument warnPass = XDocument.Load(warnFolder + "Warnings.xml");

                var warnDocument = warnPass.Element("Warnings").Elements("Warning").Where(x => x.Element("Player")?.Value == player.CSteamID.ToString()).FirstOrDefault();

                if (warnDocument != null)
                {
                    caller.sendMessage($"[<color=red> Strike </color>] {player.DisplayName} has {warnDocument.Element("Strikes").Value} strike(s)");
                }
            } else
            {
                UP player = (UP)caller;

                String warnFolder = Rocket.Core.Environment.PluginsDirectory + "/StrikesPlugin/Databases/Warnings/";
                XDocument warnPass = XDocument.Load(warnFolder + "Warnings.xml");

                var warnDocument = warnPass.Element("Warnings").Elements("Warning").Where(x => x.Element("Player")?.Value == player.CSteamID.ToString()).FirstOrDefault();

                if (warnDocument != null)
                {
                    caller.sendMessage($"[<color=red> Strike </color>] You have {warnDocument.Element("Strikes").Value} strike(s)");
                }
            }
        }
    }
}
