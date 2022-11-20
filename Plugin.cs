using System;
using System.Timers;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.IO;
using System.Net;
using System.Xml;
using System.Xml.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;
using System.Globalization;
using Rocket.API;
using Rocket.Core;
using Rocket.Unturned;
using Rocket.Unturned.Events;
using Rocket.Unturned.Items;
using Rocket.Unturned.Player;
using Rocket.Unturned.Enumerations;
using Rocket.Unturned.Plugins;
using Rocket.Core.Logging;
using Rocket.API.Collections;
using Rocket.Core.Plugins;
using SDG.Unturned;
using Steamworks;
using SDG;
using UnityEngine;
using UnityEngine.Events;
using UP = Rocket.Unturned.Player.UnturnedPlayer;
using Rocket.API.Serialisation;
using Rocket.Unturned.Chat;
using SDG.Provider;
using Logger = Rocket.Core.Logging.Logger;

namespace StrikesPlugin
{
    public class StrikesPlugin : RocketPlugin<Configuration>
    {
        public static StrikesPlugin Instance;
        public string Creator = "XXFOGS";
        public string PluginName = "StrikesPlugin";
        public string Version = "1.0.0";

        protected override void Load()
        {
            Instance = this;

            String warnFolder = Rocket.Core.Environment.PluginsDirectory + "/StrikesPlugin/Databases/Warnings/";

            if (!System.IO.Directory.Exists(warnFolder))
            {
                System.IO.Directory.CreateDirectory(warnFolder);

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.IndentChars = ("    ");
                settings.CloseOutput = true;
                settings.OmitXmlDeclaration = true;

                using (XmlWriter writer = XmlWriter.Create(warnFolder + "Warnings.xml", settings))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("Warnings");
                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                    writer.Flush();
                }
            }

            U.Events.OnPlayerConnected += PlayerConnected;

            Logger.Log($"{PluginName} by {Creator} has been loaded! Version: {Version}");
        }

        protected override void Unload()
        {
            Instance = null;

            U.Events.OnPlayerConnected -= PlayerConnected;

            Logger.Log($"{PluginName} has been unloaded");
        }

        private void PlayerConnected(UnturnedPlayer player)
        {
            String warnFolder = Rocket.Core.Environment.PluginsDirectory + "/StrikesPlugin/Databases/Warnings/";
            XDocument warnPass = XDocument.Load(warnFolder + "Warnings.xml");

            var warnDocument = warnPass.Element("Warnings").Elements("Warning").Where(x => x.Element("Player")?.Value == player.CSteamID.ToString()).FirstOrDefault();

            if (warnDocument == null)
            {
                XDocument doc = XDocument.Load(warnFolder + "Warnings.xml");
                XElement root = new XElement("Warning", new XAttribute("player", player.CSteamID));
                root.Add(new XElement("Player", $"{player.CSteamID}"));
                root.Add(new XElement("Strikes", "0"));
                doc.Element("Warnings").Add(root);
                doc.Save(warnFolder + "Warnings.xml");
            }
        }
    }
}
