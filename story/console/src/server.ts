import { createServer } from 'node:http';
import { open, stat } from 'node:fs/promises';
import { resolve, dirname } from 'node:path';
import { fileURLToPath } from 'node:url';

type Role = 'solo' | 'host' | 'peer';
type GameEvent = {
  version: 1;
  kind: 'world_loaded' | 'world_role' | 'world_heartbeat';
  unix_time: number;
  role: Role;
  faction?: string;
  story_mode?: boolean | null;
  peer_count?: number | null;
};

const policy = Object.freeze({ campaign: 'story', faction: 'stalker', gameplay_difficulty: 'easy', progression_difficulty: 'easy', max_players_target: 5 });

const here = dirname(fileURLToPath(import.meta.url));
const defaultEvents = resolve(here, '../../Anomaly/appdata/outlands-events.jsonl');
const args = process.argv.slice(2);
function argument(name: string): string | undefined {
  const index = args.indexOf(name);
  return index >= 0 ? args[index + 1] : undefined;
}
const eventsPath = resolve(argument('--events') ?? defaultEvents);
const port = Number(argument('--port') ?? '37771');
const replay = args.includes('--replay');
if (!Number.isInteger(port) || port < 1024 || port > 65535) {
  throw new Error('Port must be an integer between 1024 and 65535.');
}

const startedAt = new Date().toISOString();
let cursor = 0;
let pending = '';
let eventsSeen = 0;
let invalidLines = 0;
let worldLoaded = false;
let lastGameSeen = 0;
let role: Role | 'unknown' = 'unknown';
let lastEvent: GameEvent | null = null;
let reading = false;
const MAX_PENDING = 4096;

function log(level: 'INFO' | 'WARN', message: string): void {
  console.log(`${new Date().toISOString()} ${level.padEnd(4)} ${message}`);
}

function validEvent(value: unknown): value is GameEvent {
  if (!value || typeof value !== 'object') return false;
  const event = value as Record<string, unknown>;
  return event.version === 1 &&
    (event.kind === 'world_loaded' || event.kind === 'world_role' || event.kind === 'world_heartbeat') &&
    (event.role === 'solo' || event.role === 'host' || event.role === 'peer') &&
    typeof event.unix_time === 'number' && Number.isFinite(event.unix_time) &&
    (event.faction === undefined || (typeof event.faction === 'string' && /^[a-z_]{3,32}$/.test(event.faction))) &&
    (event.story_mode === undefined || event.story_mode === null || typeof event.story_mode === 'boolean') &&
    (event.peer_count === undefined || event.peer_count === null ||
      (Number.isInteger(event.peer_count) && (event.peer_count as number) >= 0 && (event.peer_count as number) <= 64));
}

function acceptLine(line: string): void {
  if (!line || line.length > MAX_PENDING) { invalidLines++; return; }
  try {
    const data: unknown = JSON.parse(line);
    if (!validEvent(data)) { invalidLines++; return; }
    eventsSeen++;
    lastEvent = data;
    lastGameSeen = Date.now();
    role = data.role;
    if (data.kind === 'world_loaded') worldLoaded = true;
    if (data.kind !== 'world_heartbeat') log('INFO', `GAME ${data.kind} role=${data.role}`);
  } catch {
    invalidLines++;
    log('WARN', 'Ignored malformed game event');
  }
}

async function pollEvents(): Promise<void> {
  if (reading) return;
  reading = true;
  try {
    let size: number;
    try { size = (await stat(eventsPath)).size; }
    catch { return; }
    if (size < cursor) { cursor = 0; pending = ''; log('WARN', 'Game event file was truncated; restarting reader'); }
    if (size === cursor) return;
    const handle = await open(eventsPath, 'r');
    try {
      while (cursor < size) {
        const buffer = Buffer.allocUnsafe(Math.min(65536, size - cursor));
        const { bytesRead } = await handle.read(buffer, 0, buffer.length, cursor);
        if (bytesRead === 0) break;
        cursor += bytesRead;
        pending += buffer.toString('utf8', 0, bytesRead);
        const lines = pending.split('\n');
        pending = lines.pop() ?? '';
        for (const line of lines) acceptLine(line.trim());
        if (pending.length > MAX_PENDING) { pending = ''; invalidLines++; log('WARN', 'Discarded oversized game event'); }
      }
    } finally { await handle.close(); }
  } catch (error) {
    log('WARN', `Event reader: ${error instanceof Error ? error.message : String(error)}`);
  } finally { reading = false; }
}

function snapshot() {
  const gameLive = worldLoaded && Date.now() - lastGameSeen < 30000;
  const faction = gameLive ? lastEvent?.faction ?? null : null;
  const storyMode = gameLive ? lastEvent?.story_mode ?? null : null;
  const policyMatch = faction && storyMode !== null ? faction === policy.faction && storyMode === true : null;
  const players = gameLive && role === 'solo' ? 1 :
    gameLive && role === 'host' && typeof lastEvent?.peer_count === 'number' ? lastEvent.peer_count + 1 : null;
  return { server: 'OUTLANDS // GAMMA', started_at: startedAt, uptime_seconds: Math.floor(process.uptime()),
    event_file: eventsPath, events_seen: eventsSeen, invalid_lines: invalidLines,
    world_loaded: gameLive, role: gameLive ? role : 'unknown', last_event: lastEvent,
    last_game_event_at: lastGameSeen ? new Date(lastGameSeen).toISOString() : null,
    faction, story_mode: storyMode, policy_match: policyMatch, policy,
    connected_players: players, note: 'Host player count comes from xrRazom; remote rule enforcement is not implemented.' };
}

const html = `<!doctype html><html lang="en"><meta charset="utf-8"><meta name="viewport" content="width=device-width,initial-scale=1">
<title>OUTLANDS // GAMMA</title><style>
  :root{color-scheme:dark;font-family:Manrope,Arial,sans-serif;background:#080b11;color:#f0f0f2}
  *{box-sizing:border-box}body{margin:0;padding:32px;max-width:1120px;margin-inline:auto}
  header{border:1px solid #394047;padding:30px;background:linear-gradient(120deg,#11151c,#080b11 65%)}
  .eyebrow{font:800 11px ui-monospace,monospace;letter-spacing:.14em;color:#73dce8}
  h1{font-size:clamp(36px,7vw,72px);line-height:.98;margin:18px 0;font-weight:900;letter-spacing:-.06em}
  h1 span{color:#f4ef27}.sub{color:#aeb8c3;max-width:620px;line-height:1.6}
  .grid{display:grid;grid-template-columns:repeat(auto-fit,minmax(230px,1fr));gap:12px;margin-top:20px}
  .card{border:1px solid #394047;background:#11151c;padding:22px;min-height:155px}
  .label{font:800 11px ui-monospace,monospace;letter-spacing:.1em;color:#73dce8}
  .value{font-weight:800;font-size:26px;margin-top:20px;overflow-wrap:anywhere}.good{color:#f4ef27}.muted{color:#aeb8c3}
  footer{border-top:1px solid #394047;margin-top:26px;padding-top:18px;color:#8e9aa6;font:12px ui-monospace,monospace}
 </style><header><div class="eyebrow">PRIVATE NETWORK // STORY WORKSHOP</div><h1>OUTLANDS<span> // GAMMA.</span></h1>
 <p class="sub">Local session console. Game events appear here when the isolated Story profile loads a world.</p></header>
 <main class="grid"><div class="card"><div class="label">SERVICE</div><div id="service" class="value good">ONLINE</div></div>
 <div class="card"><div class="label">WORLD</div><div id="world" class="value muted">WAITING</div></div>
 <div class="card"><div class="label">GAME ROLE</div><div id="role" class="value muted">UNKNOWN</div></div>
 <div class="card"><div class="label">PLAYERS</div><div id="players" class="value muted">UNKNOWN</div></div>
 <div class="card"><div class="label">POLICY</div><div id="policy" class="value muted">UNVERIFIED</div></div>
 <div class="card"><div class="label">EVENTS</div><div id="events" class="value">0</div></div></main>
 <footer id="detail">LOCAL ONLY // 127.0.0.1</footer><script>
  async function refresh(){try{const r=await fetch('/api/status');const s=await r.json();
  document.getElementById('world').textContent=s.world_loaded?'LOADED':'WAITING';
  document.getElementById('role').textContent=s.role.toUpperCase();
  document.getElementById('players').textContent=s.connected_players===null?'UNKNOWN':String(s.connected_players);
  document.getElementById('policy').textContent=s.policy_match===null?'UNVERIFIED':s.policy_match?'MATCH':'MISMATCH';
  document.getElementById('events').textContent=String(s.events_seen);
  document.getElementById('detail').textContent='LOCAL ONLY // 127.0.0.1 // UP '+s.uptime_seconds+'s // LAST '+(s.last_event?.kind??'none');
  }catch{document.getElementById('service').textContent='OFFLINE'}}refresh();setInterval(refresh,1000);
 </script></html>`;

const server = createServer((request, response) => {
  const path = request.url?.split('?')[0] ?? '/';
  response.setHeader('Cache-Control', 'no-store');
  if (request.method !== 'GET') { response.writeHead(405); response.end(); return; }
  if (path === '/api/status') {
    response.writeHead(200, { 'Content-Type': 'application/json; charset=utf-8' });
    response.end(JSON.stringify(snapshot()));
  } else if (path === '/api/policy') {
    response.writeHead(200, { 'Content-Type': 'application/json; charset=utf-8' });
    response.end(JSON.stringify(policy));
  } else if (path === '/metrics') {
    response.writeHead(200, { 'Content-Type': 'text/plain; version=0.0.4; charset=utf-8' });
    const state = snapshot();
    response.end(`outlands_gamma_events_total ${eventsSeen}\noutlands_gamma_invalid_events_total ${invalidLines}\noutlands_gamma_world_loaded ${state.world_loaded ? 1 : 0}\noutlands_gamma_policy_match ${state.policy_match === null ? -1 : state.policy_match ? 1 : 0}\noutlands_gamma_connected_players ${state.connected_players ?? -1}\n`);
  } else if (path === '/') {
    response.writeHead(200, { 'Content-Type': 'text/html; charset=utf-8' }); response.end(html);
  } else { response.writeHead(404); response.end(); }
});

if (replay) { void pollEvents(); }
else {
  try { cursor = (await stat(eventsPath)).size; } catch { cursor = 0; }
}
const poller = setInterval(() => { void pollEvents(); }, 250);
server.listen(port, '127.0.0.1', () => {
  log('INFO', `OUTLANDS // GAMMA listening on http://127.0.0.1:${port}`);
  log('INFO', `Game events: ${eventsPath}`);
});
function shutdown(): void { clearInterval(poller); server.close(() => process.exit(0)); }
process.on('SIGINT', shutdown); process.on('SIGTERM', shutdown);
