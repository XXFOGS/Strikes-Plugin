## StrikesPlugin

**Description:**

Strike plugin punishing users who reach too many strikes

**Features:**

- Kicks players reaching a set amount of strikes
- Bans players reaching a set amount of strikes
- Ability to clear strikes
- Ability to view your strikes
- Ability to issue strikes
- Clears strikes after reaching a set amount of them
- Announces strikes globally (customizable)
- Announces only bans and kicks globally (customizable)

**Commands:**

- /clearstrikes <player>
- /strikes <player>
- /strike <player> <reason>

**Permissions:**

- strikesplugin.clearstrikes - Permission to clear users strikes
- strikesplugin.strike - Permission to strike a user
- strikesplugin.strikes - Permission to view users strikes

**Configuration:**

1. Plugin must be added to the server
2. According to ones preference all fields can be changed
3. Done

If you wish to wipe all strikes, navigate to `/StrikesPlugin/Databases/Warnings` and delete `Warnings.xml`. This plugin uses a dependancy System.Xml.Linq, make sure to install it, it can be found under releases. Feel free to download this plugin and edit it as you please.