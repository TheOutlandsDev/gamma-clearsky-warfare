using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace GammaCoop {
    public class Settings {
        public string Root = "";
        public string Player = "CoopPlayer";
        public string Host = "";
        public int Port = 27015;
        public string ExpectedFingerprint = "";
    }
    public class Install {
        public int schema;
        public string engine;
        public string profile;
        public string mod;
        public string original_root;
        public List<InstalledFile> verified_files;
    }
    public class InstalledFile { public string file; public string sha256; }
    public class VpnStatus { public string BackendState; public string[] TailscaleIPs; }
    public class BackupFile { public string file; public string sha256; public long size; }
    public class BackupResult { public string folder; public List<BackupFile> files; }

    public static class Core {
        public static readonly JavaScriptSerializer Json = new JavaScriptSerializer { MaxJsonLength = 16000000 };
        public static readonly UTF8Encoding Utf8 = new UTF8Encoding(false);
        public const string Adapter = "Local GAMMA Co-op Launcher Bridge";
        public static string Canonical(string path) {
            string full = Path.GetFullPath(path);
            return full.Length > Path.GetPathRoot(full).Length ? full.TrimEnd(Path.DirectorySeparatorChar) : full;
        }
        public static bool Within(string path, string root) {
            string p = Canonical(path), r = Canonical(root);
            return p.Equals(r, StringComparison.OrdinalIgnoreCase) || p.StartsWith(r + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase);
        }
        public static void CheckPath(string path, string root) {
            if (!Within(path, root)) throw new InvalidOperationException("Path escaped the co-op folder.");
            string current = Canonical(path);
            while (!string.IsNullOrEmpty(current)) {
                if ((Directory.Exists(current) || File.Exists(current)) && (File.GetAttributes(current) & FileAttributes.ReparsePoint) != 0)
                    throw new InvalidOperationException("Linked folders/files cannot be used for co-op data.");
                current = Path.GetDirectoryName(current);
            }
        }
        public static void ValidComponent(string value) {
            if (string.IsNullOrWhiteSpace(value) || value == "." || value == ".." || value != value.Trim() || value.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0 || value.Contains("/") || value.Contains("\\"))
                throw new InvalidOperationException("Profile and mod names must be a single folder name.");
        }
        public static string OwnedFile(string root, params string[] parts) {
            string path = root;
            foreach (string part in parts) path = Path.Combine(path, part);
            CheckPath(path, root);
            return path;
        }
        public static IEnumerable<string> SafeFiles(string root, string directory) {
            CheckPath(directory, root);
            if (!Directory.Exists(directory)) yield break;
            var pending = new Stack<string>(); pending.Push(directory);
            while (pending.Count > 0) {
                string current = pending.Pop(); CheckPath(current, root);
                foreach (string file in Directory.GetFiles(current)) { CheckPath(file, root); yield return file; }
                foreach (string child in Directory.GetDirectories(current)) { CheckPath(child, root); pending.Push(child); }
            }
        }
        public static string Hash(string path) {
            using (var sha = SHA256.Create()) using (var file = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                return BitConverter.ToString(sha.ComputeHash(file)).Replace("-", "").ToLowerInvariant();
        }
        public static void AtomicWrite(string path, string text, string root) {
            CheckPath(path, root);
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            string temporary = path + "." + Guid.NewGuid().ToString("N") + ".tmp";
            File.WriteAllText(temporary, text, Utf8);
            if (File.Exists(path)) File.Replace(temporary, path, null);
            else File.Move(temporary, path);
        }
        public static string Relative(string path, string root) { return Canonical(path).Substring(Canonical(root).Length + 1).Replace('\\', '/'); }
        public static string ValidAddress(string value) {
            IPAddress address;
            value = (value ?? "").Trim();
            if (!Regex.IsMatch(value, @"^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$") || !IPAddress.TryParse(value, out address) || address.AddressFamily != System.Net.Sockets.AddressFamily.InterNetwork || address.ToString() != value)
                throw new InvalidOperationException("Enter the host's Tailscale IPv4 address (100.x.x.x).");
            byte[] bytes = address.GetAddressBytes();
            if (bytes[0] != 100 || bytes[1] < 64 || bytes[1] > 127)
                throw new InvalidOperationException("This launcher connects through Tailscale. Use its 100.64–100.127 IPv4 address.");
            return address.ToString();
        }
        public static string ValidPlayer(string value) {
            value = value.Trim();
            if (!Regex.IsMatch(value, "^[A-Za-z0-9_-]{1,24}$"))
                throw new InvalidOperationException("Use 1–24 letters, numbers, underscores or hyphens for your player name. Keep the same name to retain your co-op character.");
            return value;
        }
        public static void ValidPort(int value) { if (value < 1024 || value > 65535) throw new InvalidOperationException("Use a port between 1024 and 65535."); }
        public static string Quote(string value) {
            if (value.Contains("\"") || value.Contains("\r") || value.Contains("\n")) throw new InvalidOperationException("Invalid process argument.");
            return "\"" + value + "\"";
        }
        public static Install ValidateInstall(string root) {
            root = Canonical(root);
            CheckPath(root, root);
            string marker = OwnedFile(root, "coop-install.json");
            if (!File.Exists(marker)) throw new InvalidOperationException("Select the prepared Co-op Warfare folder. The original single-player installation cannot be launched here.");
            var install = Json.Deserialize<Install>(File.ReadAllText(marker));
            if (install.schema != 1 || install.engine != "xrRazom 1.4" || string.IsNullOrWhiteSpace(install.profile) || string.IsNullOrWhiteSpace(install.mod))
                throw new InvalidOperationException("Unsupported co-op installation marker.");
            ValidComponent(install.profile); ValidComponent(install.mod);
            Quote(install.profile);
            if (!string.IsNullOrEmpty(install.original_root) && Canonical(root).Equals(Canonical(install.original_root), StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("The original single-player folder is protected.");
            string anomaly = Path.Combine(root, "Anomaly"), gamma = Path.Combine(root, "GAMMA");
            foreach (string directory in new[] { anomaly, gamma, Path.Combine(anomaly, "appdata"), Path.Combine(gamma, "profiles", install.profile), Path.Combine(gamma, "overwrite") }) {
                CheckPath(directory, root);
                if (!Directory.Exists(directory)) throw new InvalidOperationException("The isolated co-op folder is incomplete.");
            }
            string ini = File.ReadAllText(OwnedFile(root, "GAMMA", "ModOrganizer.ini"));
            if (!File.Exists(OwnedFile(root, "GAMMA", "portable.txt"))) throw new InvalidOperationException("Use a portable Mod Organizer installation inside the co-op copy.");
            if (Regex.IsMatch(ini, @"(?im)^\s*(base_directory|mod_directory|profiles_directory|overwrite_directory)\s*="))
                throw new InvalidOperationException("Use default portable MO storage paths. Explicit base/mod/profile/overwrite path overrides need manual review.");
            string escaped = anomaly.Replace("\\", "\\\\");
            if (!ini.Contains("gamePath=@ByteArray(" + escaped + ")"))
                throw new InvalidOperationException("MO2 points to a different Anomaly folder. Reconfigure the copied installation before launching.");
            string profileSettings = File.ReadAllText(OwnedFile(root, "GAMMA", "profiles", install.profile, "settings.ini"));
            foreach (string key in new[] { "LocalSaves", "LocalSettings" })
                if (!Regex.IsMatch(profileSettings, @"(?im)^" + key + @"=false\s*$"))
                    throw new InvalidOperationException("Disable MO profile-specific saves and game settings in this isolated copy so the launcher and engine use its Anomaly/appdata folder.");
            var custom = new Dictionary<string,string>();
            bool customSection = false;
            foreach (string raw in ini.Replace("\r\n", "\n").Split('\n')) {
                string line = raw.Trim();
                if (line.StartsWith("[")) { customSection = line == "[customExecutables]"; continue; }
                int split = line.IndexOf('=');
                if (customSection && split > 0) custom[line.Substring(0, split)] = line.Substring(split + 1);
            }
            var titles = custom.Where(p => Regex.IsMatch(p.Key, @"^\d+\\title$") && p.Value == "Anomaly (DX11)").ToList();
            if (titles.Count != 1) throw new InvalidOperationException("Configure exactly one Anomaly (DX11) executable in the copied MO instance.");
            string prefix = titles[0].Key.Substring(0, titles[0].Key.LastIndexOf('\\') + 1), binary, arguments;
            if (!custom.TryGetValue(prefix + "binary", out binary) || !Canonical(binary).Equals(Canonical(Path.Combine(anomaly, "bin", "AnomalyDX11.exe")), StringComparison.OrdinalIgnoreCase)
                || !custom.TryGetValue(prefix + "arguments", out arguments) || arguments.Length != 0)
                throw new InvalidOperationException("The MO executable must point to this copy's Anomaly/bin/AnomalyDX11.exe with empty arguments.");
            if (!File.ReadAllText(OwnedFile(root, "Anomaly", "fsgame.ltx")).Contains("$fs_root$"))
                throw new InvalidOperationException("The copied game filesystem needs review.");
            string appDataLine = File.ReadAllLines(OwnedFile(root, "Anomaly", "fsgame.ltx")).FirstOrDefault(s => s.TrimStart().StartsWith("$app_data_root$"));
            if (appDataLine == null || !Regex.IsMatch(appDataLine, @"\$app_data_root\$\s*=\s*true\s*\|\s*false\s*\|\s*\$fs_root\$\s*\|\s*appdata\\\s*$"))
                throw new InvalidOperationException("The game must save inside this copy's Anomaly/appdata folder.");
            var enabled = Enabled(root, install);
            if (!enabled.Contains(install.mod) || !enabled.Contains(Adapter)) throw new InvalidOperationException("Enable xrRazom and the launcher bridge in the co-op profile.");
            if (enabled.Count < 2 || enabled[0] != Adapter || enabled[1] != install.mod) throw new InvalidOperationException("The launcher bridge and xrRazom need highest priority in the co-op profile.");
            if (!File.Exists(OwnedFile(root, "GAMMA", "mods", Adapter, "gamedata", "scripts", "modxml_zzz_gamma_coop_launcher.script")))
                throw new InvalidOperationException("The launcher bridge script is missing from the enabled local mod.");
            string mo2 = Path.Combine(gamma, "ModOrganizer.exe");
            CheckPath(mo2, root);
            if (!File.Exists(mo2)) throw new InvalidOperationException("Mod Organizer is missing.");
            foreach (string file in new[] { "Anomaly/bin/AnomalyDX11.exe", "Anomaly/bin/GameNetworkingSockets.dll", "Anomaly/bin/steam_api64.dll" }) {
                string target = Path.Combine(root, file.Replace('/', '\\'));
                CheckPath(target, root);
                var expected = install.verified_files == null ? null : install.verified_files.FirstOrDefault(f => f.file == file);
                if (expected == null || !File.Exists(target) || Hash(target) != expected.sha256)
                    throw new InvalidOperationException("The verified multiplayer engine is missing or was replaced: " + file);
            }
            return install;
        }
        public static List<string> Enabled(string root, Install install) {
            ValidComponent(install.profile);
            var mods = File.ReadAllLines(OwnedFile(root, "GAMMA", "profiles", install.profile, "modlist.txt"))
                .Where(s => s.StartsWith("+") && !s.EndsWith("_separator")).Select(s => s.Substring(1)).ToList();
            foreach (string mod in mods) {
                ValidComponent(mod);
                if (!Directory.Exists(OwnedFile(root, "GAMMA", "mods", mod))) throw new InvalidOperationException("An enabled mod folder is missing from this copy.");
            }
            return mods;
        }
        public static void NoGameRunning(string root) {
            foreach (var process in Process.GetProcesses()) {
                using (process) {
                    string name;
                    try { name = process.ProcessName; } catch { continue; }
                    if (Regex.IsMatch(name, "^AnomalyDX[0-9]+(AVX)?$", RegexOptions.IgnoreCase))
                        throw new InvalidOperationException("Close the running Anomaly game before changing co-op launch settings or taking a consistent save backup.");
                    if (name.Equals("ModOrganizer", StringComparison.OrdinalIgnoreCase)) {
                        string path;
                        try { path = process.MainModule.FileName; } catch { throw new InvalidOperationException("Close other Mod Organizer windows before launching the co-op copy."); }
                        if (!Within(path, Path.Combine(root, "GAMMA"))) throw new InvalidOperationException("Close the single-player Mod Organizer first so its existing instance cannot receive the co-op launch.");
                    }
                }
            }
        }
        public static BackupResult Backup(string root, Action<string> log) {
            string saves = Path.Combine(root, "Anomaly", "appdata", "savedgames");
            CheckPath(saves, root);
            string folder = Path.Combine(root, "Save Backups", DateTime.UtcNow.ToString("yyyyMMddTHHmmssfffZ") + "-" + Guid.NewGuid().ToString("N").Substring(0, 6));
            CheckPath(folder, root);
            Directory.CreateDirectory(folder);
            var result = new BackupResult { folder = folder, files = new List<BackupFile>() };
            if (Directory.Exists(saves)) {
                foreach (var source in SafeFiles(root, saves)) {
                    CheckPath(source, root);
                    string relative = Relative(source, saves), target = Path.Combine(folder, "savedgames", relative.Replace('/', '\\'));
                    CheckPath(target, root);
                    string before = Hash(source);
                    Directory.CreateDirectory(Path.GetDirectoryName(target));
                    File.Copy(source, target, false);
                    if (Hash(target) != before || Hash(source) != before) throw new IOException("A save changed during backup. Nothing will be launched. Keep this partial backup and try again after closing the game.");
                    result.files.Add(new BackupFile { file = relative, sha256 = before, size = new FileInfo(source).Length });
                }
            }
            string settings = Path.Combine(root, "Anomaly", "appdata", "user.ltx");
            CheckPath(settings, root);
            if (File.Exists(settings)) File.Copy(settings, Path.Combine(folder, "user.ltx"), false);
            File.WriteAllText(Path.Combine(folder, "manifest.json"), Json.Serialize(result), Utf8);
            log("Verified backup: " + result.files.Count + " save files → " + folder);
            return result;
        }
        public static string VpnExe() {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Tailscale", "tailscale.exe");
            if (!File.Exists(path)) throw new InvalidOperationException("Install Tailscale from tailscale.com/download on this PC and your friend's PC, then sign in.");
            return path;
        }
        public static string RunHidden(string exe, string arguments, int timeout) {
            using (var p = Process.Start(new ProcessStartInfo(exe, arguments) { UseShellExecute = false, CreateNoWindow = true, RedirectStandardOutput = true, RedirectStandardError = true })) {
                Task<string> output = p.StandardOutput.ReadToEndAsync(), error = p.StandardError.ReadToEndAsync();
                if (!p.WaitForExit(timeout)) { p.Kill(); throw new IOException("The network check timed out. Try again after checking Tailscale."); }
                if (p.ExitCode != 0) throw new IOException("The Tailscale command failed. Review its tray app locally; do not publish raw account diagnostics.");
                return output.Result.Trim();
            }
        }
        public static VpnStatus Vpn() {
            var status = Json.Deserialize<VpnStatus>(RunHidden(VpnExe(), "status --json", 8000));
            if (status.BackendState != "Running") throw new InvalidOperationException("Tailscale is " + status.BackendState + ". Connect/sign in using its tray app.");
            if (status.TailscaleIPs == null || !status.TailscaleIPs.Any(ip => ip.Contains("."))) throw new InvalidOperationException("Tailscale has not assigned an IPv4 address.");
            return status;
        }
        public static string Fingerprint(string root, Install install, Action<string> log) {
            var files = new SortedDictionary<string, string>(StringComparer.Ordinal);
            Action<string> overlay = delegate(string dir) {
                if (!Directory.Exists(dir)) return;
                CheckPath(dir, root);
                foreach (string path in SafeFiles(root, dir)) {
                    CheckPath(path, root);
                    string key = Relative(path, dir).ToLowerInvariant();
                    if (!(key.StartsWith("scripts/") || key.StartsWith("configs/"))) continue;
                    if (key == "configs/axr_options.ltx") continue;
                    files[key] = path;
                }
            };
            overlay(Path.Combine(root, "Anomaly", "gamedata"));
            var mods = Enabled(root, install);
            foreach (string mod in mods.AsEnumerable().Reverse()) overlay(Path.Combine(root, "GAMMA", "mods", mod, "gamedata"));
            overlay(Path.Combine(root, "GAMMA", "overwrite", "gamedata"));
            var result = new StringBuilder("gamma-coop-content-v1\n");
            result.Append(string.Join("\n", mods)).Append("\n");
            foreach (string path in Directory.GetFiles(Path.Combine(root, "Anomaly", "bin"))) {
                CheckPath(path, root);
                string name = Path.GetFileName(path).ToLowerInvariant();
                if (name.EndsWith(".exe") || name.EndsWith(".dll")) files["bin/" + name] = path;
            }
            foreach (var file in files) result.Append(file.Key).Append(" ").Append(Hash(file.Value)).Append("\n");
            using (var sha = SHA256.Create()) {
                string fingerprint = BitConverter.ToString(sha.ComputeHash(Utf8.GetBytes(result.ToString()))).Replace("-", "").ToLowerInvariant();
                string report = Path.Combine(root, "Launcher Data", "content-manifest.txt");
                AtomicWrite(report, result.ToString(), root);
                log("Content check: " + files.Count + " engine/script/config files; " + mods.Count + " enabled mods.");
                log("Fingerprint: " + fingerprint);
                return fingerprint;
            }
        }
        public static string UpdateConsole(string text, Dictionary<string, string> values) {
            var remaining = text.Replace("\r\n", "\n").Split('\n').Where(s => !values.ContainsKey(s.Trim().Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault() ?? ""));
            return string.Join("\n", remaining).TrimEnd('\n') + "\n" + string.Join("\n", values.Select(p => p.Key + " " + p.Value)) + "\n";
        }
        public static void Prepare(Settings settings, bool host, Action<string> log) {
            var install = ValidateInstall(settings.Root);
            NoGameRunning(settings.Root);
            ValidPort(settings.Port);
            string player = ValidPlayer(settings.Player), address = host ? "" : ValidAddress(settings.Host);
            Vpn();
            if (Process.GetProcessesByName("steam").Length > 0) throw new InvalidOperationException("Close Steam normally on both PCs before using VPN/IP co-op. xrRazom chooses its network transport at startup.");
            if (!host) {
                log("Checking the host's Tailscale path...");
                RunHidden(VpnExe(), "ping --c 1 --timeout 8s " + address, 12000);
                log("Tailscale route check passed. This does not verify the game UDP port.");
            }
            if (!string.IsNullOrWhiteSpace(settings.ExpectedFingerprint)) {
                if (!Regex.IsMatch(settings.ExpectedFingerprint.Trim(), "^[A-Fa-f0-9]{64}$")) throw new InvalidOperationException("A content fingerprint must contain exactly 64 hexadecimal characters.");
                string actual = Fingerprint(settings.Root, install, log);
                if (!actual.Equals(settings.ExpectedFingerprint.Trim(), StringComparison.OrdinalIgnoreCase))
                    throw new InvalidOperationException("The host/client content fingerprints differ. Match the co-op profile before joining.");
            }
            Backup(settings.Root, log);
            string user = Path.Combine(settings.Root, "Anomaly", "appdata", "user.ltx");
            CheckPath(user, settings.Root);
            var values = new Dictionary<string, string> {
                { "xrr_host_session", host ? "on" : "off" }, { "xrr_host_steam", "off" },
                { "xrr_host_lan_port", settings.Port.ToString() }, { "xrr_skip_mod_check", "off" }, { "xrr_player_name", player }
            };
            AtomicWrite(user, UpdateConsole(File.Exists(user) ? File.ReadAllText(user) : "", values), settings.Root);
            long expires = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds + 300;
            string request = "role=" + (host ? "host" : "join") + "\nhost=" + address + "\nport=" + settings.Port + "\nexpires=" + expires + "\n";
            AtomicWrite(Path.Combine(settings.Root, "Anomaly", "appdata", "gamma_coop_request.txt"), request, settings.Root);
            log(host ? "Host settings saved. Load a Warfare save in the copied game to start hosting." : "Join settings saved. The game will attempt the private endpoint at the menu.");
        }
        public static void Launch(Settings settings, bool host, Action<string> log) {
            Prepare(settings, host, log);
            var install = ValidateInstall(settings.Root);
            var process = Process.Start(new ProcessStartInfo(Path.Combine(settings.Root, "GAMMA", "ModOrganizer.exe"), "-p " + Quote(install.profile) + " run -e \"Anomaly (DX11)\"") {
                WorkingDirectory = Path.Combine(settings.Root, "GAMMA"), UseShellExecute = true
            });
            log("Launch requested through the isolated Mod Organizer. PID " + process.Id + ".");
        }
    }

    public class LauncherForm : Form {
        Settings settings;
        readonly string settingsFile;
        TextBox folder, player, host, expected;
        NumericUpDown port;
        Label vpnLabel, engineLabel;
        RichTextBox output;
        Button hostButton, joinButton, checksButton, backupButton, copyButton;
        string vpnIp = "", fingerprint = "";
        bool busy;
        public LauncherForm(string rootOverride) {
            settingsFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "launcher-settings.json");
            try { settings = File.Exists(settingsFile) ? Core.Json.Deserialize<Settings>(File.ReadAllText(settingsFile)) : new Settings(); }
            catch { settings = new Settings(); }
            if (!string.IsNullOrWhiteSpace(rootOverride)) settings.Root = rootOverride;
            Text = "GAMMA Private Co-op — Host / Join";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(990, 710); MinimumSize = new Size(890, 720);
            Font = new Font("Segoe UI", 10f);
            BackColor = Color.FromArgb(25, 30, 28); ForeColor = Color.FromArgb(228, 231, 222);
            var layout = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(24), ColumnCount = 1, RowCount = 8 };
            foreach (int height in new[] { 74, 54, 52, 85, 64, 62, 35 }) layout.RowStyles.Add(new RowStyle(SizeType.Absolute, height));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); Controls.Add(layout);
            var title = new Label { Text = "GAMMA / PRIVATE CO-OP\nYour campaign. Your host. Connected through Tailscale.", Dock = DockStyle.Fill, Font = new Font("Segoe UI", 16f, FontStyle.Bold), AutoSize = false };
            layout.Controls.Add(title, 0, 0);
            var rootPanel = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 3 };
            rootPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100)); rootPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100)); rootPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 95));
            rootPanel.Controls.Add(new Label { Text = "Co-op copy", AutoSize = true, Anchor = AnchorStyles.Left }, 0, 0);
            folder = Box(settings.Root); folder.Dock = DockStyle.Fill; rootPanel.Controls.Add(folder, 1, 0);
            var browse = Button("Browse", delegate { using (var dialog = new FolderBrowserDialog { Description = "Select the prepared Co-op Warfare folder" }) { if (dialog.ShowDialog(this) == DialogResult.OK) folder.Text = dialog.SelectedPath; } });
            rootPanel.Controls.Add(browse, 2, 0); layout.Controls.Add(rootPanel, 0, 1);
            var statusPanel = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2 };
            statusPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50)); statusPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            engineLabel = new Label { Text = "Engine: not checked", Dock = DockStyle.Fill }; vpnLabel = new Label { Text = "Tailscale: not checked", Dock = DockStyle.Fill };
            statusPanel.Controls.Add(engineLabel, 0, 0); statusPanel.Controls.Add(vpnLabel, 1, 0); layout.Controls.Add(statusPanel, 0, 2);
            var connection = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 3, RowCount = 2 };
            connection.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35)); connection.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45)); connection.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
            foreach (var pair in new[] { "Player name (keep it stable)", "Host Tailscale IP (for joining)", "Game port" }.Select((text, index) => new { text, index })) connection.Controls.Add(new Label { Text = pair.text, AutoSize = true }, pair.index, 0);
            player = Box(settings.Player); host = Box(settings.Host); port = new NumericUpDown { Minimum = 1024, Maximum = 65535, Value = Math.Max(1024, Math.Min(65535, settings.Port)), Dock = DockStyle.Fill };
            connection.Controls.Add(player, 0, 1); connection.Controls.Add(host, 1, 1); connection.Controls.Add(port, 2, 1); layout.Controls.Add(connection, 0, 3);
            var actions = new FlowLayoutPanel { Dock = DockStyle.Fill, WrapContents = false };
            hostButton = Button("HOST CAMPAIGN", async delegate { await Work(delegate { Core.Launch(ReadSettings(), true, Log); }); });
            joinButton = Button("JOIN HOST", async delegate { await Work(delegate { Core.Launch(ReadSettings(), false, Log); }); });
            checksButton = Button("Check setup", async delegate { await CheckSetup(); });
            backupButton = Button("Back up saves", async delegate { await Work(delegate { var s = ReadSettings(); Core.ValidateInstall(s.Root); Core.NoGameRunning(s.Root); Core.Backup(s.Root, Log); }); });
            copyButton = Button("Copy invite", delegate { CopyInvite(); });
            foreach (var button in new[] { hostButton, joinButton, checksButton, backupButton, copyButton }) actions.Controls.Add(button);
            layout.Controls.Add(actions, 0, 4);
            var match = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2 };
            match.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 235)); match.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            match.Controls.Add(new Label { Text = "Host fingerprint (optional check)", AutoSize = true }, 0, 0); expected = Box(settings.ExpectedFingerprint); expected.Dock = DockStyle.Fill; match.Controls.Add(expected, 1, 0);
            layout.Controls.Add(match, 0, 5);
            layout.Controls.Add(new Label { Text = "Host: load your copied Warfare save. Join: choose your loadout when prompted. Close Steam on both PCs.", Dock = DockStyle.Fill, Font = new Font("Segoe UI", 9f) }, 0, 6);
            output = new RichTextBox { Dock = DockStyle.Fill, ReadOnly = true, BackColor = Color.FromArgb(17, 21, 19), ForeColor = Color.FromArgb(199, 216, 185), BorderStyle = BorderStyle.FixedSingle, Font = new Font("Consolas", 9f) };
            layout.Controls.Add(output, 0, 7);
            folder.TextChanged += delegate { fingerprint = ""; engineLabel.Text = "Engine: run Check setup for this folder"; };
            Shown += delegate { Log("Experimental xrRazom 1.4 front end. Select an independently prepared co-op copy, then run Check setup. No dedicated server is provided."); };
        }
        TextBox Box(string text) { return new TextBox { Text = text ?? "", Dock = DockStyle.Top, Margin = new Padding(3, 3, 12, 3), BackColor = Color.FromArgb(42, 49, 44), ForeColor = ForeColor, BorderStyle = BorderStyle.FixedSingle }; }
        Button Button(string text, EventHandler click) { var b = new Button { Text = text, AutoSize = true, Height = 39, FlatStyle = FlatStyle.Flat, Padding = new Padding(10, 5, 10, 5), BackColor = Color.FromArgb(62, 79, 52), ForeColor = ForeColor, Margin = new Padding(0, 0, 9, 0) }; b.Click += click; return b; }
        Settings ReadSettings() {
            if (InvokeRequired) return (Settings)Invoke(new Func<Settings>(ReadSettings));
            settings.Root = folder.Text.Trim(); settings.Player = player.Text.Trim(); settings.Host = host.Text.Trim(); settings.Port = (int)port.Value; settings.ExpectedFingerprint = expected.Text.Trim();
            return settings;
        }
        void Log(string text) {
            if (InvokeRequired) { BeginInvoke(new Action<string>(Log), text); return; }
            output.AppendText(DateTime.Now.ToString("HH:mm:ss") + "  " + text + Environment.NewLine); output.ScrollToCaret();
        }
        async Task Work(Action action) {
            if (busy) return; busy = true;
            foreach (var b in new[] { hostButton, joinButton, checksButton, backupButton, copyButton }) b.Enabled = false;
            try { ReadSettings(); File.WriteAllText(settingsFile, Core.Json.Serialize(settings), Core.Utf8); await Task.Run(action); }
            catch (Exception e) { Log("Stopped: " + e.Message); MessageBox.Show(this, e.Message, "Co-op setup needs attention", MessageBoxButtons.OK, MessageBoxIcon.Information); }
            finally { busy = false; foreach (var b in new[] { hostButton, joinButton, checksButton, backupButton, copyButton }) b.Enabled = true; }
        }
        async Task CheckSetup() {
            await Work(delegate {
                var s = ReadSettings(); var install = Core.ValidateInstall(s.Root);
                BeginInvoke(new Action(delegate { engineLabel.Text = "Engine: xrRazom 1.4 ready / isolated copy"; }));
                var vpn = Core.Vpn(); vpnIp = vpn.TailscaleIPs.First(ip => ip.Contains("."));
                BeginInvoke(new Action(delegate { vpnLabel.Text = "Tailscale: connected / " + vpnIp; }));
                Log("Verified multiplayer engine and isolated save location.");
                fingerprint = Core.Fingerprint(s.Root, install, Log);
                Log("Ready for local host testing. A second PC is required to verify an actual co-op connection.");
            });
        }
        void CopyInvite() {
            try {
                if (string.IsNullOrEmpty(vpnIp) || string.IsNullOrEmpty(fingerprint)) throw new InvalidOperationException("Run Check setup first.");
                var s = ReadSettings();
                string text = "GAMMA private co-op\r\nHost Tailscale IP: " + vpnIp + "\r\nGame port: " + s.Port + "\r\nEngine: xrRazom 1.4 (protocol 116)\r\nContent fingerprint: " + fingerprint + "\r\nClose Steam before launching. Use the same co-op mod profile. Join with GAMMA Private Co-op Launcher.\r\n";
                Clipboard.SetText(text); Log("Connection details copied to your clipboard. Send them to your co-op partner.");
            } catch (Exception e) { Log(e.Message); }
        }
    }

    static class Program {
        [STAThread] static int Main(string[] args) {
            string root = "";
            for (int i = 0; i + 1 < args.Length; i++) if (args[i] == "--root") root = args[i + 1];
            if (args.Contains("--self-test")) return SelfTest(root);
            if (args.Contains("--inspect")) {
                try { var install = Core.ValidateInstall(root); var vpn = Core.Vpn(); var result = new { engine = install.engine, profile = install.profile, vpn = vpn.BackendState, ip = vpn.TailscaleIPs.First(ip => ip.Contains(".")), fingerprint = Core.Fingerprint(root, install, delegate { }) }; File.WriteAllText(Path.Combine(root, "Launcher Data", "inspection.json"), Core.Json.Serialize(result), Core.Utf8); return 0; }
                catch { File.WriteAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "inspection-error.txt"), "Inspection failed. Check the isolated setup locally; remove private paths and network identifiers before sharing diagnostics."); return 1; }
            }
            Application.EnableVisualStyles(); Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LauncherForm(root)); return 0;
        }
        static int SelfTest(string root) {
            var results = new List<string>();
            string temp = Path.Combine(string.IsNullOrEmpty(root) ? AppDomain.CurrentDomain.BaseDirectory : root, "launcher-test-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(temp);
            Action<bool, string> check = delegate(bool condition, string name) { if (!condition) throw new Exception(name); results.Add("PASS " + name); };
            try {
                check(Core.ValidAddress("100.64.0.1") == "100.64.0.1", "Synthetic Tailscale address accepted");
                foreach (string invalid in new[] { "8.8.8.8", "127.0.0.1", "100.1.1.1", "100.128.1.1", "100.64.0.1\nload injected", "hostname & executable", "100.64.1", "100.064.0.1" }) {
                    bool rejected = false; try { Core.ValidAddress(invalid); } catch { rejected = true; } check(rejected, "Reject unsafe/non-VPN address: " + invalid.Replace("\n", " "));
                }
                bool badName = false; try { Core.ValidPlayer("Player\nxrr_skip_mod_check on"); } catch { badName = true; } check(badName, "Reject console injection in player names");
                foreach (string component in new[] { "..", "../other", "a\\b", "name\nvalue" }) {
                    bool rejected = false; try { Core.ValidComponent(component); } catch { rejected = true; } check(rejected, "Reject unsafe profile/mod component");
                }
                check(!Core.Within(temp + "-outside/file", temp), "Sibling-prefix path escape rejected");
                check(Core.Within(Path.Combine(temp, "savedgames", "save.scop"), temp), "Nested save path allowed");
                string saves = Path.Combine(temp, "Anomaly", "appdata", "savedgames"); Directory.CreateDirectory(saves);
                string source = Path.Combine(saves, "warfare.scop"); File.WriteAllText(source, "original campaign bytes"); string hash = Core.Hash(source);
                var first = Core.Backup(temp, delegate { }); File.WriteAllText(source, "advanced campaign bytes"); var second = Core.Backup(temp, delegate { });
                check(first.folder != second.folder, "Repeated backups preserve distinct restore points");
                check(Core.Hash(Path.Combine(first.folder, "savedgames", "warfare.scop")) == hash, "Earlier campaign backup remains unchanged");
                check(Core.Hash(source) == Core.Hash(Path.Combine(second.folder, "savedgames", "warfare.scop")), "Backup content matches current campaign");
                string patched = Core.UpdateConsole("xrr_host_session on\r\nxrr_host_session on\r\nr2_sun on\r\n", new Dictionary<string,string> { {"xrr_host_session", "off"} });
                check(patched.Split('\n').Count(line => line.StartsWith("xrr_host_session ")) == 1 && patched.Contains("xrr_host_session off") && patched.Contains("r2_sun on"), "Join role disables hosting while retaining graphics settings");
                bool noMarker = false; try { Core.ValidateInstall(temp); } catch { noMarker = true; } check(noMarker, "An unmarked/original install cannot launch");
                string fixture = Path.Combine(temp, "fixture"), profile = "Mock Profile", mod = "Mock xrRazom";
                foreach (string dir in new[] { "Anomaly/appdata", "Anomaly/bin", "GAMMA/overwrite", "GAMMA/profiles/" + profile, "GAMMA/mods/" + mod, "GAMMA/mods/" + Core.Adapter + "/gamedata/scripts" }) Directory.CreateDirectory(Path.Combine(fixture, dir.Replace('/', '\\')));
                File.WriteAllText(Path.Combine(fixture, "GAMMA", "mods", Core.Adapter, "gamedata", "scripts", "modxml_zzz_gamma_coop_launcher.script"), "-- mock, never executed");
                string fixtureAnomaly = Path.Combine(fixture, "Anomaly");
                string fixtureIni = "[General]\ngamePath=@ByteArray(" + fixtureAnomaly.Replace("\\", "\\\\") + ")\n[customExecutables]\n1\\title=Anomaly (DX11)\n1\\binary=" + fixtureAnomaly.Replace('\\', '/') + "/bin/AnomalyDX11.exe\n1\\arguments=\n";
                string iniFile = Path.Combine(fixture, "GAMMA", "ModOrganizer.ini"), settingsFile = Path.Combine(fixture, "GAMMA", "profiles", profile, "settings.ini");
                File.WriteAllText(iniFile, fixtureIni);
                File.WriteAllText(Path.Combine(fixture, "GAMMA", "portable.txt"), "");
                File.WriteAllText(settingsFile, "[General]\nLocalSaves=false\nLocalSettings=false\n");
                File.WriteAllText(Path.Combine(fixtureAnomaly, "fsgame.ltx"), "$app_data_root$ = true | false | $fs_root$ | appdata\\\n");
                File.WriteAllText(Path.Combine(fixture, "GAMMA", "profiles", profile, "modlist.txt"), "+" + Core.Adapter + "\n+" + mod + "\n");
                File.WriteAllText(Path.Combine(fixture, "GAMMA", "ModOrganizer.exe"), "mock, never executed");
                var marker = new Install { schema = 1, engine = "xrRazom 1.4", profile = profile, mod = mod, verified_files = new List<InstalledFile>() };
                foreach (string file in new[] { "Anomaly/bin/AnomalyDX11.exe", "Anomaly/bin/GameNetworkingSockets.dll", "Anomaly/bin/steam_api64.dll" }) {
                    string target = Path.Combine(fixture, file.Replace('/', '\\')); File.WriteAllText(target, "mock, never loaded or executed");
                    marker.verified_files.Add(new InstalledFile { file = file, sha256 = Core.Hash(target) });
                }
                File.WriteAllText(Path.Combine(fixture, "coop-install.json"), Core.Json.Serialize(marker));
                check(Core.ValidateInstall(fixture).profile == profile, "Isolated mock installation passes structural checks");
                check(Core.Fingerprint(fixture, marker, delegate { }).Length == 64, "Mock content fingerprint has SHA256 format");
                marker.profile = "../outside"; File.WriteAllText(Path.Combine(fixture, "coop-install.json"), Core.Json.Serialize(marker));
                bool markerEscape = false; try { Core.ValidateInstall(fixture); } catch { markerEscape = true; } check(markerEscape, "Marker profile path traversal is rejected");
                marker.profile = profile; File.WriteAllText(Path.Combine(fixture, "coop-install.json"), Core.Json.Serialize(marker));
                File.AppendAllText(Path.Combine(fixture, "GAMMA", "profiles", profile, "modlist.txt"), "+../outside\n");
                bool modEscape = false; try { Core.ValidateInstall(fixture); } catch { modEscape = true; } check(modEscape, "Enabled mod path traversal is rejected");
                File.WriteAllText(Path.Combine(fixture, "GAMMA", "profiles", profile, "modlist.txt"), "+" + Core.Adapter + "\n+" + mod + "\n");
                File.WriteAllText(settingsFile, "[General]\nLocalSaves=false\nLocalSettings=true\n");
                bool redirected = false; try { Core.ValidateInstall(fixture); } catch { redirected = true; } check(redirected, "Profile-specific settings redirect is rejected");
                File.WriteAllText(settingsFile, "[General]\nLocalSaves=false\nLocalSettings=false\n");
                File.WriteAllText(iniFile, fixtureIni.Replace(fixtureAnomaly.Replace('\\', '/') + "/bin/AnomalyDX11.exe", "C:/Other/AnomalyDX11.exe"));
                bool wrongExe = false; try { Core.ValidateInstall(fixture); } catch { wrongExe = true; } check(wrongExe, "MO executable from a different installation is rejected");
                File.WriteAllText(iniFile, fixtureIni + "[Settings]\nmod_directory=external\n");
                bool externalStorage = false; try { Core.ValidateInstall(fixture); } catch { externalStorage = true; } check(externalStorage, "Explicit MO storage overrides are rejected");
                File.WriteAllText(iniFile, fixtureIni);
                File.WriteAllText(Path.Combine(fixtureAnomaly, "bin", "AnomalyDX11.exe"), "modified mock engine");
                bool modified = false; try { Core.ValidateInstall(fixture); } catch { modified = true; } check(modified, "Engine hash change is rejected");
                File.WriteAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "self-test-results.txt"), string.Join(Environment.NewLine, results), Core.Utf8); return 0;
            } catch (Exception e) { results.Add("FAIL " + e); File.WriteAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "self-test-results.txt"), string.Join(Environment.NewLine, results), Core.Utf8); return 1; }
        }
    }
}
